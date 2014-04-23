﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface ILoginModule
    {
        Boolean login(String username, String password);

        void logout();
    }
}
