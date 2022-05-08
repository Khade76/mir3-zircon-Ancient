using System;
using Library;
using Library.Network;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class MirrorImage : MonsterObject

    {
        public bool Mode;
        public DateTime ModeTime;

        public override bool CanAttack => base.CanAttack && Mode;

        public Element Element
        {
            get; set;
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();

            CurrentMap.Broadcast(CurrentLocation, new S.MapEffect { Location = CurrentLocation, Effect = Effect.MirrorImage, Direction = Direction });

            ActionTime = SEnvir.Now.AddSeconds(2);
        }

        public override bool CanBeSeenBy(PlayerObject ob)
        {
            return Visible && base.CanBeSeenBy(ob);
        }

        public void Appear()
        {
            Visible = true;
            AddAllObjects();
        }
        public override void Process()
        {
            if (!Dead && SEnvir.Now > ActionTime)
            {
                if (Target != null)
                    ModeTime = SEnvir.Now.AddSeconds(10);

                if (!Mode && SEnvir.Now < ModeTime)
                {
                    Mode = true;
                    Broadcast(new S.ObjectShow { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    ActionTime = SEnvir.Now.AddSeconds(2);
                }
                else if (Mode && SEnvir.Now > ModeTime)
                {
                    Mode = false;
                    Broadcast(new S.ObjectHide() { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    ActionTime = SEnvir.Now.AddSeconds(2);
                }
            }
            base.Process();
        }
        public override void ProcessSearch()
        {
        }

        public override bool CanAttackTarget(MapObject ob)
        {
            return false;
        }


    }
}
