namespace FinancialDataAnalysis.Dto
{
    public class MainResponse
    {
        public MainResponse(decimal volitility, GraphData graphData)
        {
            Value=Math.Round(volitility, 2);
            GraphData=graphData;
        }

        public decimal Value { get; set; }
        public GraphData GraphData { get; set; }
    }
}
