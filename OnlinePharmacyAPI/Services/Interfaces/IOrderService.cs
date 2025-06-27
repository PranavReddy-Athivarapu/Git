using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> orderItems);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task CancelOrderAsync(int orderId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}
