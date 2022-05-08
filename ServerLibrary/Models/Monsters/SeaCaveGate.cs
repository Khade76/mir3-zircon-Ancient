using System;
using System.Drawing;
using Library;
using Server.Envir;

namespace Server.Models.Monsters
{
    public class SeaCaveGate : MonsterObject
    {
        public override bool CanMove => false;
        public override bool CanAttack => false;

        public DateTime DespawnTime;


        public SeaCaveGate()
        {
            Direction = MirDirection.Up;
        }

        public override void ProcessNameColour()
        {
            NameColour = Color.Lime;
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();


            DespawnTime = SEnvir.Now.AddMinutes(20);

            foreach (SConnection con in SEnvir.AuthenticatedConnections)
                con.ReceiveChat(string.Format(con.Language.SeaCaveGateOpen, CurrentMap.Info.Description, CurrentLocation), MessageType.System);

        }

        public override void Process()
        {
            base.Process();

            if (SEnvir.Now >= DespawnTime)
            {
                if (SpawnInfo != null)
                    SpawnInfo.AliveCount--;

                foreach (SConnection con in SEnvir.AuthenticatedConnections)
                    con.ReceiveChat(con.Language.SeaCaveGateClosed, MessageType.System);

                SpawnInfo = null;
                Despawn();
                return;
            }

            if (SEnvir.Now >= SearchTime && SEnvir.SeaCaveMapRegion != null && SEnvir.SeaCaveMapRegion.PointList.Count > 0)
            {
                SearchTime = SEnvir.Now.AddSeconds(3);
                Map map = SEnvir.GetMap(SEnvir.SeaCaveMapRegion.Map);

                if (map == null)
                {
                    SearchTime = SEnvir.Now.AddSeconds(60);
                    return;
                }

                for (int i = CurrentMap.Objects.Count - 1; i >= 0; i--)
                {
                    MapObject ob = CurrentMap.Objects[i];

                    if (ob == this)
                        continue;

                    if (ob is Guard)
                        continue;

                    switch (ob.Race)
                    {
                        case ObjectType.Player:
                            if (ob.InSafeZone)
                                continue;

                            if (ob.Dead || !Functions.InRange(ob.CurrentLocation, CurrentLocation, MonsterInfo.ViewRange))
                                continue;

                            ob.Teleport(map, SEnvir.SeaCaveMapRegion.PointList[SEnvir.Random.Next(SEnvir.SeaCaveMapRegion.PointList.Count)]);
                            break;
                        default:
                            continue;
                    }

                }
            }

        }

        public override void ProcessSearch()
        {
        }

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            return 0;
        }
        public override bool ApplyPoison(Poison p)
        {
            return false;
        }
    }
}
