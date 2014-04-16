﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVaultModule
    {
        void create();

        object read(int id);

        void update(int id);

        void delete();
    }
}
