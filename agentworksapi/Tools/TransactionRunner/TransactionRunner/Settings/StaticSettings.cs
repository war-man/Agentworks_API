using TransactionRunner.Interfaces;

namespace TransactionRunner.Settings
{
    public static class StaticSettings
    {
        private static ISettingsSvc _mySettingsSvc;

        public static ISettingsSvc SettingsSvc
        {
            get { return _mySettingsSvc ?? (_mySettingsSvc = new SettingsSvc()); }
            set { _mySettingsSvc = value; }
        }
    }
}