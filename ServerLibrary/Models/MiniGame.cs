using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Library;
using Library.SystemModels;
using Server.DBModels;
using Server.Envir;
using Server.Models.Monsters;
using S = Library.Network.ServerPackets;

namespace Server.Models
{
    public class MiniGame
    {
        public DateTime StartTime, EndTime;
        public List<CharacterInfo> TeamA, TeamB, MGChars;

        public List<PlayerObject> Players;
        public Map Map, LobbyMap;

        public bool Started, Ended, NextMessage, CanRevive;

        public MiniGameInfo MGInfo;
        public Dictionary<CharacterInfo, UserArenaPvPStats> Stats = new Dictionary<CharacterInfo, UserArenaPvPStats>();
        public Dictionary<CharacterInfo, bool> EntryFeePass = new Dictionary<CharacterInfo, bool>();


        public bool CreateGame(PlayerObject player, MiniGameInfo info)
        {
            if (info == null)
                return false;
            if (player == null)
                return false;

            foreach (var mgs in SEnvir.MiniGames)
            {
                if (mgs.MGChars.Contains(player.Character))
                {
                    player.Connection.ReceiveChat($"You are already entered in to {mgs.MGInfo.MiniGame.DescriptionToString()} level {mgs.MGInfo.MinLevel} - {mgs.MGInfo.MaxLevel}", MessageType.System);
                    return false;
                }
            }

            MGChars = new List<CharacterInfo>();
            MGInfo = info;
            LobbyMap = SEnvir.GetMap(MGInfo.MapLobby);
            Map = SEnvir.GetMap(MGInfo.MapParameter);

            if (LobbyMap == null || Map == null)
                return false;

            if (!player.RemoveFreePass())
                return false;

            Players = new List<PlayerObject>();
            TeamA = new List<CharacterInfo>();
            TeamB = new List<CharacterInfo>();
            NextMessage = false;
            StartTime = SEnvir.Now.AddMinutes(1);
            EndTime = StartTime.AddMinutes(MGInfo.Duration);
            CanRevive = info.CanRevive;

            foreach (SConnection con in SEnvir.AuthenticatedConnections)
                con.ReceiveChat($"The {MGInfo.MiniGame.DescriptionToString()} for levels {MGInfo.MinLevel}-{MGInfo.MaxLevel} will start in {Functions.ToString(StartTime - SEnvir.Now, true, true)}", MessageType.Announcement);

            SEnvir.MiniGames.Add(this);
            return true;
        }
        public virtual bool ReJoinGame(PlayerObject player)
        {
            bool exists = false;

            if (MGChars.Contains(player.Character))
                exists = true;

            if (exists)
            {
                if (Started)
                {
                    if (MGInfo.TeamGame && CanRevive)
                    {
                        if (TeamA.Contains(player.Character))
                        {
                            player.EventTeam = 1;
                            player.Teleport(MGInfo.TeamASpawn);
                            return true;
                        }
                        if (TeamB.Contains(player.Character))
                        {
                            player.EventTeam = 2;
                            player.Teleport(MGInfo.TeamBSpawn);
                            return true;
                        }
                    }
                    if (CanRevive)
                    {
                        Players.Add(player);
                        player.Teleport(MGInfo.TeamASpawn);
                        return true;
                    }
                }
                else
                {
                    if (player.Teleport(LobbyMap, LobbyMap.GetRandomLocation()))
                        return true;

                }
                MGChars.Remove(player.Character); //game started before you could be put into a team or you can no longer revive/rejoin
                return false;
            }
            return false;
        }
        public bool JoinGame(PlayerObject player, bool FreePass)
        {
            foreach (var mgs in SEnvir.MiniGames)
            {
                if (mgs.MGChars.Contains(player.Character))
                {
                    if (mgs.MGInfo == MGInfo)
                    {
                        ReJoinGame(player);
                    }
                    player.Connection.ReceiveChat($"You are already entered in to {mgs.MGInfo.MiniGame.DescriptionToString()} level {mgs.MGInfo.MinLevel} - {mgs.MGInfo.MaxLevel}", MessageType.System);
                    return false;
                }
            }
            if (FreePass)
            {
                Players.Add(player);
                MGChars.Add(player.Character);
                EntryFeePass[player.Character] = true;
                return true;
            }
            else
            {
                foreach (CharacterInfo MGPlayers in MGChars)
                {
                    if (MGPlayers == player.Character)
                        return false;
                }
                if (player.Gold >= MGInfo.EntryFee)
                {
                    player.Gold -= MGInfo.EntryFee;
                    player.GoldChanged();
                    Players.Add(player);
                    MGChars.Add(player.Character);
                    EntryFeePass[player.Character] = false;
                    return true;
                }
                else
                    return false;
            }
        }
        public virtual void StartGame()
        {
            if (MGInfo == null)
            {
                SEnvir.Log($"[ERROR] Unable to start minigame.");
                foreach (PlayerObject Player in Players)
                {
                    if (Player == null)
                        continue;
                    Player.Connection.ReceiveChat("Error Starting Game", MessageType.System);
                }
                TTPlayers();
                return;
            }

            if (Players.Count < MGInfo.MinPlayers)
            {
                RefundEntry();
                return;
            }

            foreach (PlayerObject Player in Players)
            {
                if (Player == null)
                    continue;
                Player.Connection.ReceiveChat($"{MGInfo.MiniGame.DescriptionToString()} has started, good luck", MessageType.System);
            }

            Started = true;
            if (MGInfo.TeamGame)
                SetTeams();
            PingPlayers();

            SEnvir.SendMiniGamesUpdate();
        }
        public virtual void RefundEntry()
        {
            foreach (KeyValuePair<CharacterInfo, bool> pair in EntryFeePass)
            {
                List<UserItem> Items = new List<UserItem>();
                UserItem item = new UserItem();
                ItemInfo info = new ItemInfo();
                if (pair.Value)
                {
                    info = SEnvir.ItemInfoList.Binding.FirstOrDefault(x => x.ItemName == "Freedom Pass");
                }
                else
                {
                    info = SEnvir.ItemInfoList.Binding.FirstOrDefault(x => x.Effect == ItemEffect.Gold);
                }

                item = SEnvir.CreateFreshItem(info);

                if (item.Info.Effect == ItemEffect.Gold)
                    item.Count = MGInfo.EntryFee;
                else
                    item.Count = 1;

                Items.Add(item);

                if (Items.Count > 0)
                    MailRewards(pair.Key, Items);
            }

            EndGame(true);

        }
        public virtual void Process()
        {
            CheckConnections();
            if (!Started)
            {
                if (SEnvir.Now >= StartTime)
                {
                    StartGame();
                    return;
                }
                if (!NextMessage)
                {
                    TimeSpan ts = StartTime - SEnvir.Now;
                    if (ts >= TimeSpan.FromSeconds(240) && ts < TimeSpan.FromSeconds(241))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(180) && ts < TimeSpan.FromSeconds(181))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(120) && ts < TimeSpan.FromSeconds(121))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(60) && ts < TimeSpan.FromSeconds(61))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(30) && ts < TimeSpan.FromSeconds(31))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(20) && ts < TimeSpan.FromSeconds(21))
                        NextMessage = true;
                    if (ts >= TimeSpan.FromSeconds(1) && ts < TimeSpan.FromSeconds(11))
                        NextMessage = true;

                    if (NextMessage)
                    {
                        if (ts >= TimeSpan.FromSeconds(10))
                        {
                            foreach (SConnection con in SEnvir.AuthenticatedConnections)
                                con.ReceiveChat($"The {MGInfo.MiniGame.DescriptionToString()} for levels {MGInfo.MinLevel}-{MGInfo.MaxLevel} will start in {Functions.ToString(StartTime - SEnvir.Now, true, true)}", MessageType.Announcement);
                        }
                        else
                        {
                            foreach (PlayerObject player in Players)
                                player.Connection?.ReceiveChat($"The {MGInfo.MiniGame.DescriptionToString()} for levels {MGInfo.MinLevel}-{MGInfo.MaxLevel} will start in {Functions.ToString(StartTime - SEnvir.Now, true, true)}", MessageType.Announcement);
                        }
                        NextMessage = false;
                    }
                }
            }

            if (SEnvir.Now < EndTime)
                return;

            EndGame(false);
        }

        public virtual void CheckConnections()
        {
            bool refreshplayers = false;
            List<PlayerObject> newList = new List<PlayerObject>();
            foreach (PlayerObject Player in Players)
            {
                if (Player.HasNode())
                {
                    newList.Add(Player);
                    continue;
                }
                refreshplayers = true;
            }
            if (refreshplayers)
            {
                Players.Clear();
                foreach (PlayerObject player in newList)
                    Players.Add(player);
            }
            /* if (Started && MGInfo.TeamGame)
             {
                 foreach (PlayerObject Player in Players)
                 {
                     if (Player.Node != null)
                     {
                         if (TeamA.Contains(Player.Character))
                             TeamA.Remove(Player.Character);

                         if (TeamB.Contains(Player.Character))
                             TeamB.Remove(Player.Character);

                         refreshplayers = true;
                     }
                 }


                 if (refreshplayers)
                 {
                     Players.Clear();
                     foreach (CharacterInfo Player in TeamA)
                     {
                         Players.Add(SEnvir.Players.FirstOrDefault(x => x.Character==Player));
                     }
                     foreach (CharacterInfo Player in TeamB)
                     {
                         Players.Add(SEnvir.Players.FirstOrDefault(x => x.Character == Player));
                     }
                 }
             }*/
        }

        public virtual void EndGame(bool didntstart)
        {

            Ended = true;

            if (didntstart)
            {
                foreach (SConnection con in SEnvir.AuthenticatedConnections)
                    con.ReceiveChat($"The {MGInfo.MiniGame.ToString()} didnt start due to insufficient players", MessageType.Announcement);
            }
            else
            {
                foreach (SConnection con in SEnvir.AuthenticatedConnections)
                    con.ReceiveChat($"The {MGInfo.MiniGame.ToString()} has ended", MessageType.Announcement);
            }

            foreach (PlayerObject player in Players)
            {
                if (MGInfo.TeamGame)
                {
                    player.EventTeam = 0;
                    player.TeamA = false;
                    player.TeamB = false;
                    player.Enqueue(new S.SetTeam { ObjectID = player.ObjectID, team = 0 });
                }
                if (MGInfo.MiniGame == MiniGames.CaptureTheFlag)
                {
                    player.HasFlag = false;
                    player.Enqueue(new S.HasFlag { ObjectID = player.ObjectID, hasFLag = false });
                }
            }


            TTPlayers();

            SEnvir.MiniGames.Remove(this);
            SEnvir.SendMiniGamesUpdate();
        }
        public void SetTeams()
        {
            List<PlayerObject> War = new List<PlayerObject>();
            List<PlayerObject> Wiz = new List<PlayerObject>();
            List<PlayerObject> Tao = new List<PlayerObject>();
            List<PlayerObject> Sin = new List<PlayerObject>();
            List<MirClass> ListClasses = new List<MirClass>();
            int count = SEnvir.Random.Next(5);
            foreach (PlayerObject player in Players)
            {
                switch (player.Class)
                {
                    case MirClass.Warrior:
                        War.Add(player);
                        if (!ListClasses.Contains(MirClass.Warrior))
                            ListClasses.Add(MirClass.Warrior);
                        break;
                    case MirClass.Wizard:
                        Wiz.Add(player);
                        if (!ListClasses.Contains(MirClass.Wizard))
                            ListClasses.Add(MirClass.Wizard);
                        break;
                    case MirClass.Taoist:
                        Tao.Add(player);
                        if (!ListClasses.Contains(MirClass.Taoist))
                            ListClasses.Add(MirClass.Taoist);
                        break;
                    case MirClass.Assassin:
                        Sin.Add(player);
                        if (!ListClasses.Contains(MirClass.Assassin))
                            ListClasses.Add(MirClass.Assassin);
                        break;
                }
            }
            int loop = ListClasses.Count;

            while (loop > 0)
            {
                MirClass addclass = ListClasses[SEnvir.Random.Next(ListClasses.Count)];
                switch (addclass)
                {
                    case MirClass.Warrior:
                        foreach (PlayerObject player in War)
                        {
                            if (count % 2 == 0)
                                TeamA.Add(player.Character);
                            else
                                TeamB.Add(player.Character);
                            player.EventTeam = (count % 2) + 1;
                            count++;
                        }
                        ListClasses.Remove(MirClass.Warrior);
                        break;
                    case MirClass.Wizard:
                        foreach (PlayerObject player in Wiz)
                        {
                            if (count % 2 == 0)
                                TeamA.Add(player.Character);
                            else
                                TeamB.Add(player.Character);
                            player.EventTeam = (count % 2) + 1;
                            count++;
                        }
                        ListClasses.Remove(MirClass.Wizard);
                        break;
                    case MirClass.Taoist:
                        foreach (PlayerObject player in Tao)
                        {
                            if (count % 2 == 0)
                                TeamA.Add(player.Character);
                            else
                                TeamB.Add(player.Character);
                            player.EventTeam = (count % 2) + 1;
                            count++;
                        }
                        ListClasses.Remove(MirClass.Taoist);
                        break;
                    case MirClass.Assassin:
                        foreach (PlayerObject player in Sin)
                        {
                            if (count % 2 == 0)
                                TeamA.Add(player.Character);
                            else
                                TeamB.Add(player.Character);
                            player.EventTeam = (count % 2) + 1;
                            count++;
                        }
                        ListClasses.Remove(MirClass.Assassin);
                        break;
                }
                loop = ListClasses.Count;
            }
            foreach (PlayerObject player in Players)
                player.Enqueue(new S.SetTeam { ObjectID = player.ObjectID, team = player.EventTeam });
        }
        public void PingPlayers()
        {
            if (MGInfo.TeamGame)
            {
                foreach (CharacterInfo chara in TeamA)
                {
                    PlayerObject player = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                    if (player.CurrentMap == LobbyMap)
                        player.Teleport(MGInfo.TeamASpawn);
                }
                foreach (CharacterInfo chara in TeamB)
                {
                    PlayerObject player = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                    if (player.CurrentMap == LobbyMap)
                        player.Teleport(MGInfo.TeamBSpawn);
                }
            }
            else
            {
                foreach (PlayerObject player in Players)
                    player.Teleport(MGInfo.TeamASpawn);
            }
        }
        public void TTPlayers()
        {
            foreach (PlayerObject Player in Players)
            {
                if (Player.CurrentMap == Map || Player.CurrentMap == LobbyMap)
                {
                    if (!Player.Teleport(SEnvir.Maps[Player.Character.BindPoint.BindRegion.Map], Player.Character.BindPoint.ValidBindPoints[SEnvir.Random.Next(Player.Character.BindPoint.ValidBindPoints.Count)]))
                        continue;
                }
                else
                    continue;
            }

        }
        public void MailRewards(List<PlayerObject> winners)
        {
            List<CharacterInfo> chars = new List<CharacterInfo>();

            foreach (PlayerObject player in winners)
            {
                chars.Add(player.Character);
            }
            if (chars != null)
                MailRewards(chars);
        }
        public void MailRewards(List<CharacterInfo> winners)
        {
            if (winners != null)
            {
                UserItem item;
                foreach (CharacterInfo Character in winners)
                {
                    List<UserItem> Items = new List<UserItem>();
                    foreach (RewardInfo info in MGInfo.Rewards)
                    {
                        if (SEnvir.Random.Next(100) <= info.Chance)
                        {
                            int amount = info.Amount;

                            while (amount > 0)
                            {

                                item = SEnvir.CreateFreshItem(info.Item);

                                if (item.Info.Effect == ItemEffect.Gold)
                                    item.Count = amount;
                                else
                                    item.Count = Math.Min(info.Item.StackSize, info.Amount);
                                amount -= (int)item.Count;
                                Items.Add(item);
                            }
                        }
                    }
                    if (Items.Count > 0)
                        MailRewards(Character, Items);
                }
            }
        }
        public void MailRewards(CharacterInfo winners, List<UserItem> items)
        {
            if (winners != null && items != null)
            {
                MailInfo mail = SEnvir.MailInfoList.CreateNewObject();

                mail.Account = winners.Account;
                mail.Sender = "Mini Game Rewards";
                mail.Subject = MGInfo.MiniGame.ToString() + " Rewards";
                mail.Message = MGInfo.MiniGame.ToString() + " level: " + MGInfo.MinLevel + " to " + MGInfo.MaxLevel + " rewards";
                foreach (UserItem item in items)
                {
                    item.Slot = mail.Items.Count;
                    item.Mail = mail;
                    mail.HasItem = true;
                }
                if (winners.Account.Connection?.Player != null)
                    winners.Account.Connection.Enqueue(new S.MailNew
                    {
                        Mail = mail.ToClientInfo(),
                        ObserverPacket = false,
                    });
            }

        }

        public UserArenaPvPStats GetStat(CharacterInfo character)
        {
            UserArenaPvPStats user;

            if (!Stats.TryGetValue(character, out user))
            {
                user = SEnvir.UserArenaPvPStatsList.CreateNewObject();

                user.Character = character;

                user.PvPEventTime = StartTime;

                user.GuildName = character.Account.GuildMember?.Guild.GuildName ?? "No Guild.";
                user.CharacterName = character.CharacterName;
                user.Class = character.Class;
                user.Level = character.Level;
                user.MiniGame = MGInfo.MiniGame;

                Stats[character] = user;
            }

            return user;
        }
    }

    public sealed class CaptureTheFlag : MiniGame
    {
        public CTFFlag FlagA = new CTFFlag();
        public CTFFlag FlagB = new CTFFlag();
        public CTFInfo gameInfo = new CTFInfo();
        public PlayerObject FlagAHolder, FlagBHolder;


        public override void Process()
        {
            base.Process();

            foreach (CharacterInfo chara in TeamA)
            {
                PlayerObject player = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                if (player == null)
                    continue;
                if (player.CurrentMap.Info != MGInfo.MapParameter)
                    continue;

                if (FlagBHolder != null)
                {
                    if (gameInfo.TeamAFlagReturn.PointList.Contains(player.CurrentLocation) && FlagBHolder == player)
                    {
                        EndGame(TeamA);
                    }
                }

            }
            foreach (CharacterInfo chara in TeamB)
            {
                PlayerObject player = SEnvir.Players.FirstOrDefault(x => x.Character == chara);
                if (player == null)
                    continue;
                if (player.CurrentMap.Info != MGInfo.MapParameter)
                    continue;

                if (FlagAHolder != null)
                {
                    if (gameInfo.TeamBFlagReturn.PointList.Contains(player.CurrentLocation) && FlagAHolder == player)
                    {
                        EndGame(TeamB);
                    }
                }

            }

        }
        public void EndGame(List<CharacterInfo> winners)
        {
            if (winners != null)
            {
                MailRewards(winners);
            }
            if (!FlagA.Dead)
            {
                FlagA.EXPOwner = null;
                FlagA.Die();
            }
            if (!FlagB.Dead)
            {
                FlagB.EXPOwner = null;
                FlagB.Die();
            }

            EndGame(false);
        }
        public override void StartGame()
        {
            gameInfo = MGInfo.CTFInfo.FirstOrDefault(x => x.MiniGame == MGInfo);


            if (gameInfo == null)
                return;
            if (gameInfo.FlagMonster == null)
                return;


            FlagA = new CTFFlag
            {
                MonsterInfo = gameInfo.FlagMonster,
                CTFGame = this,
            };

            FlagA.FlagColour = Color.Yellow;
            FlagA.FlagShape = SEnvir.Random.Next(0, 5);
            FlagA.Name = "TeamA";
            FlagA.EventTeam = 1;
            FlagA.Spawn(gameInfo.TeamAFlagSpawn);

            foreach (MapObject MO in Map.Players)
                MO.Broadcast(new S.ObjectFlagColour { ObjectID = FlagA.ObjectID, FlagColour = FlagA.FlagColour, FlagShape = FlagA.FlagShape, name = FlagA.Name });

            FlagB = new CTFFlag
            {
                MonsterInfo = gameInfo.FlagMonster,
                CTFGame = this,
            };

            FlagB.FlagColour = Color.Blue;
            FlagB.FlagShape = SEnvir.Random.Next(6, 10);
            FlagB.Name = "TeamB";
            FlagB.EventTeam = 2;
            FlagB.Spawn(gameInfo.TeamBFlagSpawn);

            foreach (MapObject MO in Map.Players)
                MO.Broadcast(new S.ObjectFlagColour { ObjectID = FlagB.ObjectID, FlagColour = FlagB.FlagColour, FlagShape = FlagB.FlagShape, name = FlagB.Name });


            base.StartGame();
        }
        public void RespawnFlag(int team, Point location)
        {
            if (team == 1)
            {
                FlagA = new CTFFlag
                {
                    MonsterInfo = gameInfo.FlagMonster,
                    CTFGame = this,
                };

                FlagA.FlagColour = Color.Yellow;
                FlagA.FlagShape = SEnvir.Random.Next(0, 5);
                FlagA.Name = "TeamA";
                FlagA.EventTeam = 1;
                FlagA.Spawn(MGInfo.MapParameter, location);

                foreach (MapObject MO in Map.Players)
                    MO.Broadcast(new S.ObjectFlagColour { ObjectID = FlagA.ObjectID, FlagColour = FlagA.FlagColour, FlagShape = FlagA.FlagShape, name = FlagA.Name });

                foreach (PlayerObject player in Players)
                {
                    player.Connection.ReceiveChat($"TeamA Flag has respawned at { location}", MessageType.System);
                }
            }

            if (team == 2)
            {
                FlagB = new CTFFlag
                {
                    MonsterInfo = gameInfo.FlagMonster,
                    CTFGame = this,
                };

                FlagB.FlagColour = Color.Blue;
                FlagB.FlagShape = SEnvir.Random.Next(6, 10);
                FlagB.Name = "TeamB";
                FlagB.EventTeam = 2;
                FlagB.Spawn(MGInfo.MapParameter, location);

                foreach (MapObject MO in Map.Players)
                    MO.Broadcast(new S.ObjectFlagColour { ObjectID = FlagB.ObjectID, FlagColour = FlagB.FlagColour, FlagShape = FlagB.FlagShape, name = FlagB.Name });

                foreach (PlayerObject player in Players)
                {
                    player.Connection.ReceiveChat($"TeamA Flag has respawned at { location}", MessageType.System);
                }
            }
        }
    }
    public sealed class PvPArena : MiniGame
    {
        public int topRewards = 1;
        private List<UserItem> Items = new List<UserItem>();
        private List<UserItem> Items1 = new List<UserItem>();
        private List<UserItem> Items2 = new List<UserItem>();
        private List<UserItem> Items3 = new List<UserItem>();
        public List<CharacterInfo> PlayersDead = new List<CharacterInfo>();
        public List<CharacterInfo> PlayersAlive = new List<CharacterInfo>();
        public DateTime RevivalEnd;

        public override void Process()
        {
            base.Process();

            if (Started)
            {
                if (SEnvir.Now > RevivalEnd && CanRevive)
                {
                    CanRevive = false;

                    foreach (PlayerObject player in Players)
                        player.Connection.ReceiveChat($"You can no longer revive on death, may the best fighters win", MessageType.System);
                }
                if (!MGInfo.TeamGame)
                {
                    if (PlayersAlive.Count == 1)
                    {
                        EndTime = SEnvir.Now;

                        populateRewards();
                        foreach (CharacterInfo chara in PlayersAlive)
                            MailRewards(chara, Items);
                        rewardTopPlayers();
                    }
                }
                else
                {
                    int countA = 0;
                    int countB = 0;
                    foreach (PlayerObject player in Players)
                    {
                        if (!PlayersDead.Contains(player.Character))
                            continue;
                        if (player.EventTeam == 0)
                            countA++;
                        if (player.EventTeam == 1)
                            countB++;

                        if (countA >= TeamA.Count || countB >= TeamB.Count)
                        {
                            EndTime = SEnvir.Now;

                            populateRewards();

                            if (countA >= TeamA.Count)
                            {
                                foreach (CharacterInfo chara in TeamB)
                                    MailRewards(chara, Items);
                            }
                            else
                            {
                                foreach (CharacterInfo chara in TeamB)
                                    MailRewards(chara, Items);
                            }
                            rewardTopPlayers(TeamA);
                            rewardTopPlayers(TeamB);
                        }
                    }
                }
            }

        }
        public void rewardTopPlayers(List<CharacterInfo> team = null)
        {
            int kills1 = 0;
            int kills2 = 0;
            int kills3 = 0;

            List<CharacterInfo> Top1Char = new List<CharacterInfo>();
            List<CharacterInfo> Top2Char = new List<CharacterInfo>();
            List<CharacterInfo> Top3Char = new List<CharacterInfo>();

            foreach (KeyValuePair<CharacterInfo, UserArenaPvPStats> entry in Stats)
            {
                if (MGInfo.TeamGame && !team.Contains(entry.Key))
                    continue;
                int kills = entry.Value.PvPKillCount;
                if (kills > kills1) //new top 1
                {
                    if (Top1Char.Count > 0)
                    {
                        if (Top2Char.Count > 0)
                        {
                            kills3 = kills2;
                            Top3Char.Clear();
                            Top3Char = Top2Char;
                        }
                        kills2 = kills1;
                        Top2Char.Clear();
                        Top2Char = Top1Char;
                    }
                    kills1 = kills;
                    Top1Char.Clear();
                    Top1Char.Add(entry.Key);
                    continue;
                }
                if (kills == kills1) //same as top 1
                {
                    Top1Char.Add(entry.Key);
                    continue;
                }
                if (kills > kills2) // new top 2
                {
                    kills3 = kills2;
                    Top3Char.Clear();
                    Top3Char = Top2Char;

                    kills2 = kills;
                    Top2Char.Clear();
                    Top2Char.Add(entry.Key);
                    continue;
                }
                if (kills == kills2) //same as top 2
                {
                    kills2 = kills;
                    continue;
                }
                if (kills > kills3) //new top3
                {
                    kills3 = kills;
                    Top3Char.Clear();
                    Top3Char.Add(entry.Key);
                    continue;
                }
                if (kills == kills3) //same as top 3
                {
                    kills3 = kills;
                    continue;
                }
            }
            if (Top1Char.Count > 2) //3 or more players so dont need to reward 2 and 3
            {
                Top2Char.Clear();
                Top3Char.Clear();
            }
            if (Top1Char.Count > 1) //2 players so dont need to reward 3rd
                Top3Char.Clear();
            if (Top2Char.Count > 1 || MGInfo.TeamGame) //2 or more in top2 OR teamgame so no reward 3rd
                Top3Char.Clear();

            foreach (CharacterInfo chara in Top1Char)
                MailRewards(chara, Items1);
            foreach (CharacterInfo chara in Top2Char)
                MailRewards(chara, Items2);
            foreach (CharacterInfo chara in Top3Char)
                MailRewards(chara, Items3);
        }
        public void populateRewards()
        {
            UserItem item;
            foreach (RewardInfo info in MGInfo.Rewards)
            {
                int amount = info.Amount;
                if (info.Top1 || info.Top2 || info.Top3)
                {
                    if (SEnvir.Random.Next(100) <= info.Chance)
                    {
                        while (amount > 0)
                        {

                            item = SEnvir.CreateFreshItem(info.Item);

                            if (item.Info.Effect == ItemEffect.Gold)
                                item.Count = amount;
                            else
                                item.Count = Math.Min(info.Item.StackSize, info.Amount);
                            amount -= (int)item.Count;

                            if (info.Top1)
                                Items1.Add(item);
                            if (info.Top2)
                                Items2.Add(item);
                            if (info.Top3)
                                Items3.Add(item);
                        }
                    }
                    continue;
                }

                if (SEnvir.Random.Next(100) <= info.Chance)
                {

                    while (amount > 0)
                    {

                        item = SEnvir.CreateFreshItem(info.Item);

                        if (item.Info.Effect == ItemEffect.Gold)
                            item.Count = amount;
                        else
                            item.Count = Math.Min(info.Item.StackSize, info.Amount);
                        amount -= (int)item.Count;
                        Items.Add(item);
                    }
                }
            }

        }
        public override void StartGame()
        {

            base.StartGame();
            if (Players.Count > MGInfo.MinPlayers * 2)
                topRewards = 2;
            if (Players.Count > MGInfo.MinPlayers * 3)
                topRewards = 3;

            foreach (PlayerObject player in Players)
            {
                PlayersAlive.Add(player.Character);
            }
            RevivalEnd = StartTime.AddSeconds(MGInfo.Duration * 30);
        }
        public override bool ReJoinGame(PlayerObject player)
        {
            if (CanRevive)
            {
                base.ReJoinGame(player);
                if (PlayersDead.Contains(player.Character))
                    PlayersDead.Remove(player.Character);
                if (!PlayersAlive.Contains(player.Character))
                    PlayersAlive.Add(player.Character);
            }
            return false;
        }
    }
}


