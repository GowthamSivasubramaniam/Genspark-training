public class DashboardAnalyticsDto
{
    // Customers
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }

    // Mechanics
    public int TotalMechanics { get; set; }
    public int ActiveMechanics { get; set; }

    // Bookings
    public int TotalBookings { get; set; }
    public int ActiveBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int ReviewedBookings { get; set; }

    // Bills
    public int TotalBills { get; set; }
    public int DispatchedBills { get; set; }
    public int ApprovedBills { get; set; }
    public int PaidBills { get; set; }

    // Services
    public int TotalServices { get; set; }
    public int ActiveServices { get; set; }
    public int CompletedServices { get; set; }
    public int AbortedServices { get; set; }

    // Customer phone -> total bills paid amount
    public Dictionary<string, float> CustomerBillSummary { get; set; } = new();

    // Stats per mechanic for service counts by status
    public List<MechanicServiceStatsDto> MechanicServiceStats { get; set; } = new();

    // Booking count grouped by date (for bar graph)
    public Dictionary<DateTime, int> BookingCountsByDate { get; set; } = new();

    // Service count grouped by date (for bar graph)
    public Dictionary<DateTime, int> ServiceCountsByDate { get; set; } = new();
}

public class MechanicServiceStatsDto
{
    public string MechanicPhone { get; set; } = string.Empty;
    public string MechanicName { get; set; } = string.Empty;
    public int Active { get; set; }
    public int Completed { get; set; }
    public int Aborted { get; set; }
}
