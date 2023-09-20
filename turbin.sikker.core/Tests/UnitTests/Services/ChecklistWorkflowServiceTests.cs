using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class ChecklistWorkflowServiceTests
    {
        [Fact]
        public async void WorkflowService_DoesUserHaveChecklist_ReturnsBool()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var userId = "User 1";
            var checklistId = "Checklist 1";

            //Act
            var result = await workflowService.DoesUserHaveChecklistAsync(userId, checklistId);

            //Assert
            Assert.IsType<bool>(result);
            Assert.Equal(result, true);
        }

        [Fact]
        public async void WorkflowService_GetWorkflowById_ReturnsWorkflow()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var id = "Workflow 1";

            //Act
            var workflow = await workflowService.GetChecklistWorkflowByIdAsync(id);

            //Assert
            Assert.IsType<ChecklistWorkflowResponseDto>(workflow);
            Assert.Equal(workflow.Id, id);
        }

        [Fact]
        public async void WorkflowService_GetAllWorkflows_ReturnsWorkflowList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            //Act
            var workflows = await workflowService.GetAllChecklistWorkflowsAsync();

            //Assert
            Assert.IsType<List<ChecklistWorkflowResponseDto>>(workflows);
            Assert.Equal(workflows.Count(), 10);
        }

        [Fact]
        public async void WorkflowService_GetAllWorkflowsByUserId_ReturnsWorkflowList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var userId = "User 1";

            //Act
            var workflows = await workflowService.GetAllChecklistWorkflowsByUserIdAsync(userId);

            //Assert
            Assert.IsType<List<ChecklistWorkflowResponseDto>>(workflows);
            Assert.Equal(workflows.Count(), 10);
        }

        [Fact]
        public async void WorkflowService_UpdateWorkflow_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var updatedWorkflow = new ChecklistWorkflowUpdateDto
            {
                Status = "Committed",
                Id = "Workflow 1"
            };

            //Act
            await workflowService.UpdateChecklistWorkflowAsync(updatedWorkflow);
            var workflow = await workflowService.GetChecklistWorkflowByIdAsync("Workflow 1");

            //Assert
            Assert.Equal(workflow.Status, "Committed");
            Assert.Equal(workflow.User.Id, "User 1");
        }

        [Fact]
        public async void WorkflowService_CreateWorkflowReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var userIds = new List<string>
            {
                "User 3"
            };
            var newWorkflow = new ChecklistWorkflowCreateDto
            {
                ChecklistId = "Checklist 1",
                UserIds = userIds,
                CreatorId = "User 2"
            };

            //Act
            await workflowService.CreateChecklistWorkflowAsync(newWorkflow);
            var workflows = await workflowService.GetAllChecklistWorkflowsByUserIdAsync("User 3");
            var allWorkflows = await workflowService.GetAllChecklistWorkflowsAsync();

            //Assert
            Assert.Equal(workflows.Count(), 1);
            Assert.Equal(allWorkflows.Count(), 11);
        }

        [Fact]
        public async void WorkflowService_DeleteWorkflow_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var workflowUtilities = new ChecklistWorkflowUtilities();
            var workflowService = new ChecklistWorkflowService(dbContext, workflowUtilities);

            var id = "Workflow 1";

            //Act
            await workflowService.DeleteChecklistWorkflowAsync(id);
            var workflows = await workflowService.GetAllChecklistWorkflowsAsync();

            //Assert
            Assert.Equal(workflows.Count(), 9);
        }
    }
}