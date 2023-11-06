using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Utilities
{
public interface IPunchUtilities
    {
        bool IsValidStatus(string value);
        string GetPunchStatus(PunchStatus status);
        public string GetPunchSeverity(PunchSeverity status);

        public PunchResponseDto PunchToResponseDto(Punch? punch);
    }
}