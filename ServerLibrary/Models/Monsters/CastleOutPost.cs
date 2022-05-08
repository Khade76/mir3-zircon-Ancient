using System;
using System.Collections.Generic;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class CastleOutPost : MonsterObject
    {
        public override bool CanMove => false;

        public ConquestWar War;
        public List<CastleGuard> CastleGuards;
        public DateTime SpawnDelay = SEnvir.Now;
        public int maxspawns = 15;

        public CastleOutPost()
        {
            Direction = MirDirection.Up;
            CastleGuards = new List<CastleGuard>();
        }

        protected override bool InAttackRange()
        {
            return Target.CurrentMap == CurrentMap;
        }

        public override void ProcessAction(DelayedAction action)
        {
            switch (action.Type)
            {
                case ActionType.Function:

                    SpawnGuards(SEnvir.Random.Next(5) + 2, Target);
                    break;
            }

            base.ProcessAction(action);
        }

        public void SpawnGuards(int fixedCount, MapObject target)
        {
            if (CastleGuards.Count > maxspawns) return;

            while (fixedCount > 0)
            {
                CastleGuard mob = new CastleGuard();
                switch (fixedCount % 2)
                {
                    case 1:
                        mob = new CastleGuard
                        {
                            MonsterInfo = War.Castle.OutPostGuard1,
                            War = War,
                            outpost = this,
                        };
                        break;
                    case 0:
                        mob = new CastleGuard
                        {
                            MonsterInfo = War.Castle.OutPostGuard2,
                            War = War,
                            outpost = this,
                            AttackRange = 5,
                            range = true,
                        };
                        break;

                }
                if (mob == null)
                    return;

                mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 5));

                mob.Target = target;
                mob.Master = this;
                CastleGuards.Add(mob);

                fixedCount--;
            }
        }

        protected override void Attack()
        {
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            UpdateAttackTime();

            if (SEnvir.Now > SpawnDelay)

            {
                ActionList.Add(new DelayedAction(SEnvir.Now.AddMilliseconds(600), ActionType.Function));
                SpawnDelay = SEnvir.Now.AddSeconds(10);
            }
        }


        public override bool ShouldAttackTarget(MapObject ob)
        {
            if (ob == null || ob.Race != ObjectType.Player || ob.Dead)
                return false;

            PlayerObject player = (PlayerObject)ob;

            if (player.Character.Account.GuildMember.Guild.Castle != null && player.Character.Account.GuildMember.Guild.Castle == War.Castle)
                return false;

            return true;

        }
        public override bool CanAttackTarget(MapObject ob)
        {
            if (ob == null || ob.Race != ObjectType.Player || ob.Dead)
                return false;

            PlayerObject player = (PlayerObject)ob;

            if (player.Character.Account.GuildMember.Guild.Castle != null && player.Character.Account.GuildMember.Guild.Castle == War.Castle)
                return false;

            return true;

        }
        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            if (attacker == null || attacker.Race != ObjectType.Player)
                return 0;

            PlayerObject player = (PlayerObject)attacker;

            if (War == null)
                return 0;
            if (player.Character.Account.GuildMember == null)
                return 0;

            if (player.Character.Account.GuildMember.Guild.Castle != null && player.Character.Account.GuildMember.Guild.Castle == War.Castle)
                return 0;

            if (War.Participants.Count > 0 && !War.Participants.Contains(player.Character.Account.GuildMember.Guild))
                return 0;

            RegenTime = SEnvir.Now + RegenDelay;
            return base.Attacked(attacker, 1, element, ignoreShield, canCrit);
        }

        public override bool ApplyPoison(Poison p)
        {
            return false;
        }
        public override void Die()
        {
            foreach (CastleGuard guard in CastleGuards)
            {
                guard.EXPOwner = null;
                guard.War = null;
                guard.outpost = null;
                guard.Die();
                guard.Despawn();
            }
            CastleGuards.Clear();

            if (War != null)
            {
                War.OutPosts.Remove(this);
                War = null;
            }


            base.Die();
        }
    }
}
