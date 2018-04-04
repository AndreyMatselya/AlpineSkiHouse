namespace AlpineSkiHouse.Web.Services
{
    public interface ICsrInformationService
    {
        int OnlineRepresentatives { get; }
        string CallCenterPhoneNumber { get; }
        bool CallCenterOnline { get; }
    }
}
