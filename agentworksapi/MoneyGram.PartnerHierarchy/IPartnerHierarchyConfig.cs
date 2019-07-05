namespace MoneyGram.PartnerHierarchy
{   
    /// <summary>
    /// Defines dependency injected configuration interface for configuration driven PartnerHierarchy settings.
    /// </summary>
    public interface IPartnerHierarchyConfig
    {
        string PartnerHierarchyUrl { get; }
    }
}
