using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
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

        // public bool IsValidStatus(string value)
        // {
        //     string lowerCaseValue = value.ToLower();
        //     return lowerCaseValue == "pending" || lowerCaseValue == "approved" || lowerCaseValue == "rejected";
        // }


        // public string GetPunchStatus(PunchStatus status)
        // {
        //     switch (status)
        //     {
        //         case PunchStatus.Pending:
        //             return "Pending";
        //         case PunchStatus.Approved:
        //             return "Approved";
        //         case PunchStatus.Rejected:
        //             return "Rejected";
        //         default:
        //             return "Pending";
        //     }
        // }

        // public string GetPunchSeverity(PunchSeverity status)
        // {
        //     switch (status)
        //     {
        //         case PunchSeverity.Minor:
        //             return "Minor";
        //         case PunchSeverity.Major:
        //             return "Major";
        //         case PunchSeverity.Critical:
        //             return "Critical";
        //         default:
        //             return "Critical";
        //     }
        // }

        public async Task<IEnumerable<PunchResponseDto>> GetAllPunches()
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(p => p.ChecklistWorkflow)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .Select(p => _punchUtilities.PunchToResponseDto(p))
                            .ToListAsync();
        }

        public async Task<PunchResponseDto> GetPunchById(string id)
        {
            var punch = await _context.Punch
                                .Include(p => p.ChecklistTask)
                                .Include(p => p.ChecklistWorkflow)
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
                                    .Include(p => p.ChecklistWorkflow)
                                    .Include(p => p.CreatedByUser)
                                    .ThenInclude(u => u.UserRole)
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
                            .Include(p => p.ChecklistWorkflow)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .Where(c => c.CreatedBy == id)
                            .Select(c => _punchUtilities.PunchToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowId(string id)
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(p => p.ChecklistWorkflow)
                            .Include(p => p.CreatedByUser)
                            .ThenInclude(u => u.UserRole)
                            .Where(c => c.ChecklistWorkflowId == id)
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
                Severity = Enum.Parse<PunchSeverity>(punchDto.Severity)
            };

            _context.Punch.Add(punch);
            await _context.SaveChangesAsync();

            string newPunchId = punch.Id;

            return newPunchId;
        }

        public async Task UpdatePunch(string punchId, PunchUpdateDto updatedPunch)
        {
            var punch = await _context.Punch.FirstOrDefaultAsync(u => u.Id == punchId);

            if (punch != null)
            {
                //punch.Active = updatedPunch.Active;
                punch.ChecklistWorkflowId = updatedPunch.ChecklistWorkflowId;
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

                if (updatedPunch.Severity != null)
                {
                    string severity = updatedPunch.Severity.ToLower();

                    if (severity == "minor")
                    {
                        punch.Severity = PunchSeverity.Minor;
                    }
                    if (severity == "major")
                    {
                        punch.Severity = PunchSeverity.Major;
                    }
                    if (severity == "critical")
                    {
                        punch.Severity = PunchSeverity.Critical;
                    }
                }

                //punch.UserId = updatedPunch.UserId;

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