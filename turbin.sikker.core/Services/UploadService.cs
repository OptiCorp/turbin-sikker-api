using turbin.sikker.core.Model;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Azure.Storage.Blobs;
using Azure.Identity;

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


        public async Task<UploadResponseDto> GetUploadByIdAsync(string id)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/container-turbinsikker-test";

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

            var stream = new MemoryStream();

            var blobClient = containerClient.GetBlobClient(upload.BlobRef);

            await blobClient.DownloadToAsync(stream);

            var uploadResponse = _uploadUtilities.ToResponseDto(upload, stream.ToArray());

            return uploadResponse;
        }

        public async Task<IEnumerable<UploadResponseDto>> GetUploadsByPunchIdAsync(string id)
        {
            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/container-turbinsikker-test";
            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            var uploadList = new List<UploadResponseDto>();

            var uploads = _context.Upload.Where(u => u.PunchId == id);

            foreach (var upload in uploads)
            {
                var stream = new MemoryStream();

                var blobClient = containerClient.GetBlobClient(upload.BlobRef);

                await blobClient.DownloadToAsync(stream);

                var uploadResponse = _uploadUtilities.ToResponseDto(upload, stream.ToArray());

                uploadList.Add(uploadResponse);
            }

            return uploadList;
        }


        public async Task<string> CreateUploadAsync(UploadCreateDto upload)
        {

            var newUpload = new Upload
            {
                PunchId = upload.PunchId,
                BlobRef = Guid.NewGuid().ToString(),
                ContentType = upload.File.ContentType
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
                        await containerClient.UploadBlobAsync(newUpload.BlobRef, stream);
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

        public async Task UpdateUploadAsync(UploadUpdateDto updatedUpload)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == updatedUpload.Id);

            if (upload != null)
            {
                if (updatedUpload.PunchId != null)
                {
                    upload.PunchId = updatedUpload.PunchId;

                    await _context.SaveChangesAsync();
                }
            
            }
        }

        public async Task DeleteUploadAsync(string id)
        {
            var upload = await _context.Upload.FirstOrDefaultAsync(u => u.Id == id);

            if (upload != null)
            {
                string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/container-turbinsikker-test";
                BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

                var blobClient = containerClient.GetBlobClient(upload.BlobRef);

                await blobClient.DeleteAsync();

                _context.Upload.Remove(upload);
                await _context.SaveChangesAsync();
            }
        }

    }

}