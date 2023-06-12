using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
	public class ChecklistTaskService : IChecklistTaskService
	{
        public readonly TurbinSikkerDbContext _context;

        public ChecklistTaskService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public ChecklistTask GetChecklistTaskById(string id)
        {
            return _context.Checklist_Task.FirstOrDefault(checklistTask => checklistTask.Id == id);
            
        }

        public void CreateChecklistTask(ChecklistTask checklistTask)
        {
            _context.Checklist_Task.Add(checklistTask);
            _context.SaveChanges();
        }

        public void UpdateChecklistTask(ChecklistTask updatedChecklistTask)
        {
            var checklistTask = _context.Checklist_Task.FirstOrDefault(checklistTask => checklistTask.Id == updatedChecklistTask.Id);

            if (checklistTask != null)
            {
                checklistTask.CategoryId = updatedChecklistTask.CategoryId;

                checklistTask.Description = updatedChecklistTask.Description;

                _context.SaveChanges();
            }
        }

        public void DeleteChecklistTask(string id)
        {

            var checklistTask = _context.Checklist_Task.FirstOrDefault(checklistTask => checklistTask.Id == id);
            if (checklistTask != null)
            {
                _context.Checklist_Task.Remove(checklistTask);
                _context.SaveChanges();
            }
        }
    }
}

