using FinancialDataAnalysis.Dto;

namespace FinancialDataAnalysis.Models
{
    public interface IDbContext
    {
        int AssetCount();
        List<string> GetAssetNames();
        List<Asset> GetAssetsByName(string name);
        MainResponse GetAssetVolitility(string assetName, DateTime startDate, DateTime endDate);
        MainResponse GetAssetCorrelation(string assetName, string asset2Name, DateTime startDate, DateTime endDate);
        MainResponse GetAssetReturns(string assetName, decimal investedAmount, DateTime startDate, DateTime endDate);
        string GetMaxDate();
        string GetMinDate();
    }
}
