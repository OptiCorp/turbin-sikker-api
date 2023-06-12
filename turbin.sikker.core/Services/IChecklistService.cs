using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IChecklistService
    {
        Checklist GetChecklistById(string id);
        void UpdateChecklist(Checklist checklist);
        void CreateChecklist(Checklist checklist);
        void DeleteChecklist(string id);
    }
}

