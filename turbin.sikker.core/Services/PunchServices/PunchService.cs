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

        public async Task<IEnumerable<PunchResponseDto>> GetAllPunchesAsync()
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.Creator)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(p => _punchUtilities.PunchToResponseDto(p))
                            .ToListAsync();
        }

        public async Task<PunchResponseDto> GetPunchByIdAsync(string id)
        {
            var punch = await _context.Punch
                                .Include(p => p.ChecklistTask)
                                .Include(u => u.Uploads)
                                .Include(p => p.Creator)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (punch == null) return null;

            return _punchUtilities.PunchToResponseDto(punch);
        }


        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByLeaderIdAsync(string id)
        {
            var allPunches = new List<PunchResponseDto>();
            var workflows = await _context.Workflow.Where(c => c.CreatorId == id).ToListAsync();
            foreach (Workflow workflow in workflows)
            {
                var punches = await _context.Punch
                                    .Where(c => c.WorkflowId == workflow.Id)
                                    .Include(p => p.ChecklistTask)
                                    .Include(u => u.Uploads)
                                    .Include(p => p.Creator)
                                    .OrderByDescending(c => c.CreatedDate)
                                    .Select(c => _punchUtilities.PunchToResponseDto(c))
                                    .ToListAsync();
                allPunches.AddRange(punches);
            }
            return allPunches;
        }


        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByInspectorIdAsync(string id)
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.Creator)
                            .Where(c => c.CreatorId == id)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(c => _punchUtilities.PunchToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowIdAsync(string id)
        {
            return await _context.Punch
                            .Include(p => p.ChecklistTask)
                            .Include(u => u.Uploads)
                            .Include(p => p.Creator)
                            .Where(c => c.WorkflowId == id)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(c => _punchUtilities.PunchToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<string> CreatePunchAsync(PunchCreateDto punchDto)
        {

            var punch = new Punch
            {
                Description = punchDto.Description,
                CreatorId = punchDto.CreatorId,
                WorkflowId = punchDto.WorkflowId,
                ChecklistTaskId = punchDto.ChecklistTaskId,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")),
                Severity = Enum.Parse<PunchSeverity>(punchDto.Severity),
                Status = PunchStatus.Pending
            };

            _context.Punch.Add(punch);
            await _context.SaveChangesAsync();

            return punch.Id;
        }

        public async Task UpdatePunchAsync(PunchUpdateDto updatedPunch)
        {
            var punch = await _context.Punch.FirstOrDefaultAsync(u => u.Id == updatedPunch.Id);

            if (punch != null)
            {
                //punch.Active = updatedPunch.Active;
                if (updatedPunch.WorkflowId != null)
                {
                    punch.WorkflowId = updatedPunch.WorkflowId;
                }

                if (updatedPunch.Description != null)
                {
                    punch.Description = updatedPunch.Description;
                }

                if (updatedPunch.Message != null)
                {
                    punch.Message = updatedPunch.Message;
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

                punch.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePunchAsync(string id)
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
