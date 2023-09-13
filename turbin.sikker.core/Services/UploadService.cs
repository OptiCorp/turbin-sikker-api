using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO;

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

        public async Task<IEnumerable<Upload>> GetUploadsByPunchId(string id)
        {
            return await _context.Upload.Where(c => c.PunchId == id).ToListAsync();
        }

        // public async Task<IEnumerable<Upload>> GetUploadsByWorkflowId(string id)
        // {
        //     return await _context.Upload.Where(c => c.ChecklistWorkflowId == id).ToListAsync();
        // }

        public async Task<string> CreateUpload(UploadCreateDto uploadDto)
        {   
            var upload = new Upload{
                PunchId = uploadDto.PunchId,
                BlobRef = uploadDto.BlobRef
            };
            await _context.Upload.AddAsync(upload);
            await _context.SaveChangesAsync();

            return upload.Id;
        }

        public async Task UpdateUpload(UploadUpdateDto updatedUpload)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == updatedUpload.Id);

            if (upload != null)
            {
                upload.PunchId = updatedUpload.PunchId;
                upload.BlobRef = updatedUpload.BlobRef;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUpload(string id)
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