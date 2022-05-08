using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Library.Network;
using Library.Network.ServerPackets;
using Library.SystemModels;
using MirDB;

namespace Library
{
    public static class Globals
    {
        public static ItemInfo GoldInfo;

        public static DBCollection<ItemInfo> ItemInfoList;
        public static DBCollection<MagicInfo> MagicInfoList;
        //public static DBCollection<MapInfo> MapInfoList;
        public static DBCollection<NPCPage> NPCPageList;
        public static DBCollection<MonsterInfo> MonsterInfoList;
        public static DBCollection<StoreInfo> StoreInfoList;
        public static DBCollection<FamePointInfo> FamePointInfoList;
        public static DBCollection<NPCInfo> NPCInfoList;
        public static DBCollection<MovementInfo> MovementInfoList;
        public static DBCollection<QuestInfo> QuestInfoList;
        public static DBCollection<QuestTask> QuestTaskList;
        public static DBCollection<CompanionInfo> CompanionInfoList;
        public static DBCollection<CompanionLevelInfo> CompanionLevelInfoList;
        public static DBCollection<CraftLevelInfo> CraftingLevelsInfoList;
        public static DBCollection<CraftItemInfo> CraftingItemInfoList;
        public static DBCollection<MiniGameInfo> MiniGameInfoList;
        public static DBCollection<HorseInfo> HorseInfoList;


        public static Random Random  = new Random();

        public static readonly Regex EMailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
        public static readonly Regex PasswordRegex = new Regex(@"^[\S]{" + MinPasswordLength + "," + MaxPasswordLength + "}$", RegexOptions.Compiled);
        public static readonly Regex CharacterReg = new Regex(@"^[A-Za-z0-9]{" + MinCharacterNameLength + "," + MaxCharacterNameLength + @"}$", RegexOptions.Compiled);
        public static readonly Regex GuildNameRegex = new Regex(@"^[A-Za-z0-9]{" + MinGuildNameLength + "," + MaxGuildNameLength + "}$", RegexOptions.Compiled);

        public static Color NoneColour = Color.White,
                            FireColour = Color.OrangeRed,
                            IceColour = Color.PaleTurquoise,
                            LightningColour = Color.LightSkyBlue,
                            WindColour = Color.LightSeaGreen,
                            HolyColour = Color.DarkKhaki,
                            DarkColour = Color.SaddleBrown,
                            PhantomColour = Color.Purple,

                            BrownNameColour = Color.Brown,
                            RedNameColour = Color.Red,
                            CommonColour = Color.White,
                            AddedColour = Color.LightSkyBlue,
                            SuperiorColour = Color.PaleGreen,
                            RareColour = Color.RoyalBlue,
                            EliteColour = Color.MediumPurple,
                            LegendaryColour = Color.DarkOrange,
                            WarriorColour = Color.DarkRed,
                            TaoistColour = Color.LawnGreen,
                            WizardColour = Color.Violet;

        public const int
            MinPasswordLength = 5,
            MaxPasswordLength = 15,

            MinRealNameLength = 3,
            MaxRealNameLength = 20,

            MaxEMailLength = 50,

            MinCharacterNameLength = 3,
            MaxCharacterNameLength = 15,
            MaxCharacterCount = 6,

            MinGuildNameLength = 2,
            MaxGuildNameLength = 20,

            MaxChatLength = 250,
            MaxGuildNoticeLength = 4000,

            MaxBeltCount = 10,
            MaxAutoPotionCount = 8,

            MagicRange = 10,

            DuraLossRate = 15,

            GroupLimit = 16,

            CloakRange = 3,
            MarketPlaceFee = 1000,
            AccessoryLevelCost = 2500,
            AccessoryResetCost = 1000000,

            CraftWeaponPercentCost = 1000000,

            CommonCraftWeaponPercentCost = 30000000,
            SuperiorCraftWeaponPercentCost = 60000000,
            MinAdded = 3,
            EliteCraftWeaponPercentCost = 80000000,
            MaxDailyQuestResets = 2,
            BaseSmeltingFail = 20;          

        public static decimal MarketPlaceTax = 0.07M;  //2.5x Item cost


        public static long
            GuildCreationCost = 10000000,
            GuildMemberCost = 1000000,
            GuildStorageCost = 350000,
            GuildWarCost = 250000;

        public static long
            MasterRefineCost = 50000,
            MasterRefineEvaluateCost = 250000;


        public static List<Size> ValidResolutions = new List<Size>
        {
            new Size(1024, 768),
            new Size(1366, 768),
            new Size(1280, 800),
            new Size(1440, 900),
            new Size(1600, 900),
            new Size(1920, 1080),
        };

        public static List<string> Languages = new List<string>
        {
            "English",
            "Chinese",
        };








        public static List<decimal> ExperienceList = new List<decimal>
        {
            0, // level 1
            100, // level 2
            200, // level 3
            300, // level 4
            400, // level 5
            600, // level 6
            900, // level 7
            1200, // level 8
            1700, // level 9
            2500, // level 10
            6000, // level 11
            8000, // level 12
            10000, // level 13
            15000, // level 14
            30000, // level 15
            40000, // level 16
            50000, // level 17
            70000, // level 18
            100000, // level 19
            120000, // level 20
            140000, // level 21
            250000, // level 22
            300000, // level 23
            350000, // level 24
            400000, // level 25
            500000, // level 26
            700000, // level 27
            1000000, // level 28
            1400000, // level 29
            1800000, // level 30
            2000000, // level 31
            2400000, // level 32
            2800000, // level 33
            3200000, // level 34
            3600000, // level 35
            4000000, // level 36
            4800000, // level 37
            5600000, // level 38
            8200000, // level 39
            9000000, // level 40
            11000000, // level 41
            14000000, // level 42
            25000000, // level 43
            45000000, // level 44
            70000000, // level 45
            90000000, // level 46
            110000000, // level 47
            130000000, // level 48
            150000000, // level 49
            170000000, // level 50
            210000000, // level 51
            230000000, // level 51
            270000000, // level 52
            310000000, // level 53
            330000000, // level 54
            350000000, // level 55
            370000000, // level 56
            400000000, // level 57
            430000000, // level 58
            460000000, // level 59
            495000000, // level 60
            530000000, // level 61
            565000000, // level 62
            600000000, // level 63
            640000000, // level 64
            680000000, // level 65
            720000000, // level 66
            760000000, // level 67
            800000000, // level 68
            1600000000, // level 69
            1800000000, // level 70
            2000000000, // level 71
            2200000000, // level 72
            2400000000, // level 73
            3200000000, // level 74
            3600000000, // level 75
            4000000000, // level 76
            4500000000, // level 77
            5000000000, // level 78
            15000000000, // level 79
            45000000000, // level 80
            50000000000, // level 81
            55000000000, // level 82
            60000000000, // level 83
            100000000000, // level 84
            120000000000, // level 85
            135000000000, // level 86
            170000000000, // level 87
            300000000000, // level 88
            400000000000, // level 89
            440000000000, // level 90
            460000000000, // level 91
            490000000000, // level 92
            620000000000, // level 93
            660000000000, // level 94
            720000000000, // level 95
            800000000000, // level 96
            880000000000, // level 97
            99999999999999, // level 98



       };

        public static List<decimal> OldExperienceList = new List<decimal>
        {
            0, // Lv 0
            100, // Lv 1
            200,
            300,
            400,
            600,
            900,
            1200,
            1700,
            2500,
            6000, // Lv 10
            8000,
            10000,
            15000,
            30000,
            40000,
            50000,
            70000,
            100000,
            120000,
            140000, // Lv 20
            250000,
            300000,
            350000,
            400000,
            500000,
            700000,
            1000000,
            1400000,
            1800000,
            2000000, // Lv 30
            2400000,
            2800000,
            3200000,
            3600000,
            4000000,
            4800000,
            5600000,
            8200000,
            9000000,
            11000000, // Lv 40
            14000000,
            25000000,
            45000000,
            70000000,
            90000000,
            110000000,
            130000000,
            150000000,
            170000000,
            210000000, // Lv 50
            230000000,
            250000000,
            270000000,
            310000000,
            330000000,
            350000000,
            370000000,
            400000000,
            400000000,
            400000000, // Lv 60
            400000000,
            400000000,
            400000000,
            400000000,
            400000000,
            400000000,
            400000000,
            400000000,
            400000000,
            400000000, // Lv 70
            400000000,
            800000000,
            1400000000,
            2200000000,
            3200000000,
            3600000000,
            4000000000,
            4500000000,
            5000000000,
            15000000000, // Lv 80
            45000000000,
            50000000000,
            55000000000,
            60000000000,
            100000000000,
            120000000000,
            135000000000,
            150000000000,
            170000000000,
            300000000000, // Lv 90
            400000000000,
            440000000000,
            460000000000,
            490000000000,
            620000000000,
            660000000000,
            720000000000,
            800000000000,
            880000000000,
            1000000000000, // Lv 100
        };

        public static List<decimal> WeaponExperienceList = new List<decimal>
        {
            0, //0

            300000,
            350000,
            400000,
            450000,
            500000,
            550000,
            600000,
            650000,
            700000,
            750000, //10

            800000,
            850000,
            900000,
            1000000,
            1300000,
            2000000,
        };

        public static List<decimal> AccessoryExperienceList = new List<decimal>
        {
            0,

            5,
            20,
            80,
            350,
            1500,
            6200,
            26500,
            114000,
            490000,
            2090000,
        };


        public const int InventorySize = 49,
                         EquipmentSize = 20,
                         CompanionInventorySize = 40,
                         CompanionEquipmentSize = 4,
                         PartsStorageOffset = 2000,
                         EquipmentOffSet = 1000,
                         SkinStorageOffset = 3000,
                         StorageSize = 100;

        public const int AttackDelay = 1500,
                         ASpeedRate = 47,
                         ProjectileSpeed = 48;

        public static TimeSpan TurnTime = TimeSpan.FromMilliseconds(300),
                               HarvestTime = TimeSpan.FromMilliseconds(600),
                               MoveTime = TimeSpan.FromMilliseconds(600),
                               AttackTime = TimeSpan.FromMilliseconds(600),
                               CastTime = TimeSpan.FromMilliseconds(600),
                               MagicDelay = TimeSpan.FromMilliseconds(2000);


        public static bool RealNameRequired = false,
                           BirthDateRequired = false;

        public static Dictionary<RefineQuality, TimeSpan> RefineTimes = new Dictionary<RefineQuality, TimeSpan>
        {
            [RefineQuality.Rush] = TimeSpan.FromMinutes(1),
            [RefineQuality.Standard] = TimeSpan.FromHours(1),
        };
    }

    public sealed class SelectInfo
    {
        public int CharacterIndex { get; set; }
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public MirGender Gender { get; set; }
        public MirClass Class { get; set; }
        public int Location { get; set; }
        public DateTime LastLogin { get; set; }
    }

    public sealed class StartInformation
    {
        public int Index { get; set; }
        public uint ObjectID { get; set; }
        public string Name { get; set; }
        public Color NameColour { get; set; }
        public string GuildName { get; set; }
        public string GuildRank { get; set; }
        public int FlagShape { get; set; }
        public Color FlagColour { get; set; }

        public MirClass Class { get; set; }
        public MirGender Gender { get; set; }
        public Point Location { get; set; }
        public MirDirection Direction { get; set; }

        public int MapIndex { get; set; }

        public long Gold { get; set; }
        public int GameGold { get; set; }

        public int Level { get; set; }
        public int HairType { get; set; }
        public Color HairColour { get; set; }
        public int Weapon { get; set; }
        public int Armour { get; set; }
        public int Shield { get; set; }
        public Color ArmourColour { get; set; }
        public int ArmourImage { get; set; }
        public int WingShape { get; set; }
        public int WeaponEffect { get; set; }

        public decimal Experience { get; set; }

        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }

        public AttackMode AttackMode { get; set; }
        public PetMode PetMode { get; set; }

        public int HermitPoints { get; set; }

        public float DayTime { get; set; }
        public bool AllowGroup { get; set; }

        public List<ClientUserItem> Items { get; set; }
        public List<ClientBeltLink> BeltLinks { get; set; }
        public List<ClientAutoPotionLink> AutoPotionLinks { get; set; }
        public List<ClientUserCrafting> CraftInfo { get; set; }
        public List<ClientUserMagic> Magics { get; set; }
        public List<ClientBuffInfo> Buffs { get; set; }
        public List<ClientMapInfo> MapInfos { get; set; }
        public List<ClientMiniGames> CMiniGames { get; set; }
        public PoisonType Poison { get; set; }

        public bool InSafeZone { get; set; }
        public bool Observable { get; set; }

        public bool Dead { get; set; }

        public HorseType Horse { get; set; } //Horse Armour too

        public int HelmetShape { get; set; }
        public int HorseShape { get; set; }

        public List<ClientUserQuest> Quests { get; set; }

        public List<int> CompanionUnlocks { get; set; }
        public List<CompanionInfo> AvailableCompanions = new List<CompanionInfo>();

        public List<ClientUserCompanion> Companions { get; set; }
        public List<int> HorseUnlocks { get; set; }
        public List<HorseInfo> AvailableHorses = new List<HorseInfo>();

        public List<int> Horses { get; set; }

        public int Companion { get; set; }

        public int StorageSize { get; set; }
        public int DailyQuestResets { get; set; }

        public DBCollection<MapInfo> MapInfoList;

        [CompleteObject]
        public void OnComplete()
        {
            foreach (int index in CompanionUnlocks)
                AvailableCompanions.Add(Globals.CompanionInfoList.Binding.First(x => x.Index == index));
            foreach (int index in HorseUnlocks)
                AvailableHorses.Add(Globals.HorseInfoList.Binding.First(x => x.Index == index));
        }
    }
    
    public sealed class ClientUserItem
    {
        public ItemInfo Info;

        public int Index { get; set; } //ItemID
        public int InfoIndex { get; set; }

        public int CurrentDurability { get; set; }
        public int MaxDurability { get; set; }

        public long Count { get; set; }
        
        public int Slot { get; set; }

        public int Level { get; set; }
        public decimal Experience { get; set; }

        public Color Colour { get; set; }

        public TimeSpan SpecialRepairCoolDown { get; set; }
        public TimeSpan ResetCoolDown { get; set; }

        public bool New;
        public DateTime NextSpecialRepair, NextReset;

        public Stats AddedStats { get; set; }

        public UserItemFlags Flags { get; set; }
        public TimeSpan ExpireTime { get; set; }

        public Rarity Rarity { get; set; }
        public bool CraftInfoOnly;


        [IgnorePropertyPacket]
        public int Weight
        {
            get
            {
                switch (Info.ItemType)
                {
                    case ItemType.Poison:
                    case ItemType.Amulet:
                        return Info.Weight;
                    default:
                        return (int) Math.Min(int.MaxValue, Info.Weight * Count);
                }
            }
        }

        [CompleteObject]
        public void Complete()
        {
            Info = Globals.ItemInfoList.Binding.FirstOrDefault(x => x.Index == InfoIndex);

            NextSpecialRepair = Time.Now + SpecialRepairCoolDown;
            NextReset = Time.Now + ResetCoolDown;
        }

        public ClientUserItem()
        { }
        public ClientUserItem(ItemInfo info, long count)
        {
            Info = info;
            Count = count;
            MaxDurability = info.Durability;
            CurrentDurability = info.Durability;
            Level = 1;
            AddedStats = new Stats();
            Rarity = info.Rarity;
            CraftInfoOnly = false;
        }
        public ClientUserItem(ClientUserItem item, long count)
        {
            Info = item.Info;

            Index = item.Index;
            InfoIndex = item.InfoIndex;

            CurrentDurability = item.CurrentDurability;
            MaxDurability = item.MaxDurability;

            Count = count;
            
            Slot = item.Slot;

            Level = item.Level;
            Experience = item.Experience;

            Colour = item.Colour;

            SpecialRepairCoolDown = item.SpecialRepairCoolDown;

            Flags = item.Flags;
            ExpireTime = item.ExpireTime;

            New = item.New;
            NextSpecialRepair = item.NextSpecialRepair;
            
            AddedStats = new Stats(item.AddedStats);
            Rarity = item.Info.Rarity;
            CraftInfoOnly = false;
        }


        public long Price(long count)
        {
            if ((Flags & UserItemFlags.Worthless) == UserItemFlags.Worthless) return 0;

            decimal p = Info.Price;

            if (Info.Durability > 0)
            {
                decimal r = Info.Price / 2M / Info.Durability;

                p = MaxDurability * r;

                r = MaxDurability > 0 ? CurrentDurability / (decimal)MaxDurability : 0;

                p = Math.Floor(p / 2M + p / 2M * r + Info.Price / 2M);
            }

            p = p * (AddedStats.Count * 0.1M + 1M);

            if (Info.Stats[Stat.SaleBonus20] > 0 && Info.Stats[Stat.SaleBonus20] <= count)
                p *= 1.2M;
            else if (Info.Stats[Stat.SaleBonus15] > 0 && Info.Stats[Stat.SaleBonus15] <= count)
                p *= 1.15M;
            else if (Info.Stats[Stat.SaleBonus10] > 0 && Info.Stats[Stat.SaleBonus10] <= count)
                p *= 1.1M;
            else if (Info.Stats[Stat.SaleBonus5] > 0 && Info.Stats[Stat.SaleBonus5] <= count)
                p *= 1.05M;

            return (long)(p * count * Info.SellRate);
        }

        public int RepairCost(bool special)
        {
            if (Info.Durability == 0 || CurrentDurability >= MaxDurability) return 0;

            int rate = special ? 2 : 1;
            
            decimal p = Math.Floor(MaxDurability*(Info.Price/2M/Info.Durability) + Info.Price/2M);
            p = p*(AddedStats.Count*0.1M + 1M);

            return (int) (p*Count - Price(Count))*rate;


        }
        public bool CanAccessoryUpgrade()
        {
            switch (Info.ItemType)
            {
                case ItemType.Ring:
                case ItemType.Bracelet:
                case ItemType.Necklace:
                    break;
                default: return false;

            }

            return (Flags & UserItemFlags.NonRefinable) != UserItemFlags.NonRefinable && (Flags & UserItemFlags.Refinable) == UserItemFlags.Refinable;
        }
        public bool CanFragment()
        {
            if ((Flags & UserItemFlags.NonRefinable) == UserItemFlags.NonRefinable || (Flags & UserItemFlags.Worthless) == UserItemFlags.Worthless) return false;

            switch (Rarity)
            {
                case Rarity.Common:
                    if (Info.RequiredAmount <= 15) return false;
                    break;
                case Rarity.Superior:
                    break;
                case Rarity.Rare:
                    break;
                case Rarity.Elite:
                    break;
                case Rarity.Legendary:
                    break;
            }

            switch (Info.ItemType)
            {
                case ItemType.Weapon:
                case ItemType.Armour:
                case ItemType.Helmet:
                case ItemType.Necklace:
                case ItemType.Bracelet:
                case ItemType.Ring:
                case ItemType.Shoes:
                    break;
                default:
                    return false;
            }

            return true;
        }
        public int FragmentCost()
        {
            switch (Rarity)
            {
                case Rarity.Common:
                    switch (Info.ItemType)
                    {
                        case ItemType.Armour:
                        case ItemType.Weapon:
                        case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Info.RequiredAmount * 10000 / 9;
                       /* case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Info.RequiredAmount * 7000 / 9;*/
                        default:
                            return 0;
                    }
                case Rarity.Superior:
                    switch (Info.ItemType)
                    {
                        case ItemType.Weapon:
                        case ItemType.Armour:
                        case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Info.RequiredAmount * 10000 / 2;
                      /*  case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Info.RequiredAmount * 10000 / 10;*/
                        default:
                            return 0;
                    }
                case Rarity.Elite:
                    switch (Info.ItemType)
                    {
                        case ItemType.Weapon:
                        case ItemType.Armour:
                            return 250000;
                        case ItemType.Helmet:
                            return 50000;
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                            return 150000;
                        case ItemType.Shoes:
                            return 30000;
                        default:
                            return 0;
                    }
                default:
                    return 0;
            }
        }
        public int FragmentCount()
        {
            switch (Rarity)
            {
                case Rarity.Common:
                    switch (Info.ItemType)
                    {
                        case ItemType.Armour:
                        case ItemType.Weapon:
                        case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Math.Max(1, Info.RequiredAmount / 2 + 5);
                      /*  case ItemType.Helmet:
                            return Math.Max(1, (Info.RequiredAmount - 30) / 6);
                        case ItemType.Necklace:
                            return Math.Max(1, Info.RequiredAmount / 8);
                        case ItemType.Bracelet:
                            return Math.Max(1, Info.RequiredAmount / 15);
                        case ItemType.Ring:
                            return Math.Max(1, Info.RequiredAmount / 9);
                        case ItemType.Shoes:
                            return Math.Max(1, (Info.RequiredAmount - 35) / 6);*/
                        default:
                            return 0;
                    }
                case Rarity.Superior:
                    switch (Info.ItemType)
                    {
                        case ItemType.Armour:
                        case ItemType.Weapon:
                        case ItemType.Helmet:
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                        case ItemType.Shoes:
                            return Math.Max(1, Info.RequiredAmount / 2 + 5);
                      /*  case ItemType.Helmet:
                            return Math.Max(1, (Info.RequiredAmount - 30) / 6);
                        case ItemType.Necklace:
                            return Math.Max(1, Info.RequiredAmount / 10);
                        case ItemType.Bracelet:
                            return Math.Max(1, Info.RequiredAmount / 15);
                        case ItemType.Ring:
                            return Math.Max(1, Info.RequiredAmount / 10);
                        case ItemType.Shoes:
                            return Math.Max(1, (Info.RequiredAmount - 35) / 6);*/
                        default:
                            return 0;
                    }
                case Rarity.Elite:
                    switch (Info.ItemType)
                    {
                        case ItemType.Armour:
                        case ItemType.Weapon:
                            return 50;
                        case ItemType.Helmet:
                            return 5;
                        case ItemType.Necklace:
                        case ItemType.Bracelet:
                        case ItemType.Ring:
                            return 10;
                        case ItemType.Shoes:
                            return 3;
                        default:
                            return 0;
                    }
                default:
                    return 0;
            }
        }
    }
    
    public sealed class ClientBeltLink
    {
        public int Slot { get; set; }
        public int LinkInfoIndex { get; set; }
        public int LinkItemIndex { get; set; }
    }

    public sealed class ClientAutoPotionLink
    {
        public int Slot { get; set; }
        public int LinkInfoIndex { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public bool Enabled { get; set; }
    }
    
    public class ClientUserMagic
    {
        public int Index { get; set; }
        public int InfoIndex { get; set; }
        public MagicInfo Info;

        public SpellKey Set1Key { get; set; }
        public SpellKey Set2Key { get; set; }
        public SpellKey Set3Key { get; set; }
        public SpellKey Set4Key { get; set; }

        public int Level { get; set; }
        public long Experience { get; set; }

        public TimeSpan Cooldown { get; set; }

        public DateTime NextCast;
        

        [IgnorePropertyPacket]
        public int Cost => Info.BaseCost + Level * Info.LevelCost / 3;

        [CompleteObject]
        public void Complete()
        {
            NextCast = Time.Now + Cooldown;
            Info = Globals.MagicInfoList.Binding.FirstOrDefault(x => x.Index == InfoIndex);
        }
    }


    public class CellLinkInfo
    {
        public GridType GridType { get; set; }
        public int Slot { get; set; }
        public long Count { get; set; }
    }
    
    public class ClientBuffInfo
    {
        public int Index { get; set; }
        public BuffType Type { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public TimeSpan TickFrequency { get; set; }
        public Stats Stats { get; set; }
        public bool Pause { get; set; }
        public int ItemIndex { get; set; }
    }

    public class ClientRefineInfo
    {
        public int Index { get; set; }
        public ClientUserItem Weapon { get; set; }
        public RefineType Type { get; set; }
        public RefineQuality Quality { get; set; }
        public int Chance { get; set; }
        public int MaxChance { get; set; }
        public TimeSpan ReadyDuration { get; set; }

        public DateTime RetrieveTime;

        [CompleteObject]
        public void Complete()
        {
            RetrieveTime = Time.Now + ReadyDuration;
        }
    }


    public sealed class RankInfo
    {
        public int Rank { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public MirClass Class { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }
        public bool Online { get; set; }
        public bool Observable { get; set; }
        public int Rebirth { get; set; }
    }
    
    public class ClientMarketPlaceInfo
    {
        public int Index { get; set; }
        public ClientUserItem Item { get; set; }
        
        public int Price { get; set; }

        public string Seller { get; set; }
        public string Message { get; set; }
        public bool IsOwner { get; set; }

        public bool Loading;
    }

    public class ClientMailInfo
    {
        public int Index { get; set; }
        public bool Opened { get; set; }
        public bool HasItem { get; set; }
        public DateTime Date { get; set; }

        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public int Gold { get; set; }
        public List<ClientUserItem> Items { get; set; }
    }

    public class ClientGuildInfo
    {
        public string GuildName { get; set; }

        public string Notice { get; set; }

        public int MemberLimit { get; set; }

        public long GuildFunds { get; set; }
        public long DailyGrowth { get; set; }
        
        public long TotalContribution { get; set; }
        public long DailyContribution { get; set; }

        public int UserIndex { get; set; }

        public int StorageLimit { get; set; }
        public int Tax { get; set; }

        public string DefaultRank { get; set; }
        public GuildPermission DefaultPermission { get; set; }

        public List<ClientGuildMemberInfo> Members { get; set; }

        public List<ClientUserItem> Storage { get; set; }
        public int FlagShape { get; set; }
        public Color FlagColour { get; set; }

        [IgnorePropertyPacket]
        public GuildPermission Permission => Members.FirstOrDefault(x => x.Index == UserIndex)?.Permission ?? GuildPermission.None;
    }

    public class ClientGuildMemberInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public long TotalContribution { get; set; }
        public long DailyContribution { get; set; }
        public TimeSpan Online { get; set; }

        public GuildPermission Permission { get; set; }

        public DateTime LastOnline;
        public uint ObjectID { get; set; }

        [CompleteObject]
        public void Complete()
        {
            if (Online == TimeSpan.MinValue)
                LastOnline = DateTime.MaxValue;
            else
                LastOnline = Time.Now - Online;
        }

    }

    public class ClientUserQuest
    {
        public int Index { get; set; }

        [IgnorePropertyPacket]
        public QuestInfo Quest { get; set; }

        public int QuestIndex { get; set; }

        public bool Track { get; set; }

        public bool Completed { get; set; }

        public int SelectedReward { get; set; }

        [IgnorePropertyPacket]
        public bool IsComplete => Tasks.Count == Quest.Tasks.Count && Tasks.All(x => x.Completed);

        public List<ClientUserQuestTask> Tasks { get; set; }

        [CompleteObject]
        public void Complete()
        {
            Quest = Globals.QuestInfoList.Binding.First(x => x.Index == QuestIndex);
        }
    }

    public class ClientUserQuestTask
    {
        public int Index { get; set; }

        [IgnorePropertyPacket]
        public QuestTask Task { get; set; }

        public int TaskIndex { get; set; }

        public long Amount { get; set; }

        [IgnorePropertyPacket]
        public bool Completed => Amount >= Task.Amount;

        [CompleteObject]
        public void Complete()
        {
            Task = Globals.QuestTaskList.Binding.First(x => x.Index == TaskIndex);
        }
    }

    public class ClientCompanionObject
    {
        public string Name { get; set; }

        public int HeadShape { get; set; }
        public int BackShape { get; set; }
    }

    public class ClientUserHorse
    {
        public int HorseNum { get; set; }
        [IgnorePropertyPacket]
        public HorseInfo HorseInfo { get; set; }
    }
    public class ClientUserCompanion
    {
        public int Index { get; set; }
        public string Name { get; set; }

        public int CompanionIndex { get; set; }
        public CompanionInfo CompanionInfo;
        
        public int Level { get; set; }
        public int Hunger { get; set; }
        public int Experience { get; set; }

        public Stats Level3 { get; set; }
        public Stats Level5 { get; set; }
        public Stats Level7 { get; set; }
        public Stats Level10 { get; set; }
        public Stats Level11 { get; set; }
        public Stats Level13 { get; set; }
        public Stats Level15 { get; set; }

        public string CharacterName { get; set; }

        public List<ClientUserItem> Items { get; set; }

        public ClientUserItem[] EquipmentArray = new ClientUserItem[Globals.CompanionEquipmentSize], InventoryArray = new ClientUserItem[Globals.CompanionInventorySize];


        [CompleteObject]
        public void OnComplete()
        {
            CompanionInfo = Globals.CompanionInfoList.Binding.First(x => x.Index == CompanionIndex);


            foreach (ClientUserItem item in Items)
            {
                if (item.Slot < Globals.EquipmentOffSet)
                    InventoryArray[item.Slot] = item;
                else
                    EquipmentArray[item.Slot - Globals.EquipmentOffSet] = item;
            }

        }

    }

    public class ClientPlayerInfo 
    {
        public uint ObjectID { get; set; }

        public string Name { get; set; }
    }
    public class ClientObjectData
    {
        public uint ObjectID;

        public int MapIndex;
        public Point Location;

        public string Name;

        //Guild/Grorup
        public MonsterInfo MonsterInfo;
        public ItemInfo ItemInfo;

        public string PetOwner;

        public int Health;
        public int MaxHealth;

        public int Mana;
        public int MaxMana;
        public Stats Stats { get; set; }

        public bool Dead;
        public int level;
        public bool Supermob;
    }

    public class ClientBlockInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }

    public class ClientFortuneInfo
    {
        public int ItemIndex { get; set; }
        public ItemInfo ItemInfo;

        public TimeSpan CheckTime { get; set; }
        public long DropCount { get; set; }
        public decimal Progress { get; set; }

        public DateTime CheckDate;

        [CompleteObject]
        public void OnComplete()
        {
            ItemInfo = Globals.ItemInfoList.Binding.First(x => x.Index == ItemIndex);

            CheckDate = Time.Now - CheckTime;
        }
    }
    public sealed class ClientUserCrafting
    {
        public CraftType Type { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
    }

    public sealed class ClientMapInfo
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public int MiniMap { get; set; }
        public LightSetting Light { get; set; }
        public FightSetting Fight { get; set; }
        public bool AllowRT { get; set; }
        public int SkillDelay { get; set; }
        public bool CanHorse { get; set; }
        public bool AllowTT { get; set; }
        public bool CanMine { get; set; }
        public bool CanMarriageRecall { get; set; }
        public bool AllowRecall { get; set; }
        public int MinimumLevel { get; set; }
        public int MaximumLevel { get; set; }
        public MapInfo ReconnectMap { get; set; }
        public SoundIndex Music { get; set; }
        public int MonsterHealth { get; set; }
        public int MonsterDamage { get; set; }
        public int DropRate { get; set; }
        public int ExperienceRate { get; set; }
        public int KillStreakExperienceRate { get; set; }
        public DateTime KillStreakEndTime { get; set; }
        public Boolean KillSteakActive { get; set; }
        public int InstanceIndex { get; set; }
        public int GoldRate { get; set; }
        public int MaxMonsterHealth { get; set; }
        public int MaxMonsterDamage { get; set; }
        public int MaxDropRate { get; set; }
        public int MaxExperienceRate { get; set; }
        public int MaxGoldRate { get; set; }
        public List<MapRegion> Regions { get; set; }
        public List<MineInfo> Mining { get; set; }
        public bool AllowGEO { get; set; }

    }

    public sealed class ClientMiniGames
    {
        public int index { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Started { get; set; }


    }
}


