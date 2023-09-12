using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class ChecklistTests
    {
        private readonly IChecklistUtilities _checklistUtilities;

        public ChecklistTests()
        {
            _checklistUtilities = new ChecklistUtilities();
        }

        [Fact]
        public void checklistExists_exists_True()
        {
            List<ChecklistMultipleResponseDto> checklists = new List<ChecklistMultipleResponseDto>();

            var user = new User{
                Id = "userID",
                CreatedDate = DateTime.Now,
                Status = UserStatus.Active
            };

            var list1 = new ChecklistMultipleResponseDto{
                Title = "Checklist 1",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            var list2 = new ChecklistMultipleResponseDto{
                Title = "Checklist 2",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            var list3 = new ChecklistMultipleResponseDto{
                Title = "Checklist 3",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            
            checklists.Add(list1);
            checklists.Add(list2);
            checklists.Add(list3);

            var list4 = new ChecklistMultipleResponseDto{
                Title = "Checklist 3",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };

            Assert.True(_checklistUtilities.checklistExists(checklists, user.Id, list4.Title));
        }

        [Fact]
        public void checklistExists_notExists_False()
        {
            List<ChecklistMultipleResponseDto> checklists = new List<ChecklistMultipleResponseDto>();

            var user = new User{
                Id = "userID",
                CreatedDate = DateTime.Now,
                Status = UserStatus.Active
            };

            var list1 = new ChecklistMultipleResponseDto{
                Title = "Checklist 1",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            var list2 = new ChecklistMultipleResponseDto{
                Title = "Checklist 2",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            var list3 = new ChecklistMultipleResponseDto{
                Title = "Checklist 3",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };
            
            checklists.Add(list1);
            checklists.Add(list2);
            checklists.Add(list3);

            var list4 = new ChecklistResponseDto{
                Title = "Checklist 4",
                Status = "Active",
                CreatedDate = DateTime.Now,
                User = user
            };

            Assert.False(_checklistUtilities.checklistExists(checklists, user.Id, list4.Title));
        }

    }
}