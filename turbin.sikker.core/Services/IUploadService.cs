using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<UploadResponseDto> GetUploadByIdAsync(string id);
        Task<IEnumerable<UploadResponseDto>> GetUploadsByPunchIdAsync(string punchId);
        Task UpdateUploadAsync(UploadUpdateDto upload);
        Task<string> CreateUploadAsync(UploadCreateDto upload);
        Task DeleteUploadAsync(string id);
    }
}