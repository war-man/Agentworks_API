using System;
using System.Collections.Generic;

namespace AwApi.ViewModels.Location
{
    [Serializable]
    public class User
    {
        public bool AllLocationsAllowed { get; set; }
        public string MainOfficeAgentId { get; set; }
        public string UserId { get; set; }
        public List<UserAgentActivity> UserAgentList { get; set; }
        public UserStatus Status { get; set; }
    }
}