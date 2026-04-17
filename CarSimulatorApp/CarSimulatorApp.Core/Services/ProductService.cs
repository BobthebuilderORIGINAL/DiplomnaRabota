using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarSimulatorApp.Core.Contracts;
using CarSimulatorApp.Data;
using CarSimulatorApp.Infrastructure.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;

namespace CarSimulatorApp.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description)
        {
            Product item = new Product
            {
                ProductName = name,
                Brand = _context.Brands.Find(brandId),
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount,
                Description = description
            };

            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public List<Product> GetProducts(
      string searchString,
      string searchStringCategoryName,
      string searchStringBrandName)
        {
            var products = _context.Products
                .AsQueryable();

            // ✅ PRODUCT NAME SEARCH
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.ProductName.ToLower()
                    .Contains(searchString.ToLower()));
            }

            // ✅ CATEGORY FILTER
            if (!string.IsNullOrEmpty(searchStringCategoryName))
            {
                products = products.Where(p =>
                    p.Category.CategoryName.ToLower()
                    .Contains(searchStringCategoryName.ToLower()));
            }

            // ✅ BRAND FILTER
            if (!string.IsNullOrEmpty(searchStringBrandName))
            {
                products = products.Where(p =>
                    p.Brand.BrandName.ToLower()
                    .Contains(searchStringBrandName.ToLower()));
            }

            return products.ToList();
        }

        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }
        public bool Update(int productId, string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description)
        {
            var product = GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            product.ProductName = name;
            product.Brand = _context.Brands.Find(brandId);
            product.Category = _context.Categories.Find(categoryId);
            product.Picture = picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;
            product.Description = description;

            // If your Product entity has BrandId and CategoryId properties, you could use:
            // product.BrandId = brandId;
            // product.CategoryId = categoryId;

            _context.Products.Update(product);
            return _context.SaveChanges() > 0;
        }

    }
}
