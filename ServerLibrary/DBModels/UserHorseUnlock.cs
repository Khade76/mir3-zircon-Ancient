using Library.SystemModels;
using MirDB;

namespace Server.DBModels
{
    [UserObject]
    public sealed class UserHorseUnlock : DBObject
    {
        [Association("HorseUnlocks")]
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

        public HorseInfo HorseInfo
        {
            get
            {
                return _HorseInfo;
            }
            set
            {
                if (_HorseInfo == value)
                    return;

                var oldValue = _HorseInfo;
                _HorseInfo = value;

                OnChanged(oldValue, value, "HorseInfo");
            }
        }
        private HorseInfo _HorseInfo;

        protected override void OnDeleted()
        {
            Account = null;

            base.OnDeleted();
        }

    }
}
