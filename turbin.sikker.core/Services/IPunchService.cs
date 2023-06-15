using System;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Punch GetPunchById(string id);
        void UpdatePunch(Punch upload);
        void CreatePunch(Punch upload);
        void DeletePunch(string id);
    }
}

