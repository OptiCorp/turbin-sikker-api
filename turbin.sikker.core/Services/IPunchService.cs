﻿using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Task<IEnumerable<PunchResponseDto>> GetAllPunches();
        Task<PunchResponseDto> GetPunchById(string id);
        Task UpdatePunch(string punchId, PunchUpdateDto punch);
        Task<string> CreatePunch(PunchCreateDto punch);
        Task DeletePunch(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByWorkflowId(string checklistId);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByInspectorId(string id);
        Task<IEnumerable<PunchResponseDto>> GetPunchesByLeaderId(string id);
        // string GetPunchStatus(PunchStatus status);
        // bool IsValidStatus(string value);
        // public string GetPunchSeverity(PunchSeverity status);
    }
}