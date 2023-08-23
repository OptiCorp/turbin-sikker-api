using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Services
{
    public class UploadService : IUploadService
    {
        private readonly TurbinSikkerDbContext _context;

        public UploadService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public async Task<Upload> GetUploadById(string id)
        {
            return await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async void CreateUpload(Upload upload)
        {
            _context.Upload.Add(upload);
            await _context.SaveChangesAsync();
        }

        public async void UpdateUpload(Upload updatedUpload)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == updatedUpload.Id);

            if (upload != null)
            {
                upload.PunchId = updatedUpload.PunchId;
                upload.BlobRef = updatedUpload.BlobRef;

                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteUpload(string id)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);

            if (upload != null)
            {
                _context.Upload.Remove(upload);
                await _context.SaveChangesAsync();
            }
        }

    }

}