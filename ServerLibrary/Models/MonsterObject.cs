using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Library;
using Library.Network;
using Library.SystemModels;
using Server.DBModels;
using Server.Envir;
using Server.Models.Monsters;
using S = Library.Network.ServerPackets;


namespace Server.Models
{
    public class MonsterObject : MapObject
    {
        public override ObjectType Race => ObjectType.Monster;

        public sealed override MirDirection Direction
        {
            get; set;
        }

        public DateTime SearchTime, RoamTime, EXPOwnerTime, DeadTime, RageTime, TameTime, teleportDelay, NextEnrageTime, EnrageEndTime;
        public bool Enraged = false;
        public bool isClone = false;

        public TimeSpan SearchDelay = TimeSpan.FromSeconds(3),
                        RoamDelay = TimeSpan.FromSeconds(2),
                        EXPOwnerDelay = TimeSpan.FromSeconds(20);

        public MonsterInfo MonsterInfo;

        public SpawnInfo SpawnInfo;
        public int DropSet;
        public int MaxStage, MinSpawn, RandomSpawn, Stage, MaxMinion;

        public DateTime FearTime;

        public int AttackRange = 1;
        public int FearRate = 6;
        public int FearDuration = 2;

        public MapObject Target
        {
            get
            {
                return _Target;
            }
            set
            {
                if (_Target == value)
                    return;

                _Target = value;

                if (_Target == null)
                    SearchTime = DateTime.MinValue;
            }
        }
        private MapObject _Target;

        public bool PlayerTagged;

        public Dictionary<MonsterInfo, int> SpawnList = new Dictionary<MonsterInfo, int>();
        public bool Skeleton;
        public List<Damagers> DamageList = new List<Damagers>();
        #region EXPOwner

        public PlayerObject EXPOwner
        {
            get
            {
                return _EXPOwner;
            }
            set
            {
                if (_EXPOwner == value)
                    return;

                PlayerObject oldValue = _EXPOwner;
                _EXPOwner = value;

                OnEXPOwnerChanged(oldValue, value);
            }
        }
        private PlayerObject _EXPOwner;
        public virtual void OnEXPOwnerChanged(PlayerObject oValue, PlayerObject nValue)
        {
            oValue?.TaggedMonsters.Remove(this);

            nValue?.TaggedMonsters.Add(this);
        }

        #endregion

        public Dictionary<AccountInfo, List<UserItem>> Drops;

        public virtual decimal Experience => MonsterInfo.Experience;

        public decimal ExtraExperienceRate = 0;

        public bool Passive, NeedHarvest, AvoidFireWall;
        public int HarvestCount;

        public int DeathCloudDurationMin = 4000, DeathCloudDurationRandom = 0;

        public int MoveDelay;
        public int AttackDelay;

        public List<MonsterObject> MinionList = new List<MonsterObject>();
        public MonsterObject Master;
        public int MaxMinions = 20;

        public PlayerObject PetOwner;
        public HashSet<UserMagic> Magics = new HashSet<UserMagic>();
        public int SummonLevel;
        public bool SuperMob = false;

        public int ViewRange
        {
            get
            {
                return PoisonList.Any(x => x.Type == PoisonType.Abyss) ? 2 : MonsterInfo.ViewRange;
            }
        }

        public PoisonType PoisonType;
        public int PoisonRate = 10;
        public int PoisonTicks = 5;
        public int PoisonFrequency = 2;

        public bool IgnoreShield;

        public bool EasterEventMob, HalloweenEventMob, ChristmasEventMob;

        public int MapHealthRate, MapDamageRate, MapExperienceRate, MapDropRate, MapGoldRate;

        public Element AttackElement => Stats.GetAffinityElement();

        public override bool CanMove => base.CanMove && (Poison & PoisonType.Silenced) != PoisonType.Silenced && MoveDelay > 0 && (PetOwner == null || PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Move || PetOwner.PetMode == PetMode.PvP);
        public override bool CanAttack => base.CanAttack && (Poison & PoisonType.Silenced) != PoisonType.Silenced && AttackDelay > 0 && (PetOwner == null || PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Attack || PetOwner.PetMode == PetMode.PvP);


        public static MonsterObject GetMonster(MonsterInfo monsterInfo)
        {
            switch (monsterInfo.AI)
            {
                case -2:
                    return new Flag { MonsterInfo = monsterInfo };
                case -1:
                    return new Guard { MonsterInfo = monsterInfo };
                case 1:
                    return new MonsterObject { MonsterInfo = monsterInfo, Passive = true, NeedHarvest = true, HarvestCount = 2 };
                case 2:
                    return new MonsterObject { MonsterInfo = monsterInfo, Passive = true, NeedHarvest = true, HarvestCount = 3 };
                case 3:
                    return new MonsterObject { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 3 };
                case 4:
                    return new TreeMonster { MonsterInfo = monsterInfo };
                case 5:
                    return new CarnivorousPlant { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2 };
                case 6:
                    return new SpittingSpider { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2, PoisonType = PoisonType.Green };
                case 7:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 8:
                    return new MonsterObject { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 12 };
                case 9:
                    return new GhostSorcerer { MonsterInfo = monsterInfo, AttackRange = 6 };
                case 10:
                    return new GhostMage { MonsterInfo = monsterInfo };
                case 11:
                    return new VoraciousGhost { MonsterInfo = monsterInfo };
                case 12:
                    return new HealerAnt { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 13:
                    return new LordNiJae { MonsterInfo = monsterInfo };
                case 14:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 15:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 16:
                    return new UmaKing { MonsterInfo = monsterInfo };
                case 17:
                    return new ArachnidGrazer
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Larva)] = 1 }
                    };
                case 18:
                    return new Larva { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 19:
                    return new RedMoonTheFallen { MonsterInfo = monsterInfo };
                case 20:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, FearRate = 2, FearDuration = 4, AttackRange = 7 };
                case 21:
                    return new ZumaGuardian { MonsterInfo = monsterInfo };
                case 22:
                    return new ZumaKing
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaArcherMonster)] = 50,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaFanaticMonster)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaGuardianMonster)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaKeeperMonster)] = 1
                        }
                    };
                case 23:
                    return new Monkey { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 24:
                    return new Monkey { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red };
                case 25:
                    return new EvilElephant { MonsterInfo = monsterInfo };
                case 26:
                    return new NumaMage { MonsterInfo = monsterInfo };
                case 27:
                    return new GhostMage { MonsterInfo = monsterInfo };
                case 28:
                    return new WindfurySorcerer { MonsterInfo = monsterInfo };
                case 29:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 30:
                    return new NetherworldGate { MonsterInfo = monsterInfo };
                case 31:
                    return new SonicLizard { MonsterInfo = monsterInfo };
                case 33:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 9, IgnoreShield = true, };
                case 34:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, AttackRange = 9, FearRate = 6, FearDuration = 2, };
                case 35:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 36:
                    return new NumaMage { MonsterInfo = monsterInfo };
                case 37:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 38:
                    return new BanyaLeftGuard { MonsterInfo = monsterInfo };
                case 39:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 40:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 41:
                    return new EmperorSaWoo { MonsterInfo = monsterInfo };
                case 42:
                    return new SpittingSpider { MonsterInfo = monsterInfo };
                case 43:
                    return new ArchLichTaedu
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneArcher)] = 90,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneSoldier)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneBladesman)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneCaptain)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SkeletonEnforcer)] = 1
                        }
                    };
                case 44:
                    return new WedgeMothLarva
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.LesserWedgeMoth)] = 1 }
                    };
                case 45:
                    return new RazorTusk { MonsterInfo = monsterInfo };
                case 46:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red, PoisonTicks = 1, PoisonFrequency = 10, PoisonRate = 25 };
                case 47:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 7, PoisonRate = 15 };
                case 48:
                    return new SonicLizard { MonsterInfo = monsterInfo, IgnoreShield = true };
                case 49:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 8, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 15 };
                case 50:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 8 };
                case 52:
                    return new WhiteBone() { MonsterInfo = monsterInfo };
                case 53:
                    return new Shinsu { MonsterInfo = monsterInfo };
                case 54:
                    return new GiantLizard { MonsterInfo = monsterInfo, RangeCooldown = TimeSpan.FromSeconds(5), AttackRange = 9 };
                case 56:
                    return new CorrosivePoisonSpitter { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 7, PoisonRate = 15, IgnoreShield = true };
                case 57:
                    return new CorrosivePoisonSpitter { MonsterInfo = monsterInfo };
                case 58:
                    return new Stomper { MonsterInfo = monsterInfo };
                case 59:
                    return new CrimsonNecromancer() { MonsterInfo = monsterInfo };
                case 60:
                    return new ChaosKnight() { MonsterInfo = monsterInfo };
                case 61:
                    return new PachontheChaosbringer { MonsterInfo = monsterInfo };
                case 62:
                    return new NumaHighMage { MonsterInfo = monsterInfo, AttackRange = 6 };
                case 63:
                    return new NumaStoneThrower { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 64:
                    return new Monkey { MonsterInfo = monsterInfo };
                case 65:
                    return new IcyGoddess { MonsterInfo = monsterInfo, FindRange = 3, AttackRange = 8 };
                case 66:
                    return new IcySpiritWarrior { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 25, AttackRange = 8 };
                case 67:
                    return new IcySpiritGeneral
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                        AttackRange = 8,
                    };
                case 68:
                    return new Warewolf
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                    };
                case 69:
                    return new JinamStoneGate { MonsterInfo = monsterInfo };
                case 70:
                    return new FrostLordHwa { MonsterInfo = monsterInfo };
                case 71:
                    return new BanyoWarrior { MonsterInfo = monsterInfo };
                case 72:
                    return new BanyoCaptain { MonsterInfo = monsterInfo };
                case 74:
                    return new BanyoLordGuzak
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BanyoCaptain)] = 2,
                        }
                    };
                case 75:
                    return new DepartedMonster
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MatureEarwig)] = 1 }
                    };
                case 76:
                    return new DepartedMonster
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoldenArmouredBeetle)] = 1 }
                    };
                case 77:
                    return new EnragedLordNiJae
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Millipede)] = 1 },
                        MaxMinions = 200,
                    };
                case 78:
                    return new JinchonDevil { MonsterInfo = monsterInfo };
                case 79:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 10, RangeCooldown = TimeSpan.FromSeconds(5) };
                case 80:
                    return new SunFeralWarrior
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.FerociousFlameDemon)] = 5,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.FlameDemon)] = 1,
                        }
                    };
                case 81:
                    return new MoonFeralWarrior
                    {
                        MonsterInfo = monsterInfo
                    };
                case 82:
                    return new OxFeralGeneral
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                    };
                case 83:
                    return new FlameDemon
                    {
                        MonsterInfo = monsterInfo,
                        Min = -2,
                        Max = 2,
                    };
                case 84:
                    return new WingedHorror
                    {
                        MonsterInfo = monsterInfo,
                        RangeChance = 1,
                    };
                case 85:
                    return new EmperorSaWoo { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 8 };
                case 86:
                    return new FlameDemon
                    {
                        MonsterInfo = monsterInfo,
                        Passive = true,
                        Min = 0,
                        Max = 8,
                    };
                case 87:
                    return new OmaWarlord
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Abyss,
                        PoisonTicks = 1,
                        PoisonFrequency = 7,
                        PoisonRate = 15
                    };
                case 88:
                    return new GoruSpearman
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 89:
                    return new GoruArcher
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10,
                        AttackRange = 7,
                        FearRate = 6,
                        FearDuration = 2,
                    };
                case 90:
                    return new OmaWarlord
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Paralysis,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 25
                    };
                case 91:
                    return new EnragedArchLichTaedu
                    {
                        MonsterInfo = monsterInfo,

                        MinSpawn = 5,
                        RandomSpawn = 5,

                        PoisonType = PoisonType.Red,
                        PoisonTicks = 1,
                        PoisonFrequency = 25,
                        PoisonRate = 5,

                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruArcher)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruGeneral)] = 5,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruSpearman)] = 5,
                        }
                    };
                case 92:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 9 };
                case 93:
                    return new EscortCommander { MonsterInfo = monsterInfo };
                case 94:
                    return new FieryDancer { MonsterInfo = monsterInfo, AttackRange = 12 };
                case 95:
                    return new FieryDancer
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Paralysis,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 15,
                        AttackRange = 12
                    };
                case 96:
                    return new QueenOfDawn { MonsterInfo = monsterInfo };
                case 97:
                    return new SonicLizard { MonsterInfo = monsterInfo, IgnoreShield = true, Range = 5 };
                case 98:
                    return new YumgonWitch
                    {
                        MonsterInfo = monsterInfo,
                        AoEElement = Element.Lightning,
                        AttackRange = 10
                    };
                case 99:
                    return new JinhwanSpirit
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [monsterInfo] = 1,
                        }
                    };
                case 100:
                    return new YumgonWitch
                    {
                        MonsterInfo = monsterInfo,
                        AttackRange = 10
                    };
                case 101:
                    return new DragonQueen
                    {
                        MonsterInfo = monsterInfo,
                        DragonLordInfo = SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.DragonLord),

                        SpawnList =
                         {
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OYoungBeast)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonWitch)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarden)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarlord)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanSpirit)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanGuardian)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OyoungGeneral)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonGeneral)] = 2,
                         }
                    };
                case 102:
                    return new DragonLord
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                         {
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OYoungBeast)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonWitch)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarden)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarlord)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanSpirit)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanGuardian)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OyoungGeneral)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonGeneral)] = 10000,

                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.DragonLord)] = 1,
                         }
                    };
                case 103:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 5 };
                case 104:
                    return new FerociousIceTiger { MonsterInfo = monsterInfo };
                case 105:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 5, IgnoreShield = true, CanPvPRange = true };
                case 106:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 7, CanPvPRange = true };
                case 107:
                    return new SamaFireGuardian { MonsterInfo = monsterInfo };
                case 108:
                    return new SamaIceGuardian { MonsterInfo = monsterInfo };
                case 109:
                    return new SamaLightningGuardian { MonsterInfo = monsterInfo };
                case 110:
                    return new SamaWindGuardian { MonsterInfo = monsterInfo };

                case 111:
                    return new SamaPhoenix { MonsterInfo = monsterInfo };
                case 112:
                    return new SamaBlack { MonsterInfo = monsterInfo };
                case 113:
                    return new SamaBlue { MonsterInfo = monsterInfo };
                case 114:
                    return new SamaWhite { MonsterInfo = monsterInfo };

                case 115:
                    return new SamaProphet
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SamaSorcerer)] = 1,
                        }
                    };
                case 116:
                    return new SamaScorcer()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 117:
                    return new BanyoWarrior { MonsterInfo = monsterInfo, DoubleDamage = true };
                case 118:
                    return new OmaMage { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 119:
                    return new MonsterObject
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10
                    };
                case 120:
                    return new DoomClaw()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 121:
                    return new PinkBat { MonsterInfo = monsterInfo };
                case 122:
                    return new QuartzTurtleSub
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzMiniTurtle)] = 2,
                        }
                    };
                case 123:
                    return new Larva
                    {
                        MonsterInfo = monsterInfo,
                        Range = 3,
                    };
                case 124:
                    return new QuartzTree
                    {
                        MonsterInfo = monsterInfo,
                        SubBossInfo = SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzTurtleSub),
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzBlueBat)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzPinkBat)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzBlueCrystal)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzRedHood)] = 2,
                        }
                    };
                case 125:
                    return new CarnivorousPlant { MonsterInfo = monsterInfo, HideRange = 1, FindRange = 1 };
                case 126:
                    return new MonasteryBoss
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Sacrafice)] = 1,
                        }
                    };
                case 127:
                    return new JinchonDevil { MonsterInfo = monsterInfo, CastDelay = TimeSpan.FromSeconds(8), DeathCloudDurationMin = 2000, DeathCloudDurationRandom = 5000 };
                case 128:
                    return new CastleOutPost
                    {
                        MonsterInfo = monsterInfo
                    };
                case 129:
                    return new CastleFlag { MonsterInfo = monsterInfo };
                case 130:
                    return new OmaWarlock { MonsterInfo = monsterInfo, AttackRange = 7, FearRate = 6, FearDuration = 2, };
                case 131:
                    return new OmaKing { MonsterInfo = monsterInfo };
                case 132:
                    return new NumaKing
                    {
                        MonsterInfo = monsterInfo,
                        AttackRange = 2,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.NumaArmoredSoldier)] = 50,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.NumaCavalry)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.NumaHighMage)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.NumaStoneThrower)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.NumaRoyalGuard)] = 3
                        }
                    };
                case 133:
                    return new CTFFlag { MonsterInfo = monsterInfo };
                case 134:
                    return new DeathLordJinchon
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Red,
                        PoisonTicks = 1,
                        PoisonFrequency = 10,
                        PoisonRate = 10,
                        AttackRange = 4,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SawToothLizard)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.VenomSpitter)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SonicLizard)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GiantLizard)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.CrazedLizard)] = 3
                        }
                    };
                case 135:
                    return new JinchonWarlord
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10

                    };
                case 136:
                    return new CastleGuard
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 137:
                    return new CastleGuard
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 138:
                    return new OrcLord
                    {
                        MonsterInfo = monsterInfo,
                    };
                    
                case 139:
                    return new CrimsonNecromancer()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 140:
                    return new SkeletonAxeThrower   
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10,
                        AttackRange = 7,
                        FearRate = 6,
                        FearDuration = 2,
                    };
                case 141:
                    return new Ikikra()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 142:
                    return new OrcBoss
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Paralysis,
                        PoisonTicks = 1,
                        PoisonFrequency = 10,
                        PoisonRate = 10,
                        AttackRange = 2,
                      /*  SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SawToothLizard)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.VenomSpitter)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SonicLizard)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GiantLizard)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.CrazedLizard)] = 3
                        } */
                    };
                case 143:
                    return new SeaCaveGate { MonsterInfo = monsterInfo };
                case 144:
                    return new FlowerGate { MonsterInfo = monsterInfo };
                default:
                    return new MonsterObject { MonsterInfo = monsterInfo };
            }
        }
        public MonsterObject()
        {
            Stats = new Stats();
            Direction = (MirDirection)SEnvir.Random.Next(8);
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();

            if (MonsterInfo.CanSuper && MonsterInfo.SuperChance > 0 && SEnvir.Random.Next(MonsterInfo.SuperChance) == 0 && PetOwner == null && Master == null)
            {
                SuperMob = true;
                SEnvir.Broadcast(new S.Chat { Text = ("Super " + MonsterInfo.MonsterName + " has spawned on map " + CurrentMap.Info.Description), Type = MessageType.System });
                CurrentMap.Bosses.Add(this);
            }

            if (SpawnInfo != null && SpawnInfo.Info.EasterEventChance > 0 && SEnvir.Now < Config.EasterEventEnd)
                EasterEventMob = SEnvir.Random.Next(SpawnInfo.Info.EasterEventChance) == 0;

            int offset = 1000000;

            MapHealthRate = SEnvir.Random.Next(CurrentMap.Info.MonsterHealth + offset, CurrentMap.Info.MaxMonsterHealth + offset);
            MapDamageRate = SEnvir.Random.Next(CurrentMap.Info.MonsterDamage + offset, CurrentMap.Info.MaxMonsterDamage + offset);

            if (MapHealthRate >= CurrentMap.Info.ExperienceRate && MapHealthRate <= CurrentMap.Info.MaxExperienceRate)
                MapExperienceRate = MapHealthRate;
            else if (CurrentMap.Info.ExperienceRate > CurrentMap.Info.MaxExperienceRate)
                MapExperienceRate = CurrentMap.Info.ExperienceRate + offset;
            else
                MapExperienceRate = SEnvir.Random.Next(CurrentMap.Info.ExperienceRate + offset, CurrentMap.Info.MaxExperienceRate + offset);

            MapDropRate = SEnvir.Random.Next(CurrentMap.Info.DropRate + offset, CurrentMap.Info.MaxDropRate + offset);
            MapGoldRate = SEnvir.Random.Next(CurrentMap.Info.GoldRate + offset, CurrentMap.Info.MaxGoldRate + offset);


            MapHealthRate -= offset;
            MapDamageRate -= offset;
            MapExperienceRate -= offset;
            MapDropRate -= offset;
            MapGoldRate -= offset;
            if (SuperMob)
            {
                MapExperienceRate += (10000);
                MapDropRate += (10000);
                MapGoldRate += (10000);
            }

            RefreshStats();
            CurrentHP = Stats[Stat.Health];
            DisplayHP = CurrentHP;

            RegenTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)RegenDelay.TotalMilliseconds));
            SearchTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)SearchDelay.TotalMilliseconds));
            RoamTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)RoamDelay.TotalMilliseconds));

            ActionTime = SEnvir.Now.AddSeconds(1);

            Level = MonsterInfo.Level;

            if (SuperMob)
            {
                Level = 250;
                CoolEye = true;
            }
            else
            {
                CoolEye = SEnvir.Random.Next(100) < MonsterInfo.CoolEye;
                Level = MonsterInfo.Level;
            }

            AddAllObjects();

            Activate();
        }

        public override void RefreshStats()
        {
            base.RefreshStats();

            Stats.Clear();
            Stats.Add(MonsterInfo.Stats);

            ApplyBonusStats();

            MoveDelay = MonsterInfo.MoveDelay;
            AttackDelay = MonsterInfo.AttackDelay;


            if (SummonLevel > 0)
            {
                Stats[Stat.Health] += Stats[Stat.Health] * SummonLevel / 10;

                Stats[Stat.MinAC] += Stats[Stat.MinAC] * SummonLevel / 10;
                Stats[Stat.MaxAC] += Stats[Stat.MaxAC] * SummonLevel / 10;

                Stats[Stat.MinMR] += Stats[Stat.MinMR] * SummonLevel / 10;
                Stats[Stat.MaxMR] += Stats[Stat.MaxMR] * SummonLevel / 10;

                Stats[Stat.MinDC] += Stats[Stat.MinDC] * SummonLevel / 10;
                Stats[Stat.MaxDC] += Stats[Stat.MaxDC] * SummonLevel / 10;

                Stats[Stat.MinMC] += Stats[Stat.MinMC] * SummonLevel / 10;
                Stats[Stat.MaxMC] += Stats[Stat.MaxMC] * SummonLevel / 10;

                Stats[Stat.MinSC] += Stats[Stat.MinSC] * SummonLevel / 10;
                Stats[Stat.MaxSC] += Stats[Stat.MaxSC] * SummonLevel / 10;

                Stats[Stat.Accuracy] += Stats[Stat.Accuracy] * SummonLevel / 10;
                Stats[Stat.Agility] += Stats[Stat.Agility] * SummonLevel / 10;
            }
            if (SuperMob)
            {
                string smob = MonsterInfo.SuperName;
                if (MonsterInfo.SuperName == "")
                    smob = "SuperMob";
                var smobinfo = SEnvir.GetMonsterInfo(smob);

                Stats[Stat.Health] *= 5;

                Stats.Add(smobinfo.Stats);

                Stats[Stat.FireResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.IceResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.LightningResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.WindResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.HolyResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.DarkResistance] += SEnvir.Random.Next(1, 3);
                Stats[Stat.PhantomResistance] += SEnvir.Random.Next(1, 3);

                Stats[Stat.Accuracy] += 50;

                MoveDelay = 700;
                AttackDelay = 600;
            }

            Stats[Stat.CriticalChance] = 1;

            if (Buffs.Any(x => x.Type == BuffType.MagicWeakness))
            {
                Stats[Stat.MinMR] = 0;
                Stats[Stat.MaxMR] = 0;
            }

            if (Buffs.Any(x => x.Type == BuffType.DesecratedArmour))
            {
                Stats[Stat.MinAC] = 0;
                Stats[Stat.MaxAC] = 0;
            }

            if (Buffs.Any(x => x.Type == BuffType.AuraofDread))
            {
                Stats[Stat.HandWeight] = -1;
                Stats[Stat.WearWeight] = -1;
            }

            foreach (BuffInfo buff in Buffs)
            {
                if (buff.Stats == null)
                    continue;
                Stats.Add(buff.Stats);
            }

            if (PetOwner != null && PetOwner.Stats[Stat.PetDCPercent] > 0)
            {
                Stats[Stat.MinDC] += Stats[Stat.MinDC] * PetOwner.Stats[Stat.PetDCPercent] / 100;
                Stats[Stat.MaxDC] += Stats[Stat.MaxDC] * PetOwner.Stats[Stat.PetDCPercent] / 100;
                Stats[Stat.MinAC] += Stats[Stat.MinAC] * PetOwner.Stats[Stat.PetACPercent] / 100;
                Stats[Stat.MaxAC] += Stats[Stat.MaxAC] * PetOwner.Stats[Stat.PetACPercent] / 100;
                Stats[Stat.MinMR] += Stats[Stat.MinMR] * PetOwner.Stats[Stat.PetMRPercent] / 100;
                Stats[Stat.MaxMR] += Stats[Stat.MaxMR] * PetOwner.Stats[Stat.PetMRPercent] / 100;

                foreach (UserMagic magic in Magics)
                {
                    switch (magic.Info.Magic)
                    {
                        case MagicType.DemonicRecovery:
                            Stats[Stat.Health] += (magic.Level + 1) * 300;
                            break;
                    }
                }
            }




            /*
            Stats[Stat.FireResistance] = Math.Min(5, Stats[Stat.FireResistance]);
            Stats[Stat.IceResistance] = Math.Min(5, Stats[Stat.IceResistance]);
            Stats[Stat.LightningResistance] = Math.Min(5, Stats[Stat.LightningResistance]);
            Stats[Stat.WindResistance] = Math.Min(5, Stats[Stat.WindResistance]);
            Stats[Stat.HolyResistance] = Math.Min(5, Stats[Stat.HolyResistance]);
            Stats[Stat.DarkResistance] = Math.Min(5, Stats[Stat.DarkResistance]);
            Stats[Stat.PhantomResistance] = Math.Min(5, Stats[Stat.PhantomResistance]);
            */


            Stats[Stat.Health] += (int)(Stats[Stat.Health] * (long)Stats[Stat.HealthPercent] / 100);
            Stats[Stat.Mana] += (int)(Stats[Stat.Mana] * (long)Stats[Stat.ManaPercent] / 100);

            Stats[Stat.MinDC] += (int)(Stats[Stat.MinDC] * (long)Stats[Stat.DCPercent] / 100);
            Stats[Stat.MaxDC] += (int)(Stats[Stat.MaxDC] * (long)Stats[Stat.DCPercent] / 100);

            Stats[Stat.MinMC] += (int)(Stats[Stat.MinMC] * (long)Stats[Stat.MCPercent] / 100);
            Stats[Stat.MaxMC] += Stats[Stat.MaxMC] * Stats[Stat.MCPercent] / 100;

            Stats[Stat.MinSC] += (int)(Stats[Stat.MinSC] * (long)Stats[Stat.SCPercent] / 100);
            Stats[Stat.MaxSC] += (int)(Stats[Stat.MaxSC] * (long)Stats[Stat.SCPercent] / 100);

            Stats[Stat.MinAC] += (int)(Stats[Stat.MinAC] * (long)Stats[Stat.ACPercent] / 100);
            Stats[Stat.MaxAC] += (int)(Stats[Stat.MaxAC] * (long)Stats[Stat.ACPercent] / 100);

            Stats[Stat.MinMR] += (int)(Stats[Stat.MinMR] * (long)Stats[Stat.MRPercent] / 100);
            Stats[Stat.MaxMR] += (int)(Stats[Stat.MaxMR] * (long)Stats[Stat.MRPercent] / 100);

            if (PetOwner == null && CurrentMap != null)
            {
                Stats[Stat.Health] += (int)(Stats[Stat.Health] * (long)MapHealthRate / 100);

                Stats[Stat.MinDC] += (int)(Stats[Stat.MinDC] * (long)MapDamageRate / 100);
                Stats[Stat.MaxDC] += (int)(Stats[Stat.MaxDC] * (long)MapDamageRate / 100);
            }


            Stats[Stat.Health] = Math.Max(1, Stats[Stat.Health]);
            Stats[Stat.Mana] = Math.Max(1, Stats[Stat.Mana]);

            Stats[Stat.MinAC] = Math.Max(0, Stats[Stat.MinAC]);
            Stats[Stat.MaxAC] = Math.Max(0, Stats[Stat.MaxAC]);
            Stats[Stat.MinMR] = Math.Max(0, Stats[Stat.MinMR]);
            Stats[Stat.MaxMR] = Math.Max(0, Stats[Stat.MaxMR]);
            Stats[Stat.MinDC] = Math.Max(0, Stats[Stat.MinDC]);
            Stats[Stat.MaxDC] = Math.Max(0, Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Max(0, Stats[Stat.MinMC]);
            Stats[Stat.MaxMC] = Math.Max(0, Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Max(0, Stats[Stat.MinSC]);
            Stats[Stat.MaxSC] = Math.Max(0, Stats[Stat.MaxSC]);

            Stats[Stat.MinDC] = Math.Min(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Min(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Min(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

            if (EasterEventMob)
                Stats[Stat.Health] = 1;

            //if (ChristmasEventMob)
              //  Stats[Stat.Health] = 10;

            S.DataObjectMaxHealthMana p = new S.DataObjectMaxHealthMana { ObjectID = ObjectID, Stats = Stats };

            foreach (PlayerObject player in DataSeenByPlayers)
                player.Enqueue(p);


            if (CurrentHP > Stats[Stat.Health])
                SetHP(Stats[Stat.Health]);
            if (CurrentMP > Stats[Stat.Mana])
                SetMP(Stats[Stat.Mana]);
        }
        public virtual void ApplyBonusStats()
        {

        }

        public override void CleanUp()
        {
            base.CleanUp();

            _Target = null;

            SpawnList?.Clear();
            SpawnList = null;

            _EXPOwner = null;

            Drops?.Clear();


            Magics?.Clear();
        }


        public override void Activate()
        {
            if (Activated)
                return;

            if (NearByPlayers.Count == 0 && MonsterInfo.ViewRange <= Config.MaxViewRange && !MonsterInfo.IsBoss && PetOwner == null)
                return;

            Activated = true;
            SEnvir.AddActiveObject(this);
        }
        public override void DeActivate()
        {
            if (!Activated)
                return;

            if (NearByPlayers.Count > 0 || MonsterInfo.ViewRange > Config.MaxViewRange || Target != null || MonsterInfo.IsBoss || PetOwner != null || ActionList.Count > 0 || CurrentHP < Stats[Stat.Health])
                return;

            Activated = false;
            SEnvir.RemoveActiveObject(this.ObjectID);
        }



        public override void ProcessAction(DelayedAction action)
        {
            switch (action.Type)
            {
                case ActionType.DelayAttack:
                    Attack((MapObject)action.Data[0], (int)action.Data[1], (Element)action.Data[2]);
                    return;
                case ActionType.DelayMagic:
                    switch ((MagicType)action.Data[0])
                    {
                        case MagicType.FireWall:
                            FireWallEnd((Cell)action.Data[1]);
                            break;
                        case MagicType.DragonRepulse:
                            DragonRepulseEnd((MapObject)action.Data[1]);
                            break;
                        case MagicType.Purification:
                            Purify((MapObject)action.Data[1]);
                            break;
                        case MagicType.Tempest:
                            TempestEnd((Cell)action.Data[1]);
                            break;
                        case MagicType.MonsterDeathCloud:
                            DeathCloudEnd((Cell)action.Data[1], (bool)action.Data[2], (Point)action.Data[3]);
                            break;
                        case MagicType.MonsterParalysisCloud:
                            ParalysisCloudEnd((Cell)action.Data[1], (bool)action.Data[2], (Point)action.Data[3]);
                            break;
                    }
                    break;
            }

            base.ProcessAction(action);
        }

        public override void Process()
        {
            base.Process();

            if (Dead)
            {
                Target = null;

                if (SEnvir.Now > DeadTime)
                {
                    Despawn();
                    return;
                }
            }
            
            if (Target.HasNoNode() || Target.Dead || Target.CurrentMap != CurrentMap || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Config.MaxViewRange) ||
                ((Poison & PoisonType.Abyss) == PoisonType.Abyss && !Functions.InRange(CurrentLocation, Target.CurrentLocation, ViewRange)) || !CanAttackTarget(Target))
                Target = null;

            if (Target != null && Target.Buffs.Any(x => x.Type == BuffType.Cloak) && !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2) && Stats[Stat.IgnoreStealth] == 0)
                Target = null;

            if (Target != null && Target.Buffs.Any(x => x.Type == BuffType.Transparency))
                Target = null;

            ProcessAI();
        }
        public override void ProcessNameColour()
        {
            NameColour = Color.White;

            if (SEnvir.Now < ShockTime)
                NameColour = Color.Peru;
            else if (SEnvir.Now < RageTime)
                NameColour = Color.Red;

            if (SuperMob)
                NameColour = Color.DarkRed;
        }

        public virtual void ProcessAI()
        {
            if (Dead)
                return;

            if (PetOwner.HasNode())
            {
                if (Target != null)
                {
                    if (PetOwner.PetMode == PetMode.PvP && Target.Race != ObjectType.Player)
                        Target = null;

                    if (PetOwner.PetMode == PetMode.None || PetOwner.PetMode == PetMode.Move)
                        Target = null;
                }
                if (SEnvir.Now > TameTime)
                    UnTame();
                else if (Visible && !PetOwner.VisibleObjects.Contains(this) && (PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Move || PetOwner.PetMode == PetMode.PvP))
                    PetRecall(); //Could be killed.
            }

            ProcessRegen();
            ProcessSearch();
            ProcessRoam();
            ProcessTarget();
        }
        public override void OnSafeDespawn()
        {
            base.OnSafeDespawn();

            Master?.MinionList.Remove(this);
            Master = null;

            PetOwner?.Pets.Remove(this);
            PetOwner = null;

            if (MinionList != null)
            {
                for (int i = MinionList.Count - 1; i >= 0; i--)
                    MinionList[i].Master = null;

                MinionList.Clear();
            }


            if (SpawnInfo != null)
                SpawnInfo.AliveCount--;

            ProcessEvents();

            SpawnInfo = null;

            EXPOwner = null;
        }


        public void UnTame()
        {
            PetOwner?.Pets.Remove(this);
            PetOwner = null;
            Target = null;
            SearchTime = DateTime.MinValue;
            Magics.Clear();
            SummonLevel = 0;
            RefreshStats();

            SetHP(Math.Min(CurrentHP, Stats[Stat.Health] / 10));

            Broadcast(new S.ObjectPetOwnerChanged { ObjectID = ObjectID });
        }
        public void PetRecall()
        {
            Cell cell = PetOwner.CurrentMap.GetCell(Functions.Move(PetOwner.CurrentLocation, PetOwner.Direction, -1));

            if (cell == null || cell.Movements != null)
                cell = PetOwner.CurrentCell;

            Teleport(PetOwner.CurrentMap, cell.Location);
        }
        public virtual void ProcessRegen()
        {
            if (SEnvir.Now < RegenTime)
                return;

            RegenTime = SEnvir.Now + RegenDelay;

            if (CurrentHP >= Stats[Stat.Health])
                return;

            int regen = (int)Math.Max(1, Stats[Stat.Health] * 0.02F); //2% every 10 seconds aprox

            ChangeHP(regen);
        }

        public virtual void ProcessSearch()
        {
            if (PetOwner != null)
            {
                ProperSearch();
                return;
            }

            if (Target != null)
            {
                if (!Visible)
                {
                    if (SEnvir.Now < SearchTime)
                        return;
                }
                else if (!CanMove && !CanAttack)
                    return;
            }
            else if (SEnvir.Now < SearchTime || CurrentMap.Players.Count == 0)
                return;

            SearchTime = SEnvir.Now + SearchDelay;


            int bestDistance = int.MaxValue;
            List<MapObject> closest = new List<MapObject>();


            //Save resources
            foreach (PlayerObject player in CurrentMap.Players)
            {
                int distance;

                foreach (MonsterObject pet in player.Pets)
                {
                    if (pet.CurrentMap != CurrentMap)
                        continue;

                    distance = Functions.Distance(pet.CurrentLocation, CurrentLocation);

                    if (distance > ViewRange)
                        continue;

                    if (distance > bestDistance || !ShouldAttackTarget(pet))
                        continue;

                    if (distance != bestDistance)
                        closest.Clear();

                    closest.Add(pet);
                    bestDistance = distance;
                }

                distance = Functions.Distance(player.CurrentLocation, CurrentLocation);

                if (distance > ViewRange)
                    continue;

                if (distance > bestDistance || !ShouldAttackTarget(player))
                    continue;

                if (distance != bestDistance)
                    closest.Clear();

                closest.Add(player);
                bestDistance = distance;
            }

            if (closest.Count == 0)
                return;

            Target = closest[SEnvir.Random.Next(closest.Count)];
        }
        public void ProperSearch()
        {
            if (Target != null)
            {
                if (!CanMove && !CanAttack && Visible)
                    return;
            }
            else
            {
                if (SEnvir.Now < SearchTime)
                    return;

                SearchTime = SEnvir.Now + SearchDelay;

                if (CurrentMap.Players.Count == 0 && !HalloweenEventMob)
                    return;
            }

            for (int d = 0; d <= ViewRange; d++)
            {
                List<MapObject> closest = new List<MapObject>();
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0)
                        continue;
                    if (y >= CurrentMap.Height)
                        break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0)
                            continue;
                        if (x >= CurrentMap.Width)
                            break;

                        Cell cell = CurrentMap.Cells[x, y];

                        if (cell?.Objects == null)
                            continue;

                        foreach (MapObject ob in cell.Objects)
                        {
                            if (!ShouldAttackTarget(ob))
                                continue;

                            closest.Add(ob);
                        }
                    }
                }
                if (closest.Count == 0)
                    continue;

                Target = closest[SEnvir.Random.Next(closest.Count)];

                return;
            }

        }

        public virtual void ProcessRoam()
        {
            if (!CanMove)
                return;

            if (PetOwner != null)
            {
                if (Target == null)
                    MoveTo(Functions.Move(PetOwner.CurrentLocation, PetOwner.Direction, -1));
                return;
            }

            if (SEnvir.Now < RoamTime || SeenByPlayers.Count == 0)
                return;

            RoamTime = SEnvir.Now + RoamDelay;


            foreach (MapObject ob in CurrentCell.Objects)
            {
                if (ob == this || !ob.Blocking)
                    continue;

                MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                for (int d = 0; d < 8; d++)
                {
                    if (Walk(direction))
                        return;

                    direction = Functions.ShiftDirection(direction, rotation);
                }
                return;
            }

            if (Target != null || SEnvir.Random.Next(10) > 0)
                return;

            if (SEnvir.Random.Next(3) > 0)
                Walk(Direction);
            else
                Turn((MirDirection)SEnvir.Random.Next(8));
        }
        public virtual void ProcessTarget()
        {
            if (Target == null)
                return;

            if (!InAttackRange())
            {
                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction))
                            break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);

                return;
            }

            if (!CanAttack)
                return;

            Attack();
        }

        public void SpawnMinions(int fixedCount, int randomCount, MapObject target)
        {
            int count = Math.Min(MaxMinions - MinionList.Count, SEnvir.Random.Next(randomCount + 1) + fixedCount);

            for (int i = 0; i < count; i++)
            {
                MonsterInfo info = SEnvir.GetMonsterInfo(SpawnList);

                if (info == null)
                    continue;

                MonsterObject mob = GetMonster(info);

                if (!SpawnMinion(mob))
                    return;

                mob.Target = target;
                mob.Master = this;
                MinionList.Add(mob);
            }
        }
        public virtual bool SpawnMinion(MonsterObject mob)
        {
            return mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 6));
        }
        public override int Pushed(MirDirection direction, int distance)
        {
            if (!MonsterInfo.CanPush)
                return 0;

            return base.Pushed(direction, distance);
        }

        protected virtual bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap)
                return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
        protected virtual int NearbyTargets(int range = 1)
        {
            int count = 0;

            foreach (PlayerObject ob in CurrentMap.Players)
            {
                if (Functions.InRange(CurrentLocation, ob.CurrentLocation, range))
                    count++;
            }

            return count;
        }
        public bool TeleportToTarget()
        {
            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Cell cell = null;
            for (int i = 0; i < 8; i++)
            {
                cell = CurrentMap.GetCell(Functions.Move(Target.CurrentLocation, Functions.ShiftDirection(dir, i), 1));

                if (cell == null || cell.Movements != null)
                {
                    cell = null;
                    continue;
                }
                break;
            }

            if (cell != null)
            {
                Direction = Functions.DirectionFromPoint(cell.Location, Target.CurrentLocation);
                Teleport(CurrentMap, cell.Location);
                return true;
            }
            return false;
        }
        public override bool CanAttackTarget(MapObject ob)
        {
            if (ob == this || ob.HasNoNode() || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord)
                return false;

            switch (ob.Race)
            {
                case ObjectType.Item:
                case ObjectType.NPC:
                case ObjectType.Spell:
                    return false;
            }

            switch (ob.Race)
            {
                case ObjectType.Player:
                    PlayerObject player = (PlayerObject)ob;

                    if (player.GameMaster)
                        return false;

                    if (PetOwner == null)
                        return true;
                    if (PetOwner == player)
                        return false;

                    if (InSafeZone || player.InSafeZone)
                        return false;

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                            return false;
                    }
                    if (CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, player);
                    }
                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return false;
                            break;
                    }

                    //Are any Pets attacking target or any player's pets attacking pets

                    return true;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null)
                    {
                        if (mob.PetOwner == null)
                            return SEnvir.Now < RageTime; //Wild vs Wild

                        return true; //Wild vs Pet
                    }

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                        case PetMode.PvP:
                            return false;
                    }

                    //Pet vs Wild
                    if (mob.PetOwner == null)
                        return true;

                    //Pet vs Pet
                    if (mob.InSafeZone || InSafeZone)
                        return false;

                    if (PetOwner == mob.PetOwner)
                        return false;

                    if (CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, mob.PetOwner);
                    }

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return false;
                            break;
                    }

                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        public override bool CanHelpTarget(MapObject ob)
        {
            if (ob.HasNoNode() || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord)
                return false;

            if (ob == this)
                return true;

            switch (ob.Race)
            {
                case ObjectType.Player:
                    if (PetOwner == null)
                        return false;

                    PlayerObject player = (PlayerObject)ob;

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return true;

                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return true;
                            break;

                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return true;
                            break;

                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return true;
                            break;
                    }

                    return true;

                case ObjectType.Monster:


                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null)
                        return mob.PetOwner == null;

                    if (mob.PetOwner == null)
                        return false;

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return true;

                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return true;
                            break;

                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return true;
                            break;

                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return true;
                            break;
                    }

                    return true;

                default:
                    return false;
            }
        }
        public virtual bool ShouldAttackTarget(MapObject ob)
        {
            if (Passive || ob == this || ob.HasNoNode() || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord)
                return false;

            switch (ob.Race)
            {
                case ObjectType.Item:
                case ObjectType.NPC:
                case ObjectType.Spell:
                    return false;
            }

            if (ob.Buffs.Any(x => x.Type == BuffType.Invisibility) && !CoolEye)
                return false;

            if (ob.Buffs.Any(x => x.Type == BuffType.Cloak) && Stats[Stat.IgnoreStealth] == 0)
            {
                if (!CoolEye)
                    return false;
                if (!Functions.InRange(ob.CurrentLocation, CurrentLocation, 2))
                    return false;
                if (ob.Level >= Level)
                    return false;
            }

            if (ob.Buffs.Any(x => x.Type == BuffType.Transparency) && ((Poison & PoisonType.Infection) != PoisonType.Infection || Level < 100))
                    return false;

            switch (ob.Race)
            {
                case ObjectType.Player:
                    PlayerObject player = (PlayerObject)ob;
                    if (player.GameMaster)
                        return false;

                    if (PetOwner == null)
                        return true;
                    if (PetOwner == player)
                        return false;

                    if (InSafeZone || player.InSafeZone)
                        return false;

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                            return false;
                    }

                    if (CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, player);
                    }

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return false;
                            break;
                    }

                    //Are any Pets attacking target or any player's pets attacking pets

                    if (PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null)
                            return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == player;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == player;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }))
                        return true;

                    if (player.Pets.Any(x =>
                    {
                        if (x.Target == null)
                            return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }))
                        return true;

                    return false;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null)
                    {
                        if (mob.PetOwner == null)
                            return SEnvir.Now < RageTime; //Wild vs Wild

                        return true; //Wild vs Pet
                    }

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                        case PetMode.PvP:
                            return false;
                    }

                    //Pet vs Wild
                    if (mob.PetOwner == null)
                    {
                        if (mob.EXPOwner == PetOwner || PetOwner.InGroup(mob.EXPOwner) || PetOwner.InGuild(mob.EXPOwner))
                            return true;

                        //Is mob's Targets = master or group/guild member

                        if (mob.EXPOwner != null)
                            return false; //Someone else's mob


                        if (mob.Target == null)
                            return false;

                        PlayerObject mobTarget;

                        if (mob.Target.Race == ObjectType.Monster)
                            mobTarget = ((MonsterObject)mob.Target).PetOwner;
                        else
                            mobTarget = (PlayerObject)mob.Target;

                        if (mobTarget.HasNoNode())
                            return false;

                        if (mobTarget == PetOwner || PetOwner.InGroup(mobTarget) || PetOwner.InGuild(mobTarget))
                            return true;

                        return false;
                    }

                    //Pet vs Pet
                    if (mob.InSafeZone || InSafeZone)
                        return false;

                    if (PetOwner == mob.PetOwner)
                        return false;
                    if (CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, mob.PetOwner);
                    }
                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return false;
                            break;
                    }


                    if (PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null)
                            return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == mob.PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == mob.PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }))
                        return true;

                    if (mob.PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null)
                            return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }))
                        return true;

                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
        protected virtual void Attack()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                               SEnvir.Now.AddMilliseconds(400),
                               ActionType.DelayAttack,
                               Target,
                               GetDC(),
                               AttackElement));
        }
        public virtual void AttackLocation(Point location)
        {
            Cell cell = CurrentMap.GetCell(location);

            if (cell?.Objects == null)
                return;

            foreach (MapObject ob in cell.Objects)
            {
                if (!CanAttackTarget(ob))
                    continue;
                
                ActionList.Add(new DelayedAction(
                               SEnvir.Now.AddMilliseconds(400),
                               ActionType.DelayAttack,
                               ob,
                               GetDC(),
                               AttackElement));
             }
        }

        public virtual int Attack(MapObject ob, int power, Element element)
        {
            if (ob.HasNoNode() || ob.Dead)
                return 0;

            int damage;

            if (PoisonList.Any(x => x.Type == PoisonType.Abyss) && SEnvir.Random.Next(2) > 0)
            {
                ob.Dodged();
                return 0;
            }

            if (element == Element.None)
            {
                int accuracy = Stats[Stat.Accuracy];

                if (SEnvir.Random.Next(ob.Stats[Stat.Agility]) > accuracy)
                {
                    ob.Dodged();
                    return 0;
                }

                damage = power - ob.GetAC();
            }
            else
            {
                damage = power - ob.GetMR();
            }

            int res = ob.Stats.GetResistanceValue(element);

            if (res > 0)
                damage -= damage * res / 10;
            else if (res < 0)
                damage -= damage * res / 5;

            if (damage <= 0)
            {
                ob.Blocked();
                return 0;
            }


            damage = ob.Attacked(this, damage, element, true, IgnoreShield);

            if (damage <= 0)
                return damage;

            LifeSteal += damage * Stats[Stat.LifeSteal] / 100M;

            if (LifeSteal > 1)
            {
                int heal = (int)Math.Floor(LifeSteal);
                LifeSteal -= heal;
                ChangeHP(heal);
            }

            foreach (UserMagic magic in Magics)
                PetOwner?.LevelMagic(magic);

            if (PoisonType == PoisonType.None || SEnvir.Random.Next(PoisonRate) > 0)
                return damage;

            ob.ApplyPoison(new Poison
            {
                Owner = this,
                Type = PoisonType,
                Value = GetSC(),
                TickFrequency = TimeSpan.FromSeconds(PoisonFrequency),
                TickCount = PoisonTicks,
            });


            return damage;
        }

        #region Magics

        public void AttackMagic(MagicType magic, Element element, bool travel, int damage = 0)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Targets = new List<uint> { Target.ObjectID } });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500 + (travel ? Functions.Distance(CurrentLocation, Target.CurrentLocation) * 48 : 0)),
                ActionType.DelayAttack,
                Target,
                damage == 0 ? GetDC() : damage,
                element));
        }

        public void AttackAoE(int radius, MagicType magic, Element element, int damage = 0)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Locations = new List<Point> { Target.CurrentLocation } });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, radius);

            foreach (MapObject ob in targets)
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    ob,
                    damage == 0 ? GetDC() : damage,
                    element));
            }
        }
        public void SamaGuardianFire()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.SamaGuardianFire, Locations = new List<Point> { Target.CurrentLocation } });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, 5);

            foreach (MapObject ob in targets)
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    ob,
                    GetDC(),
                    Element.Fire));
            }
        }
        public void LineAoE(int distance, int min, int max, MagicType magic, Element element)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();


            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();
            int damage = GetDC() * 2 / 3;

            for (int d = min; d <= max; d++)
            {
                MirDirection direction = Functions.ShiftDirection(Direction, d);

                if (magic == MagicType.LightningBeam || magic == MagicType.BlowEarth)
                    locations.Add(Functions.Move(CurrentLocation, direction, distance));

                for (int i = 1; i <= distance; i++)
                {
                    Point location = Functions.Move(CurrentLocation, direction, i);
                    Cell cell = CurrentMap.GetCell(location);

                    if (cell == null)
                        continue;

                    if (magic != MagicType.LightningBeam && magic != MagicType.BlowEarth)
                        locations.Add(cell.Location);

                    if (cell.Objects != null)
                    {
                        foreach (MapObject ob in cell.Objects)
                        {
                            if (!CanAttackTarget(ob))
                                continue;

                            ActionList.Add(new DelayedAction(
                                SEnvir.Now.AddMilliseconds(500 + i * 75),
                                ActionType.DelayAttack,
                                ob,
                                damage,
                                element));
                        }
                    }

                    switch (direction)
                    {
                        case MirDirection.Up:
                        case MirDirection.Right:
                        case MirDirection.Down:
                        case MirDirection.Left:
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, -2)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob))
                                        continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        damage / 2,
                                        element));
                                }
                            }
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, 2)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob))
                                        continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        damage / 2,
                                        element));
                                }
                            }
                            break;
                        case MirDirection.UpRight:
                        case MirDirection.DownRight:
                        case MirDirection.DownLeft:
                        case MirDirection.UpLeft:
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, -1)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob))
                                        continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        damage / 2,
                                        element));
                                }
                            }
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, 1)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob))
                                        continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        damage / 2,
                                        element));
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void FireWall()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.FireWall, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, 20);

            if (targets.Count == 0)
                return;

            Point location = targets[SEnvir.Random.Next(targets.Count)].CurrentLocation;

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(location)));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Up))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Down))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Left))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Right))));
        }
        public void FireWallEnd(Cell cell)
        {
            if (cell == null)
                return;

            if (cell.Objects != null)
            {
                for (int i = cell.Objects.Count - 1; i >= 0; i--)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell)
                        continue;

                    SpellObject spell = (SpellObject)cell.Objects[i];

                    if (spell.Effect != SpellEffect.FireWall && spell.Effect != SpellEffect.MonsterFireWall && spell.Effect != SpellEffect.Tempest && spell.Effect != SpellEffect.MonsterTempest)
                        continue;

                    spell.Despawn();
                }
            }

            SpellObject ob = new SpellObject
            {
                DisplayLocation = cell.Location,
                TickCount = 15,
                TickFrequency = TimeSpan.FromSeconds(2),
                Owner = this,
                Effect = SpellEffect.MonsterFireWall
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }

        public void Tempest()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Tempest, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, 20);

            if (targets.Count == 0)
                return;

            Point location = targets[SEnvir.Random.Next(targets.Count)].CurrentLocation;

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Tempest,
                CurrentMap.GetCell(location)));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Tempest,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Up))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Tempest,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Down))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Tempest,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Left))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Tempest,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Right))));
        }
        public void TempestEnd(Cell cell)
        {
            if (cell == null)
                return;

            if (cell.Objects != null)
            {
                for (int i = cell.Objects.Count - 1; i >= 0; i--)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell)
                        continue;

                    SpellObject spell = (SpellObject)cell.Objects[i];

                    if (spell.Effect != SpellEffect.FireWall && spell.Effect != SpellEffect.MonsterFireWall && spell.Effect != SpellEffect.Tempest && spell.Effect != SpellEffect.MonsterTempest)
                        continue;

                    spell.Despawn();
                }
            }

            SpellObject ob = new SpellObject
            {
                DisplayLocation = cell.Location,
                TickCount = 15,
                TickFrequency = TimeSpan.FromSeconds(2),
                Owner = this,
                Effect = SpellEffect.MonsterTempest
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }

        public void DeathCloud(Point location)
        {
            bool visible = true;
            foreach (Cell cell in CurrentMap.GetCells(location, 0, 2))
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayMagic,
                    MagicType.MonsterDeathCloud,
                    cell,
                    visible,
                    location));

                visible = false;
            }
        }
        public void DeathCloudEnd(Cell cell, bool visible, Point displaylocation)
        {
            if (cell == null)
                return;

            SpellObject ob = new SpellObject
            {
                DisplayLocation = displaylocation,
                TickCount = 1,
                TickTime = SEnvir.Now.AddMilliseconds(DeathCloudDurationMin + SEnvir.Random.Next(DeathCloudDurationRandom)),
                Owner = this,
                Effect = SpellEffect.MonsterDeathCloud,
                Visible = visible,
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }
        public void ParalysisCloud(Point location)
        {
            bool visible = true;
            foreach (Cell cell in CurrentMap.GetCells(location, 0, 2))
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayMagic,
                    MagicType.MonsterParalysisCloud,
                    cell,
                    visible,
                    location));

                visible = false;
            }
        }
        public void ParalysisCloudEnd(Cell cell, bool visible, Point displaylocation)
        {
            if (cell == null)
                return;

            SpellObject ob = new SpellObject
            {
                DisplayLocation = displaylocation,
                TickCount = 1,
                TickTime = SEnvir.Now.AddMilliseconds(DeathCloudDurationMin + SEnvir.Random.Next(DeathCloudDurationRandom)),
                Owner = this,
                Effect = SpellEffect.ParalysisCloud,
                Visible = visible,
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }
        public void MassLightningBall()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.LightningBall, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X - 20, CurrentLocation.Y - i));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + 20, CurrentLocation.Y - i));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + i, CurrentLocation.Y - 20));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + i, CurrentLocation.Y + 20));

            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, Config.MaxViewRange);

            foreach (MapObject ob in targets)
            {
                targetIDs.Add(ob.ObjectID);

                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500 + Functions.Distance(ob.CurrentLocation, CurrentLocation) * 48),
                    ActionType.DelayAttack,
                    ob,
                    GetDC(),
                    Element.Lightning));
            }
        }
        public void MassThunderBolt()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.ThunderBolt, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();


            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, Config.MaxViewRange);
            foreach (Cell cell in cells)
            {
                if (cell.Objects == null)
                {
                    if (SEnvir.Random.Next(50) == 0)
                        locations.Add(cell.Location);

                    continue;
                }

                foreach (MapObject ob in cell.Objects)
                {
                    if (SEnvir.Random.Next(2) > 0)
                        continue;
                    if (!CanAttackTarget(ob))
                        continue;

                    targetIDs.Add(ob.ObjectID);

                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(500),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        Element.Lightning));
                }
            }
        }
        /* public void ThunderBolt(int damage)
         {
             Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

             Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.ThunderBolt, Targets = new List<uint> { Target.ObjectID } });

             UpdateAttackTime();

             ActionList.Add(new DelayedAction(
                 SEnvir.Now.AddMilliseconds(500),
                 ActionType.DelayAttack,
                 Target,
                 GetDC(),
                 Element.Lightning));
         }*/
        public void MonsterThunderStorm(int damage)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.MonsterThunderStorm, Locations = new List<Point> { CurrentLocation } });

            UpdateAttackTime();

            foreach (MapObject target in GetTargets(CurrentMap, CurrentLocation, 2))
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    target,
                    damage,
                    Element.Lightning));
            }

        }

        public void Purification()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Purification, Targets = new List<uint> { Target.ObjectID } });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Purification,
                Target));
        }

        public void MassPurification()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targets = new List<uint>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Purification, Targets = targets });

            UpdateAttackTime();

            List<MapObject> obs = GetAllObjects(CurrentLocation, Globals.MagicRange);

            foreach (MapObject ob in obs)
            {
                if (!CanHelpTarget(ob) && !CanAttackTarget(ob))
                    continue;

                targets.Add(ob.ObjectID);

                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayMagic,
                    MagicType.Purification,
                    ob));
            }

        }


        public void MassCyclone()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Cyclone, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, Config.MaxViewRange);
            foreach (Cell cell in cells)
            {
                if (cell.Objects == null)
                {
                    if (SEnvir.Random.Next(30) == 0)
                        locations.Add(cell.Location);

                    continue;
                }

                foreach (MapObject ob in cell.Objects)
                {
                    if (SEnvir.Random.Next(4) == 0)
                        continue;
                    if (!CanAttackTarget(ob))
                        continue;

                    targetIDs.Add(ob.ObjectID);

                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(500),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        Element.Wind));
                }
            }
        }
        /*
                public void MonsterIceStorm()
                {
                    Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

                    List<uint> targetIDs = new List<uint>();
                    List<Point> locations = new List<Point> { Target.CurrentLocation };

                    Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.MonsterIceStorm, Targets = targetIDs, Locations = locations });

                    UpdateAttackTime();

                    List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, 1);

                    foreach (MapObject target in targets)
                    {
                        ActionList.Add(new DelayedAction(
                            SEnvir.Now.AddMilliseconds(500),
                            ActionType.DelayAttack,
                            target,
                            GetDC(),
                            Element.Ice));
                    }
                }*/


        public void PoisonousCloud()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.PoisonousCloud, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, 2);

            foreach (Cell cell in cells)
            {
                SpellObject ob = new SpellObject
                {
                    Visible = cell == CurrentCell,
                    DisplayLocation = CurrentLocation,
                    TickCount = 1,
                    TickFrequency = TimeSpan.FromSeconds(20),
                    Owner = this,
                    Effect = SpellEffect.PoisonousCloud,
                    Power = 20
                };

                ob.Spawn(CurrentMap.Info, cell.Location);
            }

        }



        public void DragonRepulse()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.DragonRepulse, Targets = targetIDs, Locations = locations });

            UpdateAttackTime();

            BuffInfo buff = BuffAdd(BuffType.DragonRepulse, TimeSpan.FromSeconds(6), null, true, false, TimeSpan.FromSeconds(1));
            buff.TickTime = TimeSpan.FromMilliseconds(500);
        }
        public void DragonRepulseEnd(MapObject ob)
        {
            if (Attack(ob, GetDC(), AttackElement) > 0)
            {
                MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, ob.CurrentLocation);
                if (ob.Pushed(dir, 1) == 0)
                {
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int i = 1; i < 2; i++)
                    {
                        if (ob.Pushed(Functions.ShiftDirection(dir, i * rotation), 1) > 0)
                            break;
                        if (ob.Pushed(Functions.ShiftDirection(dir, i * -rotation), 1) > 0)
                            break;
                    }
                }
            }


        }

        public void MassBeckon()
        {
            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, 9);

            foreach (MapObject ob in targets)
            {
                if (ob.Race != ObjectType.Player)
                    continue;

                if (!CanAttackTarget(ob))
                    continue;

                if (SEnvir.Random.Next(9) > 1 * 2)
                    continue;

                if (!ob.Teleport(CurrentMap, CurrentMap.GetRandomLocation(CurrentLocation, 3)))
                    continue;

                ob.ApplyPoison(new Poison
                {
                    Owner = this,
                    Type = PoisonType.Paralysis,
                    TickFrequency = TimeSpan.FromSeconds(1 + 1),
                    TickCount = 1,
                });
            }
        }
        protected void ConeAttack(int distance, int max)
        {
            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.Move(CurrentLocation, Direction, i);


                if (target == Target.CurrentLocation)
                {
                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(400),
                        ActionType.DelayAttack,
                        Target,
                        GetDC(),
                        AttackElement));
                }
                else
                {
                    Cell cell = CurrentMap.GetCell(target);
                    if (cell?.Objects == null)
                        continue;

                    foreach (MapObject ob in cell.Objects)
                    {
                        if (!CanAttackTarget(ob))
                            continue;

                        ActionList.Add(new DelayedAction(
                            SEnvir.Now.AddMilliseconds(400),
                            ActionType.DelayAttack,
                            ob,
                            GetDC(),
                            AttackElement));

                        break;
                    }
                }

            }
        }

        protected void DarknessAttack()
        {
            foreach (MapObject ob in GetTargets(CurrentMap, CurrentLocation, Config.MaxViewRange))
            {

                ob.ApplyPoison(new Poison
                {
                    Owner = this,
                    Type = PoisonType.Darkness,
                    TickFrequency = TimeSpan.FromSeconds(4),
                    TickCount = 1,
                });

                ob.ApplyPoison(new Poison
                {
                    Owner = this,
                    Type = PoisonType.Red,
                    TickFrequency = TimeSpan.FromSeconds(10),
                    TickCount = 1,
                });

            }
        }
        #endregion

        public void UpdateAttackTime()
        {
            AttackTime = SEnvir.Now.AddMilliseconds(AttackDelay);
            ActionTime = SEnvir.Now.AddMilliseconds(Math.Min(MoveDelay, AttackDelay - 100));

            Poison poison = PoisonList.FirstOrDefault(x => x.Type == PoisonType.Slow);
            if (poison != null)
            {
                AttackTime += TimeSpan.FromMilliseconds(poison.Value * 100);
                ActionTime += TimeSpan.FromMilliseconds(poison.Value * 100);
            }
        }
        public void UpdateMoveTime()
        {
            MoveTime = SEnvir.Now.AddMilliseconds(MoveDelay);
            ActionTime = SEnvir.Now.AddMilliseconds(Math.Min(MoveDelay - 100, AttackDelay));

            Poison poison = PoisonList.FirstOrDefault(x => x.Type == PoisonType.Slow);
            if (poison != null)
            {
                AttackTime += TimeSpan.FromMilliseconds(poison.Value * 100);
                ActionTime += TimeSpan.FromMilliseconds(poison.Value * 100);
            }
        }

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            if (attacker.HasNoNode() || power == 0 || Dead || attacker.CurrentMap != CurrentMap || !Functions.InRange(attacker.CurrentLocation, CurrentLocation, Config.MaxViewRange) || Stats[Stat.Invincibility] > 0)
                return 0;

            PlayerObject player;



            switch (attacker.Race)
            {
                case ObjectType.Player:
                    PlayerTagged = true;
                    player = (PlayerObject)attacker;
                    break;
                case ObjectType.Monster:
                    player = ((MonsterObject)attacker).PetOwner;
                    break;
                default:
                    throw new NotImplementedException();
            }

            ShockTime = DateTime.MinValue;

            if (EXPOwner == null && PetOwner == null)
                EXPOwner = player;

            if (EXPOwner == player && player != null)
                EXPOwnerTime = SEnvir.Now + EXPOwnerDelay;

            //Damage Counting statistics.

            if (StruckTime != DateTime.MaxValue && SEnvir.Now > StruckTime.AddMilliseconds(300))
            {
                StruckTime = SEnvir.Now;
                Broadcast(new S.ObjectStruck { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, AttackerID = attacker.ObjectID, Element = element });
            }

            if ((Poison & PoisonType.Red) == PoisonType.Red)
                power = (int)(power * 1.2F);

            for (int i = 0; i < attacker.Stats[Stat.Rebirth]; i++)
                power = (int)(power * 1.5F);


            BuffInfo buff = Buffs.FirstOrDefault(x => x.Type == BuffType.MagicShield);

            if (buff != null)
                buff.RemainingTime -= TimeSpan.FromMilliseconds(power * 10);

            power -= power * Stats[Stat.MagicShield] / 100;

            if (SEnvir.Random.Next(100) < attacker.Stats[Stat.CriticalChance] && canCrit && power > 0)
            {
                power += power + (power * attacker.Stats[Stat.CriticalDamage] / 100);
                Critical();
            }
            buff = Buffs.FirstOrDefault(x => x.Type == BuffType.MagicShieldPhysical);

            if (buff != null)
                buff.RemainingTime -= TimeSpan.FromMilliseconds(power * 10);

            power -= power * Stats[Stat.MagicShieldPhysical] / 100;

            if (SEnvir.Random.Next(100) < attacker.Stats[Stat.CriticalChance] && canCrit && power > 0)
            {
                power += power + (power * attacker.Stats[Stat.CriticalDamage] / 100);
                Critical();
            }

            buff = Buffs.FirstOrDefault(x => x.Type == BuffType.SuperiorMagicShield);

            if (buff != null)
            {
                Stats[Stat.SuperiorMagicShield] -= power;
                if (Stats[Stat.SuperiorMagicShield] <= 0)
                    BuffRemove(buff);
            }
            else
                ChangeHP(-power);

            if (MonsterInfo.IsDamageDrop)
            {

                Damagers damager = DamageList.FirstOrDefault(x => x.attackers == player);

                if (damager == null)
                {
                    Damagers Damagers = new Damagers();
                    Damagers.attackers = player;
                    Damagers.damage = power;
                    Damagers.inCombat = SEnvir.Now.AddSeconds(30);
                    DamageList.Add(Damagers);
                }
                else
                {
                    damager.damage += power;
                    damager.inCombat = SEnvir.Now.AddSeconds(30);
                }
                foreach (Damagers Damagers in DamageList.ToList())
                {
                    if (SEnvir.Now > Damagers.inCombat)
                    {
                        DamageList.Remove(Damagers);
                    }
                }

                DamageList.Sort((a, b) => -1 * a.damage.CompareTo(b.damage));
            }


            if (Dead)
                return power;

            if (CanAttackTarget(attacker) && PetOwner == null || Target == null)
                Target = attacker;


            return power;
        }
        public override bool ApplyPoison(Poison p)
        {
            bool res = base.ApplyPoison(p);

            if (res && CanAttackTarget(p.Owner) && Target == null)
                Target = p.Owner;

            if (p.Owner.Race == ObjectType.Player)
                PlayerTagged = true;

            return res;
        }

        public override void Die()
        {
            base.Die();

            if(!isClone)
                YieldReward();

            Master?.MinionList.Remove(this);
            Master = null;

            PetOwner?.Pets.Remove(this);
            PetOwner = null;

            for (int i = MinionList.Count - 1; i >= 0; i--)
                MinionList[i].Master = null;

            MinionList.Clear();

            DeadTime = SEnvir.Now + Config.DeadDuration;

            if (Drops != null)
                DeadTime += Config.HarvestDuration;

            if (SpawnInfo != null)
                SpawnInfo.AliveCount--;

            ProcessEvents();

            SpawnInfo = null;

            EXPOwner = null;
        }

        private void ProcessEvents()
        {
            if (SpawnInfo == null)
                return;

            foreach (EventTarget target in MonsterInfo.Events)
            {
                int KSDelay = target.Event.CoolDownTime;
                if ((DropSet & target.DropSet) != target.DropSet)
                    continue;
                if (target.Event.EventType == EventTypes.KILLSTREAK && target.Event.MapParameter != null && target.Event.MapParameter != CurrentMap.Info)
                    continue;

                if (target.Event.EventType == EventTypes.KILLSTREAK && target.Event.MapParameter == CurrentMap.Info)
                {

                    if (SEnvir.Now > CurrentMap.Info.KillStreakEndTime)
                    {
                        target.Event.CurrentValue = 0;
                    }
                    CurrentMap.Info.KillStreakEndTime = SEnvir.Now.AddSeconds(KSDelay);
                    CurrentMap.Info.KillSteakActive = true;
                }

                int start = target.Event.CurrentValue;
                int end = Math.Min(target.Event.MaxValue, Math.Max(0, start + target.Value));

                target.Event.CurrentValue = end;



                foreach (EventAction action in target.Event.Actions)
                {

                    Map map;
                    if (action.TriggerValue == 0)
                    {
                        if (target.Event.EventType == EventTypes.KILLSTREAK && target.Event.MapParameter == null)
                        {
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (SEnvir.Now > map.Info.KillStreakEndTime)
                            {
                                target.Event.CurrentValue = 0;

                                start = target.Event.CurrentValue;
                                end = Math.Min(target.Event.MaxValue, Math.Max(0, start + target.Value));

                                target.Event.CurrentValue = end;
                            }
                            map.Info.KillStreakEndTime = SEnvir.Now.AddSeconds(KSDelay);
                            map.Info.KillSteakActive = true;
                            continue;
                        }

                    }
                    if (start >= action.TriggerValue || end < action.TriggerValue)
                        continue;

                    switch (action.Type)
                    {
                        case EventActionType.GlobalMessage:
                            SEnvir.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.MapMessage:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null)
                                continue;

                            map.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.PlayerMessage:
                            if (EXPOwner == null)
                                continue;

                            EXPOwner.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.MonsterSpawn:
                            SpawnInfo spawn = SEnvir.Spawns.FirstOrDefault(x => x.Info == action.RespawnParameter1);

                            if (spawn == null)
                                continue;

                            spawn.DoSpawn(true);
                            break;
                        case EventActionType.MonsterPlayerSpawn:

                            MonsterObject mob = GetMonster(action.MonsterParameter1);
                            mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 10));
                            break;
                        case EventActionType.KillStreak:
                            map = SEnvir.GetMap(action.MapParameter1);

                            if (map == null)
                            {
                                CurrentMap.Info.KillStreakExperienceRate += action.BonusParameter1;
                                CurrentMap.Broadcast(new S.Chat { Text = (action.StringParameter1 + " " + action.BonusParameter1 + "%"), Type = MessageType.System });

                                foreach (PlayerObject players in CurrentMap.Players)
                                {
                                    players.ApplyMapBuff();
                                }
                            }
                            else
                            {
                                map.Info.KillStreakExperienceRate += action.BonusParameter1;
                                map.Broadcast(new S.Chat { Text = (action.StringParameter1 + " " + action.BonusParameter1 + "%"), Type = MessageType.System });

                                foreach (PlayerObject players in map.Players)
                                {
                                    players.ApplyMapBuff();
                                }
                            }
                            break;
                        case EventActionType.SuperRespawn:
                            var Super = GetMonster(MonsterInfo);
                            map = SEnvir.GetMap(action.MapParameter1);

                            if (map == null || Super == null || Super.MonsterInfo.IsBoss)
                            {
                                target.Event.CurrentValue -= target.Value;
                                continue;
                            }
                            if (!(EXPOwner != null && EXPOwner.CurrentMap == map))
                                continue;

                            Super.SuperMob = true;
                            Super.RefreshStats();
                            CurrentMap.Broadcast(new S.Chat { Text = (Super.MonsterInfo.MonsterName + " has gone on a rampage help defeat it."), Type = MessageType.System });

                            Super.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 15));
                            target.Event.CurrentValue += action.BonusParameter1;
                            break;
                        case EventActionType.MovementSettings:
                            break;
                        case EventActionType.PlayerRecall:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null)
                                continue;

                            for (int i = map.Players.Count - 1; i >= 0; i--)
                            {
                                PlayerObject player = map.Players[i];
                                player.Teleport(action.RegionParameter1);
                            }
                            break;
                        case EventActionType.PlayerEscape:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null)
                                continue;

                            for (int i = map.Players.Count - 1; i >= 0; i--)
                            {
                                PlayerObject player = map.Players[i];
                                player.Teleport(player.Character.BindPoint.BindRegion);
                            }
                            break;
                    }
                }
            }
        }

        protected void YieldReward()
        {
            if (EXPOwner == null || PetOwner != null)
                return;

            decimal eRate = 1M + ExtraExperienceRate + (decimal)CurrentMap.Info.KillStreakExperienceRate / 100;
            decimal dRate = 1M + (decimal)CurrentMap.Info.KillStreakExperienceRate / 100;
            int totalLevels = 0;
            List<PlayerObject> ePlayers = new List<PlayerObject>();
            List<PlayerObject> dPlayers = new List<PlayerObject>();

            if (EXPOwner.GroupMembers != null)
            {
                int eWarrior = 0, eWizard = 0, eTaoist = 0, eAssassin = 0;
                int dWarrior = 0, dWizard = 0, dTaoist = 0, dAssassin = 0;


                foreach (PlayerObject ob in EXPOwner.GroupMembers)
                {
                    if (ob.CurrentMap != CurrentMap || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Config.MaxViewRange))
                        continue;

                    switch (ob.Class)
                    {
                        case MirClass.Warrior:
                            dWarrior++;
                            break;
                        case MirClass.Wizard:
                            dWizard++;
                            break;
                        case MirClass.Taoist:
                            dTaoist++;
                            break;
                        case MirClass.Assassin:
                            dAssassin++;
                            break;
                    }

                    dPlayers.Add(ob);

                    if (ob.Dead)
                        continue;

                    switch (ob.Class)
                    {
                        case MirClass.Warrior:
                            eWarrior++;
                            break;
                        case MirClass.Wizard:
                            eWizard++;
                            break;
                        case MirClass.Taoist:
                            eTaoist++;
                            break;
                        case MirClass.Assassin:
                            eAssassin++;
                            break;
                    }

                    ePlayers.Add(ob);
                    totalLevels += ob.Level;
                }

                switch (Math.Min(dWarrior, Math.Min(dWizard, Math.Min(dTaoist, dAssassin))))
                {
                    case 1:
                        dRate *= 1.1M;
                        break;
                    case 2:
                        dRate *= 1.2M;
                        break;
                    case 3:
                        dRate *= 1.3M;
                        break;
                }
                switch (Math.Min(eWarrior, Math.Min(eWizard, Math.Min(eTaoist, eAssassin))))
                {
                    case 1:
                        eRate *= 1.1M;
                        break;
                    case 2:
                        eRate *= 1.25M;
                        break;
                    case 3:
                        eRate *= 1.5M;
                        break;
                }
            }

            if (PetOwner == null && CurrentMap != null)
                eRate *= 1 + MapExperienceRate / 100M;

            decimal exp = Math.Min(Experience * eRate, 500000000);

            if (ePlayers.Count == 0)
            {
                if (!EXPOwner.Dead && EXPOwner.CurrentMap == CurrentMap && Functions.InRange(EXPOwner.CurrentLocation, CurrentLocation, Config.MaxViewRange))
                {
                    if (EXPOwner.Stats[Stat.Rebirth] > 0 && ExtraExperienceRate > 0)
                        exp /= ExtraExperienceRate;

                    EXPOwner.GainExperience(exp, PlayerTagged, Level);
                }
            }
            else
            {
                if (ePlayers.Count > 1)
                    exp += exp * 0.06M * ePlayers.Count; //6% per nearby member.



                foreach (PlayerObject player in ePlayers)
                {
                    decimal expfinal = exp * player.Level / totalLevels;

                    if (player.Stats[Stat.Rebirth] > 0 && ExtraExperienceRate > 0)
                        expfinal /= ExtraExperienceRate;

                    player.GainExperience(expfinal, PlayerTagged, Level);
                }
            }

            if (!MonsterInfo.IsDamageDrop)
            {
                if (dPlayers.Count == 0)
                {
                    if (!EXPOwner.Dead && EXPOwner.CurrentMap == CurrentMap && Functions.InRange(EXPOwner.CurrentLocation, CurrentLocation, Config.MaxViewRange))
                        Drop(EXPOwner, 1, dRate);
                }
                else
                {
                    foreach (PlayerObject player in dPlayers)

                        Drop(player, dPlayers.Count, dRate);
                }
            }
            else
            {
                int count = 1;
                foreach (Damagers Damage in DamageList)
                {
                    decimal Lrate = 1;
                    if (count < 2)
                        Lrate = 5;

                    if (Damage.attackers.Dead)
                        Lrate /= 2;
                    Drop(Damage.attackers, 1, Lrate);
                    count++;
                }
            }

        }

        public virtual List<UserItem> DropItem(DropInfo drop, PlayerObject owner, int players, decimal rate)
        {
            List<UserItem> drops = null;
            if (drop?.Item == null || drop.Chance == 0 || (DropSet & drop.DropSet) != drop.DropSet)
                return null;

            if (drop.EasterEvent && !EasterEventMob)
                return null;


            if (owner.Level > Level)
                rate /=  Math.Min(2.5M, 1.0M+(owner.Level - (Level)) * 0.05M);

            long amount = Math.Max(1, drop.Amount / 2 + SEnvir.Random.Next(drop.Amount));

            long chance;
            if (drop.Item.Effect == ItemEffect.Gold)
            {
                if (owner.Character.Account.GoldBot && Level < owner.Level)
                    return null;

                chance = int.MaxValue / drop.Chance;

                amount /= players;

                amount += (int)(amount * owner.Stats[Stat.GoldRate] / 100M);

                amount += (int)(amount * owner.Stats[Stat.BaseGoldRate] / 100M);

                if (PetOwner == null && CurrentMap != null)
                    amount += (int)(amount * MapGoldRate / 100M);

                if (amount == 0)
                    return null;
            }
            else
            {
                chance = (long)(int.MaxValue / (drop.Chance * players) * rate);
            }


            UserDrop userDrop = owner.Character.Account.UserDrops.FirstOrDefault(x => x.Item == drop.Item);

            if (userDrop == null)
            {
                userDrop = SEnvir.UserDropList.CreateNewObject();
                userDrop.Item = drop.Item;
                userDrop.Account = owner.Character.Account;
            }

            decimal progress = chance / (decimal)int.MaxValue;

            progress *= amount;

            if (!drop.PartOnly)
                userDrop.Progress += progress;

            if (SEnvir.Random.Next() > chance ||
                    (drop.Item.Effect != ItemEffect.Gold && owner.Character.Account.ItemBot))
                return null;

            if (drop.PartOnly)
            {
                if (drop.Item.PartCount <= 1)
                    return null;

                UserItem item = SEnvir.CreateDropItem(SEnvir.ItemPartInfo);

                item.AddStat(Stat.ItemIndex, drop.Item.Index, StatSource.Added);
                item.StatsChanged();


                item.IsTemporary = true;

                if (NeedHarvest)
                {
                    if (drops == null)
                        drops = new List<UserItem>();

                    if (drop.Item.Rarity != Rarity.Common)
                    {
                        owner.Connection.ReceiveChat(
                            string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                            MessageType.System);

                        foreach (SConnection con in owner.Connection.Observers)
                            con.ReceiveChat(string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                MessageType.System);
                    }

                    drops.Add(item);
                    return null;
                }


                Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;


                ItemObject ob = new ItemObject
                {
                    Item = item,
                    Account = owner.Character.Account,
                    MonsterDrop = true,
                };

                ob.Spawn(CurrentMap.Info, cell.Location);

                if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                {
                    long goldAmount = 0;

                    if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                        ob.Account.GuildMember.Guild.GuildTax > 0)
                        goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                    ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                        ob.Item.ExpireTime);

                    if (owner.Companion.CanGainItems(true, check))
                        ob.PickUpItem(owner.Companion);

                }

                return null;
            }

            // if (drop.Item.Effect != ItemEffect.Gold && Math.Floor(userDrop.Progress) > userDrop.DropCount + amount) ##guarenteed drops
            //     amount = (long)(userDrop.Progress - userDrop.DropCount);

            userDrop.DropCount += amount;
            while (amount > 0)
            {
                UserItem item;
                if (SuperMob)
                {
                    item = SEnvir.CreateDropItem(drop.Item, (Config.DropAddedChance / 5), (Config.DropRarityInc / 5));
                }
                else
                {
                    if (MonsterInfo.IsLord)
                    {
                        if (owner.Dead)
                            item = SEnvir.CreateDropItem(drop.Item, (Config.DropAddedChance / 2), (Config.DropRarityInc / 2));
                        else
                            item = SEnvir.CreateDropItem(drop.Item, (Config.DropAddedChance / 5), (Config.DropRarityInc / 5));
                    }
                    else
                    {
                        if (MonsterInfo.IsBoss)
                        {
                            item = SEnvir.CreateDropItem(drop.Item, (Config.DropAddedChance / 2), (Config.DropRarityInc / 5));
                        }
                        else
                            item = SEnvir.CreateDropItem(drop.Item);
                    }
                }
                item.Count = Math.Min(drop.Item.StackSize, amount);
                amount -= item.Count;

                item.IsTemporary = true; //REMOVE ON Gain

                if (NeedHarvest)
                {
                    if (drops == null)
                        drops = new List<UserItem>();

                    if (item.Rarity != Rarity.Common)
                    {
                        owner.Connection.ReceiveChat(string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName), MessageType.System);

                        foreach (SConnection con in owner.Connection.Observers)
                            con.ReceiveChat(string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName), MessageType.System);
                    }

                    drops.Add(item);
                    continue;
                }

                Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                ItemObject ob = new ItemObject
                {
                    Item = item,
                    Account = owner.Character.Account,
                    MonsterDrop = true,
                };


                ob.Spawn(CurrentMap.Info, cell.Location);

                if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                {
                    long goldAmount = 0;

                    if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                        ob.Account.GuildMember.Guild.GuildTax > 0)
                        goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                    ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                        ob.Item.ExpireTime);

                    if (owner.Companion.CanGainItems(true, check))
                        ob.PickUpItem(owner.Companion);

                }
            }
            return drops;
        }

        public virtual void Drop(PlayerObject owner, int players, decimal rate)
        {
            rate *= 1M + owner.Stats[Stat.DropRate] / 100M;

            rate *= 1M + owner.Stats[Stat.BaseDropRate] / 100M;

            if (PetOwner == null && CurrentMap != null)
                rate *= 1 + MapDropRate / 100M;


            bool result = false;

            List<UserItem> drops = null;

            if(ChristmasEventMob)
            {
                string xmasmob = "Santa";
                var xmasmobinfo = SEnvir.GetMonsterInfo(xmasmob);
                foreach (MonsterDropListInfo mdroplist in xmasmobinfo.DropLists)
                {
                    DropListInfo droplist = SEnvir.DropListInfoList.Binding.FirstOrDefault(x => mdroplist.DropList.Index == x.Index);
                    if (droplist != null)
                    {
                        foreach (DropInfo dl in droplist.Drops)
                        {
                            List<UserItem> newdrops = DropItem(dl, owner, players, 1);

                            if (newdrops != null)
                            {
                                if (drops == null)
                                    drops = new List<UserItem>();
                                foreach (UserItem UI in newdrops)
                                    drops.Add(UI);
                            }
                        }

                    }
                }
            }
            if (SuperMob)
            {
                decimal SupRate = 1;
                SupRate *= 1M + owner.Stats[Stat.DropRate] / 100M;

                SupRate *= 1M + owner.Stats[Stat.BaseDropRate] / 100M;

                string smob = MonsterInfo.SuperName;
                if (MonsterInfo.SuperName == "")
                    smob = "SuperMob";
                var smobinfo = SEnvir.GetMonsterInfo(smob);
                foreach (DropInfo drop in smobinfo.Drops)
                {

                    long amount = Math.Max(1, drop.Amount / 2 + SEnvir.Random.Next(drop.Amount));

                    long chance;
                    if (drop.Item.Effect == ItemEffect.Gold)
                    {
                        if (owner.Character.Account.GoldBot && Level < owner.Level)
                            continue;

                        chance = int.MaxValue / drop.Chance;

                        amount /= players;

                        amount += (int)(amount * owner.Stats[Stat.GoldRate] / 100M);

                        amount += (int)(amount * owner.Stats[Stat.BaseGoldRate] / 100M);

                        if (amount == 0)
                            continue;
                    }
                    else
                    {
                        chance = (long)(int.MaxValue / (drop.Chance * players) * SupRate);
                    }


                    UserDrop userDrop = owner.Character.Account.UserDrops.FirstOrDefault(x => x.Item == drop.Item);

                    if (userDrop == null)
                    {
                        userDrop = SEnvir.UserDropList.CreateNewObject();
                        userDrop.Item = drop.Item;
                        userDrop.Account = owner.Character.Account;
                    }

                    decimal progress = chance / (decimal)int.MaxValue;

                    progress *= amount;

                    if (!drop.PartOnly)
                        userDrop.Progress += progress;
                    if (SEnvir.Random.Next() > chance ||
                         (drop.Item.Effect != ItemEffect.Gold && owner.Character.Account.ItemBot))
                        continue;
                    if (drop.PartOnly)
                    {
                        if (drop.Item.PartCount <= 1)
                            continue;

                        result = true;

                        UserItem item = SEnvir.CreateDropItem(SEnvir.ItemPartInfo);

                        item.AddStat(Stat.ItemIndex, drop.Item.Index, StatSource.Added);
                        item.StatsChanged();


                        item.IsTemporary = true;

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (drop.Item.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(
                                    string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                                    MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                        MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }


                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;


                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };

                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0)
                                goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check))
                                ob.PickUpItem(owner.Companion);

                        }

                        continue;
                    }


                    // if (drop.Item.Effect != ItemEffect.Gold && Math.Floor(userDrop.Progress) > userDrop.DropCount + amount) //guareenteed drops
                    //     amount = (long)(userDrop.Progress - userDrop.DropCount);

                    userDrop.DropCount += amount;

                    result = true;
                    while (amount > 0)
                    {
                        UserItem item;
                        item = SEnvir.CreateDropItem(drop.Item, (Config.DropAddedChance / 3), (Config.DropRarityInc / 4));

                        item.Count = Math.Min(drop.Item.StackSize, amount);
                        amount -= item.Count;

                        item.IsTemporary = true; //REMOVE ON Gain

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (item.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName), MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName), MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }

                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };


                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0)
                                goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check))
                                ob.PickUpItem(owner.Companion);

                        }
                    }
                }
                foreach (MonsterDropListInfo mdroplist in smobinfo.DropLists)
                {
                    DropListInfo droplist = SEnvir.DropListInfoList.Binding.FirstOrDefault(x => mdroplist.DropList.Index == x.Index);
                    if (droplist != null)
                    {
                        foreach (DropInfo dl in droplist.Drops)
                        {
                            List<UserItem> newdrops = DropItem(dl, owner, players, (SupRate / mdroplist.Multiplier));

                            if (newdrops != null)
                            {
                                if (drops == null)
                                    drops = new List<UserItem>();
                                foreach (UserItem UI in newdrops)
                                    drops.Add(UI);
                            }
                        }

                    }
                }

            }

            

            foreach (MonsterDropListInfo mdroplist in MonsterInfo.DropLists)
            {
                DropListInfo droplist = SEnvir.DropListInfoList.Binding.FirstOrDefault(x => mdroplist.DropList.Index == x.Index);
                if (droplist != null)
                {
                    foreach (DropInfo dl in droplist.Drops)
                    {
                        List<UserItem> newdrops = DropItem(dl, owner, players, (rate / mdroplist.Multiplier));

                        if (newdrops != null)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();
                            foreach (UserItem UI in newdrops)
                                drops.Add(UI);
                        }
                    }

                }
            }

            foreach (DropInfo drop in MonsterInfo.Drops)
            {
                List<UserItem> newdrops = DropItem(drop, owner, players, rate);

                if (newdrops != null)
                {
                    if (drops == null)
                        drops = new List<UserItem>();
                    foreach (UserItem UI in newdrops)
                        drops.Add(UI);
                }
            }

            foreach (UserQuest quest in owner.Character.Quests)
            {
                //For Each Active Quest
                if (quest.Completed)
                    continue;
                bool changed = false;

                foreach (QuestTask task in quest.QuestInfo.Tasks)
                {
                    bool valid = false;
                    int count = 0;
                    foreach (QuestTaskMonsterDetails details in task.MonsterDetails)
                    {
                        if (details.Monster != MonsterInfo)
                            continue;
                        if (details.Map != null && CurrentMap.Info != details.Map)
                            continue;

                        if (SEnvir.Random.Next(details.Chance) > 0)
                            continue;

                        if ((DropSet & details.DropSet) != details.DropSet)
                            continue;

                        valid = true;
                        count = details.Amount;
                        break;
                    }

                    if (!valid)
                        continue;

                    UserQuestTask userTask = quest.Tasks.FirstOrDefault(x => x.Task == task);

                    if (userTask == null)
                    {
                        userTask = SEnvir.UserQuestTaskList.CreateNewObject();
                        userTask.Task = task;
                        userTask.Quest = quest;
                    }

                    if (userTask.Completed)
                        continue;

                    switch (task.Task)
                    {
                        case QuestTaskType.KillMonster:
                            userTask.Amount = Math.Min(task.Amount, userTask.Amount + count);
                            changed = true;
                            if (userTask.Completed)
                            {
                                string description = userTask.Task.MobDescription;
                                if (string.IsNullOrEmpty(description))
                                    description = MonsterInfo.MonsterName;
                                owner.Connection.ReceiveChat($"Completed Task Kill Monster: {description}", MessageType.System);
                            }
                            break;
                        case QuestTaskType.GainItem:
                            if (task.ItemParameter == null)
                                continue;

                            UserItem item = SEnvir.CreateDropItem(task.ItemParameter);
                            item.Count = count;
                            item.UserTask = userTask;
                            item.Flags |= UserItemFlags.QuestItem;

                            item.IsTemporary = true; //REMOVE ON Gain

                            if (NeedHarvest)
                            {
                                if (drops == null)
                                    drops = new List<UserItem>();

                                drops.Add(item);
                                continue;
                            }


                            Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                            ItemObject ob = new ItemObject
                            {
                                Item = item,
                                Account = owner.Character.Account,
                                MonsterDrop = true,
                            };



                            ob.Spawn(CurrentMap.Info, cell.Location);

                            userTask.Objects.Add(ob);

                            if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                            {
                                long goldAmount = 0;

                                if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                    ob.Account.GuildMember.Guild.GuildTax > 0)
                                    goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                                ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                    ob.Item.ExpireTime);

                                if (owner.Companion.CanGainItems(true, check))
                                    ob.PickUpItem(owner.Companion);

                            }
                            break;
                    }

                }

                if (changed)
                    owner.Enqueue(new S.QuestChanged { Quest = quest.ToClientInfo() });
            }

            if (result && owner.Companion != null)
                owner.Companion.SearchTime = DateTime.MinValue;

            if (!NeedHarvest)
                return;

            if (Drops == null)
                Drops = new Dictionary<AccountInfo, List<UserItem>>();

            Drops[owner.Character.Account] = drops;
        }
        public virtual void InitEnrage(int x = 20, int y = 60)
        {
            NextEnrageTime = SEnvir.Now.AddSeconds(SEnvir.Random.Next(x, y));
            Enraged = false;
        }
        public virtual void EnrageStart(int x = 5)
        {
            MoveDelay = 650;
            AttackDelay = 400;
            EnrageEndTime = SEnvir.Now.AddSeconds(x);
            Enraged = true;
        }
        public virtual void EnrageEnd(int x = 20, int y = 60)
        {
            MoveDelay = MonsterInfo.MoveDelay;
            AttackDelay = MonsterInfo.AttackDelay;
            NextEnrageTime = SEnvir.Now.AddSeconds(SEnvir.Random.Next(x, y));
            Enraged = false;
        }

        public virtual void Turn(MirDirection direction)
        {
            if (!CanMove)
                return;

            UpdateMoveTime();

            Direction = direction;

            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }
        public virtual bool Walk(MirDirection direction)
        {
            if (!CanMove)
                return false;

            Cell cell = CurrentMap.GetCell(Functions.Move(CurrentLocation, direction));
            if (cell == null)
                return false;

            if (cell.IsBlocking(this, false))
                return false;

            if (AvoidFireWall && cell.Objects != null)
            {
                foreach (MapObject ob in cell.Objects)
                {
                    if (ob.Race != ObjectType.Spell)
                        continue;
                    SpellObject spell = (SpellObject)ob;

                    switch (spell.Effect)
                    {
                        case SpellEffect.FireWall:
                        case SpellEffect.MonsterFireWall:
                        case SpellEffect.Tempest:
                        case SpellEffect.MonsterTempest:
                            break;
                        default:
                            continue;
                    }

                    if (spell.Owner == null || !spell.Owner.CanAttackTarget(this))
                        continue;

                    return false;
                }
            }

            BuffRemove(BuffType.Invisibility);
            BuffRemove(BuffType.Transparency);

            Direction = direction;

            UpdateMoveTime();



            PreventSpellCheck = true;
            CurrentCell = cell; //.GetMovement(this);
            PreventSpellCheck = false;

            RemoveAllObjects();
            AddAllObjects();

            Broadcast(new S.ObjectMove { ObjectID = ObjectID, Direction = direction, Location = CurrentLocation, Distance = 1 });
            CheckSpellObjects();
            return true;
        }
        protected virtual void MoveTo(Point target)
        {
            if (CurrentLocation == target)
                return;

            if (Functions.InRange(target, CurrentLocation, 1))
            {
                Cell cell = CurrentMap.GetCell(target);

                if (cell == null || cell.IsBlocking(this, false))
                    return;
            }

            MirDirection direction = Functions.DirectionFromPoint(CurrentLocation, target);

            int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

            for (int d = 0; d < 8; d++)
            {
                if (Walk(direction))
                    return;

                direction = Functions.ShiftDirection(direction, rotation);
            }
        }

        public override BuffInfo BuffAdd(BuffType type, TimeSpan remainingTicks, Stats stats, bool visible, bool pause, TimeSpan tickRate)
        {
            BuffInfo info = base.BuffAdd(type, remainingTicks, stats, visible, pause, tickRate);

            info.IsTemporary = true;

            return info;
        }

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();

            if (CurrentCell == null)
                return;

            InSafeZone = CurrentCell.SafeZone != null;
        }

        public void HarvestChanged()
        {
            Skeleton = true;

            if (Drops == null)
                DeadTime -= Config.HarvestDuration;

            foreach (PlayerObject player in SeenByPlayers)
                if (Drops == null || !Drops.ContainsKey(player.Character.Account))
                    player.Enqueue(new S.ObjectHarvested { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }
        public override Packet GetInfoPacket(PlayerObject ob)
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                MonsterIndex = MonsterInfo.Index,

                Location = CurrentLocation,

                NameColour = NameColour,
                Direction = Direction,
                Dead = Dead,

                PetOwner = PetOwner?.Name,

                Skeleton = NeedHarvest && Skeleton && (Drops == null || !Drops.ContainsKey(ob.Character.Account)),

                Poison = Poison,

                EasterEvent = EasterEventMob,
                HalloweenEvent = HalloweenEventMob,
                ChristmasEvent = ChristmasEventMob,
                Supermob = SuperMob,
                Buffs = Buffs.Where(x => x.Visible).Select(x => x.Type).ToList(),
                FlagColour = FlagColour,
                FlagShape = FlagShape,
                Name = Name,
                eventTeam = EventTeam
            };
        }
        public override Packet GetDataPacket(PlayerObject ob)
        {
            return new S.DataObjectMonster
            {
                ObjectID = ObjectID,

                MonsterIndex = MonsterInfo.Index,

                MapIndex = CurrentMap.Info.Index,
                CurrentLocation = CurrentLocation,

                Health = DisplayHP,
                Stats = Stats,
                Dead = Dead,
                Supermob = SuperMob,
                Level = Level,

                PetOwner = PetOwner?.Name,
            };
        }
    }
}