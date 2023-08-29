using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Task<Punch> GetPunchById(string id);
        Task UpdatePunch(string punchId, PunchUpdateDto punch);
        Task<string> CreatePunch(PunchCreateDto punch);
        Task DeletePunch(string id);
        Task<IEnumerable<Punch>> GetPunchesByWorkflowId(string checklistId);
        // string GetPunchStatus(PunchStatus status);
        // bool IsValidStatus(string value);
        // public string GetPunchSeverity(PunchSeverity status);
    }
}