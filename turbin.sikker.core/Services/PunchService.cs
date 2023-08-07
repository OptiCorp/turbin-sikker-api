using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public class PunchService : IPunchService
    {
        private readonly TurbinSikkerDbContext _context;

        public PunchService(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "pending" || lowerCaseValue == "approved" || lowerCaseValue == "rejected";
        }


        public string GetPunchStatus(PunchStatus status)
        {
            switch (status)
            {
                case PunchStatus.Pending:
                    return "Pending";
                case PunchStatus.Approved:
                    return "Approved";
                case PunchStatus.Rejected:
                    return "Rejected";
                default:
                    return "Pending";
            }
        }

        public Punch GetPunchById(string id)
        {
            return _context.Punch.Include(p => p.CreatedByUser).ThenInclude(u => u.UserRole)
                                    .FirstOrDefault(p => p.Id == id);
        }

        public string CreatePunch(PunchCreateDto punchDto)
        {
            var punch = new Punch
            {
                PunchDescription = punchDto.PunchDescrption,
                CreatedBy = punchDto.CreatedBy,
                ChecklistId = punchDto.ChecklistId,
                //UserId = punchDto.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Punch.Add(punch);
            _context.SaveChanges();

            string newPunchId = punch.Id;

            return newPunchId;
        }

        public void UpdatePunch(string punchId, PunchUpdateDto updatedPunch)
        {
            var punch = _context.Punch.FirstOrDefault(u => u.Id == punchId);

            if (punch != null)
            {
                //punch.Active = updatedPunch.Active;
                punch.ChecklistId = updatedPunch.ChecklistId;
                punch.PunchDescription = updatedPunch.PunchDescription;
                if (updatedPunch.Status != null)
                {

                    string status = updatedPunch.Status.ToLower();

                    if (status == "pending")
                    {
                        punch.Status = PunchStatus.Pending;
                    }
                    if (status == "approved")
                    {
                        punch.Status = PunchStatus.Approved;
                    }
                    if (status == "rejected")
                    {
                        punch.Status = PunchStatus.Rejected;
                    }
                }
                punch.Severity = updatedPunch.Severity;
                //punch.UserId = updatedPunch.UserId;

                punch.UpdatedDate = DateTime.Now;
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