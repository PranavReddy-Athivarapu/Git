using OnlinePharmacyAppAPI.DTO;
using OnlinePharmacyAppAPI.Model;

namespace OnlinePharmacyAppAPI.Services
{
    public class CartService
    {
        private readonly OPADBContext _context;

        public CartService(OPADBContext context)
        {
            _context = context;
        }

        public bool AddToCart(CartDTO cartItem)
        {
            // Check if item already in cart for this user
            var existingCartItem = _context.Cart.FirstOrDefault(c =>
                c.UserId == cartItem.UserId && c.MedicineId == cartItem.MedicineId);

            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.StockQty = cartItem.StockQty;
                existingCartItem.Amount = existingCartItem.Price * existingCartItem.StockQty;
            }
            else
            {
                // Create new cart entry
                var newCart = new Cart
                {
                    UserId = cartItem.UserId,
                    MedicineId = cartItem.MedicineId,
                    MedName = cartItem.MedName,
                    Price = cartItem.Price,
                    StockQty = cartItem.StockQty,
                    Amount = cartItem.Price * cartItem.StockQty
                };

                _context.Cart.Add(newCart);
            }

            _context.SaveChanges();
            return true;
        }

        // Optional: get cart items for a user
        public List<CartDTO> GetCartByUser(int userId)
        {
            return _context.Cart
                .Where(c => c.UserId == userId)
                .Select(c => new CartDTO
                {
                    UserId = c.UserId,
                    MedicineId = c.MedicineId,
                    MedName = c.MedName,
                    Price = c.Price,
                    StockQty = c.StockQty,
                    Amount = c.Amount  // ✅ Include amount here
                })
                .ToList();
        }

        // Optional: clear cart after checkout
        public bool ClearCart(int userId)
        {
            var items = _context.Cart.Where(c => c.UserId == userId).ToList();
            _context.Cart.RemoveRange(items);
            _context.SaveChanges();
            return true;
        }
        public List<CartDTO> GetCartByUserId(int userId)
        {
            return _context.Cart
                .Where(c => c.UserId == userId)
                .Select(c => new CartDTO
                {
                    UserId = c.UserId,
                    MedicineId = c.MedicineId,
                    MedName = c.MedName,
                    Price = c.Price,
                    StockQty = c.StockQty,
                    Amount = c.Amount
                }).ToList();
        }

        public bool DeleteCartItem(int cartId)
        {
            var item = _context.Cart.FirstOrDefault(c => c.CartId == cartId);

            if (item == null)
                return false;

            _context.Cart.Remove(item);
            _context.SaveChanges();
            return true;
        }
        public decimal GetTotalAmount(int userId)
        {
            return _context.Cart
                .Where(c => c.UserId == userId)
                .Sum(c => c.Amount);
        }
        public CartSummaryDTO GetCartSummaryWithDiscount(int userId)
        {
            var cartItems = GetCartByUser(userId);
            decimal subTotal = cartItems.Sum(c => c.Amount ?? 0);

            // Fetch discount for the user (if any valid one)
            var discount = _context.Discounts
                .Where(d => d.UserId == userId
                    && d.StartDate <= DateTime.UtcNow
                    && (d.EndDate == null || d.EndDate >= DateTime.UtcNow))
                .OrderByDescending(d => d.StartDate) // prefer latest
                .FirstOrDefault();

            decimal discountAmount = 0;
            string code = "";

            if (discount != null)
            {
                if (discount.IsPercentage)
                    discountAmount = subTotal * (discount.Value / 100m);
                else
                    discountAmount = discount.Value;

                // Don't go below zero
                discountAmount = Math.Min(discountAmount, subTotal);
                code = discount.DiscountCode;
            }

            return new CartSummaryDTO
            {
                CartItems = cartItems,
                SubTotal = subTotal,
                DiscountAmount = discountAmount,
                FinalTotal = subTotal - discountAmount,
                AppliedDiscountCode = code
            };
        }


    }
}
