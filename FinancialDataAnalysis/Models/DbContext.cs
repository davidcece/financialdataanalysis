using FinancialDataAnalysis.Dto;

namespace FinancialDataAnalysis.Models
{
    public class DbContext : IDbContext
    {
        private List<Asset> _assets;

        public DbContext()
        {
            string stockPricesFile = "stock_prices_latest.csv";
            string[] lines = File.ReadAllLines(stockPricesFile);
            init(lines);
        }

        public DbContext(string[] lines)
        {
            init(lines);
        }

        private void init(string[] lines)
        {
            lines=lines.Skip(1).ToArray();
            _assets = new List<Asset>();

            foreach (var line in lines)
            {
                Asset asset = new Asset(line.Trim());
                _assets.Add(asset);
            }
        }

        public int AssetCount()
        {
            return _assets.Count;
        }


        public List<string> GetAssetNames()
        {
            return _assets.Select(x => x.Symbol).Distinct().ToList();
        }

        public List<Asset> GetAssetsByName(string name)
        {
            return _assets.FindAll(x => x.Symbol == name);
        }

        public MainResponse GetAssetVolitility(string assetName, DateTime startDate, DateTime endDate)
        {
            // v = std * sqrt(n)
            var selectedAssets = _assets.Where(a => a.Symbol==assetName)
                                        .Where(a => a.Date>=startDate)
                                        .Where(a => a.Date<=endDate)
                                        .OrderBy(a => a.Date);

            var closingAdjacents = selectedAssets.Select(x => x.CloseAdjusted).ToList();
            int n = closingAdjacents.Count;
            var stdDev = GetStdDev(closingAdjacents);
            var volitility = stdDev * (decimal)Math.Sqrt(n);
            var graphData = GetGraphData(selectedAssets);

            return new MainResponse(volitility, graphData);
        }

        public MainResponse GetAssetCorrelation(string assetName, string asset2Name, DateTime startDate, DateTime endDate)
        {
            // v = std * sqrt(n)
            var selectedAssets = _assets.Where(a => a.Symbol==assetName || a.Symbol==asset2Name)
                                        .Where(a => a.Date>=startDate)
                                        .Where(a => a.Date<=endDate)
                                        .OrderBy(a => a.Date);

            var values1 = selectedAssets.Where(a => a.Symbol==assetName).Select(a => a.CloseAdjusted).ToList();
            var values2 = selectedAssets.Where(a => a.Symbol==asset2Name).Select(a => a.CloseAdjusted).ToList();
            var corelation = GetCorrelations(values1, values2);
            var graphData = GetGraphData(selectedAssets);

            return new MainResponse(corelation, graphData);
        }

        public MainResponse GetAssetReturns(string assetName, decimal investedAmount, DateTime startDate, DateTime endDate)
        {
            // v = std * sqrt(n)
            var selectedAssets = _assets.Where(a => a.Symbol==assetName)
                                        .Where(a => a.Date>=startDate)
                                        .Where(a => a.Date<=endDate)
                                        .OrderBy(a => a.Date);

            var openingPrice = selectedAssets.First().CloseAdjusted;
            var closingPrice = selectedAssets.Last().CloseAdjusted;
            var numberOfStocks = investedAmount/openingPrice;
            var sellPrice = numberOfStocks * closingPrice;

            var diff = sellPrice - investedAmount;
            var graphData = GetGraphData(selectedAssets);

            return new MainResponse(diff, graphData);
        }



        public string GetMaxDate()
        {
            return _assets.Max(a => a.Date).Date.ToString("yyyy-MM-dd");
        }

        public string GetMinDate()
        {
            return _assets.Min(a => a.Date).Date.ToString("yyyy-MM-dd");
        }


        private GraphData GetGraphData(IOrderedEnumerable<Asset> assets)
        {
            List<string> dates = assets.Select(a => a.Date.ToString("yyyy-MM-dd")).ToList();

            Dictionary<string, List<decimal>> assetData = new Dictionary<string, List<decimal>>();
            foreach (var asset in assets)
            {
                if (!assetData.ContainsKey(asset.Symbol))
                    assetData[asset.Symbol] = new List<decimal>();
                assetData[asset.Symbol].Add(asset.CloseAdjusted);
            }

            var graphData = new GraphData(dates, assetData);
            return graphData;
        }

        public static decimal GetStdDev(List<decimal> items)
        {
            decimal mean = items.Average();
            decimal sumDeviations = items.Sum(x => (x-mean)*(x-mean));
            decimal varience = sumDeviations/items.Count;
            return (decimal)Math.Sqrt((double)varience);
        }


        public static decimal GetCorrelations(List<decimal> values1, List<decimal> values2)
        {
            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => (x - avg1) * (x - avg1));
            var sumSqr2 = values2.Sum(y => (y - avg2) * (y - avg2));
            var sum2 = (double)(sumSqr1 * sumSqr2);

            var result = sum1 / (decimal)Math.Sqrt(sum2);

            return result;
        }


    }
}
