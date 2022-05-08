using System;
using System.Collections.Generic;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class JinchonWarlord : SpittingSpider
    {
        public DateTime CastTime;
        public TimeSpan CastDelay = TimeSpan.FromSeconds(15);

        public JinchonWarlord()
        {
            AvoidFireWall = true;
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap)
                return false;
            if (Target.CurrentLocation == CurrentLocation)
                return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 3 || y > 3)
                return false;


            return true;
        }


        public override void ProcessTarget()
        {
            if (Target == null)
                return;

            if (CanAttack && SEnvir.Now > CastTime)
            {
                List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, ViewRange);
                bool casted = false;

                if (targets.Count > 0)
                {

                    foreach (MapObject ob in targets)
                    {
                        bool hascast = false;
                        if (targets.Count > 1)
                        {
                            if (CurrentHP > Stats[Stat.Health] / 2 && SEnvir.Random.Next(2) > 0)
                                continue;
                            hascast = true;
                        }
                        else
                            hascast = true;

                        if (hascast)
                        {
                            ParalysisCloud(ob.CurrentLocation);
                            casted = true;
                        }
                    }

                    if (casted)
                    {
                        UpdateAttackTime();
                        Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.None, Targets = new List<uint> { Target.ObjectID } });
                        CastTime = SEnvir.Now + CastDelay;
                    }
                }

            }

            if (!InAttackRange())
            {
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
                return;
            }

            if (!CanAttack)
                return;

            Attack();
        }

        protected override void Attack()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (SEnvir.Random.Next(3) == 0 || !Functions.InRange(Target.CurrentLocation, CurrentLocation, 2))
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Targets = new List<uint>() });
                LineAttack(4);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation }); //Animation ?

                foreach (MapObject ob in GetTargets(CurrentMap, Functions.Move(CurrentLocation, Direction), 1))
                {
                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(400),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        AttackElement));
                    break;
                }
            }

            UpdateAttackTime();
        }

    }
}
