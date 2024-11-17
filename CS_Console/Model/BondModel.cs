using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondConsoleApp.Models
{
    public class BondModel
    {
        [Name("Security Description")]
        public string SecurityDescription { get; set; }

        [Name("Security Name")]
        public string SecurityName { get; set; }

        [Name("Asset Type")]
        public string AssetType { get; set; }

        [Name("Investment Type")]
        public string InvestmentType { get; set; }

        [Name("Trading Factor")]
        public decimal TradingFactor { get; set; }

        [Name("Pricing Factor")]
        public decimal PricingFactor { get; set; }

        //public bool? IsActive { get; set; } = true;

        // SecurityIdentifier fields
        [Name("ISIN")]
        public string ISIN { get; set; }

        [Name("BBG Ticker")]
        public string BloombergTicker { get; set; }

        [Name("BBG Unique ID")]
        public string BloombergUniqueID { get; set; }

        [Name("CUSIP")]
        public string CUSIP { get; set; }

        [Name("SEDOL")]
        public string Sedol { get; set; }

        // SecurityDetails fields
        [Name("First Coupon Date")]
        public DateTime? FirstCouponDate { get; set; }

        [Name("Cap")]
        public decimal? CouponCap { get; set; }

        [Name("Floor")]
        public decimal? CouponFloor { get; set; }

        [Name("Coupon Frequency")]
        public int CouponFrequency { get; set; }

        [Name("Coupon")]
        public decimal Coupon { get; set; }

        [Name("Coupon Type")]
        public string CouponType { get; set; }

        [Name("Spread")]
        public decimal? Spread { get; set; }

        [Name("Callable Flag")]
        public bool? IsCallable { get; set; }

        [Name("Fix to Float Flag")]
        public bool? IsFixToFloat { get; set; }

        [Name("Putable Flag")]
        public bool? IsPutable { get; set; }

        [Name("Issue Date")]
        public DateTime IssueDate { get; set; }

        [Name("Last Reset Date")]
        public DateTime? LastResetDate { get; set; }

        [Name("Maturity")]
        public DateTime MaturityDate { get; set; }

        [Name("Call Notification Max Days")]
        public decimal? MaximumCallNoticeDays { get; set; }

        [Name("Put Notification Max Days")]
        public decimal? MaximumPutNoticeDays { get; set; }

        [Name("Penultimate Coupon Date")]
        public DateTime? PenultimateCouponDate { get; set; }

        [Name("Reset Frequency")]
        public string ResetFrequency { get; set; }

        [Name("Has Position")]
        public bool HasPosition { get; set; }

        // Risk fields
        [Name("Macaulay Duration")]
        public decimal? Duration { get; set; }

        [Name("30D Volatility")]
        public decimal? Volatility30D { get; set; }

        [Name("90D Volatility")]
        public decimal? Volatility90D { get; set; }

        [Name("Convexity")]
        public decimal? Convexity { get; set; }

        [Name("30Day Average Volume")]
        public decimal? AverageVolume30D { get; set; }

        // RegulatoryDetails fields
        [Name("PF Asset Class")]
        public string FormPFAssetClass { get; set; }

        [Name("PF Country")]
        public string FormPFCountry { get; set; }

        [Name("PF Credit Rating")]
        public string FormPFCreditRating { get; set; }

        [Name("PF Currency")]
        public string FormPFCurrency { get; set; }

        [Name("PF Instrument")]
        public string FormPFInstrument { get; set; }

        [Name("PF Liquidity Profile")]
        public string FormPFLiquidityProfile { get; set; }

        [Name("PF Maturity")]
        public DateTime? FormPFMaturity { get; set; }

        [Name("PF NAICS Code")]
        public string FormPFNAICSCode { get; set; }

        [Name("PF Region")]
        public string FormPFRegion { get; set; }

        [Name("PF Sector")]
        public string FormPFSector { get; set; }

        [Name("PF Sub Asset Class")]
        public string FormPFSubAssetClass { get; set; }

        // ReferenceData fields
        [Name("Bloomberg Industry Group")]
        public string BloombergIndustryGroup { get; set; }

        [Name("Bloomberg Industry Sub Group")]
        public string BloombergIndustrySubGroup { get; set; }

        [Name("Bloomberg Industry Sector")]
        public string BloombergSector { get; set; }

        [Name("Country of Issuance")]
        public string IssueCountry { get; set; }

        [Name("Issue Currency")]
        public string IssueCurrency { get; set; }

        [Name("Issuer")]
        public string Issuer { get; set; }

        [Name("Risk Currency")]
        public string RiskCurrency { get; set; }

        // PutSchedule fields
        [Name("Put Date")]
        public DateTime? PutDate { get; set; }

        [Name("Put Price")]
        public decimal? PutPrice { get; set; }

        // PricingAndAnalytics fields
        [Name("Ask Price")]
        public decimal? AskPrice { get; set; }

        [Name("High Price")]
        public decimal? HighPrice { get; set; }

        [Name("Low Price")]
        public decimal? LowPrice { get; set; }

        [Name("Open Price")]
        public decimal? OpenPrice { get; set; }

        [Name("Volume")]
        public decimal? Volume { get; set; }

        [Name("Bid Price")]
        public decimal? BidPrice { get; set; }

        [Name("Last Price")]
        public decimal? LastPrice { get; set; }

        // CallSchedule fields
        [Name("Call Date")]
        public DateTime? CallDate { get; set; }

        [Name("Call Price")]
        public decimal? CallPrice { get; set; }
    }
}
