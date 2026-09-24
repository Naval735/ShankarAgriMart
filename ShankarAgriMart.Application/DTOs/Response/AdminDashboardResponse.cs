namespace ShankarAgriMart.Application.DTOs.Response;

public class AdminDashboardResponse
{
    public int TotalProducts { get; set; }

    public int TotalCustomers { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalRevenue { get; set; }

    public int PendingOrders { get; set; }

    public int ConfirmedOrders { get; set; }

    public int ShippedOrders { get; set; }

    public int DeliveredOrders { get; set; }

    public int CancelledOrders { get; set; }
}