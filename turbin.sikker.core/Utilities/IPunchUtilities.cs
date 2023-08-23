using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface IPunchUtilities
    {
        bool IsValidStatus(string value);
        string GetPunchStatus(PunchStatus status);
        public string GetPunchSeverity(PunchSeverity status);
    }
}