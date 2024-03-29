using turbin.sikker.core.Model.DTO.ChecklistDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class ChecklistServiceTests
    {
        [Fact]
        public async void ChecklistService_GetAllChecklists_ReturnsChecklistList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);
        
            //Act
            var result = await checklistService.GetAllChecklistsAsync();
        
            //Assert
            Assert.IsType<List<ChecklistResponseDto>>(result);
            Assert.InRange(result.Count(), 10, 10);
        }

        [Fact]
        public async void ChecklistService_GetChecklistById_ReturnsChecklist()
        {

            //Arrange
            string id = "Checklist 1";
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

        
            //Act
            var result = await checklistService.GetChecklistByIdAsync(id);
        
            //Assert
            Assert.IsType<ChecklistResponseDto>(result);
            Assert.Equal<string>(id, result.Id);
        }

        [Fact]
        public async void ChecklistService_GetAllChecklistsByUserId_ReturnsChecklistList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);
            string userId = "User 1";
        
            //Act
            var result = await checklistService.GetAllChecklistsByUserIdAsync(userId);
        
            //Assert
            Assert.IsType<List<ChecklistResponseNoUserDto>>(result);
            Assert.InRange(result.Count(), 5, 5);
        }

        [Fact]
        public async void ChecklistService_SearchChecklistByName_ReturnsChecklistList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);
            string name = "Checklist";

            //Act
            var result = await checklistService.SearchChecklistByNameAsync(name);

            //Assert
            Assert.IsType<List<ChecklistResponseDto>>(result);
            Assert.InRange(result.Count(), 10, 10);
        }

        [Fact]
        public async void ChecklistService_CreateChecklist_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var newChecklist = new ChecklistCreateDto
            {
                Title = "New checklist",
                CreatorId = "User 1"
            };

            //Act
            var id = await checklistService.CreateChecklistAsync(newChecklist);
            var checklist = await checklistService.GetChecklistByIdAsync(id);

            //Assert
            Assert.IsType<string>(id);
            Assert.Equal(checklist.Title, "New checklist");
        }

        [Fact]
        public async void ChecklistService_UpdateChecklist_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var updatedChecklist = new ChecklistUpdateDto
            {
                Id = "Checklist 0",
                Title = "Updated checklist",
                Status = "Inactive"
            };

            //Act
            var oldChecklistTitle = (await checklistService.GetChecklistByIdAsync(updatedChecklist.Id)).Title;
            await checklistService.UpdateChecklistAsync(updatedChecklist);
            var newChecklist = await checklistService.GetChecklistByIdAsync(updatedChecklist.Id);

            //Assert
            Assert.NotEqual(oldChecklistTitle, newChecklist.Title);
            Assert.Equal(newChecklist.Title, updatedChecklist.Title);
            Assert.Equal(newChecklist.Status, "Inactive");
        }

        [Fact]
        public async void ChecklistService_SoftDeleteChecklist_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var id = "Checklist 0";

            //Act
            await checklistService.DeleteChecklistAsync(id);
            var checklist = await checklistService.GetChecklistByIdAsync(id);
            var checklists = await checklistService.GetAllChecklistsAsync();

            //Assert
            Assert.Equal(checklist.Status, "Inactive");
            Assert.Equal(checklists.Count(), 10);
        }

        [Fact]
        public async void ChecklistService_HardDeleteChecklist_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Checklist");
            var checklistUtilities = new ChecklistUtilities();
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var id = "Checklist 0";

            //Act
            await checklistService.HardDeleteChecklistAsync(id);
            var checklist = await checklistService.GetChecklistByIdAsync(id);
            var checklists = await checklistService.GetAllChecklistsAsync();

            //Assert
            Assert.Equal(checklist, null);
            Assert.Equal(checklists.Count(), 9);
        }
        
    }

}