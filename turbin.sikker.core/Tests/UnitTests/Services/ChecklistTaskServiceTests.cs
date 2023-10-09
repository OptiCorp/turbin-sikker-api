using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests.Services
{
    public class ChecklistTaskServiceTests
    {
        [Fact]
        public async void ChecklistTaskService_GetAllChecklistsTasks_ReturnsChecklistTaskList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
        
            //Act
            var result = await checklistTaskService.GetAllTasksAsync();
        
            //Assert
            Assert.IsType<List<ChecklistTaskResponseDto>>(result);
            Assert.InRange(result.Count(), 10, 10);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTaskById_ReturnsChecklistTask()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
            var id = "Task 1";

        
            //Act
            var result = await checklistTaskService.GetChecklistTaskByIdAsync(id);
        
            //Assert
            Assert.IsType<ChecklistTaskResponseDto>(result);
            Assert.Equal<string>(id, result.Id);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTasksByCategoryId_ReturnsChecklistTaskList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var categoryId = "Category 1";
        
            //Act
            var result = await checklistTaskService.GetAllTasksByCategoryIdAsync(categoryId);
        
            //Assert
            Assert.IsType<List<ChecklistTaskByCategoryResponseDto>>(result);
            Assert.InRange(result.Count(), 5, 5);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistsTasksByChecklistId_ReturnsChecklistTaskList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);


            var taskToChecklist = new ChecklistTaskAddTaskToChecklistDto
            {
                Id = "Task 0",
                ChecklistId = "Checklist 1"
            };

            //Act
            var oldTasks = await checklistTaskService.GetAllTasksByChecklistIdAsync(taskToChecklist.ChecklistId);
            await checklistTaskService.AddTaskToChecklistAsync(taskToChecklist);
            var tasks = await checklistTaskService.GetAllTasksByChecklistIdAsync(taskToChecklist.ChecklistId);

            //Assert
            Assert.IsType<List<ChecklistTaskResponseDto>>(tasks);
            Assert.Equal(oldTasks.Count(), 0);
            Assert.Equal(tasks.Count(), 1);
        }

        [Fact]
        public async void ChecklistTaskService_GetChecklistTasksByDescription_ReturnsChecklistTaskList()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var descriptionTest1 = "Task";
            var descriptionTest2 = "Task 1";

            //Act
            var tasksTests1 = await checklistTaskService.GetTasksByDescriptionAsync(descriptionTest1);
            var tasksTests2 = await checklistTaskService.GetTasksByDescriptionAsync(descriptionTest2);

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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);           

            var newChecklistTask = new ChecklistTaskCreateDto
            {
                CategoryId = "Category 1",
                Description = "New task",
                EstAvgCompletionTime = 1
            };

            //Act
            var id = await checklistTaskService.CreateChecklistTaskAsync(newChecklistTask);
            var checklistTask = await checklistTaskService.GetChecklistTaskByIdAsync(id);

            //Assert
            Assert.IsType<string>(id);
            Assert.Equal(checklistTask.Description, newChecklistTask.Description);
        }

        [Fact]
        public async void ChecklistTaskService_UpdateChecklistTask_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities); 

            var updatedChecklistTask = new ChecklistTaskUpdateDto
            {
                CategoryId = "Category 1",
                Description = "Updated task",
                Id = "Task 0"
            };

            //Act
            var oldChecklistTaskDescription = (await checklistTaskService.GetChecklistTaskByIdAsync(updatedChecklistTask.Id)).Description;
            await checklistTaskService.UpdateChecklistTaskAsync(updatedChecklistTask);
            var newChecklistTask = await checklistTaskService.GetChecklistTaskByIdAsync(updatedChecklistTask.Id);

            //Assert
            Assert.NotEqual(oldChecklistTaskDescription, newChecklistTask.Description);
            Assert.Equal(newChecklistTask.Description, updatedChecklistTask.Description);
        }

        [Fact]
        public async void ChecklistTaskService_UpdateChecklistTaskInChecklist_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var updatedChecklistTask = new ChecklistTaskUpdateDto
            {
                CategoryId = "Category 1",
                Description = "Updated task",
                Id = "Task 0",
                ChecklistId = "Checklist 1",
                EstAvgCompletionTime = 1
            };

            //Act
            await checklistTaskService.AddTaskToChecklistAsync(new ChecklistTaskAddTaskToChecklistDto{
                                                                    Id = "Task 0",
                                                                    ChecklistId = "Checklist 1"
                                                                });
            await checklistTaskService.AddTaskToChecklistAsync(new ChecklistTaskAddTaskToChecklistDto{
                                                                    Id = "Task 0",
                                                                    ChecklistId = "Checklist 2"
                                                                });
            await checklistTaskService.UpdateChecklistTaskInChecklistAsync(updatedChecklistTask);
            var tasksChecklist1 = await checklistTaskService.GetAllTasksByChecklistIdAsync("Checklist 1");
            var tasksChecklist2 = await checklistTaskService.GetAllTasksByChecklistIdAsync("Checklist 2");
            var tasks = await checklistTaskService.GetAllTasksAsync();

            //Assert
            Assert.NotEqual(tasksChecklist1, tasksChecklist2);
            Assert.Equal(tasks.Count(), 11);
        }

        [Fact]
        public async void ChecklistTaskService_AddChecklistTaskToChecklist_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistUtilities = new ChecklistUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);
            var checklistService = new ChecklistService(dbContext, checklistUtilities);

            var taskToChecklist = new ChecklistTaskAddTaskToChecklistDto
            {
                Id = "Task 0",
                ChecklistId = "Checklist 1"
            };

            //Act
            await checklistTaskService.AddTaskToChecklistAsync(taskToChecklist);
            var checklist = await checklistService.GetChecklistByIdAsync(taskToChecklist.ChecklistId);

            //Assert
            Assert.Equal(checklist.ChecklistTasks.Count(), 1);
        }

        [Fact]
        public async void ChecklistTaskService_DeleteChecklistTask_ReturnsVoid()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Task");
            var checklistTaskUtilities = new ChecklistTaskUtilities();
            var checklistTaskService = new ChecklistTaskService(dbContext, checklistTaskUtilities);

            var id = "Task 0";

            //Act
            await checklistTaskService.DeleteChecklistTaskAsync(id);
            var checklistTask = await checklistTaskService.GetChecklistTaskByIdAsync(id);
            var checklistTasks = await checklistTaskService.GetAllTasksAsync();

            //Assert
            Assert.Equal(checklistTask, null);
            Assert.Equal(checklistTasks.Count(), 9);
        }
    }
}