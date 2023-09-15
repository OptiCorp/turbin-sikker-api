using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class UploadServiceTests
    {
        [Fact]
        public async void UploadService_GetUploadById_ReturnsUpload()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Upload");
            var uploadUtilities = new UploadUtilities();
            var uploadService = new UploadService(dbContext, uploadUtilities);

            var uploadId = "Upload 1";

            //Act
            var upload = await uploadService.GetUploadById(uploadId);

            //Assert
            Assert.IsType<UploadResponseDto>(upload);
            Assert.Equal(upload.Id, uploadId);
            Assert.Equal(upload.BlobRef, uploadId);
        }

        [Fact]
        public async void UploadService_GetUploadsByPunchId_ReturnsUploadList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Upload");
            var uploadUtilities = new UploadUtilities();
            var uploadService = new UploadService(dbContext, uploadUtilities);

            var punchId = "Punch 1";

            //Act
            var uploads = await uploadService.GetUploadsByPunchId(punchId);

            //Assert
            Assert.IsType<List<UploadResponseDto>>(uploads);
            Assert.Equal(uploads.Count(), 5);
        }

        [Fact]
        public async void UploadService_CreateUpload_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Upload");
            var uploadUtilities = new UploadUtilities();
            var uploadService = new UploadService(dbContext, uploadUtilities);

            var newUpload = new UploadCreateDto
            {
                PunchId = "Punch 1",
                BlobRef = "Upload 10"
            };

            //Act
            var newUploadId = await uploadService.CreateUpload(newUpload);
            var uploads = await uploadService.GetAllUploads();
            var upload = await uploadService.GetUploadById(newUploadId);

            //Assert
            Assert.IsType<string>(newUploadId);
            Assert.Equal(uploads.Count(), 11);
            Assert.Equal(upload.BlobRef, "Upload 10");

        }

        [Fact]
        public async void UploadService_UpdateUpload_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Upload");
            var uploadUtilities = new UploadUtilities();
            var uploadService = new UploadService(dbContext, uploadUtilities);

            var updatedUpload = new UploadUpdateDto
            {
                PunchId = "Punch 1",
                BlobRef = "Upload 10",
                Id = "Upload 1"
            };

            //Act
            await uploadService.UpdateUpload(updatedUpload);
            var upload = await uploadService.GetUploadById("Upload 1");

            //Assert
            Assert.Equal(upload.BlobRef, "Upload 10");
            Assert.Equal(upload.PunchId, "Punch 1");
        }

        [Fact]
        public async void UploadService_DeleteUpload_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Upload");
            var uploadUtilities = new UploadUtilities();
            var uploadService = new UploadService(dbContext, uploadUtilities);

            var id = "Upload 1";

            //Act
            await uploadService.DeleteUpload(id);
            var uploads = await uploadService.GetAllUploads();

            //Assert
            Assert.Equal(uploads.Count(), 9);
        }
    }
}