﻿using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Library.SystemModels;
using Server.DBModels;
using Server.Envir;
using Server.Models.Monsters;
using S = Library.Network.ServerPackets;

namespace Server.Models
{
    public sealed class ConquestWar
    {
        public DateTime StartTime, EndTime;
        public CastleInfo Castle;

        public List<GuildInfo> Participants;
        public List<CastleOutPost> OutPosts;
        public Map Map;

        public CastleFlag CastleBoss;
        public bool Ended;

        public Dictionary<CharacterInfo, UserConquestStats> Stats = new Dictionary<CharacterInfo, UserConquestStats>();

        public void StartWar()
        {
            foreach (SConnection con in SEnvir.AuthenticatedConnections)
                con.ReceiveChat(string.Format(con.Language.ConquestStarted, Castle.Name), MessageType.System);


            Map = SEnvir.GetMap(Castle.Map);

            for (int i = Map.NPCs.Count - 1; i >= 0; i--)
            {
                NPCObject npc = Map.NPCs[i];
                //   if (!Castle.CastleRegion.PointList.Contains(npc.CurrentLocation)) continue;

                npc.Visible = false;
                npc.RemoveAllObjects();
            }

            foreach (GuildInfo guild in Participants)
                guild.Conquest?.Delete();

            SEnvir.Broadcast(new S.GuildConquestStarted { Index = Castle.Index });

            PingPlayers();

            SpawnBoss();
            SpawnOutPosts();

            SEnvir.ConquestWars.Add(this);

        }

        public void Process()
        {
            if (SEnvir.Now < EndTime)
                return;

            EndWar();
        }


        public void EndWar()
        {
            foreach (SConnection con in SEnvir.AuthenticatedConnections)
                con.ReceiveChat(string.Format(con.Language.ConquestFinished, Castle.Name), MessageType.System);

            Ended = true;


            for (int i = Map.NPCs.Count - 1; i >= 0; i--)
            {
                NPCObject npc = Map.NPCs[i];
                //     if (!Castle.CastleRegion.PointList.Contains(npc.CurrentLocation)) continue;

                npc.Visible = true;
                npc.AddAllObjects();
            }

            PingPlayers();

            DespawnBoss();
            DespawnOutPosts();

            SEnvir.ConquestWars.Remove(this);

            SEnvir.Broadcast(new S.GuildConquestFinished { Index = Castle.Index });

            GuildInfo ownerGuild = SEnvir.GuildInfoList.Binding.FirstOrDefault(x => x.Castle == Castle);


            if (ownerGuild != null)
            {
                foreach (SConnection con in SEnvir.AuthenticatedConnections)
                    con.ReceiveChat(string.Format(con.Language.ConquestOwner, ownerGuild.GuildName, Castle.Name), MessageType.System);
            }
            UserConquest conquest = SEnvir.UserConquestList.Binding.FirstOrDefault(x => x.Castle == Castle && x.Castle == ownerGuild?.Castle);

            TimeSpan warTime = TimeSpan.MinValue;

            if (conquest != null)
                warTime = (conquest.WarDate + conquest.Castle.StartTime) - SEnvir.Now;
            List<string> castleParticipants = new List<string>();
            foreach (UserConquest conquests in SEnvir.UserConquestList.Binding)
            {
                if (conquest.Castle != conquests.Castle)
                    continue;

                castleParticipants.Add(conquests.Guild.GuildName);
            }


            foreach (PlayerObject players in SEnvir.Players)
            {
                players.Connection.Enqueue(new S.GuildConquestDate { Index = Castle.Index, WarTime = warTime, ObserverPacket = false, participants = castleParticipants });
            }

        }

        public void PingPlayers()
        {
            foreach (PlayerObject player in Map.Players)
            {
                //if (!Castle.CastleRegion.PointList.Contains(player.CurrentLocation)) continue;

                if (player.Character.Account.GuildMember?.Guild?.Castle == Castle)
                    continue;

                player.Teleport(Castle.AttackSpawnRegion);
            }
        }

        public void DespawnBoss()
        {
            if (CastleBoss == null)
                return;

            CastleBoss.EXPOwner = null;
            CastleBoss.War = null;
            CastleBoss.Die();
            CastleBoss.Despawn();
            CastleBoss = null;
        }
        public void DespawnOutPosts()
        {
            if (OutPosts.Count == 0)
                return;

            foreach (CastleOutPost outpost in OutPosts)
            {
                outpost.EXPOwner = null;
                outpost.War = null;
                outpost.Die();
                outpost.Despawn();
            }
            OutPosts.Clear();
        }
        public void SpawnOutPosts()
        {
            OutPosts = new List<CastleOutPost>();
            CastleOutPost CastleOutPost1 = new CastleOutPost
            {
                MonsterInfo = Castle.OutPostMon,
                War = this,
                Name = "OutPost West",
            };

            CastleOutPost1.Spawn(Castle.OutPost1);
            OutPosts.Add(CastleOutPost1);
            CastleOutPost CastleOutPost2 = new CastleOutPost
            {
                MonsterInfo = Castle.OutPostMon,
                War = this,
                Name = "OutPost South",
            };

            CastleOutPost2.Spawn(Castle.OutPost2);
            OutPosts.Add(CastleOutPost2);
            CastleOutPost CastleOutPost3 = new CastleOutPost
            {
                MonsterInfo = Castle.OutPostMon,
                War = this,
                Name = "OutPost East",
            };

            CastleOutPost3.Spawn(Castle.OutPost3);
            OutPosts.Add(CastleOutPost3);
            CastleOutPost CastleOutPost4 = new CastleOutPost
            {
                MonsterInfo = Castle.OutPostMon,
                War = this,
                Name = "OutPost SouthEast",
            };

            CastleOutPost4.Spawn(Castle.OutPost4);
            OutPosts.Add(CastleOutPost4);
            CastleOutPost CastleOutPost5 = new CastleOutPost
            {
                MonsterInfo = Castle.OutPostMon,
                War = this,
                Name = "OutPost SouthWest",
            };

            CastleOutPost5.Spawn(Castle.OutPost5);
            OutPosts.Add(CastleOutPost5);

        }
        public void SpawnBoss()
        {
            GuildInfo ownerGuild = SEnvir.GuildInfoList.Binding.FirstOrDefault(x => x.Castle == Castle);

            CastleBoss = new CastleFlag
            {
                MonsterInfo = Castle.Monster,
                War = this,
            };

            if (ownerGuild != null)
            {
                CastleBoss.FlagColour = ownerGuild.FlagColour;
                CastleBoss.FlagShape = ownerGuild.FlagShape;
                CastleBoss.Name = ownerGuild.GuildName + "(Flag)";
            }


            CastleBoss.Spawn(Castle.CastleRegion);
            SEnvir.Broadcast(new S.ObjectFlagColour { ObjectID = CastleBoss.ObjectID, FlagColour = CastleBoss.FlagColour, FlagShape = CastleBoss.FlagShape, name = CastleBoss.Name });
        }

        public UserConquestStats GetStat(CharacterInfo character)
        {
            UserConquestStats user;

            if (!Stats.TryGetValue(character, out user))
            {
                user = SEnvir.UserConquestStatsList.CreateNewObject();

                user.Character = character;

                user.WarStartDate = StartTime;
                user.CastleName = Castle.Name;

                user.GuildName = character.Account.GuildMember?.Guild.GuildName ?? "No Guild.";
                user.CharacterName = character.CharacterName;
                user.Class = character.Class;
                user.Level = character.Level;

                Stats[character] = user;
            }

            return user;
        }
    }

}
