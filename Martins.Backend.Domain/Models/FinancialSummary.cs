namespace Martins.Backend.Domain.Models
{
    public class FinancialSummary
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public int TotalOrders { get; set; }
        public double AverageMargin { get; set; }
    }
}
