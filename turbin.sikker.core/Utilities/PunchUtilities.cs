using turbin.sikker.core.Model;

namespace turbin.sikker.core.Utilities
{
public class PunchUtilities : IPunchUtilities
	{
       public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "pending" || lowerCaseValue == "approved" || lowerCaseValue == "rejected";
        }

         public string GetPunchStatus(PunchStatus status)
        {
            switch (status)
            {
                case PunchStatus.Pending:
                    return "Pending";
                case PunchStatus.Approved:
                    return "Approved";
                case PunchStatus.Rejected:
                    return "Rejected";
                default:
                    return "Pending";
            }
        }

        public string GetPunchSeverity(PunchSeverity status)
        {
            switch (status)
            {
                case PunchSeverity.Minor:
                    return "Minor";
                case PunchSeverity.Major:
                    return "Major";
                case PunchSeverity.Critical:
                    return "Critical";
                default:
                    return "Critical";
            }
        }
    }
}