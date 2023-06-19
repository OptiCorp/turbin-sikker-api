using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {
        Checklist GetChecklistById(string id);
        void UpdateChecklist(string id, ChecklistEditDto checklist);
        string CreateChecklist(ChecklistCreateDto checklist);
        void DeleteChecklist(string id);
    }
}

