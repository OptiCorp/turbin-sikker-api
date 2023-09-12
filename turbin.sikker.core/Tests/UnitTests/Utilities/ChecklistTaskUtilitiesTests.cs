using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class ChecklistTaskTests
    {
        private readonly IChecklistTaskUtilities _checklistTaskUtilities;

        public ChecklistTaskTests()
        {
            _checklistTaskUtilities = new ChecklistTaskUtilities();
        }

        [Fact]
        public void ChecklistTaskExists_exists_True()
        {
            List<ChecklistTaskResponseDto> checklistTasks = new List<ChecklistTaskResponseDto>();

            var category = new Category{
                Id = "Category ID",
            };

            var task1 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 1",
                Category = category
            };
            var task2 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 2",
                Category = category
            };
            var task3 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 3",
                Category = category
            };
            
            checklistTasks.Add(task1);
            checklistTasks.Add(task2);
            checklistTasks.Add(task3);

            var list4 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 1",
                Category = category
            };

            Assert.True(_checklistTaskUtilities.TaskExists(checklistTasks, list4.Category.Id, list4.Description));
        }

        [Fact]
        public void ChecklistTaskExists_notExists_False()
        {
            List<ChecklistTaskResponseDto> checklistTasks = new List<ChecklistTaskResponseDto>();

            var category = new Category{
                Id = "Category ID",
            };

            var task1 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 1",
                Category = category
            };
            var task2 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 1",
                Category = category
            };
            var task3 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 1",
                Category = category
            };
            
            checklistTasks.Add(task1);
            checklistTasks.Add(task2);
            checklistTasks.Add(task3);

            var list4 = new ChecklistTaskResponseDto{
                Description = "ChecklistTask 4",
                Category = category
            };

            Assert.False(_checklistTaskUtilities.TaskExists(checklistTasks, list4.Category.Id, list4.Description));
        }
    }
}