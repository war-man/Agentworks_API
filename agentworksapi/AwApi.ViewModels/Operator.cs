using System;
using System.Collections.Generic;

namespace AwApi.ViewModels
{
    [Serializable]
    public class Operator
    {
        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string UserName { get; set; }

        public string Language { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool IsInTrainingMode { get; set; }

        public List<string> Roles { get; set; }
        public TransactionalLimits TransactionalLimits { get; set; }
    }
}