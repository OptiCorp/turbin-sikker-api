using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public class UploadService : IUploadService
    {
        private readonly TurbinSikkerDbContext _context;

        public UploadService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public Upload GetUploadById(string id)
        {
            return _context.Upload.FirstOrDefault(u => u.Id == id);
        }

        public void CreateUpload(Upload upload)
        {
            _context.Upload.Add(upload);
            _context.SaveChanges();
        }

        public void UpdateUpload(Upload updatedUpload)
        {
            var upload = _context.Upload.FirstOrDefault(u => u.Id == updatedUpload.Id);

            if (upload != null)
            {
                upload.PunchId = updatedUpload.PunchId;
                upload.BlobRef = updatedUpload.BlobRef;

                _context.SaveChanges();
            }
        }

        public void DeleteUpload(string id)
        {
            var upload = _context.Upload.FirstOrDefault(u => u.Id == id);

            if (upload != null)
            {
                _context.Upload.Remove(upload);
                _context.SaveChanges();
            }
        }

    }

}