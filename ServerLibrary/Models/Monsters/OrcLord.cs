using System;
using System.Collections.Generic;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class OrcLord : MonsterObject
    {
        public override bool CanAttack => Visible && base.CanAttack;
        public override bool CanMove => Visible && base.CanMove;
        public override bool Blocking => Visible && base.Blocking;
        public Stat Affinity;

        public DateTime TeleportTime, DarkNessTime;
       // public List<OrcLord> Clones;
        public bool isActive, DCDMG;
        public OrcLord OrcMaster;
        public List<OrcLord> Clones;

        public OrcLord()
        {
            MaxStage = 6;
            MinSpawn = 2;
            RandomSpawn = 5;
            AvoidFireWall = false;
            MaxMinions = 2;
            Stage = MaxStage;
            int randAffinity = SEnvir.Random.Next(7);
            isActive = true;
            switch (randAffinity)
            {
                case 0:
                    Affinity = Stat.FireAffinity;
                    break;
                case 1:
                    Affinity = Stat.IceAffinity;
                    break;
                case 2:
                    Affinity = Stat.LightningAffinity;
                    break;
                case 3:
                    Affinity = Stat.WindAffinity;
                    break;
                case 4:
                    Affinity = Stat.HolyAffinity;
                    break;
                case 5:
                    Affinity = Stat.DarkAffinity;
                    break;
                case 6:
                    Affinity = Stat.PhantomAffinity;
                    break;
            }
        }

        public override void ApplyBonusStats()
        {
            base.ApplyBonusStats();

            Stats[Affinity] = 1;
        }
        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            int result = 0;

            if (!isActive)
                return 0;

            if (!isClone)
                return result = base.Attacked(attacker, power, element, canReflect, ignoreShield, canCrit);

            if (element == Element.None && DCDMG)
                result = base.Attacked(attacker, power*10, element, canReflect, ignoreShield, canCrit);

            if (element != Element.None && !DCDMG)
                result = base.Attacked(attacker, power * 10, element, canReflect, ignoreShield, canCrit);

            return result;
        }
        public override void ProcessAI()
        {
            if (isActive)
                base.ProcessAI();
            else
                return;

            if (Dead)
                return;

            if (SEnvir.Now > DarkNessTime)
            {
                DarkNessTime = SEnvir.Now.AddSeconds(SEnvir.Random.Next(30)+60);
                DarknessAttack();
            }
        }
        public override void Process()
        {
            base.Process();

            if (Dead || !isActive)
                return;

            if (isClone)
                return;
            if (CurrentHP * MaxStage / Stats[Stat.Health] >= Stage-1 || Stage <= 0)
                return;

            Stage--;

            ActionTime += TimeSpan.FromMilliseconds(50);

            Broadcast(new S.ObjectHide { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            DarknessAttack();
            
            SpawnCopies(MinSpawn, Target);
            MinSpawn += 1;
            isActive = false;
            Visible = false;
        }
        public override void ProcessTarget()
        {
            if (Target == null || !isActive)
                return;

            if (!Functions.InRange(Target.CurrentLocation, CurrentLocation, 3) && SEnvir.Now > TeleportTime)
            {
                MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Cell cell = null;
                for (int i = 0; i < 8; i++)
                {
                    cell = CurrentMap.GetCell(Functions.Move(Target.CurrentLocation, Functions.ShiftDirection(dir, i), 1));

                    if (cell == null || cell.Movements != null)
                    {
                        cell = null;
                        continue;
                    }
                    break;
                }

                if (cell != null)
                {
                    Direction = Functions.DirectionFromPoint(cell.Location, Target.CurrentLocation);
                    Teleport(CurrentMap, cell.Location);

                    TeleportTime = SEnvir.Now.AddSeconds(10);
                }
            }


            if (Target.Race == ObjectType.Monster)
            {
                Target.SetHP(0);
                return;
            }

            base.ProcessTarget();
        }
        public override void Die()
        {
            if(isClone)
            {
                OrcMaster.Clones.Remove(this);
                if(OrcMaster.Clones.Count == 0)
                {
                    OrcMaster.isActive = true;
                    OrcMaster.Visible = true;
                    Broadcast(new S.ObjectShow { ObjectID = OrcMaster.ObjectID, Direction = Direction, Location = CurrentLocation });
                }
            }

            base.Die();
        }
        public override bool CanBeSeenBy(PlayerObject ob)
        {
            return Visible && base.CanBeSeenBy(ob);
        }
        public void SpawnCopies(int fixedCount, MapObject target)
        {

            while (fixedCount > 0)
            {
                OrcLord mob = new OrcLord();
                switch (fixedCount % 2)
                {
                    case 1:
                        mob = new OrcLord
                        {
                            MonsterInfo = MonsterInfo,
                            isClone = true,
                            DCDMG = true,
                        };
                        break;
                    case 0:
                        mob = new OrcLord
                        {
                            MonsterInfo = MonsterInfo,
                            isClone = true,
                            DCDMG = false,
                        };
                        break;

                }
                if (mob == null)
                    return;

                mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 5));
                mob.CurrentHP = CurrentHP;
                mob.Target = target;
                if (mob.OrcMaster == null)
                    mob.OrcMaster = new OrcLord();
                mob.OrcMaster = this;

                if (Clones == null)
                    Clones = new List<OrcLord>();
                Clones.Add(mob);

                fixedCount--;
            }
        }

    }
}
