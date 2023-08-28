using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<Upload> GetUploadById(string id);
        Task<IEnumerable<Upload>> GetUploadsByPunchId(string checklistId);
        void UpdateUpload(Upload upload);
        void CreateUpload(Upload upload);
        void DeleteUpload(string id);
    }
}

