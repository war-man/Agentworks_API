using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    public enum WidgetType
    {
        LINK,
        FRAGMENT
    }

    public class Agent : Partner
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public int HierarchyLevel { get; set; }
        public string LegacyAgentNum { get; set; }
        public string TimeZoneName { get; set; }
        public string StoreNumber { get; set; }
        public List<string> Pos { get; set; }
        public string activityType { get; set; }
    }

    public class Partner
    {
        public decimal Id { get; set; }
        public bool AuthenticationRequired { get; set; }
        public bool EmailAsUserId { get; set; }
        public Widget DefaultWidget { get; set; }
        public bool Active { get; set; }
    }

    public class Widget : Component
    {
        public List<Role> Roles { get; set; }
        public string Label { get; set; }
        public WidgetType Type { get; set; }
        public string Location { get; set; }
    }

    public class Component
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Role
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}