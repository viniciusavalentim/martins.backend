namespace Martins.Backend.Domain.Models
{
    public class DashboardModel
    {
        public KpiCardModel TotalRevenue { get; set; }
        public KpiCardModel TotalOrders { get; set; }
        public KpiCardModel TotalProfit { get; set; }
        public KpiCardModel TotalExpense { get; set; }
        public DailySalesChartModel DailySalesChart { get; set; }
        public PeriodSalesChartModel PeriodSalesChart { get; set; }
        public TopProductsChartModel TopSellingProductsChart { get; set; }
    }

    public class KpiCardModel
    {
        public decimal Value { get; set; }
        public double ChangePercentage { get; set; }
        public string ComparisonPeriod { get; set; } = "em relação ao mês passado";
    }

    public class DailySalesChartModel
    {
        public List<DailyDataPoint> DataPoints { get; set; } = new List<DailyDataPoint>();
    }

    public class DailyDataPoint
    {
        public string DateLabel { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
    }

    public class PeriodSalesChartModel
    {
        public string PeriodLabel { get; set; } = string.Empty;
        public double TrendPercentage { get; set; }
        public List<TimeDataPoint> DataPoints { get; set; } = new List<TimeDataPoint>();
    }

    public class TimeDataPoint
    {
        public string DateLabel { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class TopProductsChartModel
    {
        public List<ProductSegment> Segments { get; set; } = new List<ProductSegment>();
    }

    public class ProductSegment
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
