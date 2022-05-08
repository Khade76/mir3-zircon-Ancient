using Library;
using MirDB;

namespace Server.DBModels
{
    [UserObject]
    public sealed class UserCrafting : DBObject
    {
        [Association("Crafting")]
        public AccountInfo Account
        {
            get
            {
                return _Account;
            }
            set
            {
                if (_Account == value)
                    return;

                var oldValue = _Account;
                _Account = value;

                OnChanged(oldValue, value, "Account");
            }
        }
        private AccountInfo _Account;

        public CraftType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (_Type == value)
                    return;

                var oldValue = _Type;
                _Type = value;

                OnChanged(oldValue, value, "Type");
            }
        }
        private CraftType _Type;

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

        public int Exp
        {
            get
            {
                return _Exp;
            }
            set
            {
                if (_Exp == value)
                    return;

                var oldValue = _Exp;
                _Exp = value;

                OnChanged(oldValue, value, "Exp");
            }
        }
        private int _Exp;

        public ClientUserCrafting ToClientInfo()
        {
            return new ClientUserCrafting
            {
                Type = Type,
                Level = Level,
                Exp = Exp
            };
        }
    }
}
