using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<Upload> GetUploadById(string id);
        Task<IEnumerable<Upload>> GetUploadsByPunchId(string punchId);
        Task<IEnumerable<Upload>> GetUploadsByWorkflowId(string workflowId);
        Task UpdateUpload(Upload upload);
        Task<string> CreateUpload(Upload upload);
        Task DeleteUpload(string id);
    }
}