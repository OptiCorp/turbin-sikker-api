
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Task<IEnumerable<PunchResponseDto>> GetAllPunchesAsync();
        Task<PunchResponseDto> GetPunchByIdAsync(string id);
        Task UpdatePunchAsync(PunchUpdateDto punch);
        Task<string> CreatePunchAsync(PunchCreateDto punch);
        Task DeletePunchAsync(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowIdAsync(string checklistId);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByInspectorIdAsync(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByLeaderIdAsync(string id);
    }
}