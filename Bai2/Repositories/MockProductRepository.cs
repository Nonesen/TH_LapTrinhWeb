using Bai2.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bai2.Repositories
{
    /// <summary>
    /// Lớp triển khai của IProductRepository sử dụng dữ liệu giả lập (Mock Data) trong bộ nhớ
    /// </summary>
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        /// <summary>
        /// Hàm khởi tạo, nạp một số dữ liệu sản phẩm mẫu ban đầu
        /// </summary>
        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop MacBook Neo", Price = 1080, Description = "13 inch A18 Pro 8GB/256GB", CategoryId = 1 },
                new Product { Id = 2, Name = "iPhone 17 Pro Max", Price = 1200, Description = "Hệ điều hành: iOS 26 Chip xử lý (CPU): Apple A19 Pro 6 nhân Tốc độ CPU: 4.25 GHz", CategoryId = 2 }
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm hiện có
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        /// <summary>
        /// Lấy thông tin chi tiết một sản phẩm dựa trên Id
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>Đối tượng sản phẩm tìm được hoặc null nếu không tồn tại</returns>
        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Thêm mới một sản phẩm vào danh sách giả lập
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần thêm</param>
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        /// <summary>
        /// Cập nhật thông tin của một sản phẩm trong danh sách giả lập
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm đã thay đổi thông tin</param>
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        /// <summary>
        /// Xóa một sản phẩm ra khỏi danh sách dựa theo Id
        /// </summary>
        /// <param name="id">Id của sản phẩm cần xóa</param>
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
