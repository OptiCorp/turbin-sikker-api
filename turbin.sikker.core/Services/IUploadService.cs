using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<Upload> GetUploadById(string id);
        void UpdateUpload(Upload upload);
        void CreateUpload(Upload upload);
        void DeleteUpload(string id);
    }
}

