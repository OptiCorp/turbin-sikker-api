using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class PunchService : IPunchService
    {
        private readonly TurbinSikkerDbContext _context;
        private readonly IPunchUtilities _punchUtilities;

        public PunchService(TurbinSikkerDbContext context, IPunchUtilities punchUtilities)
        {
            _context = context;
            _punchUtilities = punchUtilities;
        }

        public async Task<IEnumerable<PunchResponseDto>> GetAllPunches()
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(p => _punchUtilities.PunchToResponseDto(p))
                            .ToListAsync();
        }

        public async Task<PunchResponseDto> GetPunchById(string id)
        {
            var punch = await _context.Punch
                                .Include(p => p.ChecklistTask)
                                .Include(u => u.Uploads)
                                .Include(p => p.CreatedByUser)
                                .ThenInclude(u => u.UserRole)
                                .FirstOrDefaultAsync(p => p.Id == id);
            
            PunchResponseDto punchResponse = _punchUtilities.PunchToResponseDto(punch);

            return punchResponse;
        }


        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByLeaderId(string id)
        {   
            var allPunches = new List<PunchResponseDto>();
            var workflows = await _context.ChecklistWorkflow.Where(c => c.CreatedById == id).ToListAsync();
            foreach (ChecklistWorkflow workflow in workflows) {
                var punches = await _context.Punch
                                    .Where(c => c.ChecklistWorkflowId == workflow.Id)
                                    .Include(p => p.ChecklistTask)
                                    .Include(u => u.Uploads)
                                    .Include(p => p.CreatedByUser)
                                    .ThenInclude(u => u.UserRole)
                                    .OrderByDescending(c => c.CreatedDate)
                                    .Select(c => _punchUtilities.PunchToResponseDto(c))
                                    .ToListAsync();
                allPunches.AddRange(punches);
            }
            return allPunches;
        }


        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByInspectorId(string id)
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .Where(c => c.CreatedBy == id)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(c => _punchUtilities.PunchToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowId(string id)
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .Where(c => c.ChecklistWorkflowId == id)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(c => _punchUtilities.PunchToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<string> CreatePunch(PunchCreateDto punchDto)
        {

            var punch = new Punch
            {
                PunchDescription = punchDto.PunchDescription,
                CreatedBy = punchDto.CreatedBy,
                ChecklistWorkflowId = punchDto.ChecklistWorkflowId,
                ChecklistTaskId = punchDto.ChecklistTaskId,
                CreatedDate = DateTime.Now,
                Severity = Enum.Parse<PunchSeverity>(punchDto.Severity),
                Status = PunchStatus.Pending
            };

            _context.Punch.Add(punch);
            await _context.SaveChangesAsync();

            string newPunchId = punch.Id;

            return newPunchId;
        }

        public async Task UpdatePunch(PunchUpdateDto updatedPunch)
        {
            var punch = await _context.Punch.FirstOrDefaultAsync(u => u.Id == updatedPunch.Id);

            if (punch != null)
            {
                //punch.Active = updatedPunch.Active;
                if (updatedPunch.ChecklistWorkflowId != null)
                {
                    punch.ChecklistWorkflowId = updatedPunch.ChecklistWorkflowId;
                }

                if (updatedPunch.PunchDescription != null)
                {
                    punch.PunchDescription = updatedPunch.PunchDescription;
                }

                if (updatedPunch.Status != null)
                {

                    string status = updatedPunch.Status.ToLower();
                    switch (status)
                    {
                        case "pending":
                            punch.Status = PunchStatus.Pending;
                            break;
                        case "approved":
                            punch.Status = PunchStatus.Approved;
                            break;
                        case "rejected":
                            punch.Status = PunchStatus.Rejected;
                            break;
                        default:
                            break;
                    }
                }

                if (updatedPunch.Severity != null)
                {
                    string severity = updatedPunch.Severity.ToLower();

                     switch (severity)
                    {
                        case "minor":
                            punch.Severity = PunchSeverity.Minor;
                            break;
                        case "major":
                            punch.Severity = PunchSeverity.Major;
                            break;
                        case "critical":
                            punch.Severity = PunchSeverity.Critical;
                            break;
                        default:
                            break;
                    }
                }

                punch.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePunch(string id)
        {
            var punch = await _context.Punch.FirstOrDefaultAsync(u => u.Id == id);

            if (punch != null)
            {
                _context.Punch.Remove(punch);
                await _context.SaveChangesAsync();
            }
        }

    }

}