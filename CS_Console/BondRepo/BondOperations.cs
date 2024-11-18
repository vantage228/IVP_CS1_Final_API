using BondConsoleApp.Models;
using CS_Console.Model;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondConsoleApp.Repository
{
    public class BondOperations : IBond
    {
        private string _connectionString;

        public BondOperations(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task<string> ImportDataFromCsv(Stream csvStream)
        {
            var records = await ReadCsvFile(csvStream);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var record in records)
                        {
                            await InsertFullBondData(record, connection, transaction);
                        }

                        await transaction.CommitAsync();
                        return("Data imported successfully.");
                    }
                    catch (DbException ex)
                    {
                        return ("Database Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return ("Error during import: " + ex.Message);
                    }
                }
            }
        }

        private async Task<List<BondModel>> ReadCsvFile(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return await Task.Run(() => csv.GetRecords<BondModel>().ToList());
        }

        private async Task InsertFullBondData(BondModel data, SqlConnection connection, SqlTransaction transaction)
        {
            using (SqlCommand command = new SqlCommand("InsertCompleteBondData", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Security Summary Params
                // Security Summary Params
                command.Parameters.AddWithValue("@SecurityName", data.SecurityName);
                command.Parameters.AddWithValue("@SecurityDescription", data.SecurityDescription ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SecurityType", 2);
                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@HasPosition", data.HasPosition);
                command.Parameters.AddWithValue("@InvestmentType", data.InvestmentType ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TradingFactor", data.TradingFactor);
                command.Parameters.AddWithValue("@PricingFactor", data.PricingFactor);
                command.Parameters.AddWithValue("@AssetType", data.AssetType);

                // Identifier Params
                command.Parameters.AddWithValue("@CUSIP", data.CUSIP ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ISIN", data.ISIN ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergUniqueID", data.BloombergUniqueID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BloombergTicker", data.BloombergTicker ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SEDOL", data.Sedol ?? (object)DBNull.Value);

                // Bond Details Params
                command.Parameters.AddWithValue("@FirstCouponDate", data.FirstCouponDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CouponRate", data.Coupon);
                command.Parameters.AddWithValue("@CouponCap", data.CouponCap ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CouponFloor", data.CouponFloor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CouponFrequency", data.CouponFrequency);
                command.Parameters.AddWithValue("@CouponType", data.CouponType);
                command.Parameters.AddWithValue("@Spread", data.Spread);
                command.Parameters.AddWithValue("@IsCallable", data.IsCallable);
                command.Parameters.AddWithValue("@IsFixToFloat", data.IsFixToFloat);
                command.Parameters.AddWithValue("@IsPutable", data.IsPutable);
                command.Parameters.AddWithValue("@IssueDate", data.IssueDate);
                command.Parameters.AddWithValue("@LastResetDate", data.LastResetDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MaturityDate", data.MaturityDate);
                command.Parameters.AddWithValue("@MaximumCallNoticeDays", data.MaximumCallNoticeDays ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MaximumPutNoticeDays", data.MaximumPutNoticeDays ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PenultimateCouponDate", data.PenultimateCouponDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ResetFrequency", data.ResetFrequency);

                // Risk Params
                command.Parameters.AddWithValue("@Duration", data.Duration ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Volatility30D", data.Volatility30D ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Volatility90D", data.Volatility90D ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Convexity", data.Convexity ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AverageVolume30D", data.AverageVolume30D ?? (object)DBNull.Value);

                // Bond Put Schedule
                command.Parameters.AddWithValue("@PutDate", data.PutDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PutPrice", data.PutPrice ?? (object)DBNull.Value);

                // Bond Call Schedule
                command.Parameters.AddWithValue("@CallDate", data.CallDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CallPrice", data.CallPrice ?? (object)DBNull.Value);

                // Pricing Details
                command.Parameters.AddWithValue("@OpenPrice", data.OpenPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastPrice", data.LastPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AskPrice", data.AskPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BidPrice", data.BidPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Volume", data.Volume ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@HighPrice", data.HighPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LowPrice", data.LowPrice ?? (object)DBNull.Value);

                // Regulatory Details
                command.Parameters.AddWithValue("@FormPFAssetClass", data.FormPFAssetClass);
                command.Parameters.AddWithValue("@FormPFCountry", data.FormPFCountry);
                command.Parameters.AddWithValue("@FormPFCreditRating", data.FormPFCreditRating ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFCurrency", data.FormPFCurrency);
                command.Parameters.AddWithValue("@FormPFInstrument", data.FormPFInstrument);
                command.Parameters.AddWithValue("@FormPFLiquidityProfile", data.FormPFLiquidityProfile);
                command.Parameters.AddWithValue("@FormPFMaturity", data.FormPFMaturity ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFNAICSCode", data.FormPFNAICSCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFRegion", data.FormPFRegion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFSector", data.FormPFSector ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FormPFSubAssetClass", data.FormPFSubAssetClass ?? (object)DBNull.Value);

                // Reference Data
                command.Parameters.AddWithValue("@IssueCountry", data.IssueCountry);
                command.Parameters.AddWithValue("@Issuer", data.Issuer ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IssueCurrency", data.IssueCurrency);
                command.Parameters.AddWithValue("@BloombergIndustrySubGroup", data.BloombergIndustrySubGroup);
                command.Parameters.AddWithValue("@BloombergIndustryGroup", data.BloombergIndustryGroup);
                command.Parameters.AddWithValue("@RiskCurrency", data.RiskCurrency);

                // Execute the query
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



        public async Task<string> UpdateBondData(EditBondModel ebm)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("UpdateBondData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set up parameters with values
                    command.Parameters.AddWithValue("@SecurityID", ebm.SecurityID);
                    command.Parameters.AddWithValue("@SecurityDescription", ebm.SecurityDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CouponRate", ebm.Coupon);
                    command.Parameters.AddWithValue("@IsCallable", ebm.IsCallable);
                    command.Parameters.AddWithValue("@PenultimateCouponDate", ebm.PenultimateCouponDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FormPFCreditRating", ebm.FormPFCreditRating ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AskPrice", ebm.AskPrice);
                    command.Parameters.AddWithValue("@BidPrice", ebm.BidPrice);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return $"Updated Bond with Id - {ebm.SecurityID}";
                    }
                    catch(DbException ex)
                    {
                        return "DB Error: "+ex.Message; 
                    }
                    catch (Exception ex)
                    {
                        return "An error occurred while updating bond data: " + ex.Message;
                    }
                }
            }

        }

        public async Task<string> DeleteBondData(int SecurityID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SoftDeleteBondSecurity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SecurityID", SecurityID);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return $"Bond with Id - {SecurityID} marked as inactive";
                    }
                    catch (DbException ex)
                    {
                        return "DB Error: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        return $"Error: {ex.Message}";
                    }
                }
            }
        }

        public async Task<List<EditBondModel>> GetBondsData()
        {
            var bonds = new List<EditBondModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("GetBondData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var bond = new EditBondModel
                                {
                                    SecurityID = reader["SecurityID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SecurityID"]),
                                    SecurityName = reader["SecurityName"] == DBNull.Value ? string.Empty : reader["SecurityName"].ToString(),
                                    SecurityDescription = reader["SecurityDescription"] == DBNull.Value ? string.Empty : reader["SecurityDescription"].ToString(),
                                    Coupon = reader["CouponRate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CouponRate"]),
                                    IsCallable = reader["CallableFlag"] == DBNull.Value ? false : Convert.ToBoolean(reader["CallableFlag"]),
                                    MaturityDate = reader["Maturity"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Maturity"]),
                                    PenultimateCouponDate = reader["PenultimateCouponDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["PenultimateCouponDate"]),
                                    FormPFCreditRating = reader["PFCreditRating"] == DBNull.Value ? string.Empty : reader["PFCreditRating"].ToString(),
                                    AskPrice = reader["AskPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["AskPrice"]),
                                    BidPrice = reader["BidPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["BidPrice"]),
                                    IsActive = reader["IsActive"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(reader["IsActive"])
                                };

                                bonds.Add(bond);
                            }
                        }
                    }
                    catch (DbException ex)
                    {
                        throw new Exception("DB Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error: {ex.Message}");
                    }
                }
            }

            return bonds;
        }

    }

}