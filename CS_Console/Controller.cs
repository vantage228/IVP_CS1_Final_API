using CS_Console.Model;
using CS_Console.EquityRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Console
{
    public class Controller
    {
        IEquity _security;
        public Controller(IEquity security)
        {
            _security = security;
        }

        public void ImportData(string filePath)
        {
            _security.ImportDataFromCsv(filePath);
        }

        public void DeleteData(int id)
        {
            _security.DeleteSecurityData(id);
        }

        public void UpdateData(EditEquityModel model)
        {
            _security.UpdateSecurityData(model);
        }

        //public List<EditEquityModel> GetData()
        //{
        //    return _security.GetSecurityData();
        //}
    }
}
