using System.Linq;
using Library;
using Server.DBModels;
using Server.Envir;
using S = Library.Network.ServerPackets;

namespace Server.Models.Monsters
{
    public class CastleFlag : MonsterObject
    {
        public override bool CanMove => false;
        public ConquestWar War;

        public CastleFlag()
        {
            Direction = MirDirection.Up;
        }

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            if (attacker == null || attacker.Race != ObjectType.Player)
                return 0;

            PlayerObject player = (PlayerObject)attacker;

            if (War == null)
                return 0;
            if (War.OutPosts.Count > 0)
                return 0;

            if (player.Character.Account.GuildMember == null)
                return 0;

            if (player.Character.Account.GuildMember.Guild.Castle == War.Castle)
                return 0;

            if (War.Participants.Count > 0 && !War.Participants.Contains(player.Character.Account.GuildMember.Guild))
                return 0;

            int result = base.Attacked(attacker, 1, element, canReflect, ignoreShield, canCrit);

            #region Conquest Stats

            switch (attacker.Race)
            {
                case ObjectType.Player:
                    UserConquestStats conquest = SEnvir.GetConquestStats((PlayerObject)attacker);

                    if (conquest != null)
                        conquest.BossDamageDealt += result;
                    break;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)attacker;
                    if (mob.PetOwner != null)
                    {
                        conquest = SEnvir.GetConquestStats(mob.PetOwner);

                        if (conquest != null)
                            conquest.BossDamageDealt += result;
                    }
                    break;
            }

            #endregion


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
            if (War != null)
            {
                if (EXPOwner.HasNoNode())
                    return;

                if (EXPOwner.Character.Account.GuildMember == null)
                    return;

                if (EXPOwner.Character.Account.GuildMember.Guild.Castle != null)
                    return;

                if (War.Participants.Count > 0 && !War.Participants.Contains(EXPOwner.Character.Account.GuildMember.Guild))
                    return;

                #region Conquest Stats

                UserConquestStats conquest = SEnvir.GetConquestStats(EXPOwner);

                if (conquest != null)
                    conquest.BossKillCount++;

                #endregion

                GuildInfo ownerGuild = SEnvir.GuildInfoList.Binding.FirstOrDefault(x => x.Castle == War.Castle);

                if (ownerGuild != null)
                    ownerGuild.Castle = null;

                EXPOwner.Character.Account.GuildMember.Guild.Castle = War.Castle;

                foreach (SConnection con in SEnvir.AuthenticatedConnections)
                    con.ReceiveChat(string.Format(con.Language.ConquestCapture, EXPOwner.Character.Account.GuildMember.Guild.GuildName, War.Castle.Name), MessageType.System);

                SEnvir.Broadcast(new S.GuildCastleInfo { Index = War.Castle.Index, Owner = EXPOwner.Character.Account.GuildMember.Guild.GuildName });
                SEnvir.CastleGuildFlagChange(War.Castle, EXPOwner.Character.Account.GuildMember.Guild);
                War.CastleBoss = null;

                War.PingPlayers();

                War.EndTime = SEnvir.Now;

                foreach (PlayerObject player in SEnvir.Players)
                    player.ApplyCastleBuff();


                War = null;
            }

            base.Die();
        }
    }
}
