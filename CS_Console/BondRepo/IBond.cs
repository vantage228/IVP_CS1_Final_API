using BondConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondConsoleApp.Repository
{
    public interface IBond
    {
        public Task<string> ImportDataFromCsv(Stream csvstream);

        public Task<string> UpdateBondData(EditBondModel ebm);

        public Task<string> DeleteBondData(int SecurityID);

        public Task<List<EditBondModel>> GetBondsData();
    }
}
