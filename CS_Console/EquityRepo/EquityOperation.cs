using CS_Console.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using BondConsoleApp.Models;

namespace CS_Console.EquityRepo
{
    public class EquityOperation : IEquity
    {
        private string _connectionString;
        
        public EquityOperation(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public EquityOperation() { }

        public async Task ImportDataFromCsv(string filePath)
        {
            var records = await ReadCsvFile(filePath);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var record in records)
                        {
                            await InsertFullSecurityData(record, connection, transaction);
                        }                       
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during import: " + ex.Message);
                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        private async Task<List<EquityDataModel>> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return await Task.Run(() =>  csv.GetRecords<EquityDataModel>().ToList());
        }

        private async Task InsertFullSecurityData(EquityDataModel data, SqlConnection connection, SqlTransaction transaction)
        {
            using (SqlCommand command = new SqlCommand("InsertCompleteEquityData", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                //Security Summary Params
                command.Parameters.AddWithValue("@SecurityName", data.SecurityName);
                command.Parameters.AddWithValue("@SecurityDescription", data.SecurityDescription ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SecurityType", 1);  // Assuming this is an int for Equity
                command.Parameters.AddWithValue("@IsActive", data.IsActiveSecurity);
                command.Parameters.AddWithValue("@HasPosition", data.HasPosition);
                command.Parameters.AddWithValue("@RoundLotSize", data.LotSize);
                command.Parameters.AddWithValue("@BloombergUniqueName", data.BbgUniqueName ?? (object)DBNull.Value);

                // Identifier Params
                command.Parameters.AddWithValue("@CUSIP", data.Cusip ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ISIN", data.Isin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SEDOL", data.Sedol ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergTicker", data.BloombergTicker ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergUniqueID", data.BloombergUniqueId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergGlobalID", data.BbgGlobalId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TickerAndExchange", data.BbgTickerExchange ?? (object)DBNull.Value);

                // Equity Details Params
                command.Parameters.AddWithValue("@IsADR", data.IsAdr);
                command.Parameters.AddWithValue("@ADRUnderlyingTicker", data.AdrUnderlyingTicker ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ADRUnderlyingCurrency", data.AdrUnderlyingCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SharesPerADR", data.SharesPerAdr ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IPODate", data.IpoDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PriceCurrency", data.PricingCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SettleDays", data.SettleDays);
                command.Parameters.AddWithValue("@SharesOutstanding", data.SharesOutstanding ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@VotingRightsPerShare", data.VotingRightsPerShare ?? (object)DBNull.Value);

                // Risk Params
                command.Parameters.AddWithValue("@TwentyDayAverageVolume", data.AverageVolume20D ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Beta", data.Beta ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ShortInterest", data.ShortInterest ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@YTDReturn", data.ReturnYtd ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NinetyDayPriceVolatility", data.Volatility90D ?? (object)DBNull.Value);

                // Dividend History Params
                command.Parameters.AddWithValue("@DeclaredDate", data.DividendDeclaredDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ExDate", data.DividendExDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@RecordDate", data.DividendRecordDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PayDate", data.DividendPayDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DividendAmount", data.DividendAmount ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Frequency", data.Frequency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DividendType", data.DividendType ?? (object)DBNull.Value);

                // Regulatory Details Params
                command.Parameters.AddWithValue("@FormPFAssetClass", data.PfAssetClass ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFCountry", data.PfCountry ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFCreditRating", data.PfCreditRating ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFCurrency", data.PfCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFInstrument", data.PfInstrument ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFLiquidityProfile", data.PfLiquidityProfile ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFMaturity", data.PfMaturity ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFNAICSCode", data.PfNaicsCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFRegion", data.PfRegion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFSector", data.PfSector ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFSubAssetClass", data.PfSubAssetClass ?? (object)DBNull.Value);

                // Reference Data Params
                command.Parameters.AddWithValue("@IssueCountry", data.CountryOfIssuance ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Issuer", data.Issuer ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IssueCurrency", data.IssueCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergIndustrySubGroup", data.BbgIndustrySubGroup ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergIndustryGroup", data.BloombergIndustryGroup ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergIndustrySector", data.BloombergSector ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@RiskCurrency", data.RiskCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TradingCurrency", data.TradingCurrency ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Exchange", data.Exchange ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CountryOfIncorporation", data.CountryOfIncorporation ?? (object)DBNull.Value);

                // Pricing Details Params
                command.Parameters.AddWithValue("@OpenPrice", data.OpenPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ClosePrice", data.ClosePrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Volume", data.Volume ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastPrice", data.LastPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AskPrice", data.AskPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BidPrice", data.BidPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PERatio", data.PeRatio ?? (object)DBNull.Value);

                SqlParameter outputParam = new SqlParameter("@SecurityID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                // Execute the stored procedure
                await command.ExecuteNonQueryAsync();

                // Retrieve the output value if needed
                int securityID = (int)outputParam.Value;
                Console.WriteLine($"New Security ID: {securityID}");
            }   
        }

        public async Task<string> UpdateSecurityData(EditEquityModel esm)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("UpdateEquityData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set up parameters with values
                    command.Parameters.AddWithValue("@SecurityID", esm.SecurityID);
                    command.Parameters.AddWithValue("@SecurityDescription", esm.SecurityDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PricingCurrency", esm.PricingCurrency ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TotalSharesOutstanding", esm.TotalSharesOutstanding);
                    command.Parameters.AddWithValue("@OpenPrice", esm.OpenPrice);
                    command.Parameters.AddWithValue("@ClosePrice", esm.ClosePrice);
                    command.Parameters.AddWithValue("@DividendDeclaredDate", esm.DividendDeclaredDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FormPFCreditRating", esm.FormPFCreditRating ?? (object)DBNull.Value);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return "Updated Data";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public async Task<string> DeleteSecurityData(int securityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SoftDeleteEquitySecurity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SecurityID", securityId);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return $"Equity with Id - {securityId} marked as inactive";
                    }
                    catch (Exception ex)
                    {
                        return $"Error: {ex.Message}";
                    }
                }
            }
        }

        public async Task<List<EditEquityModel>> GetSecurityData()
        {
            var securityList = new List<EditEquityModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetEquityData", conn))  // Your stored procedure name
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var security = new EditEquityModel
                            {
                                SecurityID = reader["SecurityID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SecurityID"]),
                                SecurityName = reader["SecurityName"] == DBNull.Value ? string.Empty : reader["SecurityName"].ToString(),
                                SecurityDescription = reader["SecurityDescription"] == DBNull.Value ? string.Empty : reader["SecurityDescription"].ToString(),
                                PricingCurrency = reader["PricingCurrency"] == DBNull.Value ? string.Empty : reader["PricingCurrency"].ToString(),
                                TotalSharesOutstanding = reader["TotalSharesOutstanding"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalSharesOutstanding"]),
                                OpenPrice = reader["OpenPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["OpenPrice"]),
                                ClosePrice = reader["ClosePrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["ClosePrice"]),
                                DividendDeclaredDate = reader["DividendDeclaredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DividendDeclaredDate"]),
                                FormPFCreditRating = reader["PFCreditRating"] == DBNull.Value ? string.Empty : reader["PFCreditRating"].ToString(),
                                YTDReturn = reader["YTDReturn"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["YTDReturn"]),
                                IsActive = reader["IsActive"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsActive"])
                            };
                            securityList.Add(security);
                        }
                    }
                }
            }

            return securityList;
        }
    }
}
