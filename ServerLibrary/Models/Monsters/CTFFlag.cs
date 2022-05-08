using System.Linq;
using Library;
using Server.DBModels;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class CTFFlag : MonsterObject
    {
        public override bool CanMove => false;
        public CaptureTheFlag CTFGame;
        public CTFFlag()
        {
            Direction = MirDirection.Up;
        }

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            if (attacker == null || attacker.Race != ObjectType.Player || !Functions.InRange(attacker.CurrentLocation, CurrentLocation, 2))
                return 0;

            PlayerObject player = (PlayerObject)attacker;

            if (CTFGame == null)
                return 0;
            int result = 0;
            if (EventTeam == 1)
            {
                if (CTFGame.TeamB.Contains(player.Character))
                {
                    result = base.Attacked(attacker, 1, element, canReflect, ignoreShield, canCrit);

                    foreach (CharacterInfo chara in CTFGame.TeamA)
                    {
                        PlayerObject teamplayer = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                        teamplayer.Connection.ReceiveChat($"Your flag is under attack", MessageType.System);
                    }
                }
            }
            if (EventTeam == 2)
            {
                if (CTFGame.TeamA.Contains(player.Character))
                {
                    result = base.Attacked(attacker, 1, element, canReflect, ignoreShield, canCrit);

                    foreach (CharacterInfo chara in CTFGame.TeamB)
                    {
                        PlayerObject teamplayer = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                        teamplayer.Connection.ReceiveChat($"Your flag is under attack", MessageType.System);
                    }
                }
            }

            if (result > 0)
                EXPOwner = player;
            else
                EXPOwner = null;


            return result;
        }

        public override bool ApplyPoison(Poison p)
        {
            return false;
        }

        public override void ProcessRegen()
        {
        }
        public override bool ShouldAttackTarget(MapObject ob)
        {
            return false;

        }
        public override bool CanAttackTarget(MapObject ob)
        {
            return false;

        }

        public override void Die()
        {
            if (CTFGame != null)
            {
                if (EXPOwner.HasNoNode())
                {
                    base.Die();
                    return;
                }

                EXPOwner.HasFlag = true;

                #region Conquest Stats

                UserArenaPvPStats stats = SEnvir.GetArenaPvPStats(EXPOwner);

                if (stats != null)
                    stats.FlagCaptures++;

                #endregion

                foreach (PlayerObject player in CTFGame.Players)
                    player.Enqueue(new S.HasFlag { ObjectID = EXPOwner.ObjectID, hasFLag = true });

                if (EventTeam == 1)
                {
                    CTFGame.FlagAHolder = EXPOwner;
                    foreach (PlayerObject teamplayer in CTFGame.Players)
                    {
                        teamplayer.Connection.ReceiveChat($"Team A flag was captured by {EXPOwner.Character.CharacterName}", MessageType.System);
                    }
                }
                if (EventTeam == 2)
                {
                    CTFGame.FlagBHolder = EXPOwner;
                    foreach (PlayerObject teamplayer in CTFGame.Players)
                    {
                        teamplayer.Connection.ReceiveChat($"Team B flag was captured by {EXPOwner.Character.CharacterName}", MessageType.System);
                    }
                }

            }

            base.Die();
        }
    }
}
