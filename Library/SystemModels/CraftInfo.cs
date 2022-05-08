using MirDB;

namespace Library.SystemModels
{
    public sealed class CraftLevelInfo : DBObject
    {
        public CraftType Type
        {
            get { return _Type; }
            set
            {
                if (_Type == value) return;

                var oldValue = _Type;
                _Type = value;

                OnChanged(oldValue, value, "Type");
            }
        }
        private CraftType _Type;

        public int Level
        {
            get { return _Level; }
            set
            {
                if (_Level == value) return;

                var oldValue = _Level;
                _Level = value;

                OnChanged(oldValue, value, "Level");
            }
        }
        private int _Level;

        public int Exp
        {
            get { return _Exp; }
            set
            {
                if (_Exp == value) return;

                var oldValue = _Exp;
                _Exp = value;

                OnChanged(oldValue, value, "Exp");
            }
        }
        private int _Exp;
    }

    public sealed class CraftItemInfo : DBObject
    {
        public CraftType Type
        {
            get { return _Type; }
            set
            {
                if (_Type == value) return;

                var oldValue = _Type;
                _Type = value;

                OnChanged(oldValue, value, "Type");
            }
        }
        private CraftType _Type;

        public int Level
        {
            get { return _Level; }
            set
            {
                if (_Level == value) return;

                var oldValue = _Level;
                _Level = value;

                OnChanged(oldValue, value, "Level");
            }
        }
        private int _Level;
        

        [Association("Crafting")]
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


        public ItemInfo Ingredient1
        {
            get { return _Ingredient1; }
            set
            {
                if (_Ingredient1 == value) return;

                var oldValue = _Ingredient1;
                _Ingredient1 = value;

                OnChanged(oldValue, value, "Ingredient1");
            }
        }
        private ItemInfo _Ingredient1;

        public int Ingredient1Amount
        {
            get { return _Ingredient1Amount; }
            set
            {
                if (_Ingredient1Amount == value) return;

                var oldValue = _Ingredient1Amount;
                _Ingredient1Amount = value;

                OnChanged(oldValue, value, "Ingredient1Amount");
            }
        }
        private int _Ingredient1Amount;


        public ItemInfo Ingredient2
        {
            get { return _Ingredient2; }
            set
            {
                if (_Ingredient2 == value) return;

                var oldValue = _Ingredient2;
                _Ingredient2 = value;

                OnChanged(oldValue, value, "Ingredient2");
            }
        }
        private ItemInfo _Ingredient2;

        public int Ingredient2Amount
        {
            get { return _Ingredient2Amount; }
            set
            {
                if (_Ingredient2Amount == value) return;

                var oldValue = _Ingredient2Amount;
                _Ingredient2Amount = value;

                OnChanged(oldValue, value, "Ingredient2Amount");
            }
        }
        private int _Ingredient2Amount;


        public ItemInfo Ingredient3
        {
            get { return _Ingredient3; }
            set
            {
                if (_Ingredient3 == value) return;

                var oldValue = _Ingredient3;
                _Ingredient3 = value;

                OnChanged(oldValue, value, "Ingredient3");
            }
        }
        private ItemInfo _Ingredient3;

        public int Ingredient3Amount
        {
            get { return _Ingredient3Amount; }
            set
            {
                if (_Ingredient3Amount == value) return;

                var oldValue = _Ingredient3Amount;
                _Ingredient3Amount = value;

                OnChanged(oldValue, value, "Ingredient3Amount");
            }
        }
        private int _Ingredient3Amount;


        public ItemInfo Ingredient4
        {
            get { return _Ingredient4; }
            set
            {
                if (_Ingredient4 == value) return;

                var oldValue = _Ingredient4;
                _Ingredient4 = value;

                OnChanged(oldValue, value, "Ingredient4");
            }
        }
        private ItemInfo _Ingredient4;

        public int Ingredient4Amount
        {
            get { return _Ingredient4Amount; }
            set
            {
                if (_Ingredient4Amount == value) return;

                var oldValue = _Ingredient4Amount;
                _Ingredient4Amount = value;

                OnChanged(oldValue, value, "Ingredient4Amount");
            }
        }
        private int _Ingredient4Amount;

        public int Cost
        {
            get { return _Cost; }
            set
            {
                if (_Cost == value) return;

                var oldValue = _Cost;
                _Cost = value;

                OnChanged(oldValue, value, "Cost");
            }
        }
        private int _Cost;

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

        public int Exp
        {
            get { return _Exp; }
            set
            {
                if (_Exp == value) return;

                var oldValue = _Exp;
                _Exp = value;

                OnChanged(oldValue, value, "Exp");
            }
        }
        private int _Exp;

        protected internal override void OnCreated()
        {
            base.OnCreated();

            Amount = 1;
        }
    }
}

