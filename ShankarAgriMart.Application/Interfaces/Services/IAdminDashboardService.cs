using ShankarAgriMart.Application.DTOs.Response;

namespace ShankarAgriMart.Application.Interfaces.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardResponse> GetDashboardAsync();
}