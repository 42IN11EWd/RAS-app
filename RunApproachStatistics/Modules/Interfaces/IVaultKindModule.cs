using RunApproachStatistics.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules.Interfaces
{
    interface IVaultKindModule
    {
        void deleteVaultKind(int id);
        ObservableCollection<vaultkind> readVaultKinds();
        void createVaultKind(vaultkind vaultKind);
        void updateVaultKind(vaultkind vaultKind);
    }
}
