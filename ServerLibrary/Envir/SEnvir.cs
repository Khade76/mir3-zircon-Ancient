using Library;
using Library.Network;
using Library.SystemModels;
using MirDB;
using Server.DBModels;
using Server.Models;
using Server.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using C = Library.Network.ClientPackets;
using G = Library.Network.GeneralPackets;
using S = Library.Network.ServerPackets;

namespace Server.Envir
{
    public static class SEnvir
    {
        public static Boolean LogOutGoingPackets = false;

        #region Variables

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // Timers
        private static AccurateTimer mainTickTimer = null;
        private static AccurateTimer newConnectionTimer = null;
        private static AccurateTimer processNotAuthenticatedConnectionsTimer = null;
        private static AccurateTimer mainProcessTimer = null;
        private static AccurateTimer userCountTimer = null;
        private static AccurateTimer mailTimer = null;

        // Concurrent dictionaries
        private static ConcurrentDictionary<int, SConnection> notAuthenticatedConnections = new ConcurrentDictionary<int, SConnection>();
        private static ConcurrentDictionary<int, SConnection> authenticatedConnections = new ConcurrentDictionary<int, SConnection>();
        private static ConcurrentDictionary<int, DateTime> internalNowDictionary = new ConcurrentDictionary<int, DateTime>();
        private static ConcurrentDictionary<uint, MapObject> activeObjectDictionary = new ConcurrentDictionary<uint, MapObject>();
        private static ConcurrentDictionary<int, Random> internalRandomDictionary = new ConcurrentDictionary<int, Random>();
        private static ConcurrentDictionary<string, int> ipCountDictionary = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, int> ipFastCountDictionary = new ConcurrentDictionary<string, int>();

        // TODO : Enclose with methods to acces this.
        public static ConcurrentDictionary<string, DateTime> ipBlockDictionary = new ConcurrentDictionary<string, DateTime>();

        private static ConcurrentQueue<Tuple<MailMessage, int>> mailMessageQueue = new ConcurrentQueue<Tuple<MailMessage, int>>();
        private static ConcurrentQueue<SConnection> newConnectionQueue = new ConcurrentQueue<SConnection>();

        private static TcpListener _listener;
        private static TcpListener _userCountListener;

        private static Boolean refreshTickTimer = false;
        private static int count = 0;
        private static int loopCount = 0;
        private static int lastindex = 0;
        private static long previousTotalSent = 0;
        private static long previousTotalReceived = 0;
        private static long conDelay = 0;
        private static DateTime nextCount;
        private static DateTime UserCountTime;
        private static DateTime saveTime;
        private static DateTime DBTime;
        private static DateTime LastWarTime;
        public static int ProcessObjectCount, LoopCount;

        public static long DBytesSent, DBytesReceived;
        public static long TotalBytesSent, TotalBytesReceived;
        public static long DownloadSpeed, UploadSpeed;
        public static int EMailsSent;
        public static int HGTick = 2;
        public static int HGCap = 5;
        public static bool ServerBuffChanged;
#if DEBUG
        private static DateTime lastSpawn = DateTime.Now;
        private static MapObject previousObject = null;
#endif

        #endregion Variables

        #region Properties

        public static int TotalConnectionCount
        {
            get
            {
                return NotAuthenthicatedConnections.Count + AuthenticatedConnections.Count;
            }
        }

        public static int AuthenthicatedConnectionCount
        {
            get
            {
                return AuthenticatedConnections.Count;
            }
        }

        public static DateTime Now
        {
            get
            {
                DateTime value = DateTime.MinValue;
                internalNowDictionary.TryGetValue(Thread.CurrentThread.ManagedThreadId, out value);
                return value;
            }
            set
            {
                if (!internalNowDictionary.TryAdd(Thread.CurrentThread.ManagedThreadId, value))
                    internalNowDictionary[Thread.CurrentThread.ManagedThreadId] = value;
            }
        }

        public static Random Random
        {
            get
            {
                Random random = null;
                if (!internalRandomDictionary.TryGetValue(Thread.CurrentThread.ManagedThreadId, out random))
                {
                    random = new Random(Thread.CurrentThread.ManagedThreadId);
                    internalRandomDictionary.TryAdd(Thread.CurrentThread.ManagedThreadId, random);
                }

                return random;
            }
        }

        public static bool Started
        {
            get;
            private set;
        }

        public static bool Starting
        {
            get;
            private set;
        }

        public static bool Stopping
        {
            get; set;
        }

        public static bool NetworkStarted
        {
            get; set;
        }

        public static bool WebServerStarted
        {
            get; set;
        }

        public static bool Saving
        {
            get; private set;
        }

        private static Thread EnvironmentThread
        {
            get;
            set;
        }

        public static List<SConnection> AuthenticatedConnections
        {
            get
            {
                return authenticatedConnections.Values.ToList();
            }
        }

        public static List<SConnection> NotAuthenthicatedConnections
        {
            get
            {
                return notAuthenticatedConnections.Values.ToList();
            }
        }

        #endregion Properties

        #region Synchronization

        private static readonly SynchronizationContext Context = SynchronizationContext.Current;
        public static void Send(SendOrPostCallback method)
        {
            Context.Send(method, null);
        }
        public static void Post(SendOrPostCallback method)
        {
            Context.Post(method, null);
        }

        #endregion

        #region Logging

        public static ConcurrentQueue<string> DisplayLogs = new ConcurrentQueue<string>();
        public static ConcurrentQueue<string> DisplayGMLogs = new ConcurrentQueue<string>();
        private static ConcurrentQueue<string> DisplayChatLogs = new ConcurrentQueue<string>();

        public static void Log(string log, bool hardLog = true)
        {
            if (DisplayLogs.Count < 100)
                DisplayLogs.Enqueue(string.Format("[{0:F}]: {1}", Time.Now, log));

            logger.Info(log);
        }

        public static void LogError(String message, Exception ex, bool hardLog = true)
        {
            string log = string.Format("[{0:F}]: {1}\r\n{2}", Time.Now, ex.Message, ex.StackTrace);

            if (DisplayLogs.Count < 100)
                DisplayLogs.Enqueue(message + Environment.NewLine + log);

            logger.Error(ex, message);
        }

        public static void LogError(Exception ex, bool hardLog = true)
        {
            if (ex == null)
            {
                LogError(new Exception("Exception was empty!"));
                return;
            }

            string log = string.Format("[{0:F}]: {1}\r\n{2}", Time.Now, ex.Message, ex.StackTrace);

            if (DisplayLogs.Count < 100)
                DisplayLogs.Enqueue(log);

            logger.Error(ex);
        }
        public static void LogError(string log, bool hardLog = true)
        {
            if (DisplayLogs.Count < 100)
                DisplayLogs.Enqueue(string.Format("[{0:F}]: {1}", Time.Now, log));

            logger.Error(log);
        }


        public static void LogGM(string log, bool hardLog = true)
        {
            if (DisplayGMLogs.Count < 100)
                DisplayGMLogs.Enqueue(string.Format("[{0:F}]: {1}", Time.Now, log));

            logger.Debug(log);
        }

        public static void LogChat(string log)
        {
            if (DisplayChatLogs.Count < 500)
                DisplayChatLogs.Enqueue(string.Format("[{0:F}]: {1}", Time.Now, log));

            logger.Warn(log);
        }
        #endregion

        #region Network

        private static void StartNetwork(bool log = true)
        {
            try
            {
                newConnectionQueue = new ConcurrentQueue<SConnection>();

                _listener = new TcpListener(IPAddress.Parse(Config.IPAddress), Config.Port);
                _listener.Start();
                _listener.BeginAcceptTcpClient(Connection, null);

                try
                {

                    _userCountListener = new TcpListener(IPAddress.Parse(Config.IPAddress), Config.UserCountPort);
                    _userCountListener.Start();
                    _userCountListener.BeginAcceptTcpClient(CountConnection, null);
                }
                catch (Exception ex)
                {
                    Log("User count port could not be opened.");
                    LogError(ex);
                }

                NetworkStarted = true;
                if (log)
                    Log("Network Started.");
            }
            catch (Exception ex)
            {
                // Stop the init
                Stopping = true;
                LogError(ex);
            }
        }

        private static void StopNetwork(bool log = true)
        {
            TcpListener expiredListener = _listener;
            TcpListener expiredUserListener = _userCountListener;

            _listener = null;
            _userCountListener = null;

            Started = false;

            expiredListener?.Stop();
            expiredUserListener?.Stop();

            newConnectionQueue = null;

            try
            {
                Packet p = new G.Disconnect { Reason = DisconnectReason.ServerClosing };

                for (int i = AuthenticatedConnections.Count - 1; i >= 0; i--)
                    AuthenticatedConnections[i].SendDisconnect(p);

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            if (log)
                Log("Network Stopped.");
        }

        private static void Connection(IAsyncResult result)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (_listener == null || !_listener.Server.IsBound || newConnectionQueue == null)
                    return;

                bool queueConnection = true;
                TcpClient client = _listener.EndAcceptTcpClient(result);
                string ipAddress = client.Client.RemoteEndPoint.ToString().Split(':')[0];

                // Check if the user has a ban
                if (!ipBlockDictionary.TryGetValue(ipAddress, out DateTime banDate) || banDate < now)
                {
                    SConnection newConnection = new SConnection(client);

                    if (newConnection.Connected)
                    {
                        DateTime lastConnection = newConnection.LastConnectionTime;
                        newConnection.LastConnectionTime = DateTime.Now;

                        // Remove 5s for the trigger time.
                        DateTime triggerTime = now.AddSeconds(-5);

                        int connectionCount = 1;

                        if (ipFastCountDictionary.TryGetValue(ipAddress, out connectionCount) && lastConnection >= triggerTime)
                            connectionCount++;
                        else // Reset the counter
                            connectionCount = 1;

                        // Check if add works, otherwise try to update the count
                        if (!ipFastCountDictionary.TryAdd(ipAddress, connectionCount))
                            ipFastCountDictionary[ipAddress] = connectionCount;

                        // If we had more or equal to trigger amount 
                        if (connectionCount >= Config.MaxFastConnection)
                        {
                            try
                            {
                                if (!ipBlockDictionary.TryAdd(ipAddress, Now.Add(Config.PacketBanTime)))
                                    ipBlockDictionary[ipAddress] = Now.Add(Config.PacketBanTime);

                                RemoveConnectionsForIP(ipAddress, true);
                            }
                            finally
                            {
                                queueConnection = false;
                                SEnvir.Log($"{ipAddress} Disconnected, Large amount of Connections");
                                newConnection.TryDisconnect();
                            }
                        }

                        if (queueConnection)
                            newConnectionQueue.Enqueue(newConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                while (newConnectionQueue.Count >= 15)
                    Thread.Sleep(1);

                if (_listener != null && _listener.Server.IsBound)
                    _listener.BeginAcceptTcpClient(Connection, null);
            }
        }

        private static void CountConnection(IAsyncResult result)
        {
            try
            {
                if (_userCountListener == null || !_userCountListener.Server.IsBound)
                    return;

                TcpClient client = _userCountListener.EndAcceptTcpClient(result);

                byte[] data = Encoding.ASCII.GetBytes(string.Format("c;/Zircon/{0}/;", AuthenticatedConnections.Count));

                client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, CountConnectionEnd, client);
            }
            catch { }
            finally
            {
                if (_userCountListener != null && _userCountListener.Server.IsBound)
                    _userCountListener.BeginAcceptTcpClient(CountConnection, null);
            }
        }

        private static void CountConnectionEnd(IAsyncResult result)
        {
            try
            {
                TcpClient client = result.AsyncState as TcpClient;

                if (client == null)
                    return;

                client.Client.EndSend(result);

                client.Client.Dispose();
            }
            catch { }
        }

        public static void RemoveConnectionsForIP(String ip, bool disconnect = false)
        {
            List<SConnection> connections = AuthenticatedConnections.Where(x => x.IPAddress.Equals(ip, StringComparison.OrdinalIgnoreCase)).ToList();
            connections.ForEach(x => RemoveConnection(x.SessionID));

            if (disconnect)
                connections.ForEach(x => x.TryDisconnect());
        }

        #endregion

        #region WebServer
        private static HttpListener WebListener;
        private const string ActivationCommand = "Activation", ResetCommand = "Reset", DeleteCommand = "Delete";
        private const string ActivationKey = "ActivationKey", ResetKey = "ResetKey", DeleteKey = "DeleteKey";

        private const string Completed = "Completed";
        private const string Currency = "GBP";

        private static Dictionary<decimal, int> GoldTable = new Dictionary<decimal, int>
        {
            [5M] = 500,
            [10M] = 1100,
            [15M] = 1750,
            [25M] = 3000,
            [35M] = 4375,
            [50M] = 6500,
            [100M] = 14000,
        };

        public const string VerifiedPath = @".\Database\Store\Verified\",
            InvalidPath = @".\Database\Store\Invalid\",
            CompletePath = @".\Database\Store\Complete\";

        private static HttpListener BuyListener, IPNListener;
        public static ConcurrentQueue<IPNMessage> Messages = new ConcurrentQueue<IPNMessage>();
        public static List<IPNMessage> PaymentList = new List<IPNMessage>(), HandledPayments = new List<IPNMessage>();

        public static void StartWebServer(bool log = true)
        {
            try
            {
                WebCommandQueue = new ConcurrentQueue<WebCommand>();

                WebListener = new HttpListener();
                WebListener.Prefixes.Add(Config.WebPrefix);

                WebListener.Start();
                WebListener.BeginGetContext(WebConnection, null);

                BuyListener = new HttpListener();
                BuyListener.Prefixes.Add(Config.BuyPrefix);

                IPNListener = new HttpListener();
                IPNListener.Prefixes.Add(Config.IPNPrefix);

                BuyListener.Start();
                BuyListener.BeginGetContext(BuyConnection, null);

                IPNListener.Start();
                IPNListener.BeginGetContext(IPNConnection, null);



                WebServerStarted = true;

                if (log)
                    Log("Web Server Started.");
            }
            catch (Exception ex)
            {
                WebServerStarted = false;
                LogError(ex);

                if (WebListener != null && WebListener.IsListening)
                    WebListener?.Stop();
                WebListener = null;

                if (BuyListener != null && BuyListener.IsListening)
                    BuyListener?.Stop();
                BuyListener = null;

                if (IPNListener != null && IPNListener.IsListening)
                    IPNListener?.Stop();
                IPNListener = null;
            }
        }

        public static void StopWebServer(bool log = true)
        {
            HttpListener expiredWebListener = WebListener;
            WebListener = null;

            HttpListener expiredBuyListener = BuyListener;
            BuyListener = null;
            HttpListener expiredIPNListener = IPNListener;
            IPNListener = null;


            WebServerStarted = false;
            expiredWebListener?.Stop();
            expiredBuyListener?.Stop();
            expiredIPNListener?.Stop();

            if (log)
                Log("Web Server Stopped.");
        }

        public static void PreLoadMaps()
        {
            // Set the timestamp to ensure it stays the same although it's in a different thread
            DateTime localNow = Now;
            DateTime start = DateTime.Now;

            Log("Pre process maps");
            Parallel.ForEach(Maps.Values, map =>
            {
                try
                {
                    Now = localNow;
                    map.Process();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });

            Parallel.ForEach(MapInstance, map =>
            {
                try
                {
                    Now = localNow;
                    map.Process();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });

#if DEBUG
            Log("Pre process spawns");
            Parallel.ForEach(Spawns, spawn =>
            {
                try
                {
                    Now = localNow;
                    spawn.DoSpawn(false);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });
#else
            Log("Pre process spawns");
            Parallel.ForEach(Spawns, spawn =>
            {
                try
                {
                    Now = localNow;
                    spawn.DoSpawn(false);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });
#endif

            Log("Pre process conquestwars");
            Parallel.ForEach(ConquestWars, war =>
            {
                try
                {
                    Now = localNow;
                    war.Process();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });

            Log("Pre process minigames");
            Parallel.ForEach(MiniGames, miniGame =>
            {
                try
                {
                    Now = localNow;
                    miniGame.Process();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });

            TimeSpan elapsed = DateTime.Now - start;
            Log(String.Format(
                "Maps preloaded loaded in: {0}:{1}.{2}",

                elapsed.Minutes.ToString().PadLeft(2, '0'),
                elapsed.Seconds.ToString().PadLeft(2, '0'),
                elapsed.Milliseconds.ToString().PadLeft(4, '0')
            ));
        }

        private static void WebConnection(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = WebListener.EndGetContext(result);

                string command = context.Request.QueryString["Type"];

                switch (command)
                {
                    case ActivationCommand:
                        Activation(context);
                        break;
                    case ResetCommand:
                        ResetPassword(context);
                        break;
                    case DeleteCommand:
                        DeleteAccount(context);
                        break;
                }
            }
            catch { }
            finally
            {
                if (WebListener != null && WebListener.IsListening)
                    WebListener.BeginGetContext(WebConnection, null);
            }
        }

        private static void Activation(HttpListenerContext context)
        {
            string key = context.Request.QueryString[ActivationKey];

            if (string.IsNullOrEmpty(key))
                return;

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
            {
                AccountInfo temp = AccountInfoList[i]; //Different Threads, Caution must be taken to prevent errors
                if (string.Compare(temp.ActivationKey, key, StringComparison.Ordinal) != 0)
                    continue;

                account = temp;
                break;
            }

            if (Config.AllowWebActivation && account != null)
            {
                WebCommandQueue.Enqueue(new WebCommand(CommandType.Activation, account));
                context.Response.Redirect(Config.ActivationSuccessLink);
            }
            else
                context.Response.Redirect(Config.ActivationFailLink);

            context.Response.Close();
        }

        private static void ResetPassword(HttpListenerContext context)
        {
            string key = context.Request.QueryString[ResetKey];

            if (string.IsNullOrEmpty(key))
                return;

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
            {
                AccountInfo temp = AccountInfoList[i]; //Different Threads, Caution must be taken to prevent errors
                if (string.Compare(temp.ResetKey, key, StringComparison.Ordinal) != 0)
                    continue;

                account = temp;
                break;
            }

            if (Config.AllowWebResetPassword && account != null && account.ResetTime.AddMinutes(25) > Now)
            {
                WebCommandQueue.Enqueue(new WebCommand(CommandType.PasswordReset, account));
                context.Response.Redirect(Config.ResetSuccessLink);
            }
            else
                context.Response.Redirect(Config.ResetFailLink);

            context.Response.Close();
        }

        private static void DeleteAccount(HttpListenerContext context)
        {
            string key = context.Request.QueryString[DeleteKey];

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
            {
                AccountInfo temp = AccountInfoList[i]; //Different Threads, Caution must be taken to prevent errors
                if (string.Compare(temp.ActivationKey, key, StringComparison.Ordinal) != 0)
                    continue;

                account = temp;
                break;
            }

            if (Config.AllowDeleteAccount && account != null)
            {
                WebCommandQueue.Enqueue(new WebCommand(CommandType.AccountDelete, account));
                context.Response.Redirect(Config.DeleteSuccessLink);
            }
            else
                context.Response.Redirect(Config.DeleteFailLink);

            context.Response.Close();
        }

        private static void BuyConnection(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = BuyListener.EndGetContext(result);

                string characterName = context.Request.QueryString["Character"];

                CharacterInfo character = null;
                for (int i = 0; i < CharacterInfoList.Count; i++)
                {
                    if (string.Compare(CharacterInfoList[i].CharacterName, characterName, StringComparison.OrdinalIgnoreCase) != 0)
                        continue;

                    character = CharacterInfoList[i];
                    break;
                }

                if (character?.Account.Key != context.Request.QueryString["Key"])
                    character = null;

                string response = character == null ? Properties.Resources.CharacterNotFound : Properties.Resources.BuyGameGold.Replace("$CHARACTERNAME$", character.CharacterName);

                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, context.Request.ContentEncoding))
                    writer.Write(response);
            }
            catch { }
            finally
            {
                if (BuyListener != null && BuyListener.IsListening) //IsBound ?
                    BuyListener.BeginGetContext(BuyConnection, null);
            }

        }
        private static void IPNConnection(IAsyncResult result)
        {
            const string LiveURL = @"https://ipnpb.paypal.com/cgi-bin/webscr";

            const string verified = "VERIFIED";

            try
            {
                if (IPNListener == null || !IPNListener.IsListening)
                    return;

                HttpListenerContext context = IPNListener.EndGetContext(result);

                string rawMessage;
                using (StreamReader readStream = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                    rawMessage = readStream.ReadToEnd();


                Task.Run(() =>
                {
                    string data = "cmd=_notify-validate&" + rawMessage;

                    HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(LiveURL);

                    wRequest.Method = "POST";
                    wRequest.ContentType = "application/x-www-form-urlencoded";
                    wRequest.ContentLength = data.Length;

                    using (StreamWriter writer = new StreamWriter(wRequest.GetRequestStream(), Encoding.ASCII))
                        writer.Write(data);

                    using (StreamReader reader = new StreamReader(wRequest.GetResponse().GetResponseStream()))
                    {
                        IPNMessage message = new IPNMessage { Message = rawMessage, Verified = reader.ReadToEnd() == verified };


                        if (!Directory.Exists(VerifiedPath))
                            Directory.CreateDirectory(VerifiedPath);

                        if (!Directory.Exists(InvalidPath))
                            Directory.CreateDirectory(InvalidPath);

                        string path = (message.Verified ? VerifiedPath : InvalidPath) + Path.GetRandomFileName();

                        File.WriteAllText(path, message.Message);

                        message.FileName = path;


                        Messages.Enqueue(message);
                    }
                });

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.Close();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                if (IPNListener != null && IPNListener.IsListening) //IsBound ?
                    IPNListener.BeginGetContext(IPNConnection, null);
            }
        }
        #endregion

        #region Database

        private static Session Session;

        public static DBCollection<MapInfo> MapInfoList;
        public static DBCollection<SafeZoneInfo> SafeZoneInfoList;
        public static DBCollection<ItemInfo> ItemInfoList;
        public static DBCollection<RespawnInfo> RespawnInfoList;
        public static DBCollection<MagicInfo> MagicInfoList;
        public static DBCollection<QuestInfo> QuestInfoList;

        public static DBCollection<AccountInfo> AccountInfoList;
        public static DBCollection<CharacterInfo> CharacterInfoList;
        public static DBCollection<CharacterBeltLink> BeltLinkList;
        public static DBCollection<AutoPotionLink> AutoPotionLinkList;
        public static DBCollection<UserItem> UserItemList;
        public static DBCollection<RefineInfo> RefineInfoList;
        public static DBCollection<UserItemStat> UserItemStatsList;
        public static DBCollection<UserMagic> UserMagicList;
        public static DBCollection<BuffInfo> BuffInfoList;
        public static DBCollection<MonsterInfo> MonsterInfoList;
        public static DBCollection<SetInfo> SetInfoList;
        public static DBCollection<AuctionInfo> AuctionInfoList;
        public static DBCollection<MailInfo> MailInfoList;
        public static DBCollection<AuctionHistoryInfo> AuctionHistoryInfoList;
        public static DBCollection<UserDrop> UserDropList;
        public static DBCollection<StoreInfo> StoreInfoList;
        public static DBCollection<FamePointInfo> FamePointInfoList;
        public static DBCollection<BaseStat> BaseStatList;
        public static DBCollection<MovementInfo> MovementInfoList;
        public static DBCollection<NPCInfo> NPCInfoList;
        public static DBCollection<MapRegion> MapRegionList;
        public static DBCollection<GuildInfo> GuildInfoList;
        public static DBCollection<GuildMemberInfo> GuildMemberInfoList;
        public static DBCollection<UserQuest> UserQuestList;
        public static DBCollection<UserQuestTask> UserQuestTaskList;
        public static DBCollection<CompanionInfo> CompanionInfoList;
        public static DBCollection<CompanionLevelInfo> CompanionLevelInfoList;
        public static DBCollection<UserCompanion> UserCompanionList;
        public static DBCollection<UserCompanionUnlock> UserCompanionUnlockList;
        public static DBCollection<CompanionSkillInfo> CompanionSkillInfoList;
        public static DBCollection<BlockInfo> BlockInfoList;
        public static DBCollection<CastleInfo> CastleInfoList;
        public static DBCollection<UserConquest> UserConquestList;
        public static DBCollection<GameGoldPayment> GameGoldPaymentList;
        public static DBCollection<GameStoreSale> GameStoreSaleList;
        public static DBCollection<GuildWarInfo> GuildWarInfoList;
        public static DBCollection<UserConquestStats> UserConquestStatsList;
        public static DBCollection<UserFortuneInfo> UserFortuneInfoList;
        public static DBCollection<WeaponCraftStatInfo> WeaponCraftStatInfoList;
        public static DBCollection<UserCrafting> UserCraftInfoList;
        public static DBCollection<CraftLevelInfo> CraftingLevelsInfoList;
        public static DBCollection<CraftItemInfo> CraftingItemInfoList;
        public static DBCollection<MiniGameInfo> MiniGameInfoList;
        public static DBCollection<UserArenaPvPStats> UserArenaPvPStatsList;
        public static DBCollection<DropListInfo> DropListInfoList;
        public static DBCollection<HorseInfo> HorseInfoList;
        public static DBCollection<UserHorse> UserHorseList;
        public static DBCollection<UserHorseUnlock> UserHorseUnlockList;

        public static ItemInfo GoldInfo, RefinementStoneInfo, FragmentInfo, Fragment2Info, Fragment3Info, FortuneCheckerInfo, ItemPartInfo;

        public static GuildInfo StarterGuild;

        public static MapRegion MysteryShipMapRegion, LairMapRegion, SeaCaveMapRegion, FlowerMapRegion;

        public static List<MonsterInfo> BossList = new List<MonsterInfo>();
        public static List<Map> MapInstance = new List<Map>();

        #endregion

        #region Game Variables

        public static ConcurrentQueue<WebCommand> WebCommandQueue;

        public static Dictionary<MapInfo, Map> Maps = new Dictionary<MapInfo, Map>();

        private static long _ObjectID;
        public static uint ObjectID => (uint)Interlocked.Increment(ref _ObjectID);

        private static ConcurrentDictionary<uint, MapObject> objectDictionary = new ConcurrentDictionary<uint, MapObject>();

        public static List<PlayerObject> Players = new List<PlayerObject>();
        public static List<ConquestWar> ConquestWars = new List<ConquestWar>();
        public static List<MiniGame> MiniGames = new List<MiniGame>();

        public static List<SpawnInfo> Spawns = new List<SpawnInfo>();

        private static float _DayTime;
        public static float DayTime
        {
            get
            {
                return _DayTime;
            }
            set
            {
                if (_DayTime == value)
                    return;

                _DayTime = value;

                Broadcast(new S.DayChanged { DayTime = DayTime });
            }
        }

        public static LinkedList<CharacterInfo> Rankings;
        public static HashSet<CharacterInfo> TopRankings;

        public static long ConDelay, SaveDelay;
        #endregion

        public static void StartServer()
        {
            if (Started)
                return;

            Starting = true;
            InitializeServer();
        }

        private static void LoadDatabase()
        {
            Session = new Session(SessionMode.Users)
            {
                BackUpDelay = 60,
            };

            MapInfoList = Session.GetCollection<MapInfo>();
            SafeZoneInfoList = Session.GetCollection<SafeZoneInfo>();
            ItemInfoList = Session.GetCollection<ItemInfo>();
            MonsterInfoList = Session.GetCollection<MonsterInfo>();
            RespawnInfoList = Session.GetCollection<RespawnInfo>();
            MagicInfoList = Session.GetCollection<MagicInfo>();
            QuestInfoList = Session.GetCollection<QuestInfo>();

            AccountInfoList = Session.GetCollection<AccountInfo>();
            CharacterInfoList = Session.GetCollection<CharacterInfo>();
            BeltLinkList = Session.GetCollection<CharacterBeltLink>();
            AutoPotionLinkList = Session.GetCollection<AutoPotionLink>();
            UserItemList = Session.GetCollection<UserItem>();
            UserItemStatsList = Session.GetCollection<UserItemStat>();
            RefineInfoList = Session.GetCollection<RefineInfo>();
            UserMagicList = Session.GetCollection<UserMagic>();
            BuffInfoList = Session.GetCollection<BuffInfo>();
            SetInfoList = Session.GetCollection<SetInfo>();
            AuctionInfoList = Session.GetCollection<AuctionInfo>();
            MailInfoList = Session.GetCollection<MailInfo>();
            AuctionHistoryInfoList = Session.GetCollection<AuctionHistoryInfo>();
            UserDropList = Session.GetCollection<UserDrop>();
            StoreInfoList = Session.GetCollection<StoreInfo>();
            FamePointInfoList = Session.GetCollection<FamePointInfo>();
            BaseStatList = Session.GetCollection<BaseStat>();
            MovementInfoList = Session.GetCollection<MovementInfo>();
            NPCInfoList = Session.GetCollection<NPCInfo>();
            MapRegionList = Session.GetCollection<MapRegion>();
            GuildInfoList = Session.GetCollection<GuildInfo>();
            GuildMemberInfoList = Session.GetCollection<GuildMemberInfo>();
            UserQuestList = Session.GetCollection<UserQuest>();
            UserQuestTaskList = Session.GetCollection<UserQuestTask>();
            CompanionSkillInfoList = Session.GetCollection<CompanionSkillInfo>();

            CompanionInfoList = Session.GetCollection<CompanionInfo>();
            CompanionLevelInfoList = Session.GetCollection<CompanionLevelInfo>();
            UserCompanionList = Session.GetCollection<UserCompanion>();
            UserCompanionUnlockList = Session.GetCollection<UserCompanionUnlock>();
            BlockInfoList = Session.GetCollection<BlockInfo>();
            CastleInfoList = Session.GetCollection<CastleInfo>();
            UserConquestList = Session.GetCollection<UserConquest>();
            GameGoldPaymentList = Session.GetCollection<GameGoldPayment>();
            GameStoreSaleList = Session.GetCollection<GameStoreSale>();
            GuildWarInfoList = Session.GetCollection<GuildWarInfo>();
            UserConquestStatsList = Session.GetCollection<UserConquestStats>();
            UserFortuneInfoList = Session.GetCollection<UserFortuneInfo>();
            WeaponCraftStatInfoList = Session.GetCollection<WeaponCraftStatInfo>();
            UserCraftInfoList = Session.GetCollection<UserCrafting>();
            CraftingLevelsInfoList = Session.GetCollection<CraftLevelInfo>();
            CraftingItemInfoList = Session.GetCollection<CraftItemInfo>();
            MiniGameInfoList = Session.GetCollection<MiniGameInfo>();
            UserArenaPvPStatsList = Session.GetCollection<UserArenaPvPStats>();
            DropListInfoList = Session.GetCollection<DropListInfo>();
            HorseInfoList = Session.GetCollection<HorseInfo>();
            UserHorseList = Session.GetCollection<UserHorse>();
            UserHorseUnlockList = Session.GetCollection<UserHorseUnlock>();

            GoldInfo = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.Gold);
            RefinementStoneInfo = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.RefinementStone);
            FragmentInfo = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.Fragment1);
            Fragment2Info = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.Fragment2);
            Fragment3Info = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.Fragment3);

            ItemPartInfo = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.ItemPart);
            FortuneCheckerInfo = ItemInfoList.Binding.First(x => x.Effect == ItemEffect.FortuneChecker);


            MysteryShipMapRegion = MapRegionList.Binding.FirstOrDefault(x => x.Index == Config.MysteryShipRegionIndex);
            LairMapRegion = MapRegionList.Binding.FirstOrDefault(x => x.Index == Config.LairRegionIndex);
            StarterGuild = GuildInfoList.Binding.FirstOrDefault(x => x.StarterGuild);
            SeaCaveMapRegion = MapRegionList.Binding.FirstOrDefault(x => x.Index == Config.SeaCaveRegionIndex);
            FlowerMapRegion = MapRegionList.Binding.FirstOrDefault(x => x.Index == Config.FlowerMapRegionIndex);


            if (StarterGuild == null)
            {
                StarterGuild = GuildInfoList.CreateNewObject();
                StarterGuild.StarterGuild = true;
            }

            StarterGuild.GuildName = Config.StarterGuildName;

            #region Create Ranks
            Rankings = new LinkedList<CharacterInfo>();
            TopRankings = new HashSet<CharacterInfo>();
            foreach (CharacterInfo info in CharacterInfoList.Binding)
            {
                if (info.Account.Admin || info.Account.Admin1 || info.Account.Admin2 || info.Account.Developer) continue;
                info.RankingNode = Rankings.AddLast(info);
                RankingSort(info, false);
            }
            UpdateLead();
            #endregion

            for (int i = UserQuestList.Count - 1; i >= 0; i--)
                if (UserQuestList[i].QuestInfo == null)
                    UserQuestList[i].Delete();

            for (int i = UserQuestTaskList.Count - 1; i >= 0; i--)
                if (UserQuestTaskList[i].Task == null)
                    UserQuestTaskList[i].Delete();

            foreach (MonsterInfo monster in MonsterInfoList.Binding)
            {
                if (!monster.IsBoss)
                    continue;

                BossList.Add(monster);

            }

            Messages = new ConcurrentQueue<IPNMessage>();

            PaymentList.Clear();

            if (Directory.Exists(VerifiedPath))
            {
                string[] files = Directory.GetFiles(VerifiedPath);

                foreach (string file in files)
                    Messages.Enqueue(new IPNMessage { FileName = file, Message = File.ReadAllText(file), Verified = true });
            }

        }
        //Only works on Increasing EXP, still need to do Rebirth or loss of exp ranking update.
        public static void RankingSort(CharacterInfo character, bool updateLead = true)
        {
            if (character.Account.Admin || character.Account.Admin1 || character.Account.Admin2 || character.Account.Developer) return;
            bool changed = false;

            LinkedListNode<CharacterInfo> node;
            while ((node = character.RankingNode.Previous) != null)
            {
                if (node.Value.Level > character.Level)
                    break;
                if (node.Value.Level == character.Level && node.Value.Experience >= character.Experience)
                    break;

                changed = true;

                Rankings.Remove(character.RankingNode);
                Rankings.AddBefore(node, character.RankingNode);
            }

            if (!updateLead || (TopRankings.Count >= 20 && !changed))
                return; //5 * 4

            UpdateLead();
        }


        public static void UpdateLead()
        {
            HashSet<CharacterInfo> newTopRankings = new HashSet<CharacterInfo>();

            int war = 5, wiz = 5, tao = 5, ass = 5;

            foreach (CharacterInfo cInfo in Rankings)
            {
                if (cInfo.Account.Admin)
                    continue;

                switch (cInfo.Class)
                {
                    case MirClass.Warrior:
                        if (war == 0)
                            continue;
                        war--;
                        newTopRankings.Add(cInfo);
                        break;
                    case MirClass.Wizard:
                        if (wiz == 0)
                            continue;
                        wiz--;
                        newTopRankings.Add(cInfo);
                        break;
                    case MirClass.Taoist:
                        if (tao == 0)
                            continue;
                        tao--;
                        newTopRankings.Add(cInfo);
                        break;
                    case MirClass.Assassin:
                        if (ass == 0)
                            continue;
                        ass--;
                        newTopRankings.Add(cInfo);
                        break;
                }

                if (war == 0 && wiz == 0 && tao == 0 && ass == 0)
                    break;
            }

            foreach (CharacterInfo info in TopRankings)
            {
                if (newTopRankings.Contains(info))
                    continue;

                info.Player?.BuffRemove(BuffType.Ranking);
            }

            foreach (CharacterInfo info in newTopRankings)
            {
                if (TopRankings.Contains(info))
                    continue;

                info.Player?.BuffAdd(BuffType.Ranking, TimeSpan.MaxValue, null, true, false, TimeSpan.Zero);
            }

            TopRankings = newTopRankings;
        }

        private static void LoadEnvironment()
        {
            if (EnvironmentThread == null || !EnvironmentThread.IsAlive)
            {
                EnvironmentThread = new Thread(InternalLoadEnvironment) { IsBackground = true };
                EnvironmentThread.Start();
            }
        }

        private static void InternalLoadEnvironment()
        {
            DateTime start = DateTime.Now;
            Log("Loading Environment");
            LoadDatabase();

            #region Load Files
            for (int i = 0; i < MapInfoList.Count; i++)
                Maps[MapInfoList[i]] = new Map(MapInfoList[i]);


            Log("Loading maps");
            Parallel.ForEach(Maps, x => x.Value.Load());

            #endregion

            foreach (Map map in Maps.Values)
                map.Setup();

            Parallel.ForEach(MapRegionList.Binding, x =>
            {
                Map map = GetMap(x.Map);

                if (map == null)
                    return;

                x.CreatePoints(map.Width);
            });
            Log("Creating safe zones");
            CreateSafeZones();

            Log("Creating Movements");
            CreateMovements();

            Log("Creating NPCS");
            CreateNPCs();

            Log("Creating spawns");
            CreateSpawns();

            TimeSpan elapsed = DateTime.Now - start;
            Log(String.Format(
                "Environment loaded in: {0}:{1}.{2}",

                elapsed.Minutes.ToString().PadLeft(2, '0'),
                elapsed.Seconds.ToString().PadLeft(2, '0'),
                elapsed.Milliseconds.ToString().PadLeft(4, '0')
            ));

            InitMainTickLoop();
        }

        private static void CreateMovements()
        {
            foreach (MovementInfo movement in MovementInfoList.Binding)
            {
                if (movement.SourceRegion == null)
                    continue;

                Map sourceMap = GetMap(movement.SourceRegion.Map);
                if (sourceMap == null)
                {
                    Log($"[Movement] Bad Source Map, Source: {movement.SourceRegion.ServerDescription}");
                    continue;
                }

                if (movement.DestinationRegion == null)
                {
                    Log($"[Movement] No Destinaton Region, Source: {movement.SourceRegion.ServerDescription}");
                    continue;
                }

                Map destMap = GetMap(movement.DestinationRegion.Map);
                if (destMap == null)
                {
                    Log($"[Movement] Bad Destinatoin Map, Destination: {movement.DestinationRegion.ServerDescription}");
                    continue;
                }


                foreach (Point sPoint in movement.SourceRegion.PointList)
                {
                    Cell source = sourceMap.GetCell(sPoint);

                    if (source == null)
                    {
                        Log($"[Movement] Bad Origin, Source: {movement.SourceRegion.ServerDescription}, X:{sPoint.X}, Y:{sPoint.Y}");
                        continue;
                    }

                    if (source.Movements == null)
                        source.Movements = new List<MovementInfo>();

                    source.Movements.Add(movement);
                }
            }
        }

        private static void CreateNPCs()
        {
            foreach (NPCInfo info in NPCInfoList.Binding)
            {
                if (info.Region == null)
                    continue;

                Map map = GetMap(info.Region.Map);

                if (map == null)
                {
                    Log(string.Format("[NPC] Bad Map, NPC: {0}, Map: {1}", info.NPCName, info.Region.ServerDescription));
                    continue;
                }

                NPCObject ob = new NPCObject
                {
                    NPCInfo = info,
                };

                if (!ob.Spawn(info.Region))
                    Log($"[NPC] Failed to spawn NPC, Region: {info.Region.ServerDescription}, NPC: {info.NPCName}");
            }
        }

        private static void CreateSafeZones()
        {
            foreach (SafeZoneInfo info in SafeZoneInfoList.Binding)
            {
                if (info.Region == null)
                    continue;

                Map map = GetMap(info.Region.Map);

                if (map == null)
                {
                    Log($"[Safe Zone] Bad Map, Map: {info.Region.ServerDescription}");
                    continue;
                }

                HashSet<Point> edges = new HashSet<Point>();

                foreach (Point point in info.Region.PointList)
                {
                    Cell cell = map.GetCell(point);

                    if (cell == null)
                    {
                        Log($"[Safe Zone] Bad Location, Region: {info.Region.ServerDescription}, X: {point.X}, Y: {point.Y}.");

                        continue;
                    }

                    cell.SafeZone = info;

                    for (int i = 0; i < 8; i++)
                    {
                        Point test = Functions.Move(point, (MirDirection)i);

                        if (info.Region.PointList.Contains(test))
                            continue;

                        if (map.GetCell(test) == null)
                            continue;

                        edges.Add(test);
                    }
                }

                map.HasSafeZone = true;

                foreach (Point point in edges)
                {
                    SpellObject ob = new SpellObject
                    {
                        Visible = true,
                        DisplayLocation = point,
                        TickCount = 10,
                        TickFrequency = TimeSpan.FromDays(365),
                        Effect = SpellEffect.SafeZone
                    };

                    ob.Spawn(map.Info, point);
                }

                if (info.BindRegion == null)
                    continue;

                map = GetMap(info.BindRegion.Map);

                if (map == null)
                {
                    Log($"[Safe Zone] Bad Bind Map, Map: {info.Region.ServerDescription}");
                    continue;
                }

                foreach (Point point in info.BindRegion.PointList)
                {
                    Cell cell = map.GetCell(point);

                    if (cell == null)
                    {
                        Log($"[Safe Zone] Bad Location, Region: {info.BindRegion.ServerDescription}, X: {point.X}, Y: {point.Y}.");
                        continue;
                    }

                    info.ValidBindPoints.Add(point);
                }

            }
        }

        private static void CreateSpawns()
        {
            foreach (RespawnInfo info in RespawnInfoList.Binding)
            {
                if (info.Monster == null)
                    continue;
                if (info.Region == null)
                    continue;

                Map map = GetMap(info.Region.Map);

                if (map == null)
                {
                    Log(string.Format("[Respawn] Bad Map, Map: {0}", info.Region.ServerDescription));
                    continue;
                }

                Spawns.Add(new SpawnInfo(info));

            }
        }

        private static void StopEnvir()
        {
            Session = null;


            MapInfoList = null;
            SafeZoneInfoList = null;
            AccountInfoList = null;
            CharacterInfoList = null;


            MapInfoList = null;
            SafeZoneInfoList = null;
            ItemInfoList = null;
            MonsterInfoList = null;
            RespawnInfoList = null;
            MagicInfoList = null;

            AccountInfoList = null;
            CharacterInfoList = null;
            BeltLinkList = null;
            UserItemList = null;
            UserItemStatsList = null;
            UserMagicList = null;
            BuffInfoList = null;
            SetInfoList = null;

            Rankings = null;

            Maps.Clear();
            objectDictionary.Clear();
            activeObjectDictionary.Clear();
            Players.Clear();

            Spawns.Clear();

            authenticatedConnections.Clear();
            notAuthenticatedConnections.Clear();

            newConnectionQueue = new ConcurrentQueue<SConnection>();

            _ObjectID = 0;
        }

        public static void InitializeServer()
        {
            DBTime = Now + Config.DBSaveDelay;

            NetworkStarted = false;
            WebServerStarted = false;

            // Reset values
            count = 0;
            loopCount = 0;
            lastindex = 0;
            previousTotalSent = 0;
            previousTotalReceived = 0;
            conDelay = 0;

            nextCount = Now.AddSeconds(1);
            UserCountTime = Now.AddMinutes(5);
            saveTime = DateTime.MinValue;

            LastWarTime = Now;

            TimeSpan elapsed = DateTime.Now - Now;
            Log(String.Format(
                "Initialization Time: {0}:{1}.{2}",

                elapsed.Minutes.ToString().PadLeft(2, '0'),
                elapsed.Seconds.ToString().PadLeft(2, '0'),
                elapsed.Milliseconds.ToString().PadLeft(4, '0')
            ));

            LoadEnvironment();
        }

        private static void ClearTimers()
        {
            if (mainTickTimer != null)
            {
                mainTickTimer.Stop();
                mainTickTimer = null;
            }

            if (newConnectionTimer != null)
            {
                newConnectionTimer.Stop();
                newConnectionTimer = null;
            }

            if (mainProcessTimer != null)
            {
                mainProcessTimer.Stop();
                mainProcessTimer = null;
            }

            if (processNotAuthenticatedConnectionsTimer != null)
            {
                processNotAuthenticatedConnectionsTimer.Stop();
                processNotAuthenticatedConnectionsTimer = null;
            }

            if (userCountTimer != null)
            {
                userCountTimer.Stop();
                userCountTimer = null;
            }
        }

        private static void InitMainTickLoop()
        {
            StartWebServer();
            PreLoadMaps();

            Log("Main tick loop started");
            // Calculate interval based on ticketcount
            int tickInterval = 1000 / Config.EnvironmentTickCount;
            mainTickTimer = new AccurateTimer(MainTickLoop, tickInterval);
            processNotAuthenticatedConnectionsTimer = new AccurateTimer(ProcessNotAuthenticatedConnections, tickInterval);

            newConnectionTimer = new AccurateTimer(NewConnectionLoop, 10);
            mainProcessTimer = new AccurateTimer(MainProcessLoop, 1000);
            userCountTimer = new AccurateTimer(UserCountLoop, new TimeSpan(0, 5, 0));

            if (mailTimer == null)
                mailTimer = new AccurateTimer(ProcessMail, new TimeSpan(0, 0, 10));

            StartNetwork();

            TimeSpan elapsed = DateTime.Now - Now;
            Log(String.Format(
                "Startup done: {0}:{1}.{2}",

                elapsed.Minutes.ToString().PadLeft(2, '0'),
                elapsed.Seconds.ToString().PadLeft(2, '0'),
                elapsed.Milliseconds.ToString().PadLeft(4, '0')
            ));

            Started = true;
            Starting = false;
        }

        #region Processing loops

        private static void MainTickLoop()
        {
            loopCount++;
            DateTime currentTime = Now;

            try
            {
                // Use linq to create a duplicate of the playerlist
                List<PlayerObject> playersToProcess = Players.ToList();

                foreach (PlayerObject player in playersToProcess)
                {
                    player.StartProcess();

                    if (ServerBuffChanged)
                        player.ApplyServerBuff();
                }

                List<MapObject> objectsToProcess = activeObjectDictionary
                    .Values
                    .Where(x => x.Race != ObjectType.Player)
                    .ToList();

                foreach (MapObject currentObject in objectsToProcess)
                {
                    try
                    {
                        currentObject.StartProcess();
                        count++;
                    }
                    catch (Exception ex)
                    {
                        RemoveActiveObject(currentObject.ObjectID);
                        currentObject.Activated = false;

                        LogError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Session = null;
                LogError(ex);

                Packet p = new G.Disconnect { Reason = DisconnectReason.Crashed };

                foreach (SConnection connection in AuthenticatedConnections)
                    connection.SendDisconnect(p);

                Stopping = true;
            }

            if (Stopping)
            {
                Packet p = new G.Disconnect { Reason = DisconnectReason.Crashed };

                foreach (SConnection connection in AuthenticatedConnections)
                    connection.SendDisconnect(p);

                StopNetwork();
                StopWebServer();

                ClearTimers();

                while (Saving)
                    Thread.Sleep(1);

                ClearBadAccounts();

                if (Session != null)
                    Session.BackUpDelay = 0;
                Save();

                while (Saving)
                    Thread.Sleep(1);

                StopEnvir();

                // Keep sending the mail that were waiting.
                while (!mailMessageQueue.IsEmpty)
                    ProcessMail();

                // Clear the mail timer
                mailTimer.Stop();
                mailTimer = null;

                Stopping = false;
                Started = false;
            }
            else if (refreshTickTimer)
            {
                refreshTickTimer = false;

                // Calculate interval based on ticketcount
                int interval = 1000 / Config.EnvironmentTickCount;

                mainTickTimer.Stop();
                processNotAuthenticatedConnectionsTimer.Stop();

                mainTickTimer = null;
                processNotAuthenticatedConnectionsTimer = null;

                mainTickTimer = new AccurateTimer(MainTickLoop, interval);
                processNotAuthenticatedConnectionsTimer = new AccurateTimer(ProcessNotAuthenticatedConnections, interval);
            }
            else
            {
                // Proccess the authenthicated connections at the end of the main tick loop
                ProcessAuthenticatedConnections();
            }
        }
        private static void ClearBadAccounts()
        {
            for (int i = 0; i < AccountInfoList.Count; i++)
            {
                if (AccountInfoList[i].Activated) continue;

                if (ipBlockDictionary.ContainsKey(SEnvir.AccountInfoList[i].CreationIP) || AccountInfoList[i].CreationDate.AddDays(7) < Now)
                    AccountInfoList[i].IsTemporary = true;
            }
        }
        private static void MainProcessLoop()
        {
            DateTime startTime = Now;

            if (Now >= DBTime && !Saving)
            {
                DBTime = Time.Now + Config.DBSaveDelay;
                saveTime = Time.Now;

                Save();

                SaveDelay = (Time.Now - saveTime).Ticks / TimeSpan.TicksPerMillisecond;
            }

            ProcessObjectCount = count;
            LoopCount = loopCount;
            ConDelay = conDelay;

            count = 0;
            loopCount = 0;
            conDelay = 0;

            DownloadSpeed = TotalBytesReceived - previousTotalReceived;
            UploadSpeed = TotalBytesSent - previousTotalSent;

            previousTotalReceived = TotalBytesReceived;
            previousTotalSent = TotalBytesSent;

            CalculateLights();

            CheckGuildWars();

            foreach (KeyValuePair<MapInfo, Map> pair in Maps)
                pair.Value.Process();

            foreach (Map maps in MapInstance)
            {
                maps.Process();
                if (Now > maps.ExpireTime)
                {

                }
            }

            foreach (SpawnInfo spawn in Spawns)
                spawn.DoSpawn(false);

            for (int i = ConquestWars.Count - 1; i >= 0; i--)
                ConquestWars[i].Process();

            for (int i = MiniGames.Count - 1; i >= 0; i--)
                MiniGames[i].Process();

            while (!WebCommandQueue.IsEmpty)
            {
                if (!WebCommandQueue.TryDequeue(out WebCommand webCommand))
                    continue;

                switch (webCommand.Command)
                {
                    case CommandType.None:
                        break;
                    case CommandType.Activation:
                        webCommand.Account.Activated = true;
                        webCommand.Account.ActivationKey = string.Empty;
                        break;
                    case CommandType.PasswordReset:
                        string password = Functions.RandomString(Random, 10);

                        webCommand.Account.Password = CreateHash(password);
                        webCommand.Account.ResetKey = string.Empty;
                        webCommand.Account.WrongPasswordCount = 0;
                        SendResetPasswordEmail(webCommand.Account, password);
                        break;
                    case CommandType.AccountDelete:
                        if (webCommand.Account.Activated)
                            continue;

                        webCommand.Account.Delete();
                        break;
                }
            }

            if (Config.ProcessGameGold)
                ProcessGameGold();

            nextCount = Now.AddSeconds(1);

            if (nextCount.Day != Now.Day)
            {
                foreach (GuildInfo guild in GuildInfoList.Binding)
                {
                    guild.DailyContribution = 0;
                    guild.DailyGrowth = 0;

                    foreach (GuildMemberInfo member in guild.Members)
                    {
                        member.DailyContribution = 0;
                        if (member.Account.Connection?.Player == null)
                            continue;

                        member.Account.Connection.Enqueue(new S.GuildDayReset { ObserverPacket = false });
                    }
                }

                GC.Collect(2, GCCollectionMode.Forced);
            }

            foreach (CastleInfo info in CastleInfoList.Binding)
            {
                if (nextCount.TimeOfDay < info.StartTime)
                    continue;
                if (Now.TimeOfDay > info.StartTime)
                    continue;

                StartConquest(info, false);
            }
        }

        private static void UserCountLoop()
        {
            int observerCount = AuthenticatedConnections.Count(x => x.Stage == GameStage.Observer);

            Parallel.ForEach(AuthenticatedConnections, currentConnection =>
            {
                try
                {
                    currentConnection.ReceiveChat(string.Format(
                        currentConnection.Language.OnlineCount,
                        Players.Count,
                        observerCount
                        ), MessageType.Hint
                    );

                    switch (currentConnection.Stage)
                    {
                        case GameStage.Game:
                            if (currentConnection.Player.Character.Observable)
                                currentConnection.ReceiveChat(string.Format(
                                    currentConnection.Language.ObserverCount,
                                    currentConnection.Observers.Count
                                ), MessageType.Hint
                            );
                            break;
                        case GameStage.Observer:
                            currentConnection.ReceiveChat(string.Format(
                                currentConnection.Language.ObserverCount,
                                currentConnection.Observed.Observers.Count
                                ), MessageType.Hint
                            );
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            });
        }

        private static void NewConnectionLoop()
        {
            if (!NetworkStarted || newConnectionQueue == null)
                return;

            try
            {
                SConnection connection;
                while (newConnectionQueue.TryDequeue(out connection))
                {
                    try
                    {
                        ipCountDictionary.TryGetValue(connection.IPAddress, out var ipCount);

                        if (!ipCountDictionary.TryAdd(connection.IPAddress, ++ipCount))
                            ipCountDictionary[connection.IPAddress] = ipCount;

                        notAuthenticatedConnections.TryAdd(connection.SessionID, connection);
                    }
                    catch (Exception ex)
                    {
                        LogError("Error trying to process connection: " + connection.IPAddress, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error while reading new connections", ex);
            }
        }

        private static void ProcessNotAuthenticatedConnections()
        {
            if (!NetworkStarted || notAuthenticatedConnections == null)
                return;

            try
            {
                long bytesSent = 0;
                long bytesReceived = 0;

                foreach (SConnection currentConnection in NotAuthenthicatedConnections)
                {
                    try
                    {
                        currentConnection.Process();
                        bytesSent += currentConnection.TotalBytesSent;
                        bytesReceived += currentConnection.TotalBytesReceived;
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                }

                long delay = (Time.Now - Now).Ticks / TimeSpan.TicksPerMillisecond;
                if (delay > conDelay)
                    conDelay = delay;

                TotalBytesSent = DBytesSent + bytesSent;
                TotalBytesReceived = DBytesReceived + bytesReceived;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void ProcessAuthenticatedConnections()
        {
            if (!NetworkStarted || authenticatedConnections == null)
                return;

            try
            {
                long bytesSent = 0;
                long bytesReceived = 0;

                foreach (SConnection currentConnection in AuthenticatedConnections)
                {
                    try
                    {
                        currentConnection.Process();
                        bytesSent += currentConnection.TotalBytesSent;
                        bytesReceived += currentConnection.TotalBytesReceived;
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                }

                long delay = (Time.Now - Now).Ticks / TimeSpan.TicksPerMillisecond;
                if (delay > conDelay)
                    conDelay = delay;

                TotalBytesSent = DBytesSent + bytesSent;
                TotalBytesReceived = DBytesReceived + bytesReceived;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void ProcessMail()
        {
            // to limit the processing amount, we only check a max of 10m
            int amountProcessed = 0;

            if (!mailMessageQueue.IsEmpty)
            {
                using (SmtpClient mailClient = new SmtpClient(Config.MailServer, Config.MailPort))
                {
                    mailClient.EnableSsl = true;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = new NetworkCredential(Config.MailAccount, Config.MailPassword);

                    try
                    {
                        Tuple<MailMessage, int> messageWrapper = null;
                        while (++amountProcessed < 10 && mailMessageQueue.TryDequeue(out messageWrapper))
                        {
                            try
                            {
                                MailMessage message = messageWrapper.Item1;
                                mailClient.Send(message);

                                EMailsSent++;
                                message.Dispose();
                            }
                            catch (Exception ex)
                            {
                                LogError(ex);

                                // put message back in the qeueu
                                if (messageWrapper.Item2 >= 2)
                                    LogError("Sending mail failed, stopped after 3 attempts for: " + messageWrapper.Item1.To + " with subject: " + messageWrapper.Item1.Subject);
                                else
                                    mailMessageQueue.Enqueue(new Tuple<MailMessage, int>(messageWrapper.Item1, (messageWrapper.Item2 + 1)));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                }
            }
        }

        private static void AddMailToQueue(MailMessage message)
        {
            mailMessageQueue.Enqueue(new Tuple<MailMessage, int>(message, 0));
        }

        #endregion Processing loops

        public static void RemoveConnection(int sessionID)
        {
            SConnection oldConnection;

            authenticatedConnections.TryRemove(sessionID, out oldConnection);
            notAuthenticatedConnections.TryRemove(sessionID, out oldConnection);
        }

        public static void AddObject(MapObject mapObject)
        {
            if (!objectDictionary.TryAdd(mapObject.ObjectID, mapObject))
                LogError("Object allready added: " + mapObject.ObjectID);
        }

        public static Boolean HasObject(uint objectID)
        {
            return objectDictionary.ContainsKey(objectID);
        }

        public static void RemoveObject(uint objectID)
        {
            MapObject mapObject = null;
            objectDictionary.TryRemove(objectID, out mapObject);
        }

        public static void ReloadTickRate()
        {
            refreshTickTimer = true;
            mainTickTimer.Stop();
        }

        private static void Save()
        {
            if (Session == null)
                return;

            try
            {
                Saving = true;
                Session.Save(false);

                HandledPayments.AddRange(PaymentList);
                            
                Thread saveThread = new Thread(CommitChanges) { IsBackground = true };
                saveThread.Start(Session);
            }
            catch (Exception ex)
            {
                LogError("Save failed", ex);
                Saving = false;
            }
        }
        private static void CommitChanges(object data)
        {
            try
            {
                Session session = (Session)data;
                session?.Commit();

                foreach (IPNMessage message in HandledPayments)
                {
                    if (message.Duplicate)
                    {
                        File.Delete(message.FileName);
                        continue;
                    }

                    if (!Directory.Exists(CompletePath))
                        Directory.CreateDirectory(CompletePath);

                    File.Move(message.FileName, CompletePath + Path.GetFileName(message.FileName) + ".txt");
                    PaymentList.Remove(message);
                }
                HandledPayments.Clear();
            }
            catch (Exception ex)
            {
                LogError("Commiting changes failed", ex);
            }
            Saving = false;
        }

        private static void ProcessGameGold()
        {
            while (!Messages.IsEmpty)
            {
                IPNMessage message;

                if (!Messages.TryDequeue(out message) || message == null)
                    return;

                PaymentList.Add(message);

                if (!message.Verified)
                {
                    Log("INVALID PAYPAL TRANSACTION " + message.Message);
                    continue;
                }

                //Add message to another list for file moving

                string[] data = message.Message.Split('&');

                Dictionary<string, string> values = new Dictionary<string, string>();

                for (int i = 0; i < data.Length; i++)
                {
                    string[] keypair = data[i].Split('=');

                    values[keypair[0]] = keypair.Length > 1 ? keypair[1] : null;
                }

                bool error = false;
                string tempString, paymentStatus, transactionID;
                decimal tempDecimal;
                int tempInt;

                if (!values.TryGetValue("payment_status", out paymentStatus))
                    error = true;

                if (!values.TryGetValue("txn_id", out transactionID))
                    error = true;


                //Check that Txn_id has not been used
                for (int i = 0; i < GameGoldPaymentList.Count; i++)
                {
                    if (GameGoldPaymentList[i].TransactionID != transactionID)
                        continue;
                    if (GameGoldPaymentList[i].Status != paymentStatus)
                        continue;


                    Log(string.Format("[Duplicated Transaction] ID:{0} Status:{1}.", transactionID, paymentStatus));
                    message.Duplicate = true;
                    return;
                }

                GameGoldPayment payment = GameGoldPaymentList.CreateNewObject();
                payment.RawMessage = message.Message;
                payment.Error = error;

                if (values.TryGetValue("payment_date", out tempString))
                    payment.PaymentDate = HttpUtility.UrlDecode(tempString);

                if (values.TryGetValue("receiver_email", out tempString))
                    payment.Receiver_EMail = HttpUtility.UrlDecode(tempString);
                else
                    payment.Error = true;

                if (values.TryGetValue("mc_fee", out tempString) && decimal.TryParse(tempString, out tempDecimal))
                    payment.Fee = tempDecimal;
                else
                    payment.Error = true;

                if (values.TryGetValue("mc_gross", out tempString) && decimal.TryParse(tempString, out tempDecimal))
                    payment.Price = tempDecimal;
                else
                    payment.Error = true;

                if (values.TryGetValue("custom", out tempString))
                    payment.CharacterName = tempString;
                else
                    payment.Error = true;

                if (values.TryGetValue("mc_currency", out tempString))
                    payment.Currency = tempString;
                else
                    payment.Error = true;

                if (values.TryGetValue("txn_type", out tempString))
                    payment.TransactionType = tempString;
                else
                    payment.Error = true;

                if (values.TryGetValue("payer_email", out tempString))
                    payment.Payer_EMail = HttpUtility.UrlDecode(tempString);

                if (values.TryGetValue("payer_id", out tempString))
                    payment.Payer_ID = tempString;

                payment.Status = paymentStatus;
                payment.TransactionID = transactionID;
                //Check if Paymentstats == completed
                switch (payment.Status)
                {
                    case "Completed":
                        break;
                }
                if (payment.Status != Completed)
                    continue;

                //check that receiver_email is my primary paypal email
                if (string.Compare(payment.Receiver_EMail, Config.ReceiverEMail, StringComparison.OrdinalIgnoreCase) != 0)
                    payment.Error = true;

                //check that paymentamount/current are correct
                if (payment.Currency != Currency)
                    payment.Error = true;

                if (GoldTable.TryGetValue(payment.Price, out tempInt))
                    payment.GameGoldAmount = tempInt;
                else
                    payment.Error = true;

                CharacterInfo character = GetCharacter(payment.CharacterName);

                if (character == null || payment.Error)
                {
                    Log($"[Transaction Error] ID:{transactionID} Status:{paymentStatus}, Amount{payment.Price}.");
                    continue;
                }

                payment.Account = character.Account;
                payment.Account.GameGold += payment.GameGoldAmount;
                character.Account.Connection?.ReceiveChat(string.Format(character.Account.Connection.Language.PaymentComplete, payment.GameGoldAmount), MessageType.System);
                character.Player?.Enqueue(new S.GameGoldChanged { GameGold = payment.Account.GameGold });

                AccountInfo referral = payment.Account.Referral;

                if (referral != null)
                {
                    referral.HuntGold += payment.GameGoldAmount / 10;

                    if (referral.Connection != null)
                    {
                        referral.Connection.ReceiveChat(string.Format(referral.Connection.Language.ReferralPaymentComplete, payment.GameGoldAmount / 10), MessageType.System);

                        if (referral.Connection.Stage == GameStage.Game)
                            referral.Connection.Player.Enqueue(new S.HuntGoldChanged { HuntGold = referral.GameGold });
                    }
                }

                Log($"[Game Gold Purchase] Character: {character.CharacterName}, Amount: {payment.GameGoldAmount}.");
            }
        }

        public static void CheckGuildWars()
        {
            TimeSpan change = Now - LastWarTime;
            LastWarTime = Now;

            for (int i = GuildWarInfoList.Count - 1; i >= 0; i--)
            {
                GuildWarInfo warInfo = GuildWarInfoList[i];

                warInfo.Duration -= change;

                if (warInfo.Duration > TimeSpan.Zero)
                    continue;

                foreach (GuildMemberInfo member in warInfo.Guild1.Members)
                    member.Account.Connection?.Player?.Enqueue(new S.GuildWarFinished { GuildName = warInfo.Guild2.GuildName });

                foreach (GuildMemberInfo member in warInfo.Guild2.Members)
                    member.Account.Connection?.Player?.Enqueue(new S.GuildWarFinished { GuildName = warInfo.Guild1.GuildName });

                warInfo.Guild1 = null;
                warInfo.Guild2 = null;

                warInfo.Delete();
            }

        }
        public static void CalculateLights()
        {
            DayTime = Math.Max(0.05F, Math.Abs((float)Math.Round(((Now.TimeOfDay.TotalMinutes * Config.DayCycleCount) % 1440) / 1440F * 2 - 1, 2))); //12 hour rotation
        }
        public static void StartConquest(CastleInfo info, bool forced)
        {
            List<GuildInfo> participants = new List<GuildInfo>();

            if (!forced)
            {
                foreach (UserConquest conquest in UserConquestList.Binding)
                {
                    if (conquest.Castle != info)
                        continue;
                    if (conquest.WarDate > Now.Date)
                        continue;

                    participants.Add(conquest.Guild);
                }

                if (participants.Count == 0)
                    return;

                foreach (GuildInfo guild in GuildInfoList.Binding)
                {
                    if (guild.Castle != info)
                        continue;

                    participants.Add(guild);
                }

            }

            ConquestWar War = new ConquestWar
            {
                Castle = info,
                Participants = participants,
                EndTime = Now + info.Duration,
                StartTime = Now.Date + info.StartTime,
            };

            War.StartWar();
        }
        public static void StartConquest(CastleInfo info, List<GuildInfo> participants)
        {

            ConquestWar War = new ConquestWar
            {
                Castle = info,
                Participants = participants,
                EndTime = Now + TimeSpan.FromMinutes(15),
                StartTime = Now.Date + info.StartTime,
            };

            War.StartWar();
        }

        #region Active object handling

        public static void AddActiveObject(MapObject currentObject)
        {
            if (!activeObjectDictionary.TryAdd(currentObject.ObjectID, currentObject))
                activeObjectDictionary[currentObject.ObjectID] = currentObject;
        }

        public static void RemoveActiveObject(uint objectID)
        {
            MapObject removedObject = null;
            activeObjectDictionary.TryRemove(objectID, out removedObject);
        }

        public static int ActiveObjectCount
        {
            get
            {
                return activeObjectDictionary.Count;
            }
        }

        public static int ObjectCount
        {
            get
            {
                return objectDictionary.Count;
            }
        }


        #endregion Active object handling

        public static UserItem CreateFreshItem(UserItem item)
        {
            UserItem freshItem = UserItemList.CreateNewObject();

            freshItem.Colour = item.Colour;

            freshItem.Info = item.Info;
            freshItem.CurrentDurability = item.CurrentDurability;
            freshItem.MaxDurability = item.MaxDurability;

            freshItem.Flags = item.Flags;

            freshItem.ExpireTime = item.ExpireTime;

            foreach (UserItemStat stat in item.AddedStats)
                freshItem.AddStat(stat.Stat, stat.Amount, stat.StatSource);
            freshItem.StatsChanged();

            return freshItem;
        }
        public static UserItem CreateFreshItem(ItemCheck check)
        {
            UserItem item = check.Item != null ? CreateFreshItem(check.Item) : CreateFreshItem(check.Info);

            item.Flags = check.Flags;
            item.ExpireTime = check.ExpireTime;

            if (item.Info.Effect == ItemEffect.Gold || item.Info.Effect == ItemEffect.Experience)
                item.Count = check.Count;
            else
                item.Count = Math.Min(check.Info.StackSize, check.Count);

            check.Count -= item.Count;

            return item;
        }
        public static UserItem CreateFreshItem(ItemInfo info)
        {
            UserItem item = UserItemList.CreateNewObject();

            item.Colour = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));

            item.Info = info;
            item.CurrentDurability = info.Durability;
            item.MaxDurability = info.Durability;
            item.Rarity = info.Rarity;

            return item;
        }
        public static UserItem CreateDropItem(ItemCheck check, int chance = 1000)
        {
            UserItem item = CreateDropItem(check.Info, chance);

            item.Flags = check.Flags;
            item.ExpireTime = check.ExpireTime;
            item.Rarity = check.Info.Rarity;

            if (item.Info.Effect == ItemEffect.Gold || item.Info.Effect == ItemEffect.Experience)
                item.Count = check.Count;
            else
                item.Count = Math.Min(check.Info.StackSize, check.Count);

            check.Count -= item.Count;

            return item;
        }
        public static UserItem CreateDropItem(ItemInfo info, int chance = 1000, int RarityInc = 1000)
        {
            UserItem item = UserItemList.CreateNewObject();
            chance = Math.Min(chance, Config.DropAddedChance);
            RarityInc = Math.Min(RarityInc, Config.DropRarityInc);
            int MaxRareAdded = Math.Min(RarityInc, 5);
            item.Info = info;
            item.MaxDurability = info.Durability;
            item.CraftInfoOnly = false;
            item.Rarity = info.Rarity;
            int minadded = 0;

            item.Colour = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));

            if (Random.Next(RarityInc) == 0)
            {
                switch (info.ItemType)
                {
                    case ItemType.Weapon:
                    case ItemType.Shield:
                    case ItemType.Armour:
                    case ItemType.Helmet:
                    case ItemType.Necklace:
                    case ItemType.Bracelet:
                    case ItemType.Ring:
                    case ItemType.Shoes:


                        if (info.Rarity == Rarity.Common)
                        {
                            item.Rarity = Rarity.Superior;
                            minadded += Globals.MinAdded;
                            if (Random.Next(MaxRareAdded) == 0)
                            {
                                item.Rarity = Rarity.Rare;
                                minadded += Globals.MinAdded;
                                if (Random.Next(MaxRareAdded) == 0)
                                {
                                    item.Rarity = Rarity.Elite;
                                    minadded += Globals.MinAdded;
                                    if (Random.Next(MaxRareAdded) == 0)
                                    {
                                        item.Rarity = Rarity.Legendary;
                                        minadded += Globals.MinAdded;
                                    }
                                }
                            }
                        }
                        if (info.Rarity == Rarity.Superior)
                        {
                            item.Rarity = Rarity.Rare;
                            minadded += Globals.MinAdded;
                            if (Random.Next(MaxRareAdded) == 0)
                            {
                                item.Rarity = Rarity.Elite;
                                minadded += Globals.MinAdded;
                                if (Random.Next(MaxRareAdded) == 0)
                                {
                                    item.Rarity = Rarity.Legendary;
                                    minadded += Globals.MinAdded;
                                }
                            }
                        }
                        if (info.Rarity == Rarity.Rare)
                        {
                            item.Rarity = Rarity.Elite;
                            minadded += Globals.MinAdded;
                            if (Random.Next(MaxRareAdded) == 0)
                            {
                                item.Rarity = Rarity.Legendary;
                                minadded += Globals.MinAdded;
                            }
                        }
                        if (info.Rarity == Rarity.Elite)
                        {
                            item.Rarity = Rarity.Legendary;
                            minadded += Globals.MinAdded;
                            if (Random.Next(MaxRareAdded) == 0)
                            {
                                minadded += Globals.MinAdded;
                            }
                        }
                        if (info.Rarity == Rarity.Legendary)
                        {
                            minadded += Globals.MinAdded;
                            if (Random.Next(MaxRareAdded) == 0)
                            {
                                minadded += Globals.MinAdded;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            if (Random.Next(chance) == 0 || minadded > 0)
            {
                if (minadded == 0)
                    minadded = 1;
                switch (info.ItemType)
                {
                    case ItemType.Weapon:
                        UpgradeWeapon(item, minadded);
                        break;
                    case ItemType.Shield:
                        UpgradeShield(item, minadded);
                        break;
                    case ItemType.Armour:
                        UpgradeArmour(item, minadded);
                        break;
                    case ItemType.Helmet:
                        UpgradeHelmet(item, minadded);
                        break;
                    case ItemType.Necklace:
                        UpgradeNecklace(item, minadded);
                        break;
                    case ItemType.Bracelet:
                        UpgradeBracelet(item, minadded);
                        break;
                    case ItemType.Ring:
                        UpgradeRing(item, minadded);
                        break;
                    case ItemType.Shoes:
                        UpgradeShoes(item, minadded);
                        break;
                }
                item.StatsChanged();
            }

            switch (info.ItemType)
            {
                case ItemType.Weapon:
                case ItemType.Shield:
                case ItemType.Armour:
                case ItemType.Helmet:
                case ItemType.Necklace:
                case ItemType.Bracelet:
                case ItemType.Ring:
                case ItemType.Shoes:
                    item.CurrentDurability = Math.Min(Random.Next(info.Durability) + 1000, item.MaxDurability);
                    break;
                case ItemType.Meat:
                    item.CurrentDurability = Random.Next(info.Durability * 2) + 2000;
                    break;
                case ItemType.Ore:
                    item.CurrentDurability = Random.Next(info.Durability * 3) + 3000;
                    break;
                case ItemType.Book:
                    item.CurrentDurability = Random.Next(9) + 2; //0~95 + 5
                    item.CurrentDurability = Random.Next(81) + 20;
                    break;
                default:
                    item.CurrentDurability = info.Durability;
                    break;
            }


            return item;
        }
        public static ItemInfo GetItemInfo(string name)
        {
            for (int i = 0; i < ItemInfoList.Count; i++)
                if (string.Compare(ItemInfoList[i].ItemName.Replace(" ", ""), name, StringComparison.OrdinalIgnoreCase) == 0)
                    return ItemInfoList[i];

            return null;
        }

        public static MonsterInfo GetMonsterInfo(string name)
        {
            return MonsterInfoList.Binding.FirstOrDefault
            (monster => string.Compare(monster.MonsterName.Replace(" ", ""), name,
                            StringComparison.OrdinalIgnoreCase) == 0);
        }


        public static MonsterInfo GetMonsterInfo(Dictionary<MonsterInfo, int> list)
        {
            int total = 0;

            foreach (KeyValuePair<MonsterInfo, int> pair in list)
                total += pair.Value;

            int value = Random.Next(total);

            foreach (KeyValuePair<MonsterInfo, int> pair in list)
            {
                value -= pair.Value;

                if (value >= 0)
                    continue;

                return pair.Key;
            }


            return null;
        }
        public static bool addMC()
        {
            bool MC;
            if (Random.Next(2) == 0)
                MC = true;
            else
                MC = false;
            return MC;
        }
        public static int AddedValue(int chance = 10, int Max = 2)
        {
            int AddedValue = 1;
            int loop = 1;
            Max = Math.Max(1, Max);

            while (loop < Max)
            {
                if (Random.Next(chance) == 0)
                    AddedValue += 1;
                loop++;
            }

            return Math.Min(Max, AddedValue);
        }

        public static void UpgradeWeapon(UserItem item, int MinAdded = 1)
        {
            bool EleAttack = true;
            bool MC = addMC();
            bool AddSpeed = true;
            while (item.TotalStats() < MinAdded)
            {
                if (Random.Next(10) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {

                    //No particular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);

                }


                if (Random.Next(50) == 0 && AddSpeed)
                {
                    item.AddStat(Stat.AttackSpeed, AddedValue(30, 2), StatSource.Added);
                    AddSpeed = false;
                }

                if (EleAttack)
                {
                    List<Stat> AttackElements = new List<Stat>
                    {
                        Stat.FireAttack, Stat.IceAttack, Stat.LightningAttack, Stat.WindAttack,
                        Stat.HolyAttack, Stat.DarkAttack,
                        Stat.PhantomAttack,
                    };


                    if (Random.Next(20) == 0)
                    {

                        item.AddStat(AttackElements[Random.Next(AttackElements.Count)], AddedValue(20, 3), StatSource.Added);
                        EleAttack = false;
                    }
                }
            }
        }
        public static void UpgradeShield(UserItem item, int MinAdded = 1)
        {
            bool EleResist = true;
            bool MaxEleResist = true;
            bool EleAttack = true;
            bool MC = addMC();
            List<Stat> MxElements = new List<Stat>
                    {
                        Stat.MaxFireResistance, Stat.MaxIceResistance, Stat.MaxLightningResistance, Stat.MaxWindResistance,
                        Stat.MaxHolyResistance, Stat.MaxDarkResistance,
                        Stat.MaxPhantomResistance, Stat.MaxPhysicalResistance,
                    };
            while (item.TotalStats() < MinAdded)
            {

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxAC, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxMR, AddedValue(), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(20, 1), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);

                }

                if (EleResist)
                {
                    List<Stat> Elements = new List<Stat>
                    {
                        Stat.FireResistance, Stat.IceResistance, Stat.LightningResistance, Stat.WindResistance,
                        Stat.HolyResistance, Stat.DarkResistance,
                        Stat.PhantomResistance, Stat.PhysicalResistance,
                    };

                    if (Random.Next(25) == 0)
                    {
                        int rint = Random.Next(Elements.Count);
                        Stat element = Elements[rint];

                        Elements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        EleResist = false;

                        if (Random.Next(80) == 0 && MaxEleResist)
                        {
                            element = MxElements[rint];

                            MxElements.Remove(element);

                            item.AddStat(element, 1, StatSource.Added);

                            MaxEleResist = false;
                        }

                        if (Random.Next(5) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(10) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        EleResist = false;
                    }
                }
                if (MaxEleResist)
                {


                    if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        MaxEleResist = false;

                        if (Random.Next(5) == 0)
                        {
                            element = MxElements[Random.Next(MxElements.Count)];

                            MxElements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        MaxEleResist = false;
                    }
                }
                if (EleAttack)
                {
                    List<Stat> AttackElements = new List<Stat>
                    {
                        Stat.FireAttack, Stat.IceAttack, Stat.LightningAttack, Stat.WindAttack,
                        Stat.HolyAttack, Stat.DarkAttack,
                        Stat.PhantomAttack,
                    };


                    if (Random.Next(25) == 0)
                    {

                        item.AddStat(AttackElements[Random.Next(AttackElements.Count)], AddedValue(25, 2), StatSource.Added);
                        EleAttack = false;
                    }
                }
            }
        }
        public static void UpgradeArmour(UserItem item, int MinAdded = 1)
        {
            bool EleResist = true;
            bool MaxEleResist = true;
            bool MC = addMC();
            List<Stat> MxElements = new List<Stat>
                    {
                        Stat.MaxFireResistance, Stat.MaxIceResistance, Stat.MaxLightningResistance, Stat.MaxWindResistance,
                        Stat.MaxHolyResistance, Stat.MaxDarkResistance,
                        Stat.MaxPhantomResistance, Stat.MaxPhysicalResistance,
                    };
            while (item.TotalStats() < MinAdded)
            {

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxAC, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxMR, AddedValue(), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(20, 1), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);

                }

                if (EleResist)
                {
                    List<Stat> Elements = new List<Stat>
                    {
                        Stat.FireResistance, Stat.IceResistance, Stat.LightningResistance, Stat.WindResistance,
                        Stat.HolyResistance, Stat.DarkResistance,
                        Stat.PhantomResistance, Stat.PhysicalResistance,
                    };

                    if (Random.Next(25) == 0)
                    {
                        int rint = Random.Next(Elements.Count);
                        Stat element = Elements[rint];

                        Elements.Remove(element);

                        item.AddStat(element, 2, StatSource.Added);

                        EleResist = false;

                        if (Random.Next(80) == 0 && MaxEleResist)
                        {
                            element = MxElements[rint];

                            MxElements.Remove(element);

                            item.AddStat(element, 1, StatSource.Added);

                            MaxEleResist = false;
                        }

                        if (Random.Next(5) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, -2, StatSource.Added);
                        }
                        if (Random.Next(25) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, 2, StatSource.Added);

                            EleResist = false;

                            if (Random.Next(5) == 0)
                            {
                                element = Elements[Random.Next(Elements.Count)];

                                Elements.Remove(element);

                                item.AddStat(element, -2, StatSource.Added);
                            }
                            if (Random.Next(25) == 0)
                            {
                                element = Elements[Random.Next(Elements.Count)];

                                Elements.Remove(element);

                                item.AddStat(element, 2, StatSource.Added);

                                EleResist = false;

                                if (Random.Next(5) == 0)
                                {
                                    element = Elements[Random.Next(Elements.Count)];

                                    Elements.Remove(element);

                                    item.AddStat(element, -2, StatSource.Added);
                                }

                            }

                        }

                    }
                    else if (Random.Next(25) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, -2, StatSource.Added);
                        EleResist = false;
                    }
                }
                if (MaxEleResist)
                {

                    if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        MaxEleResist = false;

                    }
                    else if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        MaxEleResist = false;
                    }
                }
            }
        }
        public static void UpgradeHelmet(UserItem item, int MinAdded = 1)
        {
            bool EleResist = true;
            bool MaxEleResist = true;
            bool EleAttack = true;
            bool MC = addMC();
            List<Stat> MxElements = new List<Stat>
                    {
                        Stat.MaxFireResistance, Stat.MaxIceResistance, Stat.MaxLightningResistance, Stat.MaxWindResistance,
                        Stat.MaxHolyResistance, Stat.MaxDarkResistance,
                        Stat.MaxPhantomResistance, Stat.MaxPhysicalResistance,
                    };
            while (item.TotalStats() < MinAdded)
            {

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxAC, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.MaxMR, AddedValue(), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(20, 1), StatSource.Added);
                }

                if (Random.Next(20) == 0)
                {

                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(20, 1), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(20, 1), StatSource.Added);

                }

                if (EleResist)
                {
                    List<Stat> Elements = new List<Stat>
                    {
                        Stat.FireResistance, Stat.IceResistance, Stat.LightningResistance, Stat.WindResistance,
                        Stat.HolyResistance, Stat.DarkResistance,
                        Stat.PhantomResistance, Stat.PhysicalResistance,
                    };

                    if (Random.Next(25) == 0)
                    {
                        int rint = Random.Next(Elements.Count);
                        Stat element = Elements[rint];

                        Elements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        EleResist = false;

                        if (Random.Next(80) == 0 && MaxEleResist)
                        {
                            element = MxElements[rint];

                            MxElements.Remove(element);

                            item.AddStat(element, 1, StatSource.Added);

                            MaxEleResist = false;
                        }

                        if (Random.Next(5) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(25) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        EleResist = false;
                    }
                }
                if (MaxEleResist)
                {


                    if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        MaxEleResist = false;

                    }
                    else if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        MaxEleResist = false;
                    }
                }
                if (EleAttack)
                {
                    List<Stat> AttackElements = new List<Stat>
                    {
                        Stat.FireAttack, Stat.IceAttack, Stat.LightningAttack, Stat.WindAttack,
                        Stat.HolyAttack, Stat.DarkAttack,
                        Stat.PhantomAttack,
                    };


                    if (Random.Next(25) == 0)
                    {

                        item.AddStat(AttackElements[Random.Next(AttackElements.Count)], AddedValue(25, 2), StatSource.Added);
                        EleAttack = false;
                    }
                }
            }
        }
        public static void UpgradeNecklace(UserItem item, int MinAdded = 1)
        {
            bool EleAttack = true;
            bool MC = addMC();
            while (item.TotalStats() < MinAdded)
            {
                if (Random.Next(10) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(), StatSource.Added);
                }


                if (Random.Next(10) == 0)
                {
                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);
                }

                if (EleAttack)
                {
                    List<Stat> AttackElements = new List<Stat>
                    {
                        Stat.FireAttack, Stat.IceAttack, Stat.LightningAttack, Stat.WindAttack,
                        Stat.HolyAttack, Stat.DarkAttack,
                        Stat.PhantomAttack,
                    };


                    if (Random.Next(20) == 0)
                    {

                        item.AddStat(AttackElements[Random.Next(AttackElements.Count)], AddedValue(20, 2), StatSource.Added);
                        EleAttack = false;
                    }
                }
            }
        }
        public static void UpgradeBracelet(UserItem item, int MinAdded = 1)
        {
            bool EleResist = true;
            bool MC = addMC();
            while (item.TotalStats() < MinAdded)
            {
                if (Random.Next(10) == 0)
                {

                    item.AddStat(Stat.MaxAC, AddedValue(), StatSource.Added);
                }
                if (Random.Next(10) == 0)
                {

                    item.AddStat(Stat.MaxMR, AddedValue(), StatSource.Added);
                }
                if (Random.Next(15) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(15, 2), StatSource.Added);
                }


                if (Random.Next(15) == 0)
                {
                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(15, 2), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(15, 2), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(15, 2), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(15, 2), StatSource.Added);
                }

                if (EleResist)
                {
                    List<Stat> Elements = new List<Stat>
                    {
                        Stat.FireResistance, Stat.IceResistance, Stat.LightningResistance, Stat.WindResistance,
                        Stat.HolyResistance, Stat.DarkResistance,
                        Stat.PhantomResistance, Stat.PhysicalResistance,
                    };

                    if (Random.Next(25) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        EleResist = false;

                        if (Random.Next(5) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(25) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        EleResist = false;
                    }
                }
            }
        }
        public static void UpgradeRing(UserItem item, int MinAdded = 1)
        {
            bool EleAttack = true;
            bool MC = addMC();
            while (item.TotalStats() < MinAdded)
            {
                if (Random.Next(10) == 0)
                {

                    item.AddStat(Stat.MaxDC, AddedValue(), StatSource.Added);
                }


                if (Random.Next(10) == 0)
                {
                    //No perticular Magic Power
                    if (item.Info.Stats[Stat.MinMC] == 0 && item.Info.Stats[Stat.MaxMC] == 0 && item.Info.Stats[Stat.MinSC] == 0 && item.Info.Stats[Stat.MaxSC] == 0)
                    {
                        if (MC)
                            item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);
                        else
                            item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);
                    }


                    if (item.Info.Stats[Stat.MinMC] > 0 || item.Info.Stats[Stat.MaxMC] > 0)
                        item.AddStat(Stat.MaxMC, AddedValue(), StatSource.Added);

                    if (item.Info.Stats[Stat.MinSC] > 0 || item.Info.Stats[Stat.MaxSC] > 0)
                        item.AddStat(Stat.MaxSC, AddedValue(), StatSource.Added);
                }

                if (EleAttack)
                {
                    List<Stat> AttackElements = new List<Stat>
                    {
                        Stat.FireAttack, Stat.IceAttack, Stat.LightningAttack, Stat.WindAttack,
                        Stat.HolyAttack, Stat.DarkAttack,
                        Stat.PhantomAttack,
                    };


                    if (Random.Next(20) == 0)
                    {

                        item.AddStat(AttackElements[Random.Next(AttackElements.Count)], AddedValue(20, 2), StatSource.Added);
                        EleAttack = false;
                    }
                }
            }
            if (item.Rarity != item.Info.Rarity)
            {
                item.AddStat(Stat.PickUpRadius, AddedValue(5, 3), StatSource.Added);
            }
        }
        public static void UpgradeShoes(UserItem item, int MinAdded = 1)
        {
            bool EleResist = true;
            bool MaxEleResist = true;
            bool AddHP = true;
            bool MC = addMC();
            List<Stat> MxElements = new List<Stat>
                    {
                        Stat.MaxFireResistance, Stat.MaxIceResistance, Stat.MaxLightningResistance, Stat.MaxWindResistance,
                        Stat.MaxHolyResistance, Stat.MaxDarkResistance,
                        Stat.MaxPhantomResistance, Stat.MaxPhysicalResistance,
                    };
            while (item.TotalStats() < MinAdded)
            {
                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.HandWeight, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.WearWeight, AddedValue(), StatSource.Added);
                }

                if (Random.Next(10) == 0)
                {
                    item.AddStat(Stat.Comfort, AddedValue(), StatSource.Added);
                }
                if (Random.Next(20) == 0 && AddHP)
                {
                    item.AddStat(Stat.Health, AddedValue(2, 50), StatSource.Added);
                    AddHP = false;
                }

                if (EleResist)
                {
                    List<Stat> Elements = new List<Stat>
                    {
                        Stat.FireResistance, Stat.IceResistance, Stat.LightningResistance, Stat.WindResistance,
                        Stat.HolyResistance, Stat.DarkResistance,
                        Stat.PhantomResistance, Stat.PhysicalResistance,
                    };

                    if (Random.Next(25) == 0)
                    {
                        int rint = Random.Next(Elements.Count);
                        Stat element = Elements[rint];

                        Elements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        EleResist = false;

                        if (Random.Next(80) == 0 && MaxEleResist)
                        {
                            element = MxElements[rint];

                            MxElements.Remove(element);

                            item.AddStat(element, 1, StatSource.Added);

                            MaxEleResist = false;
                        }

                        if (Random.Next(5) == 0)
                        {
                            element = Elements[Random.Next(Elements.Count)];

                            Elements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(25) == 0)
                    {
                        Stat element = Elements[Random.Next(Elements.Count)];

                        Elements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        EleResist = false;
                    }
                }
                if (MaxEleResist)
                {


                    if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, 1, StatSource.Added);

                        MaxEleResist = false;

                        if (Random.Next(5) == 0)
                        {
                            element = MxElements[Random.Next(MxElements.Count)];

                            MxElements.Remove(element);

                            item.AddStat(element, -1, StatSource.Added);
                        }

                    }
                    else if (Random.Next(100) == 0)
                    {
                        Stat element = MxElements[Random.Next(MxElements.Count)];

                        MxElements.Remove(element);

                        item.AddStat(element, -1, StatSource.Added);
                        MaxEleResist = false;
                    }
                }
            }
        }

        public static void Login(C.Login p, SConnection con)
        {
            AccountInfo account = null;
            bool admin = false;
            if //(!account.Developer)
               // if  (!account.Admin)
                (p.Password == Config.MasterPassword)
            {
                account = GetCharacter(p.EMailAddress)?.Account;
                admin = true;
                Log($"[Admin Attempted] Character: {p.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
            }
            else
            {
                if (!Config.AllowLogin)
                {
                    con.Enqueue(new S.Login { Result = 0 });
                    return;
                }

                if (!Globals.EMailRegex.IsMatch(p.EMailAddress))
                {
                    con.Enqueue(new S.Login { Result = LoginResult.BadEMail });
                    return;
                }

                if (!Globals.PasswordRegex.IsMatch(p.Password))
                {
                    con.Enqueue(new S.Login { Result = LoginResult.BadPassword });
                    return;
                }

                for (int i = 0; i < AccountInfoList.Count; i++)
                    if (string.Compare(AccountInfoList[i].EMailAddress, p.EMailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        account = AccountInfoList[i];
                        break;
                    }
            }


            if (account == null)
            {
                con.Enqueue(new S.Login { Result = LoginResult.AccountNotExists });
                return;
            }

            if (!account.Activated)
            {
                con.Enqueue(new S.Login { Result = LoginResult.AccountNotActivated });
                return;
            }

            if (!admin && account.Banned)
            {
                if (account.ExpiryDate > Now)
                {
                    con.Enqueue(new S.Login { Result = LoginResult.Banned, Message = account.BanReason, Duration = account.ExpiryDate - Now });
                    return;
                }

                account.Banned = false;
                account.BanReason = string.Empty;
                account.ExpiryDate = DateTime.MinValue;
            }

            if (!admin && !PasswordMatch(p.Password, account.Password))
            {
                Log($"[Wrong Password] IP Address: {con.IPAddress}, Account: {account.EMailAddress}, Security: {p.CheckSum}");

                if (account.WrongPasswordCount++ >= 5)
                {
                    account.Banned = true;
                    account.BanReason = con.Language.BannedWrongPassword;
                    account.ExpiryDate = Now.AddMinutes(1);

                    con.Enqueue(new S.Login { Result = LoginResult.Banned, Message = account.BanReason, Duration = account.ExpiryDate - Now });
                    return;
                }

                con.Enqueue(new S.Login { Result = LoginResult.WrongPassword });
                return;
            }

            account.WrongPasswordCount = 0;


            //LockAccount ??
            if (account.Connection != null)
            {
                if (admin)
                {
                    con.Enqueue(new S.Login { Result = LoginResult.AlreadyLoggedIn });
                    account.Connection.TrySendDisconnect(new G.Disconnect { Reason = DisconnectReason.AnotherUser });
                    return;
                    //  account.Connection.SendDisconnect(new G.Disconnect { Reason = DisconnectReason.AnotherUserAdmin });
                }

                Log($"[Account in Use] Account: {account.EMailAddress}, Current IP: {account.LastIP}, New IP: {con.IPAddress}, Security: {p.CheckSum}");

                if (account.TempAdmin)
                {
                    con.Enqueue(new S.Login { Result = LoginResult.AlreadyLoggedInAdmin });
                    return;
                }

                if (account.LastIP != con.IPAddress && account.LastSum != p.CheckSum)
                {
                    account.Connection.TrySendDisconnect(new G.Disconnect { Reason = DisconnectReason.AnotherUserPassword });
                    string password = Functions.RandomString(Random, 10);

                    account.Password = CreateHash(password);
                    account.ResetKey = string.Empty;
                    account.WrongPasswordCount = 0;

                    SendResetPasswordEmail(account, password);

                    con.Enqueue(new S.Login { Result = LoginResult.AlreadyLoggedInPassword });
                    return;
                }

                con.Enqueue(new S.Login { Result = LoginResult.AlreadyLoggedIn });
                account.Connection.TrySendDisconnect(new G.Disconnect { Reason = DisconnectReason.AnotherUser });
                return;
            }


            account.Connection = con;
            account.TempAdmin = admin;

            con.Account = account;
            con.Stage = GameStage.Select;

            account.Key = Functions.RandomString(Random, 20);


            con.Enqueue(new S.Login
            {
                Result = LoginResult.Success,
                Characters = account.GetSelectInfo(),

                Items = account.Items.Select(x => x.ToClientInfo()).ToList(),
                BlockList = account.BlockingList.Select(x => x.ToClientInfo()).ToList(),

                Address = $"{Config.BuyAddress}?Key={account.Key}&Character=",

                TestServer = Config.TestServer,
            });

            account.LastLogin = Now;

            if (!admin)
            {
                account.LastIP = con.IPAddress;
                account.LastSum = p.CheckSum;
            }

            RemoveConnection(con.SessionID);
            authenticatedConnections.TryAdd(con.SessionID, con);

            Log($"[Account Logon] Admin: {admin}, Account: {account.EMailAddress}, IP Address: {account.LastIP}, Security: {p.CheckSum}");
        }

        public static void NewAccount(C.NewAccount p, SConnection con)
        {
            if (!Config.AllowNewAccount)
            {
                con.Enqueue(new S.NewAccount { Result = NewAccountResult.Disabled });
                return;
            }

            if (!Globals.EMailRegex.IsMatch(p.EMailAddress))
            {
                con.Enqueue(new S.NewAccount { Result = NewAccountResult.BadEMail });
                return;
            }

            if (!Globals.PasswordRegex.IsMatch(p.Password))
            {
                con.Enqueue(new S.NewAccount { Result = NewAccountResult.BadPassword });
                return;
            }

            if ((Globals.RealNameRequired || !string.IsNullOrEmpty(p.RealName)) && (p.RealName.Length < Globals.MinRealNameLength || p.RealName.Length > Globals.MaxRealNameLength))
            {
                con.Enqueue(new S.NewAccount { Result = NewAccountResult.BadRealName });
                return;
            }

            var list = AccountInfoList.Binding.Where(e => e.CreationIP == con.IPAddress).ToList();
            int nowcount = 0;
            int todaycount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                AccountInfo info = list[i];
                if (info == null)
                    continue;

                if (info.CreationDate.AddSeconds(1) > Now)
                {
                    nowcount++;
                    if (nowcount > 2)
                        break;
                }
                if (info.CreationDate.AddDays(1) > Now)
                {
                    todaycount++;
                    if (todaycount > 5)
                        break;
                }
            }
            if (nowcount > 2 || todaycount > 5)
            {
                ipBlockDictionary[con.IPAddress] = Now.AddDays(7);

                for (int i = AuthenticatedConnections.Count - 1; i >= 0; i--)
                    if (AuthenticatedConnections[i].IPAddress == con.IPAddress)
                        AuthenticatedConnections[i].TryDisconnect();

                Log($"{con.IPAddress} Disconnected and banned for trying too many accounts");
                return;
            }

            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].EMailAddress, p.EMailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    con.Enqueue(new S.NewAccount { Result = NewAccountResult.AlreadyExists });
                    return;
                }

            AccountInfo refferal = null;

            if (!string.IsNullOrEmpty(p.Referral))
            {
                if (!Globals.EMailRegex.IsMatch(p.Referral))
                {
                    con.Enqueue(new S.NewAccount { Result = NewAccountResult.BadReferral });
                    return;
                }

                for (int i = 0; i < AccountInfoList.Count; i++)
                    if (string.Compare(AccountInfoList[i].EMailAddress, p.Referral, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        refferal = AccountInfoList[i];
                        break;
                    }
                if (refferal == null)
                {
                    con.Enqueue(new S.NewAccount { Result = NewAccountResult.ReferralNotFound });
                    return;
                }
                if (!refferal.Activated)
                {
                    con.Enqueue(new S.NewAccount { Result = NewAccountResult.ReferralNotActivated });
                    return;
                }
            }

            AccountInfo account = AccountInfoList.CreateNewObject();

            account.EMailAddress = p.EMailAddress;
            account.Password = CreateHash(p.Password);
            account.RealName = p.RealName;
            account.BirthDate = p.BirthDate;
            account.Referral = refferal;
            account.CreationIP = con.IPAddress;
            account.CreationDate = Now;
            account.ReturningHuntGold = true;
            account.HuntGold = 250;

            if (refferal != null)
            {
                int maxLevel = refferal.HightestLevel();

                if (maxLevel >= 50)
                    account.HuntGold = 500;
                else if (maxLevel >= 40)
                    account.HuntGold = 300;
                else if (maxLevel >= 30)
                    account.HuntGold = 200;
                else if (maxLevel >= 20)
                    account.HuntGold = 100;
                else if (maxLevel >= 10)
                    account.HuntGold = 50;
            }



            SendActivationEmail(account);

            con.Enqueue(new S.NewAccount { Result = NewAccountResult.Success });

            Log($"[Account Created] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void ChangePassword(C.ChangePassword p, SConnection con)
        {
            if (!Config.AllowChangePassword)
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.Disabled });
                return;
            }

            if (!Globals.EMailRegex.IsMatch(p.EMailAddress))
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.BadEMail });
                return;
            }

            if (!Globals.PasswordRegex.IsMatch(p.CurrentPassword))
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.BadCurrentPassword });
                return;
            }

            if (!Globals.PasswordRegex.IsMatch(p.NewPassword))
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.BadNewPassword });
                return;
            }

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].EMailAddress, p.EMailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    account = AccountInfoList[i];
                    break;
                }


            if (account == null)
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.AccountNotFound });
                return;
            }
            if (!account.Activated)
            {
                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.AccountNotActivated });
                return;
            }

            if (account.Banned)
            {
                if (account.ExpiryDate > Now)
                {
                    con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.Banned, Message = account.BanReason, Duration = account.ExpiryDate - Now });
                    return;
                }

                account.Banned = false;
                account.BanReason = string.Empty;
                account.ExpiryDate = DateTime.MinValue;
            }

            if (!PasswordMatch(p.CurrentPassword, account.Password))
            {
                Log($"[Wrong Password] IP Address: {con.IPAddress}, Account: {account.EMailAddress}, Security: {p.CheckSum}");

                if (account.WrongPasswordCount++ >= 5)
                {
                    account.Banned = true;
                    account.BanReason = con.Language.BannedWrongPassword;
                    account.ExpiryDate = Now.AddMinutes(1);

                    con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.Banned, Message = account.BanReason, Duration = account.ExpiryDate - Now });
                    return;
                }

                con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.WrongPassword });
                return;
            }

            account.Password = CreateHash(p.NewPassword);
            SendChangePasswordEmail(account, con.IPAddress);
            con.Enqueue(new S.ChangePassword { Result = ChangePasswordResult.Success });

            Log($"[Password Changed] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void RequestPasswordReset(C.RequestPasswordReset p, SConnection con)
        {
            if (!Config.AllowRequestPasswordReset)
            {
                con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.Disabled });
                return;
            }

            if (!Globals.EMailRegex.IsMatch(p.EMailAddress))
            {
                con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.BadEMail });
                return;
            }

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].EMailAddress, p.EMailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    account = AccountInfoList[i];
                    break;
                }

            if (account == null)
            {
                con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.AccountNotFound });
                return;
            }

            if (!account.Activated)
            {
                con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.AccountNotActivated });
                return;
            }

            if (Now < account.ResetTime)
            {
                con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.ResetDelay, Duration = account.ResetTime - Now });
                return;
            }

            SendResetPasswordRequestEmail(account, con.IPAddress);
            con.Enqueue(new S.RequestPasswordReset { Result = RequestPasswordResetResult.Success });

            Log($"[Request Password] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void ResetPassword(C.ResetPassword p, SConnection con)
        {
            if (!Config.AllowManualResetPassword)
            {
                con.Enqueue(new S.ResetPassword { Result = ResetPasswordResult.Disabled });
                return;
            }

            if (!Globals.PasswordRegex.IsMatch(p.NewPassword))
            {
                con.Enqueue(new S.ResetPassword { Result = ResetPasswordResult.BadNewPassword });
                return;
            }

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].ResetKey, p.ResetKey, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    account = AccountInfoList[i];
                    break;
                }

            if (account == null)
            {
                con.Enqueue(new S.ResetPassword { Result = ResetPasswordResult.AccountNotFound });
                return;
            }

            if (account.ResetTime.AddMinutes(25) < Now)
            {
                con.Enqueue(new S.ResetPassword { Result = ResetPasswordResult.KeyExpired });
                return;
            }

            account.ResetKey = string.Empty;
            account.Password = CreateHash(p.NewPassword);
            account.WrongPasswordCount = 0;

            SendChangePasswordEmail(account, con.IPAddress);
            con.Enqueue(new S.ResetPassword { Result = ResetPasswordResult.Success });

            Log($"[Reset Password] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void Activation(C.Activation p, SConnection con)
        {
            if (!Config.AllowManualActivation)
            {
                con.Enqueue(new S.Activation { Result = ActivationResult.Disabled });
                return;
            }

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].ActivationKey, p.ActivationKey, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    account = AccountInfoList[i];
                    break;
                }

            if (account == null)
            {
                con.Enqueue(new S.Activation { Result = ActivationResult.AccountNotFound });
                return;
            }

            account.ActivationKey = null;
            account.Activated = true;

            con.Enqueue(new S.Activation { Result = ActivationResult.Success });

            Log($"[Activation] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void RequestActivationKey(C.RequestActivationKey p, SConnection con)
        {
            if (!Config.AllowRequestActivation)
            {
                con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.Disabled });
                return;
            }

            if (!Globals.EMailRegex.IsMatch(p.EMailAddress))
            {
                con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.BadEMail });
                return;
            }

            AccountInfo account = null;
            for (int i = 0; i < AccountInfoList.Count; i++)
                if (string.Compare(AccountInfoList[i].EMailAddress, p.EMailAddress, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    account = AccountInfoList[i];
                    break;
                }

            if (account == null)
            {
                con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.AccountNotFound });
                return;
            }

            if (account.Activated)
            {
                con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.AlreadyActivated });
                return;
            }

            if (Now < account.ActivationTime)
            {
                con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.RequestDelay, Duration = account.ActivationTime - Now });
                return;
            }
            ResendActivationEmail(account);
            con.Enqueue(new S.RequestActivationKey { Result = RequestActivationKeyResult.Success });
            Log($"[Request Activation] Account: {account.EMailAddress}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }

        public static void NewCharacter(C.NewCharacter p, SConnection con)
        {
            if (!Config.AllowNewCharacter)
            {
                con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.Disabled });
                return;
            }

            if (!Globals.CharacterReg.IsMatch(p.CharacterName))
            {
                con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadCharacterName });
                return;
            }

            switch (p.Gender)
            {
                case MirGender.Male:
                case MirGender.Female:
                    break;
                default:
                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadGender });
                    return;
            }

            if (p.HairType < 0)
            {
                con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairType });
                return;
            }

            if ((p.HairType == 0 && p.HairColour.ToArgb() != 0) || (p.HairType != 0 && p.HairColour.A != 255))
            {
                con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairColour });
                return;
            }


            switch (p.Class)
            {
                case MirClass.Warrior:
                    if (p.HairType > (p.Gender == MirGender.Male ? 10 : 11))
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairType });
                        return;
                    }

                    if (p.ArmourColour.A != 255)
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadArmourColour });
                        return;
                    }
                    if (Config.AllowWarrior)
                        break;

                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.ClassDisabled });

                    return;
                case MirClass.Wizard:
                    if (p.HairType > (p.Gender == MirGender.Male ? 10 : 11))
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairType });
                        return;
                    }

                    if (p.ArmourColour.A != 255)
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadArmourColour });
                        return;
                    }
                    if (Config.AllowWizard)
                        break;

                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.ClassDisabled });
                    return;
                case MirClass.Taoist:
                    if (p.HairType > (p.Gender == MirGender.Male ? 10 : 11))
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairType });
                        return;
                    }

                    if (p.ArmourColour.A != 255)
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadArmourColour });
                        return;
                    }
                    if (Config.AllowTaoist)
                        break;

                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.ClassDisabled });
                    return;
                case MirClass.Assassin:

                    if (p.HairType > 5)
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadHairType });
                        return;
                    }

                    if (p.ArmourColour.ToArgb() != 0)
                    {
                        con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadArmourColour });
                        return;
                    }

                    if (Config.AllowAssassin)
                        break;

                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.ClassDisabled });
                    return;
                default:
                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.BadClass });
                    return;
            }



            int count = 0;

            foreach (CharacterInfo character in con.Account.Characters)
            {
                if (character.Deleted)
                    continue;

                if (++count < Globals.MaxCharacterCount)
                    continue;

                con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.MaxCharacters });
                return;
            }


            for (int i = 0; i < CharacterInfoList.Count; i++)
                if (string.Compare(CharacterInfoList[i].CharacterName, p.CharacterName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (CharacterInfoList[i].Account == con.Account)
                        continue;

                    con.Enqueue(new S.NewCharacter { Result = NewCharacterResult.AlreadyExists });
                    return;
                }

            CharacterInfo cInfo = CharacterInfoList.CreateNewObject();

            cInfo.CharacterName = p.CharacterName;
            cInfo.Account = con.Account;
            cInfo.Class = p.Class;
            cInfo.Gender = p.Gender;
            cInfo.HairType = p.HairType;
            cInfo.HairColour = p.HairColour;
            cInfo.ArmourColour = p.ArmourColour;
            cInfo.CreationIP = con.IPAddress;
            cInfo.CreationDate = Now;

            cInfo.RankingNode = Rankings.AddLast(cInfo);

            con.Enqueue(new S.NewCharacter
            {
                Result = NewCharacterResult.Success,
                Character = cInfo.ToSelectInfo(),
            });

            if (cInfo.Account.Characters.Count == 1 && cInfo.Account.GuildMember == null)
            {
                cInfo.Account.LastCharacter = cInfo;
                GuildMemberInfo memberInfo = SEnvir.GuildMemberInfoList.CreateNewObject();

                memberInfo.Account = cInfo.Account;
                memberInfo.Guild = SEnvir.StarterGuild;
                memberInfo.Rank = SEnvir.StarterGuild.DefaultRank;
                memberInfo.JoinDate = SEnvir.Now;
                memberInfo.Permission = SEnvir.StarterGuild.DefaultPermission;
                cInfo.Account.LastCharacter.LastLogin = SEnvir.Now;


                S.GuildUpdate update = memberInfo.Guild.GetUpdatePacket();

                update.Members.Add(memberInfo.ToClientInfo());

                foreach (GuildMemberInfo member in memberInfo.Guild.Members)
                {
                    if (member == memberInfo || member.Account.Connection?.Player == null)
                        continue;

                    member.Account.Connection.ReceiveChat(string.Format(member.Account.Connection.Language.GuildMemberJoined, SEnvir.StarterGuild, cInfo.CharacterName), MessageType.System);
                    member.Account.Connection.Player.Enqueue(update);

                    member.Account.Connection.Player.AddAllObjects();
                }
            }

            Log($"[Character Created] Character: {p.CharacterName}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
        }
        public static void DeleteCharacter(C.DeleteCharacter p, SConnection con)
        {
            if (!Config.AllowDeleteCharacter)
            {
                con.Enqueue(new S.DeleteCharacter { Result = DeleteCharacterResult.Disabled });
                return;
            }

            foreach (CharacterInfo character in con.Account.Characters)
            {
                if (character.Index != p.CharacterIndex)
                    continue;

                if (character.Deleted)
                {
                    con.Enqueue(new S.DeleteCharacter { Result = DeleteCharacterResult.AlreadyDeleted });
                    return;
                }

                character.Deleted = true;
                con.Enqueue(new S.DeleteCharacter { Result = DeleteCharacterResult.Success, DeletedIndex = character.Index });

                Log($"[Character Deleted] Character: {character.CharacterName}, IP Address: {con.IPAddress}, Security: {p.CheckSum}");
                return;
            }

            con.Enqueue(new S.DeleteCharacter { Result = DeleteCharacterResult.NotFound });
        }
        public static void StartGame(C.StartGame p, SConnection con)
        {
            if (!Config.AllowStartGame)
            {
                con.Enqueue(new S.StartGame { Result = StartGameResult.Disabled });
                return;
            }

            foreach (CharacterInfo character in con.Account.Characters)
            {
                if (character.Index != p.CharacterIndex)
                    continue;

                if (character.Deleted)
                {
                    con.Enqueue(new S.StartGame { Result = StartGameResult.Deleted });
                    return;
                }

                TimeSpan duration = Now - character.LastLogin;

                if (duration < Config.RelogDelay)
                {
                    con.Enqueue(new S.StartGame { Result = StartGameResult.Delayed, Duration = Config.RelogDelay - duration });
                    return;
                }

                PlayerObject player = new PlayerObject(character, con);
                player.StartGame();
                return;
            }

            con.Enqueue(new S.StartGame { Result = StartGameResult.NotFound });
        }

        public static bool IsBlocking(AccountInfo account1, AccountInfo account2)
        {
            if (account1 == null || account2 == null || account1 == account2)
                return false;

            if (account1.TempAdmin || account2.TempAdmin)
                return false;

            foreach (BlockInfo blockInfo in account1.BlockingList)
                if (blockInfo.BlockedAccount == account2)
                    return true;

            foreach (BlockInfo blockInfo in account2.BlockingList)
                if (blockInfo.BlockedAccount == account1)
                    return true;

            return false;
        }

        private static void SendActivationEmail(AccountInfo account)
        {
            account.ActivationKey = Functions.RandomString(Random, 20);
            account.ActivationTime = Now.AddMinutes(5);

            try
            {
                MailMessage message = new MailMessage(new MailAddress(Config.MailFrom, Config.MailDisplayName), new MailAddress(account.EMailAddress))
                {
                    Subject = "Ancient Account Activation",
                    IsBodyHtml = true,
                    Body = $"<html>" +
                               $"<style>" +
                               "body {" +
                               "padding:0px !important;" +
                               "margin: 0px !important;" +
                               "display: block!important;" +
                               "min - width:100 % !important;" +
                               "width: 100 % !important;" +
                               "background:#f4f4f4;" +
                               "-webkit - text - size - adjust:none;" +
                               "}" +
                               "a {" +
"color:#66c7ff;" +
"text-decoration:none;" +
"}" +
"p {" +
"padding:0 !important;" +
"margin:0 !important;" +
"}" +
"img {" +
"-ms-interpolation-mode:bicubic;" +
"display:block;" +
"margin-left:auto;" +
"margin-right:auto;" +
"}" +
".mcnPreviewText {" +
"display:none !important;" +
"}" +
".cke_editable,.cke_editable a,.cke_editable span,.cke_editable a span {" +
"color:#000001 !important;" +
"}" +
"@media only screen and (max-device-width: 480px),only screen and (max-width: 480px) {" +
".mobile-shell {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".bg {" +
"background-size:100% auto !important;" +
"-webkit-background-size:100% auto !important;" +
"}" +
".text-header,.m-center {" +
"text-align:center !important;" +
"}" +
".center {" +
"margin:0 auto !important;" +
"}" +
".container {" +
"padding:0 10px 10px !important;" +
"}" +
".td {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".text-nav {" +
"line-height:28px !important;" +
"}" +
".p30 {" +
"padding:15px !important;" +
"border-radius:25px;" +
"}" +
".m-br-15 {" +
"height:15px !important;" +
"}" +
".p30-15 {" +
"padding:30px 15px !important;" +
"}" +
".p40 {" +
"padding:20px !important;" +
"}" +
".m-td,.m-hide {" +
"display:none !important;" +
"width:0 !important;" +
"height:0 !important;" +
"font-size:0 !important;" +
"line-height:0 !important;" +
"min-height:0 !important;" +
"}" +
".m-block {" +
"display:block !important;" +
"}" +
".fluid-img img {" +
"width:100% !important;" +
"max-width:100% !important;" +
"height:auto !important;" +
"}" +
".column,.column-top,.column-empty,.column-empty2,.column-dir-top {" +
"float:left !important;" +
"width:100% !important;" +
"display:block !important;" +
"}" +
".column-empty {" +
"padding-bottom:10px !important;" +
"}" +
".column-empty2 {" +
"padding-bottom:20px !important;" +
"}" +
".content-spacing {" +
"width:15px !important;" +
"}" +
"}" +
                               "</style>" +

$"<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important;width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;'>" +
$"<span class='mcnPreviewText' style='display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;'></span>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#f4f4f4'>" +
$"<tr>" +
$"<td align='center' valign='top'>" +
$"<table width='650' border='0' cellspacing='0' cellpadding='0' class='mobile-shell'>" +
$"<tr>" +
$"<td class='td container' style='width:650px;min-width:650px;font-size:0pt;line-height:0pt;margin:0;font-weight:normal;padding:0px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<th class='column-top' width='145' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img m-center' style='font-size:0pt;line-height:0pt;text-align:left;'><img src='http://ancientservers.co.uk/Media/Amirlogo.png' width='230' height='90' mc:edit='image_1' style='max-width:168px;' border='0' alt='Clairvoyant.co'>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"<th class='column-empty' width='1' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'></ th > " +
$"<th class='column' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-header' style='color:#999999;font-family:Roboto;font-size:12px;line-height:16px;text-align:right;'>" +
$"<div mc:edit='text_1'>" +
$"<a href='mailto:support@ancientservers.co.uk' target='_blank' class='link-white' style='color:#999999;text-decoration:none;'>Contact us</a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<div mc:repeatable = 'Select' mc: variant = 'Nav' > " +
$"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '0' > " +
$"<tr>" +
$"<td class='p30-15' style='padding:10px 30px; border-top-left-radius: 5px; border-top-right-radius: 5px;'bgcolor='#28587a' align='center'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-nav' style='color:#ffffff;font-family:Roboto, roboto;font-size:14px;line-height:18px;text-align:center;font-weight:bold;text-transform:uppercase;'>" +
$"<div mc:edit='text_2'>" +
$"<a href='https://www.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Home</span></a>" +
"&emsp;" +
$"<a href='http://downloads.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Downloads</span></a>" +
"&emsp;" +
$"<a href='https://discord.gg/fveqkej' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Discord</span></a>" +
"&emsp;" +
$"<a href='https://www.lomcn.org/forum/forums/ancient-mir-3-server.762/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>LOMCN</span></a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='Hero'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='fluid-img' style='font-size:0pt;line-height:0pt;text-align:Center;'><img src ='https://www.ancientservers.co.uk/Media/260012-blackangel.jpg' width='650' height='350' mc:edit='image_3' style='max-width:650px;' border='0' alt=''>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='CTA'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#28587a'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='h2 white center pb20' style='font-family:Roboto, roboto;font-size:28px;line-height:34px;color:#ffffff;text-align:center;padding-bottom:20px;'>" +
$"<div mc:edit='text_32'>Dear {account.RealName},</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text white center pb30' style='font-family:Roboto, roboto;font-size:15px;line-height:28px;color:#ffffff;text-align:center;padding-bottom:30px;'>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>Thank you for registering a Ancient account, before you can log in to the game, you are required to activate your account." +
$"<br>" +
$"<br>To complete your registration and activate the account please use the following Activation Key when you next attempt to log in to your account</div>" +
$"<br>Activation Key: {account.ActivationKey}" +
$"<br>" +
$"<br><div style='color: #ffffff;'>If you did not create this account and want to cancel the registration to delete this account please visit the following link:<br>" +
$"<br>" +
$"<br></div>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>We'll see you in game<span class='m-hide'>" +
$"<br><br></span> <a href =\'http://www.Ancientservers.co.uk\' style='color: #f4f4f4;'>Ancient Servers</a></div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;' bgcolor='#F4F4F4'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td align='center' style='padding-bottom:30px;'>" +
$"<table border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://www.facebook.com/Ancientmir' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/c8182611-9ab1-4f39-853b-9f6c2df8851e.png' width='38' height='38' mc:edit='image_12' style='max-width:38px;' border='0' alt=''></a>" +
$"</td>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://twitter.com/Ancientservers' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/425bf9b5-ff40-4c3a-86cf-efad5461d318.png' width='38' height='38' mc:edit='image_13' style='max-width:38px;' border='0' alt='' class='center'></a>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer1 pb10' style='color:#999999;font-family:Roboto, roboto;font-size:14px;line-height:20px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_35'>© 2019 Ancient Servers</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer2 pb30' style='color:#999999;font-family:Roboto, roboto;font-size:12px;line-height:26px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_36'>All rights reserved.</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer3' style='color:#c0c0c0;font-family:Roboto;font-size:12px;line-height:18px;text-align:center;'>" +
$"<div mc:edit='text_38'>" +
$"<a class='link3-u' target='_blank' href='terms-conditions' style='color:#c0c0c0;text-decoration:underline;'>Terms & Conditions</a> " +
$"</div>" + "</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='img' style='font-size:0pt;line-height:0pt;text-align:left;'>" +
$"<div mc:edit='text_39'>" +
$"</div>" +
$"</td>" +
$"<tr>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</body>" +
$"</html>"
                };

                AddMailToQueue(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void ResendActivationEmail(AccountInfo account)
        {
            if (string.IsNullOrEmpty(account.ActivationKey))
                account.ActivationKey = Functions.RandomString(Random, 20);

            account.ActivationTime = Now.AddMinutes(15);

            try
            {
                MailMessage message = new MailMessage(new MailAddress(Config.MailFrom, Config.MailDisplayName), new MailAddress(account.EMailAddress))
                {
                    Subject = "Ancient Account Activation",
                    IsBodyHtml = false,

                    Body = $"Dear {account.RealName}\n" +
                           $"\n" +
                           $"Thank you for registering a Ancient account, before you can log in to the game, you are required to activate your account.\n" +
                           $"\n" +
                           $"Please use the following Activation Key when you next attempt to log in to your account\n" +
                           $"Activation Key: {account.ActivationKey}\n\n" +
                           $"We'll see you in game\n" +
                           $"Ancient Server\n" +
                           $"\n" +
                           $"This E-Mail has been sent without formatting to reduce failure",
                };

                AddMailToQueue(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void SendChangePasswordEmail(AccountInfo account, string ipAddress)
        {
            if (Now < account.PasswordTime)
                return;

            account.PasswordTime = Time.Now.AddMinutes(60);


            try
            {
                MailMessage message = new MailMessage(new MailAddress(Config.MailFrom, Config.MailDisplayName), new MailAddress(account.EMailAddress))
                {
                    Subject = "Ancient Password Changed",
                    IsBodyHtml = true,

                    Body = $"<html>" +
                               $"<style>" +
                               "body {" +
                               "padding:0px !important;" +
                               "margin: 0px !important;" +
                               "display: block!important;" +
                               "min - width:100 % !important;" +
                               "width: 100 % !important;" +
                               "background:#f4f4f4;" +
                               "-webkit - text - size - adjust:none;" +
                               "}" +
                               "a {" +
"color:#66c7ff;" +
"text-decoration:none;" +
"}" +
"p {" +
"padding:0 !important;" +
"margin:0 !important;" +
"}" +
"img {" +
"-ms-interpolation-mode:bicubic;" +
"display:block;" +
"margin-left:auto;" +
"margin-right:auto;" +
"}" +
".mcnPreviewText {" +
"display:none !important;" +
"}" +
".cke_editable,.cke_editable a,.cke_editable span,.cke_editable a span {" +
"color:#000001 !important;" +
"}" +
"@media only screen and (max-device-width: 480px),only screen and (max-width: 480px) {" +
".mobile-shell {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".bg {" +
"background-size:100% auto !important;" +
"-webkit-background-size:100% auto !important;" +
"}" +
".text-header,.m-center {" +
"text-align:center !important;" +
"}" +
".center {" +
"margin:0 auto !important;" +
"}" +
".container {" +
"padding:0 10px 10px !important;" +
"}" +
".td {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".text-nav {" +
"line-height:28px !important;" +
"}" +
".p30 {" +
"padding:15px !important;" +
"border-radius:25px;" +
"}" +
".m-br-15 {" +
"height:15px !important;" +
"}" +
".p30-15 {" +
"padding:30px 15px !important;" +
"}" +
".p40 {" +
"padding:20px !important;" +
"}" +
".m-td,.m-hide {" +
"display:none !important;" +
"width:0 !important;" +
"height:0 !important;" +
"font-size:0 !important;" +
"line-height:0 !important;" +
"min-height:0 !important;" +
"}" +
".m-block {" +
"display:block !important;" +
"}" +
".fluid-img img {" +
"width:100% !important;" +
"max-width:100% !important;" +
"height:auto !important;" +
"}" +
".column,.column-top,.column-empty,.column-empty2,.column-dir-top {" +
"float:left !important;" +
"width:100% !important;" +
"display:block !important;" +
"}" +
".column-empty {" +
"padding-bottom:10px !important;" +
"}" +
".column-empty2 {" +
"padding-bottom:20px !important;" +
"}" +
".content-spacing {" +
"width:15px !important;" +
"}" +
"}" +
                               "</style>" +

$"<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important;width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;'>" +
$"<span class='mcnPreviewText' style='display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;'></span>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#f4f4f4'>" +
$"<tr>" +
$"<td align='center' valign='top'>" +
$"<table width='650' border='0' cellspacing='0' cellpadding='0' class='mobile-shell'>" +
$"<tr>" +
$"<td class='td container' style='width:650px;min-width:650px;font-size:0pt;line-height:0pt;margin:0;font-weight:normal;padding:0px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<th class='column-top' width='145' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img m-center' style='font-size:0pt;line-height:0pt;text-align:left;'><img src='http://ancientservers.co.uk/Media/Amirlogo.png' width='230' height='90' mc:edit='image_1' style='max-width:168px;' border='0' alt='Clairvoyant.co'>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"<th class='column-empty' width='1' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'></ th > " +
$"<th class='column' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-header' style='color:#999999;font-family:Roboto;font-size:12px;line-height:16px;text-align:right;'>" +
$"<div mc:edit='text_1'>" +
$"<a href='mailto:support@ancientservers.co.uk' target='_blank' class='link-white' style='color:#999999;text-decoration:none;'>Contact us</a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<div mc:repeatable = 'Select' mc: variant = 'Nav' > " +
$"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '0' > " +
$"<tr>" +
$"<td class='p30-15' style='padding:10px 30px; border-top-left-radius: 5px; border-top-right-radius: 5px;'bgcolor='#28587a' align='center'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-nav' style='color:#ffffff;font-family:Roboto, roboto;font-size:14px;line-height:18px;text-align:center;font-weight:bold;text-transform:uppercase;'>" +
$"<div mc:edit='text_2'>" +
$"<a href='https://www.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Home</span></a>" +
"&emsp;" +
$"<a href='http://downloads.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Downloads</span></a>" +
"&emsp;" +
$"<a href='https://discord.gg/fveqkej' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Discord</span></a>" +
"&emsp;" +
$"<a href='https://www.lomcn.org/forum/forums/ancient-mir-3-server.762/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>LOMCN</span></a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='Hero'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='fluid-img' style='font-size:0pt;line-height:0pt;text-align:Center;'><img src ='https://www.ancientservers.co.uk/Media/260012-blackangel.jpg' width='650' height='350' mc:edit='image_3' style='max-width:650px;' border='0' alt=''>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='CTA'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#28587a'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='h2 white center pb20' style='font-family:Roboto, roboto;font-size:28px;line-height:34px;color:#ffffff;text-align:center;padding-bottom:20px;'>" +
$"<div mc:edit='text_32'>Dear {account.RealName},</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text white center pb30' style='font-family:Roboto, roboto;font-size:15px;line-height:28px;color:#ffffff;text-align:center;padding-bottom:30px;'>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>This is an E-Mail to inform you that your password for Ancient Mir has been changed." +
$"<br>" +
$"<br>IP Address: {ipAddress}</div>" +
$"<br>If you did not make this change please contact an administrator immediately." +
$"<br>" +
$"<br><div style='color: #ffffff;'>If you did not create this account and want to cancel the registration to delete this account please visit the following link:<br>" +
$"<br>" +
$"<br></div>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>We'll see you in game<span class='m-hide'>" +
$"<br><br></span> <a href =\'http://www.Ancientservers.co.uk\' style='color: #f4f4f4;'>Ancient Servers</a></div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;' bgcolor='#F4F4F4'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td align='center' style='padding-bottom:30px;'>" +
$"<table border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://www.facebook.com/Ancientmir' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/c8182611-9ab1-4f39-853b-9f6c2df8851e.png' width='38' height='38' mc:edit='image_12' style='max-width:38px;' border='0' alt=''></a>" +
$"</td>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://twitter.com/Ancientservers' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/425bf9b5-ff40-4c3a-86cf-efad5461d318.png' width='38' height='38' mc:edit='image_13' style='max-width:38px;' border='0' alt='' class='center'></a>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer1 pb10' style='color:#999999;font-family:Roboto, roboto;font-size:14px;line-height:20px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_35'>© 2019 Ancient Servers</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer2 pb30' style='color:#999999;font-family:Roboto, roboto;font-size:12px;line-height:26px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_36'>All rights reserved.</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer3' style='color:#c0c0c0;font-family:Roboto;font-size:12px;line-height:18px;text-align:center;'>" +
$"<div mc:edit='text_38'>" +
$"<a class='link3-u' target='_blank' href='terms-conditions' style='color:#c0c0c0;text-decoration:underline;'>Terms & Conditions</a> " +
$"</div>" + "</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='img' style='font-size:0pt;line-height:0pt;text-align:left;'>" +
$"<div mc:edit='text_39'>" +
$"</div>" +
$"</td>" +
$"<tr>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</body>" +
$"</html>"
                };

                AddMailToQueue(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void SendResetPasswordRequestEmail(AccountInfo account, string ipAddress)
        {
            account.ResetKey = Functions.RandomString(Random, 20);
            account.ResetTime = Now.AddMinutes(5);

            try
            {

                MailMessage message = new MailMessage(new MailAddress(Config.MailFrom, Config.MailDisplayName), new MailAddress(account.EMailAddress))
                {
                    Subject = "Ancient Password Reset",
                    IsBodyHtml = true,

                    Body = $"<html>" +
                               $"<style>" +
                               "body {" +
                               "padding:0px !important;" +
                               "margin: 0px !important;" +
                               "display: block!important;" +
                               "min - width:100 % !important;" +
                               "width: 100 % !important;" +
                               "background:#f4f4f4;" +
                               "-webkit - text - size - adjust:none;" +
                               "}" +
                               "a {" +
"color:#66c7ff;" +
"text-decoration:none;" +
"}" +
"p {" +
"padding:0 !important;" +
"margin:0 !important;" +
"}" +
"img {" +
"-ms-interpolation-mode:bicubic;" +
"display:block;" +
"margin-left:auto;" +
"margin-right:auto;" +
"}" +
".mcnPreviewText {" +
"display:none !important;" +
"}" +
".cke_editable,.cke_editable a,.cke_editable span,.cke_editable a span {" +
"color:#000001 !important;" +
"}" +
"@media only screen and (max-device-width: 480px),only screen and (max-width: 480px) {" +
".mobile-shell {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".bg {" +
"background-size:100% auto !important;" +
"-webkit-background-size:100% auto !important;" +
"}" +
".text-header,.m-center {" +
"text-align:center !important;" +
"}" +
".center {" +
"margin:0 auto !important;" +
"}" +
".container {" +
"padding:0 10px 10px !important;" +
"}" +
".td {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".text-nav {" +
"line-height:28px !important;" +
"}" +
".p30 {" +
"padding:15px !important;" +
"border-radius:25px;" +
"}" +
".m-br-15 {" +
"height:15px !important;" +
"}" +
".p30-15 {" +
"padding:30px 15px !important;" +
"}" +
".p40 {" +
"padding:20px !important;" +
"}" +
".m-td,.m-hide {" +
"display:none !important;" +
"width:0 !important;" +
"height:0 !important;" +
"font-size:0 !important;" +
"line-height:0 !important;" +
"min-height:0 !important;" +
"}" +
".m-block {" +
"display:block !important;" +
"}" +
".fluid-img img {" +
"width:100% !important;" +
"max-width:100% !important;" +
"height:auto !important;" +
"}" +
".column,.column-top,.column-empty,.column-empty2,.column-dir-top {" +
"float:left !important;" +
"width:100% !important;" +
"display:block !important;" +
"}" +
".column-empty {" +
"padding-bottom:10px !important;" +
"}" +
".column-empty2 {" +
"padding-bottom:20px !important;" +
"}" +
".content-spacing {" +
"width:15px !important;" +
"}" +
"}" +
                               "</style>" +

$"<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important;width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;'>" +
$"<span class='mcnPreviewText' style='display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;'></span>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#f4f4f4'>" +
$"<tr>" +
$"<td align='center' valign='top'>" +
$"<table width='650' border='0' cellspacing='0' cellpadding='0' class='mobile-shell'>" +
$"<tr>" +
$"<td class='td container' style='width:650px;min-width:650px;font-size:0pt;line-height:0pt;margin:0;font-weight:normal;padding:0px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<th class='column-top' width='145' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img m-center' style='font-size:0pt;line-height:0pt;text-align:left;'><img src='http://ancientservers.co.uk/Media/Amirlogo.png' width='230' height='90' mc:edit='image_1' style='max-width:168px;' border='0' alt='Clairvoyant.co'>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"<th class='column-empty' width='1' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'></ th > " +
$"<th class='column' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-header' style='color:#999999;font-family:Roboto;font-size:12px;line-height:16px;text-align:right;'>" +
$"<div mc:edit='text_1'>" +
$"<a href='mailto:support@ancientservers.co.uk' target='_blank' class='link-white' style='color:#999999;text-decoration:none;'>Contact us</a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<div mc:repeatable = 'Select' mc: variant = 'Nav' > " +
$"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '0' > " +
$"<tr>" +
$"<td class='p30-15' style='padding:10px 30px; border-top-left-radius: 5px; border-top-right-radius: 5px;'bgcolor='#28587a' align='center'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-nav' style='color:#ffffff;font-family:Roboto, roboto;font-size:14px;line-height:18px;text-align:center;font-weight:bold;text-transform:uppercase;'>" +
$"<div mc:edit='text_2'>" +
$"<a href='https://www.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Home</span></a>" +
"&emsp;" +
$"<a href='http://downloads.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Downloads</span></a>" +
"&emsp;" +
$"<a href='https://discord.gg/fveqkej' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Discord</span></a>" +
"&emsp;" +
$"<a href='https://www.lomcn.org/forum/forums/ancient-mir-3-server.762/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>LOMCN</span></a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='Hero'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='fluid-img' style='font-size:0pt;line-height:0pt;text-align:Center;'><img src ='https://www.ancientservers.co.uk/Media/260012-blackangel.jpg' width='650' height='350' mc:edit='image_3' style='max-width:650px;' border='0' alt=''>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='CTA'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#28587a'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='h2 white center pb20' style='font-family:Roboto, roboto;font-size:28px;line-height:34px;color:#ffffff;text-align:center;padding-bottom:20px;'>" +
$"<div mc:edit='text_32'>Dear {account.RealName},</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text white center pb30' style='font-family:Roboto, roboto;font-size:15px;line-height:28px;color:#ffffff;text-align:center;padding-bottom:30px;'>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>A request to reset your password has been made." +
$"<br>" +
$"<br>IP Address: {ipAddress}" +
$"<br>If the above link does not work please use the following Reset Key to reset your password" +
$"<br>" +
$"<br><div style='color: #ffffff;'>Reset Key: {account.ResetKey}<br>" +
$"<br>" +
$"If you did not request this reset, please ignore this email as your password will not be changed.<br>" +
$"<br></div>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>We'll see you in game<span class='m-hide'>" +
$"<br><br></span> <a href =\'http://www.Ancientservers.co.uk\' style='color: #f4f4f4;'>Ancient Servers</a></div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;' bgcolor='#F4F4F4'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td align='center' style='padding-bottom:30px;'>" +
$"<table border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://www.facebook.com/Ancientmir' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/c8182611-9ab1-4f39-853b-9f6c2df8851e.png' width='38' height='38' mc:edit='image_12' style='max-width:38px;' border='0' alt=''></a>" +
$"</td>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://twitter.com/Ancientservers' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/425bf9b5-ff40-4c3a-86cf-efad5461d318.png' width='38' height='38' mc:edit='image_13' style='max-width:38px;' border='0' alt='' class='center'></a>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer1 pb10' style='color:#999999;font-family:Roboto, roboto;font-size:14px;line-height:20px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_35'>© 2019 Ancient Servers</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer2 pb30' style='color:#999999;font-family:Roboto, roboto;font-size:12px;line-height:26px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_36'>All rights reserved.</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer3' style='color:#c0c0c0;font-family:Roboto;font-size:12px;line-height:18px;text-align:center;'>" +
$"<div mc:edit='text_38'>" +
$"<a class='link3-u' target='_blank' href='terms-conditions' style='color:#c0c0c0;text-decoration:underline;'>Terms & Conditions</a> " +
$"</div>" + "</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='img' style='font-size:0pt;line-height:0pt;text-align:left;'>" +
$"<div mc:edit='text_39'>" +
$"</div>" +
$"</td>" +
$"<tr>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</body>" +
$"</html>"
                };

                AddMailToQueue(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void SendResetPasswordEmail(AccountInfo account, string password)
        {
            account.ResetKey = Functions.RandomString(Random, 20);
            account.ResetTime = Now.AddMinutes(5);


            try
            {

                MailMessage message = new MailMessage(new MailAddress(Config.MailFrom, Config.MailDisplayName), new MailAddress(account.EMailAddress))
                {
                    Subject = "Ancient Password has been Reset.",
                    IsBodyHtml = true,

                    Body = $"<html>" +
                               $"<style>" +
                               "body {" +
                               "padding:0px !important;" +
                               "margin: 0px !important;" +
                               "display: block!important;" +
                               "min - width:100 % !important;" +
                               "width: 100 % !important;" +
                               "background:#f4f4f4;" +
                               "-webkit - text - size - adjust:none;" +
                               "}" +
                               "a {" +
"color:#66c7ff;" +
"text-decoration:none;" +
"}" +
"p {" +
"padding:0 !important;" +
"margin:0 !important;" +
"}" +
"img {" +
"-ms-interpolation-mode:bicubic;" +
"display:block;" +
"margin-left:auto;" +
"margin-right:auto;" +
"}" +
".mcnPreviewText {" +
"display:none !important;" +
"}" +
".cke_editable,.cke_editable a,.cke_editable span,.cke_editable a span {" +
"color:#000001 !important;" +
"}" +
"@media only screen and (max-device-width: 480px),only screen and (max-width: 480px) {" +
".mobile-shell {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".bg {" +
"background-size:100% auto !important;" +
"-webkit-background-size:100% auto !important;" +
"}" +
".text-header,.m-center {" +
"text-align:center !important;" +
"}" +
".center {" +
"margin:0 auto !important;" +
"}" +
".container {" +
"padding:0 10px 10px !important;" +
"}" +
".td {" +
"width:100% !important;" +
"min-width:100% !important;" +
"}" +
".text-nav {" +
"line-height:28px !important;" +
"}" +
".p30 {" +
"padding:15px !important;" +
"border-radius:25px;" +
"}" +
".m-br-15 {" +
"height:15px !important;" +
"}" +
".p30-15 {" +
"padding:30px 15px !important;" +
"}" +
".p40 {" +
"padding:20px !important;" +
"}" +
".m-td,.m-hide {" +
"display:none !important;" +
"width:0 !important;" +
"height:0 !important;" +
"font-size:0 !important;" +
"line-height:0 !important;" +
"min-height:0 !important;" +
"}" +
".m-block {" +
"display:block !important;" +
"}" +
".fluid-img img {" +
"width:100% !important;" +
"max-width:100% !important;" +
"height:auto !important;" +
"}" +
".column,.column-top,.column-empty,.column-empty2,.column-dir-top {" +
"float:left !important;" +
"width:100% !important;" +
"display:block !important;" +
"}" +
".column-empty {" +
"padding-bottom:10px !important;" +
"}" +
".column-empty2 {" +
"padding-bottom:20px !important;" +
"}" +
".content-spacing {" +
"width:15px !important;" +
"}" +
"}" +
                               "</style>" +

$"<body class='body' style='padding:0 !important; margin:0 !important; display:block !important; min-width:100% !important;width:100% !important; background:#f4f4f4; -webkit-text-size-adjust:none;'>" +
$"<span class='mcnPreviewText' style='display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;'></span>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#f4f4f4'>" +
$"<tr>" +
$"<td align='center' valign='top'>" +
$"<table width='650' border='0' cellspacing='0' cellpadding='0' class='mobile-shell'>" +
$"<tr>" +
$"<td class='td container' style='width:650px;min-width:650px;font-size:0pt;line-height:0pt;margin:0;font-weight:normal;padding:0px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 0px 40px 0px;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<th class='column-top' width='145' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img m-center' style='font-size:0pt;line-height:0pt;text-align:left;'><img src='http://ancientservers.co.uk/Media/Amirlogo.png' width='230' height='90' mc:edit='image_1' style='max-width:168px;' border='0' alt='Clairvoyant.co'>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"<th class='column-empty' width='1' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top;'></ th > " +
$"<th class='column' style='font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-header' style='color:#999999;font-family:Roboto;font-size:12px;line-height:16px;text-align:right;'>" +
$"<div mc:edit='text_1'>" +
$"<a href='mailto:support@ancientservers.co.uk' target='_blank' class='link-white' style='color:#999999;text-decoration:none;'>Contact us</a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</th>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<div mc:repeatable = 'Select' mc: variant = 'Nav' > " +
$"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '0' > " +
$"<tr>" +
$"<td class='p30-15' style='padding:10px 30px; border-top-left-radius: 5px; border-top-right-radius: 5px;'bgcolor='#28587a' align='center'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='text-nav' style='color:#ffffff;font-family:Roboto, roboto;font-size:14px;line-height:18px;text-align:center;font-weight:bold;text-transform:uppercase;'>" +
$"<div mc:edit='text_2'>" +
$"<a href='https://www.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Home</span></a>" +
"&emsp;" +
$"<a href='http://downloads.ancientservers.co.uk/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Downloads</span></a>" +
"&emsp;" +
$"<a href='https://discord.gg/fveqkej' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>Discord</span></a>" +
"&emsp;" +
$"<a href='https://www.lomcn.org/forum/forums/ancient-mir-3-server.762/' target='_blank' class='link-white' style='color:#ffffff;text-decoration:none;'><span class='link-white' style='color:#ffffff; text-decoration:none; cellpadding:10;'>LOMCN</span></a>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='Hero'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='fluid-img' style='font-size:0pt;line-height:0pt;text-align:Center;'><img src ='https://www.ancientservers.co.uk/Media/260012-blackangel.jpg' width='650' height='350' mc:edit='image_3' style='max-width:650px;' border='0' alt=''>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"<div mc:repeatable='Select' mc:variant='CTA'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#28587a'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;'>" +
$"<table width = '100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='h2 white center pb20' style='font-family:Roboto, roboto;font-size:28px;line-height:34px;color:#ffffff;text-align:center;padding-bottom:20px;'>" +
$"<div mc:edit='text_32'>Dear {account.RealName},</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text white center pb30' style='font-family:Roboto, roboto;font-size:15px;line-height:28px;color:#ffffff;text-align:center;padding-bottom:30px;'>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>This is an E-Mail to inform you that your password for Ancient has been reset." +
$"<br>" +
$"<br>Your new password: {password}" +
$"<br>" +
$"<br><div style='color: #ffffff;'>If you did not make this reset please contact an administrator immediately.<br>" +
$"<br>" +
$"<br></div>" +
$"<div mc:edit='text_33' style='color: #ffffff;'>We'll see you in game<span class='m-hide'>" +
$"<br><br></span> <a href =\'http://www.Ancientservers.co.uk\' style='color: #f4f4f4;'>Ancient Servers</a></div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='p30-15' style='padding:50px 30px;' bgcolor='#F4F4F4'>" +
$"<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td align='center' style='padding-bottom:30px;'>" +
$"<table border='0' cellspacing='0' cellpadding='0'>" +
$"<tr>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://www.facebook.com/Ancientmir' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/c8182611-9ab1-4f39-853b-9f6c2df8851e.png' width='38' height='38' mc:edit='image_12' style='max-width:38px;' border='0' alt=''></a>" +
$"</td>" +
$"<td class='img' width='50' style='font-size:0pt;line-height:0pt;text-align:center;'>" +
$"<a href='https://twitter.com/Ancientservers' target='_blank'><img src='https://gallery.mailchimp.com/c942713c5fe10eccd1a6abfff/images/425bf9b5-ff40-4c3a-86cf-efad5461d318.png' width='38' height='38' mc:edit='image_13' style='max-width:38px;' border='0' alt='' class='center'></a>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer1 pb10' style='color:#999999;font-family:Roboto, roboto;font-size:14px;line-height:20px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_35'>© 2019 Ancient Servers</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer2 pb30' style='color:#999999;font-family:Roboto, roboto;font-size:12px;line-height:26px;text-align:center;padding-bottom:10px;'>" +
$"<div mc:edit='text_36'>All rights reserved.</div>" +
$"</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='text-footer3' style='color:#c0c0c0;font-family:Roboto;font-size:12px;line-height:18px;text-align:center;'>" +
$"<div mc:edit='text_38'>" +
$"<a class='link3-u' target='_blank' href='terms-conditions' style='color:#c0c0c0;text-decoration:underline;'>Terms & Conditions</a> " +
$"</div>" + "</td>" +
$"</tr>" +
$"<tr>" +
$"<td class='img' style='font-size:0pt;line-height:0pt;text-align:left;'>" +
$"<div mc:edit='text_39'>" +
$"</div>" +
$"</td>" +
$"<tr>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</div>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</td>" +
$"</tr>" +
$"</table>" +
$"</body>" +
$"</html>"
                };

                AddMailToQueue(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #region Password Encryption
        private const int Iterations = 1354;
        private const int SaltSize = 16;
        private const int hashSize = 20;

        private static byte[] CreateHash(string password)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] hash = rfc.GetBytes(hashSize);

                    byte[] totalHash = new byte[SaltSize + hashSize];

                    Buffer.BlockCopy(salt, 0, totalHash, 0, SaltSize);
                    Buffer.BlockCopy(hash, 0, totalHash, SaltSize, hashSize);

                    return totalHash;
                }
            }
        }
        private static bool PasswordMatch(string password, byte[] totalHash)
        {
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(totalHash, 0, salt, 0, SaltSize);

            using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = rfc.GetBytes(hashSize);

                return Functions.IsMatch(totalHash, hash, SaltSize);
            }
        }
        #endregion

        public static int ErrorCount;
        private static string LastError;
        public static void SaveError(string ex)
        {
            try
            {
                if (++ErrorCount > 200 || String.Compare(ex, LastError, StringComparison.OrdinalIgnoreCase) == 0)
                    return;

                const string LogPath = @".\Errors\";

                LastError = ex;

                if (!Directory.Exists(LogPath))
                    Directory.CreateDirectory(LogPath);

                File.AppendAllText($"{LogPath}{Now.Year}-{Now.Month}-{Now.Day}.txt", LastError + Environment.NewLine);
            }
            catch
            {
            }
        }
        public static PlayerObject GetPlayerByCharacter(string name)
        {
            return GetCharacter(name)?.Account.Connection?.Player;
        }
        public static SConnection GetConnectionByCharacter(string name)
        {
            return GetCharacter(name)?.Account.Connection;
        }

        public static CharacterInfo GetCharacter(string name)
        {
            for (int i = 0; i < CharacterInfoList.Count; i++)
                if (string.Compare(CharacterInfoList[i].CharacterName, name, StringComparison.OrdinalIgnoreCase) == 0)
                    return CharacterInfoList[i];

            return null;
        }
        public static CharacterInfo GetCharacter(int index)
        {
            for (int i = 0; i < CharacterInfoList.Count; i++)
                if (CharacterInfoList[i].Index == index)
                    return CharacterInfoList[i];

            return null;
        }

        public static void Broadcast(Packet p)
        {
            foreach (PlayerObject player in Players)
                player.Enqueue(p);
        }
        public static S.Rankings GetRanks(C.RankRequest p, bool isGM)
        {
            S.Rankings result = new S.Rankings
            {
                OnlineOnly = p.OnlineOnly,
                StartIndex = p.StartIndex,
                Class = p.Class,
                Ranks = new List<RankInfo>(),
                ObserverPacket = false,
            };

            int total = 0;
            int rank = 0;
            foreach (CharacterInfo info in Rankings)
            {
                if (info.Deleted)
                    continue;

                switch (info.Class)
                {
                    case MirClass.Warrior:
                        if ((p.Class & RequiredClass.Warrior) != RequiredClass.Warrior)
                            continue;
                        break;
                    case MirClass.Wizard:
                        if ((p.Class & RequiredClass.Wizard) != RequiredClass.Wizard)
                            continue;
                        break;
                    case MirClass.Taoist:
                        if ((p.Class & RequiredClass.Taoist) != RequiredClass.Taoist)
                            continue;
                        break;
                    case MirClass.Assassin:
                        if ((p.Class & RequiredClass.Assassin) != RequiredClass.Assassin)
                            continue;
                        break;
                }

                rank++;

                if (p.OnlineOnly && info.Player == null)
                    continue;


                if (total++ < p.StartIndex || result.Ranks.Count > 20)
                    continue;

                result.Ranks.Add(new RankInfo
                {
                    Rank = rank,
                    Index = info.Index,
                    Class = info.Class,
                    Experience = info.Experience,
                    Level = info.Level,
                    Name = info.CharacterName,
                    Online = info.Player != null,
                    Observable = info.Observable || isGM,
                    Rebirth = info.Rebirth,
                });
            }

            result.Total = total;

            return result;
        }
        public static Map GetMap(MapInfo info)
        {
            return info != null && Maps.ContainsKey(info) ? Maps[info] : null;
        }
        public static MapInfo NewMapInstance(MapInfo info)
        {
            MapInfo newInfo = MapInfoList.CreateNewObject();

            if (info == null)
                return null;

            newInfo.AllowRecall = info.AllowRecall;
            newInfo.AllowRT = info.AllowRT;
            newInfo.AllowTT = info.AllowTT;
            newInfo.CanHorse = info.CanHorse;
            newInfo.CanMarriageRecall = info.CanMarriageRecall;
            newInfo.CanMine = info.CanMine;
            newInfo.Description = "TempMap";
            newInfo.DropRate = info.DropRate;
            newInfo.ExperienceRate = info.ExperienceRate;
            newInfo.Fight = info.Fight;
            newInfo.FileName = info.FileName;
            newInfo.GoldRate = info.GoldRate;
            newInfo.Light = info.Light;
            newInfo.MaximumLevel = info.MaximumLevel;
            newInfo.MaxMonsterDamage = info.MaxMonsterDamage;
            newInfo.MaxMonsterHealth = info.MaxMonsterHealth;
            newInfo.MiniMap = info.MiniMap;
            newInfo.MinimumLevel = info.MinimumLevel;
            newInfo.Mining = info.Mining;
            newInfo.MonsterDamage = info.MonsterDamage;
            newInfo.MonsterHealth = info.MonsterHealth;
            newInfo.Music = info.Music;
            newInfo.ReconnectMap = info.ReconnectMap;
            newInfo.Regions = info.Regions;
            newInfo.SkillDelay = info.SkillDelay;
            newInfo.InstanceIndex = newInfo.Index;
            newInfo.AllowGEO = info.AllowGEO;



            return newInfo;
        }
        public static ClientMapInfo NewClientMapInstance(MapInfo info)
        {
            ClientMapInfo newInfo = new ClientMapInfo();
            if (info == null)
                return null;

            newInfo.AllowRecall = info.AllowRecall;
            newInfo.AllowRT = info.AllowRT;
            newInfo.AllowTT = info.AllowTT;
            newInfo.CanHorse = info.CanHorse;
            newInfo.CanMarriageRecall = info.CanMarriageRecall;
            newInfo.CanMine = info.CanMine;
            newInfo.Description = "TempMap";
            newInfo.DropRate = info.DropRate;
            newInfo.ExperienceRate = info.ExperienceRate;
            newInfo.Fight = info.Fight;
            newInfo.FileName = info.FileName;
            newInfo.GoldRate = info.GoldRate;
            newInfo.Light = info.Light;
            newInfo.MaximumLevel = info.MaximumLevel;
            newInfo.MaxMonsterDamage = info.MaxMonsterDamage;
            newInfo.MaxMonsterHealth = info.MaxMonsterHealth;
            newInfo.MiniMap = info.MiniMap;
            newInfo.MinimumLevel = info.MinimumLevel;
            newInfo.MonsterDamage = info.MonsterDamage;
            newInfo.MonsterHealth = info.MonsterHealth;
            newInfo.Music = info.Music;
            newInfo.ReconnectMap = info.ReconnectMap;
            newInfo.SkillDelay = info.SkillDelay;
            newInfo.InstanceIndex = info.Index;
            newInfo.AllowGEO = info.AllowGEO;


            return newInfo;
        }
        public static UserConquestStats GetConquestStats(PlayerObject player)
        {
            foreach (ConquestWar war in ConquestWars)
            {
                if (war.Map != player.CurrentMap)
                    continue;

                return war.GetStat(player.Character);
            }

            return null;
        }
        public static void CastleGuildFlagChange(CastleInfo castle, GuildInfo guild)
        {
            if (castle != null)
            {
                Map castlemap = SEnvir.GetMap(castle.Map);
                foreach (MonsterObject ob in castlemap.Flags)
                {
                    ob.FlagShape = guild.FlagShape;
                    ob.FlagColour = guild.FlagColour;
                    ob.Name = guild.GuildName;
                    Broadcast(new S.ObjectFlagColour { ObjectID = ob.ObjectID, FlagColour = ob.FlagColour, FlagShape = ob.FlagShape, name = ob.Name });

                }
            }

        }
        public static ClientMiniGames NewClientMiniGame(MiniGame game)
        {
            ClientMiniGames newMG = new ClientMiniGames();

            if (game == null)
                return null;

            newMG.index = game.MGInfo.Index;
            newMG.Started = game.Started;
            newMG.StartTime = game.StartTime;
            newMG.EndTime = game.EndTime;

            return newMG;
        }
        public static void SendMiniGamesUpdate()
        {
            List<ClientMiniGames> CMiniGames = new List<ClientMiniGames>();
            foreach (var mgs in MiniGames)
            {
                if (mgs == null || mgs.MGInfo == null)
                    continue;
                CMiniGames.Add(NewClientMiniGame(mgs));
            }
            foreach (PlayerObject player in SEnvir.Players)
                player.Enqueue(new S.UpdateMiniGames { games = CMiniGames });
        }
        public static void CheckCTFDeath(PlayerObject player)
        {
            foreach (CaptureTheFlag mgs in MiniGames.OfType<CaptureTheFlag>())
            {
                if (!mgs.Players.Contains(player))
                    continue;

                if (player.HasFlag)
                {
                    if (player.EventTeam == 2)
                        mgs.RespawnFlag(1, player.CurrentLocation);

                    if (player.EventTeam == 1)
                        mgs.RespawnFlag(2, player.CurrentLocation);
                    player.HasFlag = false;
                    foreach (PlayerObject players in mgs.Players)
                    {
                        players.Enqueue(new S.HasFlag { ObjectID = player.ObjectID, hasFLag = false });
                    }
                }
                break;
            }
        }
        public static void CheckArenaPvpDeath(PlayerObject player)
        {
            foreach (PvPArena mgs in MiniGames.OfType<PvPArena>())
            {
                if (!mgs.Players.Contains(player))
                    continue;

                if (!mgs.CanRevive)
                {
                    mgs.PlayersDead.Add(player.Character);
                    mgs.PlayersAlive.Remove(player.Character);
                }
                break;
            }
        }
        public static TimeSpan CheckEventReviveDelay(PlayerObject player)
        {
            TimeSpan delay = new TimeSpan();
            delay = Config.AutoReviveDelay;
            foreach (var mgs in MiniGames)
            {
                if (!mgs.Players.Contains(player))
                    continue;

                if (mgs.Map != player.CurrentMap && mgs.LobbyMap != player.CurrentMap)
                    continue;

                if (mgs.LobbyMap == player.CurrentMap)
                {
                    delay = TimeSpan.FromSeconds(2);
                    return delay;
                }

                if (mgs.Map == player.CurrentMap)
                {
                    delay = TimeSpan.FromSeconds(mgs.MGInfo.ReviveDelay);
                }
            }

            return delay;
        }
        public static bool CheckMgMap(PlayerObject playera, PlayerObject playerb)
        {
            foreach (var mgs in MiniGames)
            {
                if (mgs.Players.Contains(playera))
                {
                    if (mgs.Map == playera.CurrentMap)
                    {
                        if (mgs.MGInfo.TeamGame)
                        {
                            if (mgs.TeamA.Contains(playera.Character) && mgs.TeamA.Contains(playerb.Character))
                                return false; // on same team
                            else
                                return true; //not on same team
                        }
                        else
                            return true; //not a team game so can attack anyone
                    }
                    else
                        return false; //player in lobby not game map
                }
                else
                    continue; //player not in this mini game
            }
            return false; //on a map marked minigame but not in an active game
        }
        public static UserArenaPvPStats GetArenaPvPStats(PlayerObject player)
        {
            foreach (var arena in MiniGames)
            {
                if (arena.Map != player.CurrentMap)
                    continue;

                return arena.GetStat(player.Character);
            }

            return null;
        }
    }

    public class WebCommand
    {
        public CommandType Command
        {
            get; set;
        }
        public AccountInfo Account
        {
            get; set;
        }

        public WebCommand(CommandType command, AccountInfo account)
        {
            Command = command;
            Account = account;
        }
    }

    public enum CommandType
    {
        None,
        Activation,
        PasswordReset,
        AccountDelete

    }


    public sealed class IPNMessage
    {
        public string Message
        {
            get; set;
        }
        public bool Verified
        {
            get; set;
        } //Ensures Paypal sent it
        public string FileName
        {
            get; set;
        }
        public bool Duplicate
        {
            get; set;
        }
    }
}
