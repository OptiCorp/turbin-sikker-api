using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class ChecklistWorkflowServiceTests
    {
        private async Task<TurbinSikkerDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TurbinSikkerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new TurbinSikkerDbContext(options);
            databaseContext.Database.EnsureCreated();

            await databaseContext.UserRole.AddRangeAsync(
                new UserRole
                {
                    Id = "Inspector",
                    Name = "Inspector"
                },
                new UserRole
                {
                    Id = "Leader",
                    Name = "Leader"
                }
            );

            await databaseContext.User.AddRangeAsync(
                new User
                {
                    Id = "User 1",
                    AzureAdUserId = "Some email",
                    UserRoleId = "Inspector",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username1",
                    Status = UserStatus.Active,
                    CreatedDate = DateTime.Now
                },
                new User
                {
                    Id = "User 2",
                    AzureAdUserId = "Some email",
                    UserRoleId = "Leader",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username2",
                    Status = UserStatus.Active,
                    CreatedDate = DateTime.Now
                },
                new User
                {
                    Id = "User 3",
                    AzureAdUserId = "Some email",
                    UserRoleId = "Inspector",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username3",
                    Status = UserStatus.Active,
                    CreatedDate = DateTime.Now
                }
            );

            await databaseContext.Checklist.AddRangeAsync(
                new Checklist
                {
                    Id = "Checklist 1",
                    Title = "Checklist 1",
                    Status = ChecklistStatus.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "User 2"
                },
                new Checklist
                {
                    Id = "Checklist 2",
                    Title = "Checklist 2",
                    Status = ChecklistStatus.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "User 2"
                }
            );

            await databaseContext.SaveChangesAsync();

            for (int i = 0; i < 10; i++)
            {
                var checklistId = string.Format("Checklist {0}", (i%2)+1);
                await databaseContext.AddAsync(
                    new ChecklistWorkflow
                    {
                        Id = string.Format("Workflow {0}", i),
                        ChecklistId = string.Format("Checklist {0}", (i%2)+1),
                        UserId = "User 1",
                        CreatedById = "User 2",
                        Status = CurrentChecklistStatus.Sent,
                        CreatedDate = DateTime.Now
                    }
                );
            }
            databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Fact]
        public async void WorkflowService_DoesUserHaveChecklist_ReturnsBool()
        {
            //Arrange
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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