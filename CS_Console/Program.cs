using CS_Console.Model;
using CS_Console.EquityRepo;

namespace CS_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = @"Server = 192.168.0.13\sqlexpress,49753; Database = IVP_O_S_CS; user Id = sa; Password = sa@12345678; TrustServerCertificate = True";
            //string csvFilePath = @"C:\Users\ossingh\Downloads\Data for securities.xlsx - Equities.csv";


            //IEquity _obj = new EquityOperation();
            //Controller obj = new Controller(_obj);

            //obj.ImportData(csvFilePath);
            //obj.DeleteData(1);
            //EditSecurityModel esmObj = new EditSecurityModel()
            //{
            //    securityId = 1,
            //    description = " International Business Machines Corp",   
            //    pricingCurrency = "USD",
            //    totalSharesOutstanding = 989660474,
            //    openPrice = 164.160000000m,
            //    closePrice = 164.160000000m,
            //    dividendDeclaredDate = new DateTime(2002, 09, 15),
            //    pfCreditRating = "AA+"
            //};
            //obj.UpdateData(esmObj);
            //List<EditEquityModel> equity = obj.GetData();
            //foreach (var equityItem in equity)
            //{
            //    Console.WriteLine($"{equityItem.securityId} - {equityItem.securityName} - {equityItem.description} - {equityItem.pricingCurrency} - {equityItem.totalSharesOutstanding} - {equityItem.openPrice} - {equityItem.closePrice} - {equityItem.dividendDeclaredDate} - {equityItem.pfCreditRating}");
            //}
        }
    }
}
