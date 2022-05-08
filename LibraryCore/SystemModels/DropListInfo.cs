using MirDB;

namespace Library.SystemModels
{
    public sealed class DropListInfo : DBObject
    {
        public string DropTier
        {
            get { return _DropTier; }
            set
            {
                if (_DropTier == value) return;

                var oldValue = _DropTier;
                _DropTier = value;

                OnChanged(oldValue, value, "DropTier");
            }
        }
        private string _DropTier;

        [Association("Drops", true)]
        public DBBindingList<DropInfo> Drops { get; set; }
        [Association("DropLists", true)]
        public DBBindingList<MonsterDropListInfo> DropLists { get; set; }


        protected internal override void OnCreated()
        {
            base.OnCreated();
        }

    }
    public sealed class MonsterDropListInfo : DBObject
    {
        [Association("DropLists")]
        public DropListInfo DropList
        {
            get { return _DropList; }
            set
            {
                if (_DropList == value) return;

                var oldValue = _DropList;
                _DropList = value;

                OnChanged(oldValue, value, "DropList");
            }
        }
        private DropListInfo _DropList;

        [Association("DropLists")]
        public MonsterInfo Monster
        {
            get { return _Monster; }
            set
            {
                if (_Monster == value) return;

                var oldValue = _Monster;
                _Monster = value;

                OnChanged(oldValue, value, "Monster");
            }
        }
        private MonsterInfo _Monster;

        public decimal Multiplier
        {
            get { return _Multiplier; }
            set
            {
                if (_Multiplier == value) return;

                var oldValue = _Multiplier;
                _Multiplier = value;

                OnChanged(oldValue, value, "Multiplier");
            }
        }
        private decimal _Multiplier;



        protected internal override void OnCreated()
        {
            base.OnCreated();
            Multiplier = 1;
        }

    }
}
