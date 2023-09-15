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
            var result = await workflowService.DoesUserHaveChecklist(userId, checklistId);

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
            var workflow = await workflowService.GetChecklistWorkflowById(id);

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
            var workflows = await workflowService.GetAllChecklistWorkflows();

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
            var workflows = await workflowService.GetAllChecklistWorkflowsByUserId(userId);

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

            var updatedWorkflow = new ChecklistWorkflowEditDto
            {
                Status = "Committed",
                Id = "Workflow 1"
            };

            //Act
            await workflowService.UpdateChecklistWorkflow(updatedWorkflow);
            var workflow = await workflowService.GetChecklistWorkflowById("Workflow 1");

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
                CreatedById = "User 2"
            };

            //Act
            await workflowService.CreateChecklistWorkflow(newWorkflow);
            var workflows = await workflowService.GetAllChecklistWorkflowsByUserId("User 3");
            var allWorkflows = await workflowService.GetAllChecklistWorkflows();

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
            await workflowService.DeleteChecklistWorkflow(id);
            var workflows = await workflowService.GetAllChecklistWorkflows();

            //Assert
            Assert.Equal(workflows.Count(), 9);
        }
    }
}