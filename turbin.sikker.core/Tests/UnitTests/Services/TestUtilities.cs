using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Tests.Services
{
    public class TestUtilities
    {
        public async Task<TurbinSikkerDbContext> GetDbContext(string testType)
        {
            var options = new DbContextOptionsBuilder<TurbinSikkerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new TurbinSikkerDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (testType == "Category")
            {
                if (await databaseContext.Category.CountAsync() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await databaseContext.Category.AddAsync(
                            new Category
                            {
                                Id = string.Format("Category {0}", i),
                                Name = string.Format("Category {0}", i)
                            }
                        );
                    }
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

            await databaseContext.Category.AddRangeAsync(
                new Category
                {
                    Id = "Category 1",
                    Name = "Category 1"
                },
                new Category
                {
                    Id = "Category 2",
                    Name = "Category 2"
                }
            );

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

            if (testType == "Checklist")
            {
                if (await databaseContext.Checklist.CountAsync() <= 0)
                {
                    string createdById = "";
                    for (int i = 0; i < 10; i++)
                    {
                        if (i%2 == 0) createdById = "User 1";
                        if (i%2 != 0) createdById = "User 2";
                        await databaseContext.Checklist.AddAsync(
                            new Checklist
                            {
                                Id = string.Format("Checklist {0}", i),
                                Title = string.Format("Checklist {0}", i),
                                Status = ChecklistStatus.Active,
                                CreatedDate = DateTime.Now,
                                CreatedBy = createdById
                            }
                        );
                    }
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

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

            if (testType == "Workflow")
            {
                if (await databaseContext.ChecklistWorkflow.CountAsync() <= 0)
                {
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
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

            if (testType == "Task")
            {
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
                                Id = string.Format("Task {0}", i),
                                CategoryId = categoryId,
                                Description = string.Format("Task {0}", i)
                            }
                        );
                    }
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

            await databaseContext.Checklist_Task.AddRangeAsync(
                new ChecklistTask
                {
                    Id = "Task 1",
                    CategoryId = "Category 1",
                    Description = "Task 1"
                },
                new ChecklistTask
                {
                    Id = "Task 2",
                    CategoryId = "Category 1",
                    Description = "Task 2"
                }
            );

            await databaseContext.ChecklistWorkflow.AddRangeAsync(
                    new ChecklistWorkflow
                    {
                        Id = "Workflow 1",
                        ChecklistId = "Checklist 1",
                        UserId = "User 1",
                        CreatedById = "User 2",
                        Status = CurrentChecklistStatus.Sent,
                        CreatedDate = DateTime.Now
                    },
                    new ChecklistWorkflow
                    {
                        Id = "Workflow 2",
                        ChecklistId = "Checklist 2",
                        UserId = "User 3",
                        CreatedById = "User 2",
                        Status = CurrentChecklistStatus.Sent,
                        CreatedDate = DateTime.Now
                    }
            );

            if (testType == "Punch")
            {
                if (await databaseContext.Punch.CountAsync() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await databaseContext.Punch.AddAsync(
                            new Punch
                            {
                                Id = string.Format("Punch {0}", i),
                                ChecklistWorkflowId = string.Format("Workflow {0}", (i%2)+1),
                                ChecklistTaskId = string.Format("Task {0}", (i%2)+1),
                                CreatedDate = DateTime.Now,
                                CreatedBy = i%2==0 ? "User 1" : "User 3",
                                PunchDescription = string.Format("Punch {0}", i),
                                Severity = PunchSeverity.Minor,
                                Status = PunchStatus.Pending,
                                Active = 1
                            }
                        );
                    }
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

            await databaseContext.Punch.AddRangeAsync(
                new Punch
                {
                    Id = "Punch 1",
                    ChecklistWorkflowId = "Workflow 1",
                    ChecklistTaskId = "Task 1",
                    CreatedDate = DateTime.Now,
                    CreatedBy = "User 1",
                    PunchDescription = "Punch 1",
                    Severity = PunchSeverity.Minor,
                    Status = PunchStatus.Pending,
                    Active = 1
                },
                new Punch
                {
                    Id = "Punch 2",
                    ChecklistWorkflowId = "Workflow 2",
                    ChecklistTaskId = "Task 2",
                    CreatedDate = DateTime.Now,
                    CreatedBy = "User 1",
                    PunchDescription = "Punch 2",
                    Severity = PunchSeverity.Minor,
                    Status = PunchStatus.Pending,
                    Active = 1
                }
            );

            if (testType == "Upload")
            {
                if (await databaseContext.Upload.CountAsync() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await databaseContext.Upload.AddAsync(
                            new Upload
                            {
                                Id = string.Format("Upload {0}", i),
                                PunchId = i%2==0 ? "Punch 1" : "Punch 2",
                                BlobRef = string.Format("Upload {0}", i)
                            }
                        );
                    }
                    await databaseContext.SaveChangesAsync();
                }
                return databaseContext;
            }

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }
    }   
}