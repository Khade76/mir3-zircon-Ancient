using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MirDB;

namespace Library.SystemModels
{
    public sealed class MiniGameInfo : DBObject
    {
        public MiniGames MiniGame
        {
            get { return _MiniGame; }
            set
            {
                if (_MiniGame == value) return;

                var oldValue = _MiniGame;
                _MiniGame = value;

                OnChanged(oldValue, value, "MiniGame");
            }
        }
        private MiniGames _MiniGame;

        public MapInfo MapParameter
        {
            get { return _MapParameter; }
            set
            {
                if (_MapParameter == value) return;

                var oldValue = _MapParameter;
                _MapParameter = value;

                OnChanged(oldValue, value, "MapParameter");
            }
        }
        private MapInfo _MapParameter;

        public MapInfo MapLobby
        {
            get { return _MapLobby; }
            set
            {
                if (_MapLobby == value) return;

                var oldValue = _MapLobby;
                _MapLobby = value;

                OnChanged(oldValue, value, "MapLobby");
            }
        }
        private MapInfo _MapLobby;

        public int MinLevel
        {
            get { return _MinLevel; }
            set
            {
                if (_MinLevel == value) return;

                var oldValue = _MinLevel;
                _MinLevel = value;

                OnChanged(oldValue, value, "MinLevel");
            }
        }
        private int _MinLevel;

        public MapRegion TeamASpawn
        {
            get { return _TeamASpawn; }
            set
            {
                if (_TeamASpawn == value) return;

                var oldValue = _TeamASpawn;
                _TeamASpawn = value;

                OnChanged(oldValue, value, "TeamASpawn");
            }
        }
        private MapRegion _TeamASpawn;

        public MapRegion TeamBSpawn
        {
            get { return _TeamBSpawn; }
            set
            {
                if (_TeamBSpawn == value) return;

                var oldValue = _TeamBSpawn;
                _TeamBSpawn = value;

                OnChanged(oldValue, value, "TeamBSpawn");
            }
        }
        private MapRegion _TeamBSpawn;

        public int MaxLevel
        {
            get { return _MaxLevel; }
            set
            {
                if (_MaxLevel == value) return;

                var oldValue = _MaxLevel;
                _MaxLevel = value;

                OnChanged(oldValue, value, "MaxLevel");
            }
        }
        private int _MaxLevel;

        public int EntryFee
        {
            get { return _EntryFee; }
            set
            {
                if (_EntryFee == value) return;

                var oldValue = _EntryFee;
                _EntryFee = value;

                OnChanged(oldValue, value, "EntryFee");
            }
        }
        private int _EntryFee;

        public int Duration
        {
            get { return _Duration; }
            set
            {
                if (_Duration == value) return;

                var oldValue = _Duration;
                _Duration = value;

                OnChanged(oldValue, value, "Duration");
            }
        }
        private int _Duration;

        public bool TeamGame
        {
            get { return _TeamGame; }
            set
            {
                if (_TeamGame == value) return;

                var oldValue = _TeamGame;
                _TeamGame = value;

                OnChanged(oldValue, value, "TeamGame");
            }
        }
        private bool _TeamGame;

        public bool CanRevive
        {
            get { return _CanRevive; }
            set
            {
                if (_CanRevive == value) return;

                var oldValue = _CanRevive;
                _CanRevive = value;

                OnChanged(oldValue, value, "CanRevive");
            }
        }
        private bool _CanRevive;

        public int ReviveDelay
        {
            get { return _ReviveDelay; }
            set
            {
                if (_ReviveDelay == value) return;

                var oldValue = _ReviveDelay;
                _ReviveDelay = value;

                OnChanged(oldValue, value, "ReviveDelay");
            }
        }
        private int _ReviveDelay;

        public int MinPlayers
        {
            get { return _MinPlayers; }
            set
            {
                if (_MinPlayers == value) return;

                var oldValue = _MinPlayers;
                _MinPlayers = value;

                OnChanged(oldValue, value, "MinPlayers");
            }
        }
        private int _MinPlayers;

        public int MaxPlayers
        {
            get { return _MaxPlayers; }
            set
            {
                if (_MaxPlayers == value) return;

                var oldValue = _MaxPlayers;
                _MaxPlayers = value;

                OnChanged(oldValue, value, "MaxPlayers");
            }
        }
        private int _MaxPlayers;

        [Association("Rewards", true)]
        public DBBindingList<RewardInfo> Rewards { get; set; }

        [Association("CTF", true)]
        public DBBindingList<CTFInfo> CTFInfo { get; set; }


    }


    public enum MiniGames
    {

        [Description("Capture The Flag"),Category("Two teams on opposing sides. The first team to capture and return the opposing teams flag back to their podium wins.")]
        CaptureTheFlag,
        [Description("Treasure Island"), Category("Everyman for themselves trying to survive the island. Be either the last player stranding or still alive at the end to be able to find the hidden treasures.")]
        TreasureIsland,
        [Description("Monster Tennis"), Category("Kill monsters faster than your opponents team. Everytime you kill a monster it respawns on the opposing players team. Be the last team standing to win.")]
        MonsterTennis,
        [Description("Battle Royal"), Category("Every player for themselves. Last 3 players standing get rewarded with bonus for top 3 killers.")]
        BattleRoyal,
        [Description("Team Battle Royal"), Category("Two teams battle it out. Score points for staying alive and earning kills. The last team standing is the winner, with bonus rewards for the top 2 killers on each team.")]
        TeamBattleRoyal,
    }

    public sealed class RewardInfo : DBObject
    {
        [Association("Rewards")]
        public MiniGameInfo MiniGame
        {
            get { return _MiniGame; }
            set
            {
                if (_MiniGame == value) return;

                var oldValue = _MiniGame;
                _MiniGame = value;

                OnChanged(oldValue, value, "MiniGame");
            }
        }
        private MiniGameInfo _MiniGame;
        
        public ItemInfo Item
        {
            get { return _Item; }
            set
            {
                if (_Item == value) return;

                var oldValue = _Item;
                _Item = value;

                OnChanged(oldValue, value, "Item");
            }
        }
        private ItemInfo _Item;

        public int Chance
        {
            get { return _Chance; }
            set
            {
                if (_Chance == value) return;

                var oldValue = _Chance;
                _Chance = value;

                OnChanged(oldValue, value, "Chance");
            }
        }
        private int _Chance;

        public int Amount
        {
            get { return _Amount; }
            set
            {
                if (_Amount == value) return;

                var oldValue = _Amount;
                _Amount = value;

                OnChanged(oldValue, value, "Amount");
            }
        }
        private int _Amount;

        public bool Top1
        {
            get { return _Top1; }
            set
            {
                if (_Top1 == value) return;

                var oldValue = _Top1;
                _Top1 = value;

                OnChanged(oldValue, value, "Top1");
            }
        }
        private bool _Top1;

        public bool Top2
        {
            get { return _Top2; }
            set
            {
                if (_Top2 == value) return;

                var oldValue = _Top2;
                _Top2 = value;

                OnChanged(oldValue, value, "Top2");
            }
        }
        private bool _Top2;

        public bool Top3
        {
            get { return _Top3; }
            set
            {
                if (_Top3 == value) return;

                var oldValue = _Top3;
                _Top3 = value;

                OnChanged(oldValue, value, "Top3");
            }
        }
        private bool _Top3;
    }
    public sealed class CTFInfo : DBObject
    {
        [Association("CTF")]
        public MiniGameInfo MiniGame
        {
            get { return _MiniGame; }
            set
            {
                if (_MiniGame == value) return;

                var oldValue = _MiniGame;
                _MiniGame = value;

                OnChanged(oldValue, value, "MiniGame");
            }
        }
        private MiniGameInfo _MiniGame;

        public MapRegion TeamAFlagSpawn
        {
            get { return _TeamAFlagSpawn; }
            set
            {
                if (_TeamAFlagSpawn == value) return;

                var oldValue = _TeamAFlagSpawn;
                _TeamAFlagSpawn = value;

                OnChanged(oldValue, value, "TeamAFlagSpawn");
            }
        }
        private MapRegion _TeamAFlagSpawn;

        public MapRegion TeamBFlagSpawn
        {
            get { return _TeamBFlagSpawn; }
            set
            {
                if (_TeamBFlagSpawn == value) return;

                var oldValue = _TeamBFlagSpawn;
                _TeamBFlagSpawn = value;

                OnChanged(oldValue, value, "TeamBFlagSpawn");
            }
        }
        private MapRegion _TeamBFlagSpawn;

        public MapRegion TeamAFlagReturn
        {
            get { return _TeamAFlagReturn; }
            set
            {
                if (_TeamAFlagReturn == value) return;

                var oldValue = _TeamAFlagReturn;
                _TeamAFlagReturn = value;

                OnChanged(oldValue, value, "TeamAFlagReturn");
            }
        }
        private MapRegion _TeamAFlagReturn;

        public MapRegion TeamBFlagReturn
        {
            get { return _TeamBFlagReturn; }
            set
            {
                if (_TeamBFlagReturn == value) return;

                var oldValue = _TeamBFlagReturn;
                _TeamBFlagReturn = value;

                OnChanged(oldValue, value, "TeamBFlagReturn");
            }
        }
        private MapRegion _TeamBFlagReturn;

        public MonsterInfo FlagMonster
        {
            get { return _FlagMonster; }
            set
            {
                if (_FlagMonster == value) return;

                var oldValue = _FlagMonster;
                _FlagMonster = value;

                OnChanged(oldValue, value, "FlagMonster");
            }
        }
        private MonsterInfo _FlagMonster;
    }

}
