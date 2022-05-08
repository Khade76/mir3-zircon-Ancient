using System;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class OmaKing : MonsterObject
    {
        private bool CanTeleport = true;
        public int RangeChance = 5;

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            int result = base.Attacked(attacker, power, element, canReflect, ignoreShield, canCrit);

            if (BonusTime != DateTime.MinValue)
                BonusTime.AddSeconds(-1);
            else
                BonusTime = SEnvir.Now.AddSeconds(30);

            if (result < 0 || Dead || !CanTeleport || CurrentHP > Stats[Stat.Health] / 2)
                return result;

            CanTeleport = false;

            TeleportNearby(7, 12);

            return result;
        }


        public DateTime TeleportTime
        {
            get; set;
        }
        public DateTime BonusTime
        {
            get; set;
        }
        public bool Bonus;
        public override void ProcessTarget()
        {
            if (Target == null)
                return;

            if (BonusTime == DateTime.MinValue)
                BonusTime = SEnvir.Now.AddSeconds(30);

            if (!Functions.InRange(Target.CurrentLocation, CurrentLocation, 1) && SEnvir.Now > TeleportTime && CanAttack)
            {
                TeleportTime = SEnvir.Now.AddSeconds(30);

                if (SEnvir.Random.Next(7) == 0)
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
                        Bonus = true;
                    }
                }
            }

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
                    MoveTo(Target.CurrentLocation);
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
            switch (SEnvir.Random.Next(3))
            {
                case 1:
                    AttackAoE(2, MagicType.FireStorm, Element.Fire, GetDC() * 2 / 3);
                    break;
                default:
                    AttackMagic(MagicType.AdamantineFireBall, Element.Fire, false, GetDC());
                    break;
            }
        }

        protected override void Attack()
        {
            if (SEnvir.Now > BonusTime)
                Bonus = true;
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            UpdateAttackTime();

            int damage = GetDC();

            if (Bonus)
                damage *= 2;

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(400),
                ActionType.DelayAttack,
                Target,
                damage,
                AttackElement));

            if (Bonus)
            {
                BonusTime = SEnvir.Now.AddSeconds(30);
                Bonus = false;
            }
        }
    }
}
