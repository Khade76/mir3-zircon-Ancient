using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MirDB;

namespace Library.SystemModels
{
    public sealed class CastleInfo : DBObject
    {
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;

                var oldValue = _Name;
                _Name = value;

                OnChanged(oldValue, value, "Name");
            }
        }
        private string _Name;

        public MapInfo Map
        {
            get { return _Map; }
            set
            {
                if (_Map == value) return;

                var oldValue = _Map;
                _Map = value;

                OnChanged(oldValue, value, "Map");
            }
        }
        private MapInfo _Map;

        public TimeSpan StartTime
        {
            get { return _StartTime; }
            set
            {
                if (_StartTime == value) return;

                var oldValue = _StartTime;
                _StartTime = value;

                OnChanged(oldValue, value, "StartTime");
            }
        }
        private TimeSpan _StartTime;

        public TimeSpan Duration
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
        private TimeSpan _Duration;

        public MapRegion CastleRegion
        {
            get { return _CastleRegion; }
            set
            {
                if (_CastleRegion == value) return;

                var oldValue = _CastleRegion;
                _CastleRegion = value;

                OnChanged(oldValue, value, "CastleRegion");
            }
        }
        private MapRegion _CastleRegion;

        public MapRegion OutPost1
        {
            get { return _OutPost1; }
            set
            {
                if (_OutPost1 == value) return;

                var oldValue = _OutPost1;
                _OutPost1 = value;

                OnChanged(oldValue, value, "OutPost1");
            }
        }
        private MapRegion _OutPost1;

        public MapRegion OutPost2
        {
            get { return _OutPost2; }
            set
            {
                if (_OutPost2 == value) return;

                var oldValue = _OutPost2;
                _OutPost2 = value;

                OnChanged(oldValue, value, "OutPost2");
            }
        }
        private MapRegion _OutPost2;

        public MapRegion OutPost3
        {
            get { return _OutPost3; }
            set
            {
                if (_OutPost3 == value) return;

                var oldValue = _OutPost3;
                _OutPost3 = value;

                OnChanged(oldValue, value, "OutPost3");
            }
        }
        private MapRegion _OutPost3;

        public MapRegion OutPost4
        {
            get { return _OutPost4; }
            set
            {
                if (_OutPost4 == value) return;

                var oldValue = _OutPost4;
                _OutPost4 = value;

                OnChanged(oldValue, value, "OutPost4");
            }
        }
        private MapRegion _OutPost4;

        public MapRegion OutPost5
        {
            get { return _OutPost5; }
            set
            {
                if (_OutPost5 == value) return;

                var oldValue = _OutPost5;
                _OutPost5 = value;

                OnChanged(oldValue, value, "OutPost5");
            }
        }
        private MapRegion _OutPost5;

        public MapRegion AttackSpawnRegion
        {
            get { return _AttackSpawnRegion; }
            set
            {
                if (_AttackSpawnRegion == value) return;

                var oldValue = _AttackSpawnRegion;
                _AttackSpawnRegion = value;

                OnChanged(oldValue, value, "AttackSpawnRegion");
            }
        }
        private MapRegion _AttackSpawnRegion;

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
        
        public MonsterInfo Monster
        {
            get => _Monster;
            set
            {
                if (_Monster == value) return;

                var oldValue = _Monster;
                _Monster = value;

                OnChanged(oldValue, value, "Monster");
            }
        }
        private MonsterInfo _Monster;

        public MonsterInfo OutPostMon
        {
            get => _OutPostMon;
            set
            {
                if (_OutPostMon == value) return;

                var oldValue = _OutPostMon;
                _OutPostMon = value;

                OnChanged(oldValue, value, "OutPostMon");
            }
        }
        private MonsterInfo _OutPostMon;

        public MonsterInfo OutPostGuard1
        {
            get => _OutPostGuard1;
            set
            {
                if (_OutPostGuard1 == value) return;

                var oldValue = _OutPostGuard1;
                _OutPostGuard1 = value;

                OnChanged(oldValue, value, "OutPostGuard1");
            }
        }
        private MonsterInfo _OutPostGuard1;

        public MonsterInfo OutPostGuard2
        {
            get => _OutPostGuard2;
            set
            {
                if (_OutPostGuard2 == value) return;

                var oldValue = _OutPostGuard2;
                _OutPostGuard2 = value;

                OnChanged(oldValue, value, "OutPostGuard2");
            }
        }
        private MonsterInfo _OutPostGuard2;

        public decimal Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount == value) return;

                var oldValue = _Discount;
                _Discount = value;

                OnChanged(oldValue, value, "Discount");
            }
        }
        private decimal _Discount;

        
        public DateTime WarDate;
    }
}



