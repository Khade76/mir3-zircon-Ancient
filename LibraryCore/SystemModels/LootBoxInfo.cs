using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MirDB;

namespace Library.SystemModels
{
    public sealed class LootBoxInfo : DBObject
    {
        [Association("LootBox")]
        public ItemInfo LootBox
        {
            get { return _LootBox; }
            set
            {
                if (_LootBox == value) return;

                var oldValue = _LootBox;
                _LootBox = value;

                OnChanged(oldValue, value, "LootBox");
            }
        }
        private ItemInfo _LootBox;

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

        public bool Guarenteed
        {
            get { return _Guarenteed; }
            set
            {
                if (_Guarenteed == value) return;

                var oldValue = _Guarenteed;
                _Guarenteed = value;

                OnChanged(oldValue, value, "Guarenteed");
            }
        }
        private bool _Guarenteed;

        public bool RandAmount
        {
            get { return _RandAmount; }
            set
            {
                if (_RandAmount == value) return;

                var oldValue = _RandAmount;
                _RandAmount = value;

                OnChanged(oldValue, value, "RandAmount");
            }
        }
        private bool _RandAmount;
    }
}
