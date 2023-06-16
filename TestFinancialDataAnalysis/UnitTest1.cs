using FinancialDataAnalysis.Models;
using System.Runtime.InteropServices;

namespace TestFinancialDataAnalysis
{
    [TestFixture]
    public class Tests
    {

        private DbContext _dbContext;

        [SetUp] public void SetUp() 
        {
            string csv =
@"symbol,date,open,high,low,close,close_adjusted,volume,split_coefficient
MSFT,9/18/2001,53.41,55,53.17,54.32,18.0802,41591300,1
MSFT,1/16/2002,68.85,69.84,67.85,67.87,22.5902,30977700,1
MSFT,10/26/2007,36.01,36.03,34.56,35.03,27.2232,288121200,1
MSFT,5/16/2016,50.8,51.96,50.75,51.83,49.7013,20032017,1
EBAY,9/18/2001,53.41,55,53.17,54.32,18.0802,41591300,1
EBAY,1/16/2002,68.85,69.84,67.85,67.87,22.5902,30977700,1
EBAY,10/26/2007,36.01,36.03,34.56,35.03,27.2232,288121200,1
EBAY,5/16/2016,50.8,51.96,50.75,51.83,49.7013,20032017,1";
            string[] lines = csv.Split('\r');
            _dbContext = new DbContext(lines);
        }


        [Test]
        public void CreateAssetFromCsvShouldHave8Assets()
        {
            Assert.That(_dbContext.AssetCount(), Is.EqualTo(8));
        }

        [Test]
        public void AssetNamesInEBAYandMSFT()
        {
            List<string> assetNames = _dbContext.GetAssetNames();

            Assert.That(assetNames.Count, Is.EqualTo(2));
            Assert.IsTrue(assetNames.Contains("EBAY"));
            Assert.IsTrue(assetNames.Contains("MSFT"));
        }


        [Test]
        public void AssetGetAssertByNameReturns4Items()
        {
            var assets = _dbContext.GetAssetsByName("MSFT");
            Assert.That(assets.Count, Is.EqualTo(4));
        }

        [Test]
        public void CorrelationValueMustBeBetweenMinus1and1()
        {
            List<decimal> values1 = new() { 1, 2, 3, 4, 5, 6 };
            List<decimal> values2 = new() { 11, 12, 12, 13, 14, 15, 16 };

            var correlations = DbContext.GetCorrelations(values1, values2);
            Assert.That(correlations, Is.AtLeast(-1).And.AtMost(1));
        }

        [Test]
        public void StdDevOfSimilarITemsShouldBeZero()
        {
            List<decimal> values1 = new() { 5, 5, 5, 5 };

            var std = DbContext.GetStdDev(values1);
            Assert.That(std, Is.EqualTo(0));
        }







    }
}