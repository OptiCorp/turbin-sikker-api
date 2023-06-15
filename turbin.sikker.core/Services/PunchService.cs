using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public class PunchService : IPunchService
    {
        private readonly TurbinSikkerDbContext _context;

        public PunchService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public Punch GetPunchById(string id)
        {
            return _context.Punch.FirstOrDefault(u => u.Id == id);
        }

        public void CreatePunch(Punch punch)
        {
            _context.Punch.Add(punch);
            _context.SaveChanges();
        }

        public void UpdatePunch(Punch updatedPunch)
        {
            var punch = _context.Punch.FirstOrDefault(u => u.Id == updatedPunch.Id);

            if (punch != null)
            {
                punch.Active = updatedPunch.Active;
                punch.FormId = updatedPunch.FormId;
                punch.PunchDescription = updatedPunch.PunchDescription;
                punch.PunchStatus = updatedPunch.PunchStatus;
                punch.Severity = updatedPunch.Severity;
                punch.UserId = updatedPunch.UserId;

                _context.SaveChanges();
            }
        }

        public void DeletePunch(string id)
        {
            var punch = _context.Punch.FirstOrDefault(u => u.Id == id);

            if (punch != null)
            {
                _context.Punch.Remove(punch);
                _context.SaveChanges();
            }
        }

    }

}