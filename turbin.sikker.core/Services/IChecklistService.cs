using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {
        Checklist GetChecklistById(string id);
        IEnumerable<Checklist> GetAllChecklists();
        IEnumerable<ChecklistViewNoUserDto> GetAllChecklistsByUserId(string userId);
        void UpdateChecklist(string id, ChecklistEditDto checklist);
        string CreateChecklist(ChecklistCreateDto checklist);
        void DeleteChecklist(string id);
    }
}

