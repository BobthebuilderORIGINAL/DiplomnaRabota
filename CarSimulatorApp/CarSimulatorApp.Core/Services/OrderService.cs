using Microsoft.EntityFrameworkCore.Query.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarSimulatorApp.Core.Contracts;
using CarSimulatorApp.Infrastructure.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;
using CarSimulatorApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CarSimulatorApp.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        public bool Create(int productId, string userId, int quantity)
        {
            var product = _context.Products
                .FirstOrDefault(x => x.Id == productId);

            if (product == null) return false;

            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            product.Quantity -= quantity;

            _context.Products.Update(product);
            _context.Orders.Add(item);

            return _context.SaveChanges() != 0;
        }

        public Order GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders
                .Include(x => x.Product)
                .Include(x => x.User)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }


        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .Include(x => x.User)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public bool RemoveById(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool Update(int orderId, int productId, string userId, int quantity)
        {
            throw new NotImplementedException();
        }
        public bool UserHasOrders(string userId)
        {
            return _context.Orders.Any(o => o.UserId == userId);
        }
    }
}
