using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Configuration;
using turbin.sikker.core.Model;

namespace turbin.sikker.core
{
    public class TurbinSikkerDbContext : DbContext
    {
        public TurbinSikkerDbContext(DbContextOptions<TurbinSikkerDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<ChecklistTask> Checklist_Task { get; set; }

        public DbSet<Checklist> Checklist { get; set; }

        public DbSet<Upload> Upload { get; set; }

        public DbSet<Punch> Punch { get; set; }

        public DbSet<ChecklistWorkflow> ChecklistWorkflow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ICollection<ChecklistTask> checklistTasksFireSafety = new Collection<ChecklistTask>();
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "a03a6435-0f77-48fa-bcf0-0e2c688ccbd9", CategoryId = "6fcbc433-e1bb-4ec5-ae41-18fd06e5b22f", Description = "Check and maintain emergency lighting and exit signs."});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});
            // checklistTasksFireSafety.Add(new ChecklistTask {Id = "", CategoryId = "", Description = ""});

            // ICollection<ChecklistTask> checklistTasksSafety = new Collection<ChecklistTask>();
            // ICollection<ChecklistTask> checklistTasksTurbineOperations = new Collection<ChecklistTask>();
            // ICollection<ChecklistTask> checklistTasksRigging = new Collection<ChecklistTask>();
            
            

            // modelBuilder.Entity<Category>().HasData(
            //     new Category {Id = "7b12d03c-2f32-4a63-9b01-6ed163f7e339", Name = "Rigging"},
            //     new Category {Id = "9e45e2a5-a6d8-47ef-9515-b3fbd3f00dd6", Name = "Safety"},
            //     new Category {Id = "a8220c29-786a-4be2-a384-9dfe27e3e576", Name = "Maintenance"},
            //     new Category {Id = "871c214d-7bde-497c-9d21-53a5d5426647", Name = "Equipment"},
            //     new Category {Id = "6fcbc433-e1bb-4ec5-ae41-18fd06e5b22f", Name = "Storage"},
            //     new Category {Id = "270746cd-61bb-49fb-93e2-21382f54831c", Name = "Emergency Systems"},
            //     new Category {Id = "25438ee0-57a4-4ce5-9701-7fe56747703a", Name = "Training"},
            //     new Category {Id = "d6effdeb-a277-430a-89c7-cf3ce61f78e3", Name = "Structures"},
            //     new Category {Id = "7d00b295-f04f-438f-a6d3-70a15dbddef7", Name = "Planning"},
            //     new Category {Id = "5d0290f3-0b92-4f90-ae31-89b03ee99326", Name = "Risk Assessment"},
            //     new Category {Id = "1490c5e6-97ea-4f61-ac91-dda882ca3cca", Name = "Signage"},
            //     new Category {Id = "3b5b055b-bac8-4963-86a1-85937fb85eee", Name = "Protocols"},
            //     new Category {Id = "c71675f2-4d1d-466b-ba38-d04ba4af88a6", Name = "Environmental"},
            //     new Category {Id = "f61459c1-466f-4333-85b1-0988a0d43788", Name = "Incident Management"},
            //     new Category {Id = "a1be4bb0-efe0-4120-a3d4-cb7cea8b054a", Name = "Monitoring"},
            //     new Category {Id = "1d523340-b16e-403a-a6c0-fc61d6341bc8", Name = "Fluid Analysis"},
            //     new Category {Id = "1b973d61-15cc-4914-a9d3-4a92fb1d23c8", Name = "Compliance"},
            //     new Category {Id = "c71b0906-1298-47c4-9609-0d49a396fedf", Name = "Analysis"},
            //     new Category {Id = "7b72b6df-e047-4a2b-b61a-526766962b9d", Name = "Procedures"},
            //     new Category {Id = "0209efdc-690f-4588-afb9-3edef8a675a4", Name = "Calculations"}
            // );

            // modelBuilder.Entity<Checklist>().HasData(
            //     new Checklist { Id = "eaf990fb-fd0d-4abc-9772-952c5bf3c598", Title = "Fire Safety", Status=ChecklistStatus.Active, CreatedDate = DateTime.Now, CreatedBy = "ad50ebc3-260a-4798-bc7b-7dd62cfb817e" },
            //     new Checklist { Id = "75efa0db-f690-4ce8-9625-7a98ef5e0ceb", Title = "Safety", Status=ChecklistStatus.Active, CreatedDate = DateTime.Now, CreatedBy = "ad50ebc3-260a-4798-bc7b-7dd62cfb817e" },
            //     new Checklist { Id = "ce219906-faf2-438d-938c-f905687c6bd8", Title = "Turbine Operations", Status=ChecklistStatus.Active, CreatedDate = DateTime.Now, CreatedBy = "ad50ebc3-260a-4798-bc7b-7dd62cfb817e" },
            //     new Checklist { Id = "e5d4cc94-c84f-48d4-8fcd-10baa6e3ad8f", Title = "Rigging", Status=ChecklistStatus.Active, CreatedDate = DateTime.Now, CreatedBy = "ad50ebc3-260a-4798-bc7b-7dd62cfb817e" }
            // );

            

            // Call the Configure method from ModelConfigurations class
            UserConfigurations.Configure(modelBuilder);
            ChecklistConfigurations.Configure(modelBuilder);
            TaskConfigurations.Configure(modelBuilder);
            PunchConfigurations.Configure(modelBuilder);
        }

    }
}
