using System;
using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class VoraciousGhost : MonsterObject
    {
        public int DeathCount;
        public int ReviveCount;
        public DateTime ReviveTime;

        public VoraciousGhost()
        {
            ReviveCount = SEnvir.Random.Next(2);
        }


        public override void Process()
        {
            base.Process();

            if (!Dead || ReviveCount == 0 || SEnvir.Now < ReviveTime)
                return;

            Broadcast(new S.ObjectShow { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = SEnvir.Now.AddMilliseconds(1500);

            Dead = false;
            SetHP((Stats[Stat.Health]));
            ReviveCount--;
        }

        public override void Die()
        {
            base.Die();

            ReviveTime = SEnvir.Now.AddSeconds(SEnvir.Random.Next(5) + 3);
            DeadTime = ReviveTime.Add(Config.DeadDuration);

            DeathCount++;
        }
    }
}
