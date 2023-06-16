namespace FinancialDataAnalysis.Models
{
    public class Asset
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal CloseAdjusted { get; set; }
        public long Volume { get; set; }
        public decimal SplitCoefficient { get; set; }

        public Asset(string line)
        {
            var items = line.Split(',');
            Symbol = items[0];
            Date = Convert.ToDateTime(items[1]);
            Open = Convert.ToDecimal(items[2]);
            High = Convert.ToDecimal(items[3]);
            Low = Convert.ToDecimal(items[4]);
            Close = Convert.ToDecimal(items[5]);
            CloseAdjusted = Convert.ToDecimal(items[6]);
            Volume = Convert.ToInt64(items[7]);
            SplitCoefficient = Convert.ToDecimal(items[8]);
        }

        public override string ToString()
        {
            return $"{Symbol} at {Date:M/dd/yyyy} | Open=>{Open}, High=>{High}," +
                $" Low=>{Low}, Close=>{Close}, CloseAdjusted=>{CloseAdjusted}," +
                $" Volume=>{Volume}, SplitCoefficient=>{SplitCoefficient}";
        }
    }
}
