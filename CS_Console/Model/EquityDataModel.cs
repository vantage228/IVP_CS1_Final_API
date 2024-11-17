using CsvHelper.Configuration.Attributes;
using System;

namespace CS_Console.Model
{
    public class EquityDataModel
    {
        // Security Summary
        [Name("Security Name")]
        public string SecurityName { get; set; }

        [Name("Security Description")]
        public string SecurityDescription { get; set; }

        [Name("Has Position")]
        public bool? HasPosition { get; set; }

        [Name("Is Active Security")]
        public bool? IsActiveSecurity { get; set; }

        [Name("Lot Size")]
        public int? LotSize { get; set; }

        [Name("BBG Unique Name")]
        public string? BbgUniqueName { get; set; }

        // Security Identifier
        [Name("CUSIP")]
        public string? Cusip { get; set; }

        [Name("ISIN")]
        public string? Isin { get; set; }

        [Name("SEDOL")]
        public string? Sedol { get; set; }

        [Name("Bloomberg Ticker")]
        public string? BloombergTicker { get; set; }

        [Name("Bloomberg Unique ID")]
        public string BloombergUniqueId { get; set; }

        [Name("BBG Global ID")]
        public string? BbgGlobalId { get; set; }

        [Name("Ticker and Exchange")]
        public string? BbgTickerExchange { get; set; }

        // Security Details
        [Name("Is ADR Flag")]
        public bool? IsAdr { get; set; }

        [Name("ADR Underlying Ticker")]
        public string? AdrUnderlyingTicker { get; set; }

        [Name("ADR Underlying Currency")]
        public string? AdrUnderlyingCurrency { get; set; }

        [Name("Shares Per ADR")]
        public float? SharesPerAdr { get; set; }

        [Name("IPO Date")]
        public DateTime? IpoDate { get; set; }

        [Name("Pricing Currency")]
        public string? PricingCurrency { get; set; }

        [Name("Settle Days")]
        public int? SettleDays { get; set; }

        [Name("Total Shares Outstanding")]
        public decimal? SharesOutstanding { get; set; }

        [Name("Voting Rights Per Share")]
        public decimal? VotingRightsPerShare { get; set; }

        // Risk
        [Name("Average Volume - 20D")]
        public decimal? AverageVolume20D { get; set; }

        [Name("Beta")]
        public decimal? Beta { get; set; }

        [Name("Short Interest")]
        public decimal? ShortInterest { get; set; }

        [Name("Return - YTD")]
        public decimal? ReturnYtd { get; set; }

        [Name("Volatility - 90D")]
        public decimal? Volatility90D { get; set; }

        // Regulatory Details
        [Name("PF Asset Class")]
        public string? PfAssetClass { get; set; }

        [Name("PF Country")]
        public string? PfCountry { get; set; }

        [Name("PF Credit Rating")]
        public string? PfCreditRating { get; set; }

        [Name("PF Currency")]
        public string? PfCurrency { get; set; }

        [Name("PF Instrument")]
        public string? PfInstrument { get; set; }

        [Name("PF Liquidity Profile")]
        public string? PfLiquidityProfile { get; set; }

        [Name("PF Maturity")]
        public string? PfMaturity { get; set; }

        [Name("PF NAICS Code")]
        public string? PfNaicsCode { get; set; }

        [Name("PF Region")]
        public string? PfRegion { get; set; }

        [Name("PF Sector")]
        public string? PfSector { get; set; }

        [Name("PF Sub Asset Class")]
        public string? PfSubAssetClass { get; set; }

        // Reference Data
        [Name("Country of Issuance")]
        public string? CountryOfIssuance { get; set; }

        [Name("Exchange")]
        public string? Exchange { get; set; }

        [Name("Issuer")]
        public string? Issuer { get; set; }

        [Name("Issue Currency")]
        public string? IssueCurrency { get; set; }

        [Name("Trading Currency")]
        public string? TradingCurrency { get; set; }

        [Name("BBG Industry Sub Group")]
        public string? BbgIndustrySubGroup { get; set; }

        [Name("Bloomberg Industry Group")]
        public string? BloombergIndustryGroup { get; set; }

        [Name("Bloomberg Sector")]
        public string? BloombergSector { get; set; }

        [Name("Country of Incorporation")]
        public string? CountryOfIncorporation { get; set; }

        [Name("Risk Currency")]
        public string? RiskCurrency { get; set; }

        // Pricing Details
        [Name("Open Price")]
        public decimal? OpenPrice { get; set; }

        [Name("Close Price")]
        public decimal? ClosePrice { get; set; }

        [Name("Volume")]
        public decimal? Volume { get; set; }

        [Name("Last Price")]
        public decimal? LastPrice { get; set; }

        [Name("Ask Price")]
        public decimal? AskPrice { get; set; }

        [Name("Bid Price")]
        public decimal? BidPrice { get; set; }

        [Name("PE Ratio")]
        public decimal? PeRatio { get; set; }

        // Dividend History
        [Name("Dividend Declared Date")]
        public DateTime? DividendDeclaredDate { get; set; }

        [Name("Dividend Ex Date")]
        public DateTime? DividendExDate { get; set; }

        [Name("Dividend Record Date ")]
        public DateTime? DividendRecordDate { get; set; }

        [Name("Dividend Pay Date")]
        public DateTime? DividendPayDate { get; set; }

        [Name("Dividend Amount")]
        public decimal? DividendAmount { get; set; }

        [Name("Frequency")]
        public string? Frequency { get; set; }

        [Name("Dividend Type")]
        public string? DividendType { get; set; }
    }
}
