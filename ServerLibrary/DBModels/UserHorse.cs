using Library.SystemModels;
using MirDB;

namespace Server.DBModels
{
    [UserObject]
    public sealed class UserHorse : DBObject
    {
        [Association("Horses")]
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

        public HorseInfo Info
        {
            get
            {
                return _Info;
            }
            set
            {
                if (_Info == value)
                    return;

                var oldValue = _Info;
                _Info = value;

                OnChanged(oldValue, value, "Info");
            }
        }
        private HorseInfo _Info;

        public int HorseNum
        {
            get
            {
                return _HorseNum;
            }
            set
            {
                if (_HorseNum == value)
                    return;

                var oldValue = _HorseNum;
                _HorseNum = value;

                OnChanged(oldValue, value, "HorseNum");
            }
        }
        private int _HorseNum;

        protected override void OnDeleted()
        {
            Account = null;
            Info = null;

            base.OnDeleted();
        }


        public int ToClientInfo()
        {
            return HorseNum;
        }


        public override string ToString()
        {
            return Account?.EMailAddress ?? string.Empty;
        }
    }
}
