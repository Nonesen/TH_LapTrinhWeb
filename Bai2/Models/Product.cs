using System.ComponentModel.DataAnnotations;

namespace Bai2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        // Mở rộng cho tính năng upload ảnh
        public string? ImageUrl { get; set; } // Đường dẫn ảnh đại diện
        public List<string>? ImageUrls { get; set; } // Danh sách các ảnh khác
    }
}
