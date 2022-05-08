using System;
using Library;
using Server.Envir;

namespace Server.Models.Monsters
{
    public class UmaKing : MonsterObject
    {
        public int RangeChance = 5;
        public int enrageDelayMin = 10;
        public int enrageDelayMax = 30;
        public int enrageTime = 5;

        public DateTime RageStart
        {
            get; set;
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
                    if (SEnvir.Random.Next(RangeChance) == 0)
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

            if (SEnvir.Random.Next(RangeChance) > 0)
            {
                if (InAttackRange())
                    Attack();
            }
            else
                RangeAttack();
        }

        public virtual void RangeAttack()
        {
            switch (SEnvir.Random.Next(5))
            {
                case 0:
                    MassThunderBolt();
                    break;
                default:
                    AttackMagic(MagicType.ThunderBolt, Element.Lightning, true);
                    break;
            }
        }
    }
}
