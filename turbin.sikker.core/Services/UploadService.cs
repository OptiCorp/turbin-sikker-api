using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class UploadService : IUploadService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IUploadUtilities _uploadUtilities;

        public UploadService(TurbinSikkerDbContext context, IUploadUtilities uploadUtilities)
        {
            _context = context;
            _uploadUtilities = uploadUtilities;
        }

        public async Task<UploadResponseDto> GetUploadById(string id)
        {   
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);
            return _uploadUtilities.ToResponseDto(upload);
        }

        public async Task<IEnumerable<Upload>> GetUploadsByPunchId(string id)
        {
            return await _context.Upload.Where(c => c.PunchId == id).ToListAsync();
        }

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