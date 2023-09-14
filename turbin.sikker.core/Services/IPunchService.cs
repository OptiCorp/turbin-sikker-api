
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Task<IEnumerable<PunchResponseDto>> GetAllPunches();
        Task<PunchResponseDto> GetPunchById(string id);
        Task UpdatePunch(PunchUpdateDto punch);
        Task<string> CreatePunch(PunchCreateDto punch);
        Task DeletePunch(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowId(string checklistId);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByInspectorId(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByLeaderId(string id);
    }
}