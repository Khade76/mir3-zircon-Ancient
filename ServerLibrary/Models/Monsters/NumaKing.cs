using System;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class NumaKing : MonsterObject
    {
        public Stat Affinity;
        public int enrageDelayMin = 10;
        public int enrageDelayMax = 30;
        public int enrageTime = 5;

        public DateTime RageStart
        {
            get; set;
        }

        public NumaKing()
        {
            switch (SEnvir.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    MaxStage = 4;
                    MinSpawn = 20;
                    RandomSpawn = 10;
                    Affinity = Stat.WindAffinity;
                    break;
                case DayOfWeek.Tuesday:
                    MaxStage = 5;
                    MinSpawn = 15;
                    RandomSpawn = 8;
                    Affinity = Stat.HolyAffinity;
                    break;
                case DayOfWeek.Wednesday:
                    MaxStage = 4;
                    MinSpawn = 20;
                    RandomSpawn = 10;
                    Affinity = Stat.DarkAffinity;
                    break;
                case DayOfWeek.Thursday:
                    MaxStage = 5;
                    MinSpawn = 15;
                    RandomSpawn = 8;
                    Affinity = Stat.FireAffinity;
                    break;
                case DayOfWeek.Friday:
                    MaxStage = 4;
                    MinSpawn = 20;
                    RandomSpawn = 10;
                    Affinity = Stat.IceAffinity;
                    break;
                case DayOfWeek.Saturday:
                    MaxStage = 5;
                    MinSpawn = 15;
                    RandomSpawn = 8;
                    Affinity = Stat.PhantomAffinity;
                    break;
                case DayOfWeek.Sunday:
                    MaxStage = 4;
                    MinSpawn = 20;
                    RandomSpawn = 10;
                    Affinity = Stat.PhantomAffinity;
                    break;
            }

            Stage = MaxStage;
            AvoidFireWall = false;
            MaxMinions = 25;
        }



        protected override void OnSpawned()
        {
            base.OnSpawned();

            Stage = MaxStage;
        }

        public override void Process()
        {
            base.Process();

            if (Dead)
                return;

            if (CurrentHP * MaxStage / Stats[Stat.Health] >= Stage || Stage <= 0)
                return;

            Stage--;

            ActionTime += TimeSpan.FromSeconds(1);

            Broadcast(new S.ObjectShow { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            SpawnMinions(MinSpawn, RandomSpawn, Target);
        }


        public override void ProcessTarget()
        {
            if (Target == null)
                return;

            if (NearbyTargets(1) > 4)
                TeleportNearby(5, 5);

            if (NextEnrageTime == DateTime.MinValue)
                InitEnrage(enrageDelayMin, enrageDelayMax);

            if (Enraged && SEnvir.Now > EnrageEndTime)
                EnrageEnd(enrageDelayMin, enrageDelayMax);

            if (!Enraged && SEnvir.Now > NextEnrageTime)
                EnrageStart(enrageTime);

            if (!InAttackRange())
            {
                if (CanAttack)
                {
                    if (SEnvir.Random.Next(2) == 0)
                        RangeAttack();
                }


                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction))
                            break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                {
                    if (SEnvir.Random.Next(10) == 0 && SEnvir.Now > teleportDelay)
                    {
                        TeleportToTarget();
                        teleportDelay = SEnvir.Now.AddSeconds(10);
                    }
                    else
                        MoveTo(Target.CurrentLocation);
                }
            }

            if (!CanAttack)
                return;

            if (SEnvir.Random.Next(5) > 0)
            {
                if (InAttackRange())
                    Attack();
            }
            else
                RangeAttack();
        }

        public void RangeAttack()
        {
            switch (SEnvir.Random.Next(5))
            {
                case 0:
                    AttackMagic(MagicType.MeteorShower, Element.Dark, false, GetDC() * 3);
                    break;
                case 1:
                    MassBeckon();
                    break;
                case 2:
                    MassPurification();
                    break;
                case 3:
                    Tempest();
                    break;
                default:
                    AttackMagic(MagicType.GreaterEvilSlayer, Element.Dark, false, GetDC() * 3);
                    break;
            }
        }
    }
}
