using System.Data;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class ChecklistTaskServiceTests
    {
        private async Task<TurbinSikkerDbContext> GetDbContext()
        {

            var options = new DbContextOptionsBuilder<TurbinSikkerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new TurbinSikkerDbContext(options);
            databaseContext.Database.EnsureCreated();

            await databaseContext.UserRole.AddAsync(
                new UserRole
                {
                    Id = "1",
                    Name = "Leader"
                }
            );

            await databaseContext.User.AddAsync(
                new User
                {
                    Id = "User 1",
                    AzureAdUserId = "Some email",
                    UserRoleId = "1",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username",
                    Status = UserStatus.Active,
                    CreatedDate = DateTime.Now
                }
            );

            await databaseContext.Category.AddRangeAsync(
                new Category{Id = "Category 1", Name = "Category 1"},
                new Category{Id = "Category 2", Name = "Category 2"}
            );

            await databaseContext.Checklist.AddRangeAsync(
                new Checklist
                        {
                            Id = "Checklist 1",
                            Title = "Checklist 1",
                            Status = ChecklistStatus.Active,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "User 1"
                        },
                new Checklist
                        {
                            Id = "Checklist 2",
                            Title = "Checklist 2",
                            Status = ChecklistStatus.Active,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "User 1"
                        }
            );
            await databaseContext.SaveChangesAsync();


            if (await databaseContext.Checklist_Task.CountAsync() <= 0)
            {
                string categoryId = "";
                for (int i = 0; i < 10; i++)
                {
                    if (i%2 == 0) categoryId = "Category 1";
                    if (i%2 != 0) categoryId = "Category 2";
                    await databaseContext.Checklist_Task.AddAsync(
                        new ChecklistTask
                        {
                            Id = i.ToString(),
                            CategoryId = categoryId,
                            Description = string.Format("Task {0}", i)
                        }
                    );
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async void ChecklistTaskService_GetAllChecklistsTasks_ReturnsChecklistTaskList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
        
            //Act
            var result = await checklistTaskService.GetAllTasks();
        
            //Assert
            Assert.IsType<List<ChecklistTaskResponseDto>>(result);
            Assert.InRange(result.Count(), 10, 10);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTaskById_ReturnsChecklistTask()
        {

            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
            var id = "1";

        
            //Act
            var result = await checklistTaskService.GetChecklistTaskById(id);
        
            //Assert
            Assert.IsType<ChecklistTaskResponseDto>(result);
            Assert.Equal<string>(id, result.Id);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTasksByCategoryId_ReturnsChecklistTaskList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var categoryId = "Category 1";
        
            //Act
            var result = await checklistTaskService.GetAllTasksByCategoryId(categoryId);
        
            //Assert
            Assert.IsType<List<ChecklistTaskByCategoryResponseDto>>(result);
            Assert.InRange(result.Count(), 5, 5);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTasksByChecklistId_ReturnsChecklistTaskList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);


            var taskToChecklist = new ChecklistTaskAddTaskToChecklistDto
            {
                Id = "0",
                ChecklistId = "Checklist 1"
            };

            //Act
            var oldTasks = await checklistTaskService.GetAllTasksByChecklistId(taskToChecklist.ChecklistId);
            await checklistTaskService.AddTaskToChecklist(taskToChecklist);
            var tasks = await checklistTaskService.GetAllTasksByChecklistId(taskToChecklist.ChecklistId);

            //Assert
            Assert.IsType<List<ChecklistTaskResponseDto>>(tasks);
            Assert.Equal(oldTasks.Count(), 0);
            Assert.Equal(tasks.Count(), 1);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistTasksByDescription_ReturnsChecklistTaskList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var descriptionTest1 = "Task";
            var descriptionTest2 = "Task 1";

            //Act
            var tasksTests1 = await checklistTaskService.GetTasksByDescription(descriptionTest1);
            var tasksTests2 = await checklistTaskService.GetTasksByDescription(descriptionTest2);

            //Assert
            Assert.IsType<List<ChecklistTaskResponseDto>>(tasksTests1);
            Assert.IsType<List<ChecklistTaskResponseDto>>(tasksTests2);
            Assert.Equal(tasksTests1.Count(), 10);
            Assert.Equal(tasksTests2.Count(), 1);
        }

        [Fact]
        public async void ChecklistTaskService_CreateChecklistTask_ReturnsString()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);           

            var newChecklistTask = new ChecklistTaskRequestDto
            {
                CategoryId = "Category 1",
                Description = "New task"
            };

            //Act
            var id = await checklistTaskService.CreateChecklistTask(newChecklistTask);
            var checklistTask = await checklistTaskService.GetChecklistTaskById(id);

            //Assert
            Assert.IsType<string>(id);
            Assert.Equal(checklistTask.Description, newChecklistTask.Description);
        }

        [Fact]
        public async void ChecklistTaskService_UpdateChecklistTask_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities); 

            var updatedChecklistTask = new ChecklistTaskUpdateDto
            {
                CategoryId = "Category 1",
                Description = "Updated task",
                Id = "0"
            };

            //Act
            var oldChecklistTaskDescription = (await checklistTaskService.GetChecklistTaskById(updatedChecklistTask.Id)).Description;
            await checklistTaskService.UpdateChecklistTask(updatedChecklistTask);
            var newChecklistTask = await checklistTaskService.GetChecklistTaskById(updatedChecklistTask.Id);

            //Assert
            Assert.NotEqual(oldChecklistTaskDescription, newChecklistTask.Description);
            Assert.Equal(newChecklistTask.Description, updatedChecklistTask.Description);
        }

        [Fact]
        public async void ChecklistTaskService_UpdateChecklistTaskInChecklist_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var updatedChecklistTask = new ChecklistTaskUpdateDto
            {
                CategoryId = "Category 1",
                Description = "Updated task",
                Id = "0",
                ChecklistId = "Checklist 1"
            };

            //Act
            await checklistTaskService.AddTaskToChecklist(new ChecklistTaskAddTaskToChecklistDto{
                                                                    Id = "0",
                                                                    ChecklistId = "Checklist 1"
                                                                });
            await checklistTaskService.AddTaskToChecklist(new ChecklistTaskAddTaskToChecklistDto{
                                                                    Id = "0",
                                                                    ChecklistId = "Checklist 2"
                                                                });
            await checklistTaskService.UpdateChecklistTaskInChecklist(updatedChecklistTask);
            var tasksChecklist1 = await checklistTaskService.GetAllTasksByChecklistId("Checklist 1");
            var tasksChecklist2 = await checklistTaskService.GetAllTasksByChecklistId("Checklist 2");
            var tasks = await checklistTaskService.GetAllTasks();

            //Assert
            Assert.NotEqual(tasksChecklist1, tasksChecklist2);
            Assert.Equal(tasks.Count(), 11);
        }

        [Fact]
        public async void ChecklistTaskService_AddChecklistTaskToChecklist_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistUtilities = new ChecklistUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var taskToChecklist = new ChecklistTaskAddTaskToChecklistDto
            {
                Id = "0",
                ChecklistId = "Checklist 1"
            };

            //Act
            await checklistTaskService.AddTaskToChecklist(taskToChecklist);
            var checklist = await checklistService.GetChecklistById(taskToChecklist.ChecklistId);

            //Assert
            Assert.Equal(checklist.ChecklistTasks.Count(), 1);
        }

        [Fact]
        public async void ChecklistTaskService_DeleteChecklistTask_ReturnsVoid()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var id = "0";

            //Act
            await checklistTaskService.DeleteChecklistTask(id);
            var checklistTask = await checklistTaskService.GetChecklistTaskById(id);
            var checklistTasks = await checklistTaskService.GetAllTasks();

            //Assert
            Assert.Equal(checklistTask, null);
            Assert.Equal(checklistTasks.Count(), 9);
        }
    }
}