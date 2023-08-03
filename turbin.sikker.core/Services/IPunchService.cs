﻿using System;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IPunchService
    {
        Punch GetPunchById(string id);
        void UpdatePunch(string punchId, PunchUpdateDto punch);
        string CreatePunch(PunchCreateDto punch);
        void DeletePunch(string id);
        string GetPunchStatus(PunchStatus status);
    }
}

