using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<Upload> GetUploadById(string id);
        Task<IEnumerable<Upload>> GetUploadsByPunchId(string punchId);
        Task UpdateUpload(UploadUpdateDto upload);
        Task<string> CreateUpload(UploadCreateDto upload);
        Task DeleteUpload(string id);
    }
}