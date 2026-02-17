using AlterdataFinanceApi.Application.DTOs.Dashboard;

namespace AlterdataFinanceApi.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync(int year);
}
