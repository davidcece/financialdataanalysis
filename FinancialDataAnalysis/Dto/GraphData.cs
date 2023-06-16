namespace FinancialDataAnalysis.Dto
{
    public class GraphData
    {
        public List<string> Dates { get; set; }
        public Dictionary<string, List<decimal>> Data { get; set; }

        public GraphData(List<string> dates, Dictionary<string, List<decimal>> data)
        {
            Dates = dates;
            Data = data;
        }
    }
}
