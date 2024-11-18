using CS_Console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Console.EquityRepo
{
    public interface IEquity
    {
        public Task<string> ImportDataFromCsv(string filePath);

        public Task<string> UpdateSecurityData(EditEquityModel esm);

        public Task<string> DeleteSecurityData(int securityId);

        public Task<List<EditEquityModel>> GetSecurityData();

        public List<Dictionary<string, object>> GetSecurityDetailsByID(int securityID);
    }
}
