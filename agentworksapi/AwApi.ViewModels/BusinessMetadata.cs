using System;

namespace AwApi.ViewModels
{
    [Serializable]
    public sealed class BusinessMetadata
    {
        public bool CodeTablesChanged { get; set; }
        public bool ProfileChanged { get; set; }
        public string OperatorLanguage { get; set; }
        public string AgentId { get; set; }
        public string PosNum { get; set; }
        public string UnitProfileId { get; set; }
        public string ApiCodeVersion { get; set; }
        public string AcVersion { get; set; }
        public string ServicesEnvironment { get; set; }
        public DataSource DataSource { get; set; }
    }
}