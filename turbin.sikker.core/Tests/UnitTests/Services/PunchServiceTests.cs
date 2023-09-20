using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class PunchServiceTests
    {
        [Fact]
        public async void PunchService_GetAllPunches_ReturnsPunchList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            //Act
            var punches = await punchService.GetAllPunchesAsync();

            //Assert
            Assert.IsType<List<PunchResponseDto>>(punches);
            Assert.Equal(punches.Count(), 10);
        }

        [Fact]
        public async void PunchService_GetPunchById_ReturnsPunch()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var id = "Punch 1";

            //Act
            var punch = await punchService.GetPunchByIdAsync(id);

            //Assert
            Assert.IsType<PunchResponseDto>(punch);
            Assert.Equal(punch.Id, id);
        }

        [Fact]
        public async void PunchService_GetPunchesByLeaderId_ReturnsPunchList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var leaderId = "User 2";

            //Act
            var punches = await punchService.GetPunchesByLeaderIdAsync(leaderId);

            //Assert
            Assert.IsType<List<PunchResponseDto>>(punches);
            Assert.Equal(punches.Count(), 10);
        }

        [Fact]
        public async void PunchService_GetPunchesByInspectorId_ReturnsPunchList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var inspectorId1 = "User 1";
            var inspectorId2 = "User 3";

            //Act
            var punchesUser1 = await punchService.GetPunchesByInspectorIdAsync(inspectorId1);
            var punchesUser2 = await punchService.GetPunchesByInspectorIdAsync(inspectorId2);

            //Assert
            Assert.IsType<List<PunchResponseDto>>(punchesUser1);
            Assert.Equal(punchesUser1.Count(), 5);
            Assert.Equal(punchesUser2.Count(), 5);
        }

        [Fact]
        public async void PunchService_GetPunchesByWorkflowId_ReturnsPunchList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var workflowId = "Workflow 1";

            //Act
            var punches = await punchService.GetPunchesByWorkflowIdAsync(workflowId);

            //Assert
            Assert.IsType<List<PunchResponseDto>>(punches);
            Assert.Equal(punches.Count(), 5);
        }

        [Fact]
        public async void PunchService_CreatePunch_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var newPunch = new PunchCreateDto
            {
                CreatorId = "User 1",
                Description = "Punch 10",
                WorkflowId = "Workflow 1",
                ChecklistTaskId = "Task 1",
                Severity = "Minor"
            };

            //Act
            var newId = await punchService.CreatePunchAsync(newPunch);
            var punchesInspector = await punchService.GetPunchesByInspectorIdAsync("User 1");
            var allPunches = await punchService.GetAllPunchesAsync();
            var punchesWorkflow = await punchService.GetPunchesByWorkflowIdAsync("Workflow 1");

            //Assert
            Assert.IsType<string>(newId);
            Assert.Equal(punchesInspector.Count(), 6);
            Assert.Equal(punchesWorkflow.Count(), 6);
            Assert.Equal(allPunches.Count(), 11);
        }

        [Fact]
        public async void PunchService_UpdatePunch_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var updatedPunch = new PunchUpdateDto
            {
                Id = "Punch 1",
                Description = "Punch 10",
                Severity = "Major"
            };

            //Act
            await punchService.UpdatePunchAsync(updatedPunch);
            var punch = await punchService.GetPunchByIdAsync("Punch 1");

            //Assert
            Assert.Equal(punch.Description, "Punch 10");
            Assert.Equal(punch.Severity, "Major");
        }

        [Fact]
        public async void PunchService_DeletePunch_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Punch");
            var punchUtilities = new PunchUtilities();
            var punchService = new PunchService(dbContext, punchUtilities);

            var punchId = "Punch 1";
            var inspectorId = "User 3";
            var workflowId = "Workflow 2";

            //Act
            await punchService.DeletePunchAsync(punchId);
            var allPunches = await punchService.GetAllPunchesAsync();
            var punchesByInspector = await punchService.GetPunchesByInspectorIdAsync(inspectorId);
            var punchesByWorkflow = await punchService.GetPunchesByWorkflowIdAsync(workflowId);

            //Assert
            Assert.Equal(allPunches.Count(), 9);
            Assert.Equal(punchesByInspector.Count(), 4);
            Assert.Equal(punchesByWorkflow.Count(), 4);
        }
    }
}