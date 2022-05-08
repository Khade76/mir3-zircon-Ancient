using System;
using Library;
using Library.SystemModels;
using MirDB;

namespace Server.DBModels
{
    [UserObject]
    public class UserArenaPvPStats : DBObject
    {
        public CharacterInfo Character
        {
            get
            {
                return _Character;
            }
            set
            {
                if (_Character == value)
                    return;

                var oldValue = _Character;
                _Character = value;

                OnChanged(oldValue, value, "Character");
            }
        }
        private CharacterInfo _Character;

        public DateTime PvPEventTime
        {
            get
            {
                return _PvPEventTime;
            }
            set
            {
                if (_PvPEventTime == value)
                    return;

                var oldValue = _PvPEventTime;
                _PvPEventTime = value;

                OnChanged(oldValue, value, "PvPEventTime");
            }
        }
        private DateTime _PvPEventTime;

        public MiniGames MiniGame
        {
            get
            {
                return _MiniGame;
            }
            set
            {
                if (_MiniGame == value)
                    return;

                var oldValue = _MiniGame;
                _MiniGame = value;

                OnChanged(oldValue, value, "MiniGame");
            }
        }
        private MiniGames _MiniGame;


        public string CharacterName
        {
            get
            {
                return _CharacterName;
            }
            set
            {
                if (_CharacterName == value)
                    return;

                var oldValue = _CharacterName;
                _CharacterName = value;

                OnChanged(oldValue, value, "CharacterName");
            }
        }
        private string _CharacterName;

        public string GuildName
        {
            get
            {
                return _GuildName;
            }
            set
            {
                if (_GuildName == value)
                    return;

                var oldValue = _GuildName;
                _GuildName = value;

                OnChanged(oldValue, value, "GuildName");
            }
        }
        private string _GuildName;

        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                if (_Level == value)
                    return;

                var oldValue = _Level;
                _Level = value;

                OnChanged(oldValue, value, "Level");
            }
        }
        private int _Level;

        public MirClass Class
        {
            get
            {
                return _Class;
            }
            set
            {
                if (_Class == value)
                    return;

                var oldValue = _Class;
                _Class = value;

                OnChanged(oldValue, value, "Class");
            }
        }
        private MirClass _Class;


        public int PvPDamageTaken
        {
            get
            {
                return _PvPDamageTaken;
            }
            set
            {
                if (_PvPDamageTaken == value)
                    return;

                var oldValue = _PvPDamageTaken;
                _PvPDamageTaken = value;

                OnChanged(oldValue, value, "PvPDamageTaken");
            }
        }
        private int _PvPDamageTaken;

        public int PvPDamageDealt
        {
            get
            {
                return _PvPDamageDealt;
            }
            set
            {
                if (_PvPDamageDealt == value)
                    return;

                var oldValue = _PvPDamageDealt;
                _PvPDamageDealt = value;

                OnChanged(oldValue, value, "PvPDamageDealt");
            }
        }
        private int _PvPDamageDealt;

        public int PvPKillCount
        {
            get
            {
                return _PvPKillCount;
            }
            set
            {
                if (_PvPKillCount == value)
                    return;

                var oldValue = _PvPKillCount;
                _PvPKillCount = value;

                OnChanged(oldValue, value, "PvPKillCount");
            }
        }
        private int _PvPKillCount;

        public int PvPDeathCount
        {
            get
            {
                return _PvPDeathCount;
            }
            set
            {
                if (_PvPDeathCount == value)
                    return;

                var oldValue = _PvPDeathCount;
                _PvPDeathCount = value;

                OnChanged(oldValue, value, "PvPDeathCount");
            }
        }
        private int _PvPDeathCount;

        public int FlagCaptures
        {
            get
            {
                return _FlagCaptures;
            }
            set
            {
                if (_FlagCaptures == value)
                    return;

                var oldValue = _FlagCaptures;
                _FlagCaptures = value;

                OnChanged(oldValue, value, "FlagCaptures");
            }
        }
        private int _FlagCaptures;

        public int FlagSaves
        {
            get
            {
                return _FlagSaves;
            }
            set
            {
                if (_FlagSaves == value)
                    return;

                var oldValue = _FlagSaves;
                _FlagSaves = value;

                OnChanged(oldValue, value, "FlagSaves");
            }
        }
        private int _FlagSaves;
    }
}
