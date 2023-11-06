using turbin.sikker.core.Model.DTO.WorkflowDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class WorkflowServiceTests
    {
        [Fact]
        public async void WorkflowService_DoesUserHaveChecklist_ReturnsBool()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

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
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            var id = "Workflow 1";

            //Act
            var workflow = await workflowService.GetWorkflowByIdAsync(id);

            //Assert
            Assert.IsType<WorkflowResponseDto>(workflow);
            Assert.Equal(workflow.Id, id);
        }

        [Fact]
        public async void WorkflowService_GetAllWorkflows_ReturnsWorkflowList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            //Act
            var workflows = await workflowService.GetAllWorkflowsAsync();

            //Assert
            Assert.IsType<List<WorkflowResponseDto>>(workflows);
            Assert.Equal(workflows.Count(), 10);
        }

        [Fact]
        public async void WorkflowService_GetAllWorkflowsByUserId_ReturnsWorkflowList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            var userId = "User 1";

            //Act
            var workflows = await workflowService.GetAllWorkflowsByUserIdAsync(userId);

            //Assert
            Assert.IsType<List<WorkflowResponseDto>>(workflows);
            Assert.Equal(workflows.Count(), 10);
        }

        [Fact]
        public async void WorkflowService_UpdateWorkflow_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Workflow");
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            var updatedWorkflow = new WorkflowUpdateDto
            {
                Status = "Committed",
                Id = "Workflow 1"
            };

            //Act
            await workflowService.UpdateWorkflowAsync(updatedWorkflow);
            var workflow = await workflowService.GetWorkflowByIdAsync("Workflow 1");

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
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            var userIds = new List<string>
            {
                "User 3"
            };
            var newWorkflow = new WorkflowCreateDto
            {
                ChecklistId = "Checklist 1",
                UserIds = userIds,
                CreatorId = "User 2"
            };

            //Act
            await workflowService.CreateWorkflowAsync(newWorkflow);
            var workflows = await workflowService.GetAllWorkflowsByUserIdAsync("User 3");
            var allWorkflows = await workflowService.GetAllWorkflowsAsync();

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
            var checklistUtilities = new ChecklistUtilities();
            var workflowUtilities = new WorkflowUtilities(checklistUtilities);
            var workflowService = new WorkflowService(dbContext, workflowUtilities);

            var id = "Workflow 1";

            //Act
            await workflowService.DeleteWorkflowAsync(id);
            var workflows = await workflowService.GetAllWorkflowsAsync();

            //Assert
            Assert.Equal(workflows.Count(), 9);
        }
    }
}
