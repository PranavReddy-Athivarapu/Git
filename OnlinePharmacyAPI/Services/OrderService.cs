using OnlinePharmacyAppAPI.Models;
using OnlinePharmacyAppAPI.Services.Interfaces;

namespace OnlinePharmacyAppAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Medicine> _medicineRepository;
        private readonly IRepository<User> _userRepository;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<Medicine> medicineRepository,
            IRepository<User> userRepository)
        {
            _orderRepository = orderRepository;
            _medicineRepository = medicineRepository;
            _userRepository = userRepository;
        }

        public async Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> orderItems)
        {
            using var transaction = await _orderRepository.BeginTransactionAsync();

            try
            {
                // 1. Verify user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) throw new ArgumentException("User not found");

                // 2. Check medicine stock
                foreach (var item in orderItems)
                {
                    var medicine = await _medicineRepository.GetByIdAsync(item.MedicineId);
                    if (medicine == null || medicine.StockQuantity < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for {medicine?.Name}");
                }

                // 3. Create order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    IsFirstOrder = !user.Orders.Any()
                };

                // 4. Add order items
                foreach (var item in orderItems)
                {
                    var medicine = await _medicineRepository.GetByIdAsync(item.MedicineId);
                    order.OrderItems.Add(new OrderItem
                    {
                        MedicineId = item.MedicineId,
                        Quantity = item.Quantity,
                        UnitPrice = medicine.Price
                    });

                    // Deduct stock
                    medicine.StockQuantity -= item.Quantity;
                    await _medicineRepository.UpdateAsync(medicine);
                }

                // 5. Save
                await _orderRepository.AddAsync(order);
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
