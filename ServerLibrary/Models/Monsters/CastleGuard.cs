using System.Collections.Generic;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class CastleGuard : MonsterObject
    {

        public ConquestWar War;
        public CastleOutPost outpost;
        public bool range;

        public CastleGuard()
        {
            outpost = new CastleOutPost();
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

        public override void ProcessTarget()
        {
            if (outpost != null)
            {
                if (Functions.InRange(outpost.CurrentLocation, CurrentLocation, 12))
                {
                    TeleportToBase();
                }
            }

            if (!range)
            {
                base.ProcessTarget();
                return;
            }

            if (Target == null)
                return;

            MirDirection direction;
            int rotation;
            if (!InAttackRange())
            {
                if (CurrentLocation == Target.CurrentLocation)
                {
                    direction = (MirDirection)SEnvir.Random.Next(8);
                    rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction))
                            break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);

                return;
            }

            if (Functions.InRange(Target.CurrentLocation, CurrentLocation, AttackRange - 1))
            {
                direction = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                for (int d = 0; d < 8; d++)
                {
                    if (Walk(direction))
                        break;

                    direction = Functions.ShiftDirection(direction, rotation);
                }
            }
            if (!CanAttack || SEnvir.Now < FearTime)
                return;

            Attack();
        }
        public bool TeleportToBase()
        {
            if (outpost == null) return false;
            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, outpost.CurrentLocation);
            Cell cell = null;
            for (int i = 0; i < 8; i++)
            {
                cell = CurrentMap.GetCell(Functions.Move(outpost.CurrentLocation, Functions.ShiftDirection(dir, i), 1));

                if (cell == null || cell.Movements != null)
                {
                    cell = null;
                    continue;
                }
                break;
            }

            if (cell != null)
            {
                Direction = Functions.DirectionFromPoint(cell.Location, outpost.CurrentLocation);
                Teleport(CurrentMap, cell.Location);
                return true;
            }
            return false;
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
            return base.Attacked(attacker, power, element, ignoreShield, canCrit);
        }

        protected override void Attack()
        {
            if (range)
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectRangeAttack
                {
                    ObjectID = ObjectID,
                    Direction = Direction,
                    Location = CurrentLocation,
                    Targets = new List<uint> { Target.ObjectID
                }
                });

                UpdateAttackTime();

                if (SEnvir.Random.Next(FearRate) == 0)
                    FearTime = SEnvir.Now.AddSeconds(FearDuration + SEnvir.Random.Next(4));

                ActionList.Add(new DelayedAction(
                                   SEnvir.Now.AddMilliseconds(400 + Functions.Distance(CurrentLocation, Target.CurrentLocation) * Globals.ProjectileSpeed),
                                   ActionType.DelayAttack,
                                   Target,
                                   GetDC(),
                                   AttackElement));
            }
            else
                base.Attack();
        }

        public override void Die()
        {
            if (outpost != null)
            {
                outpost.CastleGuards.Remove(this);
                outpost = null;
            }

            if (War != null)
            {
                War = null;
            }

            base.Die();
        }
    }
}
