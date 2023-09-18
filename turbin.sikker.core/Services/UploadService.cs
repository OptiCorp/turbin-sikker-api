using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IEnumerable<UploadResponseDto>> GetAllUploads()
        {
            var uploads = await _context.Upload.Select(u => _uploadUtilities.ToResponseDto(u)).ToListAsync();
            return uploads;
        }

        // public async Task<UploadResponseDto> GetUploadById(string id)
        // {   
        //     var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);
        //     return _uploadUtilities.ToResponseDto(upload);
        // }

        public async Task<UploadResponseDto> GetUploadById(string id)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/container-turbinsikker-test";

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

            // var stream = File.OpenWrite("../../image.png");

            var stream = new MemoryStream();

            var blobClient = containerClient.GetBlobClient(upload.BlobRef);

            await blobClient.DownloadToAsync(stream);

            var uploadResponse = new UploadResponseDto
            {
                Bytes = stream.ToArray(),
                PunchId = upload.PunchId,
                BlobRef = upload.BlobRef
            };

            return uploadResponse;
        }

        public async Task<IEnumerable<UploadResponseDto>> GetUploadsByPunchId(string id)
        {
            return await _context.Upload.Where(c => c.PunchId == id).Select(c => _uploadUtilities.ToResponseDto(c)).ToListAsync();
        }

        // public async Task<string> CreateUpload(UploadCreateDto uploadDto)
        // {   
        //     var upload = new Upload{
        //         PunchId = uploadDto.PunchId,
        //         BlobRef = uploadDto.BlobRef
        //     };
        //     await _context.Upload.AddAsync(upload);
        //     await _context.SaveChangesAsync();

        //     return upload.Id;
        // }

        public async Task<string> CreateUpload(UploadCreateDto upload)
        {

            var newUpload = new Upload
            {
                PunchId = upload.PunchId,
                BlobRef = upload.BlobRef
            };


            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/container-turbinsikker-test";

                BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

                try
                {
                    await containerClient.CreateIfNotExistsAsync();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await upload.File.CopyToAsync(stream);
                        stream.Position = 0;
                        await containerClient.UploadBlobAsync(upload.BlobRef, stream);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }


            await _context.Upload.AddAsync(newUpload);
            await _context.SaveChangesAsync();

            return newUpload.Id;
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