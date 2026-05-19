using Bai2.Models;
using System.Collections.Generic;

namespace Bai2.Repositories
{
    /// <summary>
    /// Lớp triển khai của ICategoryRepository sử dụng dữ liệu giả lập (Mock Data) trong bộ nhớ
    /// </summary>
    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categoryList;

        /// <summary>
        /// Hàm khởi tạo, nạp danh sách danh mục mẫu ban đầu
        /// </summary>
        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Điện thoại" }
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách danh mục sản phẩm hiện có
        /// </summary>
        /// <returns>Danh sách danh mục sản phẩm</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}
