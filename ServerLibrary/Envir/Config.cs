using System;
using System.IO;
using System.Security.Cryptography;
using Library;

namespace Server.Envir
{
    [ConfigPath(@".\Server.ini")]
    public static class Config
    {
        [ConfigSection("Network")]
        public static string IPAddress { get; set; } = "0.0.0.0";
        public static ushort Port { get; set; } = 7100;
        public static TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(20);
        public static TimeSpan PingDelay { get; set; } = TimeSpan.FromSeconds(2);
        public static ushort UserCountPort { get; set; } = 3000;
        public static int MaxPacket { get; set; } = 50;
        public static int MaxFastConnection { get; set; } = 14;
        public static TimeSpan PacketBanTime { get; set; } = TimeSpan.FromMinutes(5);


        [ConfigSection("System")]
        public static bool CheckVersion { get; set; } = false;
        public static string VersionPath { get; set; } = @"C:\Mir3Server\Zircon Files\Client\Ancient Mir.exe";
        public static TimeSpan DBSaveDelay { get; set; } = TimeSpan.FromMinutes(5);
        public static string MapPath { get; set; } = @"C:\Mir3Server\Zircon Files\Client\Map\";
        public static byte[] ClientHash;
        public static string MasterPassword { get; set; } = @"REDACTED";
        public static string ClientPath { get; set; } = @"C:\Mir3Server\Zircon\Debug\Client";
        public static DateTime ReleaseDate { get; set; } = new DateTime(2017, 12, 22, 18, 00, 00, DateTimeKind.Utc);
        public static bool TestServer { get; set; } = false;
        public static string StarterGuildName { get; set; } = "Ancient Guild";
        public static DateTime EasterEventEnd { get; set; } = new DateTime(2018, 04, 09, 00, 00, 00, DateTimeKind.Utc);
        public static DateTime HalloweenEventEnd { get; set; } = new DateTime(2018, 11, 07, 00, 00, 00, DateTimeKind.Utc);
        public static DateTime ChristmasEventEnd { get; set; } = new DateTime(2020, 01, 07, 00, 00, 00, DateTimeKind.Utc);
        public static int EnvironmentTickCount { get; set; } = 20;

        [ConfigSection("Control")]
        public static bool AllowLogin { get; set; } = true;
        public static bool AllowNewAccount { get; set; } = true;
        public static bool AllowChangePassword { get; set; } = true;

        public static bool AllowRequestPasswordReset { get; set; } = true;
        public static bool AllowWebResetPassword { get; set; } = true;
        public static bool AllowManualResetPassword { get; set; } = true;

        public static bool AllowDeleteAccount { get; set; } = true;

        public static bool AllowManualActivation { get; set; } = true;
        public static bool AllowWebActivation { get; set; } = true;
        public static bool AllowRequestActivation { get; set; } = true;

        public static bool AllowNewCharacter { get; set; } = true;
        public static bool AllowDeleteCharacter { get; set; } = true;
        public static bool AllowStartGame { get; set; } = true;
        public static TimeSpan RelogDelay { get; set; } = TimeSpan.FromSeconds(10);
        public static bool AllowWarrior { get; set; } = true;
        public static bool AllowWizard { get; set; } = true;
        public static bool AllowTaoist { get; set; } = true;
        public static bool AllowAssassin { get; set; } = true;

        [ConfigSection("Mail")]
        public static string MailServer { get; set; } = @"ssl0.ovh.net";
        public static int MailPort { get; set; } = 587;
        public static bool MailUseSSL { get; set; } = true;
        public static string MailAccount { get; set; } = @"noreply@ancientservers.co.uk";
        public static string MailPassword { get; set; } = @"REDACTED";
        public static string MailFrom { get; set; } = "noreply@ancientservers.co.uk";
        public static string MailDisplayName { get; set; } = "Admin";

        [ConfigSection("WebServer")]
        public static string WebPrefix { get; set; } = @"http://*/Command/";
        public static string WebCommandLink { get; set; } = @"http://*/Command";

        public static string ActivationSuccessLink { get; set; } = @"http://www.ancientservers.co.uk/activation-successful/";
        public static string ActivationFailLink { get; set; } = @"http://www.ancientservers.co.uk/activation-unsuccessful/";
        public static string ResetSuccessLink { get; set; } = @"http://www.ancientservers.co.uk/password-reset-successful/";
        public static string ResetFailLink { get; set; } = @"http://www.ancientservers.co.uk/password-reset-unsuccessful/";
        public static string DeleteSuccessLink { get; set; } = @"http://www.ancientservers.co.ukaccount-deletetion-successful/";
        public static string DeleteFailLink { get; set; } = @"http://www.ancientservers.co.uk/account-deletetion-unsuccessful/";

        public static string BuyPrefix { get; set; } = @"http://*:80/BuyGameGold/";
        public static string BuyAddress { get; set; } = @"http://51.89.150.136/BuyGameGold";
        public static string IPNPrefix { get; set; } = @"http://*:80/IPN/";
        public static string ReceiverEMail { get; set; } = @"paypal@ancientservers.co.uk";
        public static bool ProcessGameGold { get; set; } = true;
        public static bool AllowBuyGammeGold { get; set; } = true;


        [ConfigSection("Players")]
        public static int MaxViewRange { get; set; } = 18;
        public static TimeSpan ShoutDelay { get; set; } = TimeSpan.FromSeconds(10);
        public static TimeSpan GlobalDelay { get; set; } = TimeSpan.FromSeconds(60);
        public static int MaxLevel { get; set; } = 10;
        public static int DayCycleCount { get; set; } = 3;
        public static int SkillExp { get; set; } = 3;
        public static bool AllowObservation { get; set; } = true;
        public static TimeSpan BrownDuration { get; set; } = TimeSpan.FromSeconds(60);
        public static int PKPointRate { get; set; } = 50;
        public static TimeSpan PKPointTickRate { get; set; } = TimeSpan.FromSeconds(60);
        public static int RedPoint { get; set; } = 200;
        public static TimeSpan PvPCurseDuration { get; set; } = TimeSpan.FromMinutes(60);
        public static int PvPCurseRate { get; set; } = 4;
        public static TimeSpan AutoReviveDelay { get; set; } = TimeSpan.FromMinutes(10);
        public static bool DeathDrops { get; set; } = true;
        public static int DDInventory { get; set; } = 10;
        public static int DDCommon { get; set; } = 20;
        public static int DDSuperior { get; set; } = 75;
        public static int DDRare { get; set; } = 200;
        public static int DDElite { get; set; } = 500;
        public static int DDLegendary { get; set; } = 1000;
        public static int DDMaxDrop { get; set; } = 1;
        public static int StartLevel { get; set; } = 1;


        [ConfigSection("Monsters")]
        public static TimeSpan DeadDuration { get; set; } = TimeSpan.FromMinutes(1);
        public static TimeSpan HarvestDuration { get; set; } = TimeSpan.FromMinutes(5);
        public static int MysteryShipRegionIndex { get; set; } = 711;
        public static int LairRegionIndex { get; set; } = 1570;
        public static int SeaCaveRegionIndex { get; set; } = 15700;
        public static int FlowerMapRegionIndex { get; set; } = 15700;

        [ConfigSection("Items")]
        public static TimeSpan DropDuration { get; set; } = TimeSpan.FromMinutes(60);
        public static int DropDistance { get; set; } = 5;
        public static int DropLayers { get; set; } = 5;
        public static int DropAddedChance { get; set; } = 15;
        public static int DropRarityInc { get; set; } = 30;
        public static int TorchRate { get; set; } = 10;
        public static TimeSpan SpecialRepairDelay { get; set; } = TimeSpan.FromHours(8);
        public static int MaxLuck { get; set; } = 7;
        public static int MaxCurse { get; set; } = -10;
        public static int CurseRate { get; set; } = 20;
        public static int LuckRate { get; set; } = 10;
        public static int MaxStrength { get; set; } = 5;
        public static int StrengthAddRate { get; set; } = 10;
        public static int StrengthLossRate { get; set; } = 20;

        [ConfigSection("Rates")]
        public static int ExperienceRate { get; set; } = 0;
        public static int DropRate { get; set; } = 0;
        public static int GoldRate { get; set; } = 0;
        public static int SkillRate { get; set; } = 0;
        public static int CompanionRate { get; set; } = 0;


        public static void LoadVersion()
        {
            try
            {
                if (File.Exists(VersionPath))
                    using (FileStream stream = File.OpenRead(VersionPath))
                    using (MD5 md5 = MD5.Create())
                        ClientHash = md5.ComputeHash(stream);
                else
                    ClientHash = null;
            }
            catch (Exception ex)
            {
                SEnvir.LogError(ex);
            }
        }
    }
}
