using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bai2.Models;
using Bai2.Repositories;
using System.IO;

namespace Bai2.Controllers
{
    /// <summary>
    /// Controller quản lý các hoạt động CRUD cho sản phẩm (Product)
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Khởi tạo ProductController với các dịch vụ được Dependency Injection nạp vào
        /// </summary>
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Hiển thị danh sách tất cả sản phẩm
        /// </summary>
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết của một sản phẩm dựa trên Id
        /// </summary>
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Hiển thị form thêm mới sản phẩm (GET)
        /// </summary>
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Xử lý dữ liệu khi người dùng gửi form thêm mới sản phẩm (POST)
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần thêm</param>
        /// <param name="ImageUrl">Tệp tin ảnh đại diện được tải lên</param>
        /// <param name="ImageUrls">Danh sách các tệp tin ảnh khác được tải lên</param>
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            // Bỏ qua xác thực ModelState cho các thuộc tính ảnh đại diện và danh sách ảnh phụ
            // để tránh xung đột Model Binding của ASP.NET Core
            ModelState.Remove("ImageUrl");
            ModelState.Remove("ImageUrls");

            // --- RÀ SOÁT & KIỂM TRA LỖI FILE (Theo lưu ý trang 40 của giáo trình) ---
            if (imageUrl != null)
            {
                if (!IsValidImage(imageUrl, out string error))
                {
                    ModelState.AddModelError("ImageUrl", error);
                }
            }

            if (imageUrls != null && imageUrls.Count > 0)
            {
                foreach (var file in imageUrls)
                {
                    if (!IsValidImage(file, out string error))
                    {
                        ModelState.AddModelError("ImageUrls", error);
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lưu hình ảnh đại diện nếu có
                    if (imageUrl != null)
                    {
                        product.ImageUrl = await SaveImage(imageUrl);
                    }

                    // Lưu các hình ảnh liên quan khác nếu có
                    if (imageUrls != null && imageUrls.Count > 0)
                    {
                        product.ImageUrls = new List<string>();
                        foreach (var file in imageUrls)
                        {
                            product.ImageUrls.Add(await SaveImage(file));
                        }
                    }

                    _productRepository.Add(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể lưu hình ảnh lên máy chủ. Chi tiết lỗi: " + ex.Message);
                }
            }

            // Nạp lại danh sách danh mục nếu dữ liệu không hợp lệ
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        /// <summary>
        /// Hiển thị form cập nhật thông tin sản phẩm (GET)
        /// </summary>
        /// <param name="id">Id của sản phẩm cần cập nhật</param>
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        /// <summary>
        /// Xử lý dữ liệu khi người dùng gửi form cập nhật thông tin sản phẩm (POST)
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm chứa thông tin cập nhật</param>
        /// <param name="ImageUrl">Tệp tin ảnh đại diện mới tải lên (nếu muốn thay đổi)</param>
        /// <param name="ImageUrls">Danh sách các tệp tin ảnh mới khác tải lên (nếu muốn thay đổi)</param>
        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            // Bỏ qua xác thực ModelState cho các thuộc tính ảnh để tránh lỗi binding
            ModelState.Remove("ImageUrl");
            ModelState.Remove("ImageUrls");

            // --- RÀ SOÁT & KIỂM TRA LỖI FILE (Theo lưu ý trang 40 của giáo trình) ---
            if (imageUrl != null)
            {
                if (!IsValidImage(imageUrl, out string error))
                {
                    ModelState.AddModelError("ImageUrl", error);
                }
            }

            if (imageUrls != null && imageUrls.Count > 0)
            {
                foreach (var file in imageUrls)
                {
                    if (!IsValidImage(file, out string error))
                    {
                        ModelState.AddModelError("ImageUrls", error);
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _productRepository.GetById(product.Id);
                    if (existingProduct != null)
                    {
                        // Cập nhật ảnh đại diện: nếu tải lên ảnh mới thì lưu ảnh mới, ngược lại giữ nguyên ảnh cũ
                        if (imageUrl != null)
                        {
                            // Xóa file ảnh đại diện cũ trên đĩa để tránh rác
                            if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                            {
                                DeleteOldImageFile(existingProduct.ImageUrl);
                            }
                            product.ImageUrl = await SaveImage(imageUrl);
                        }
                        else
                        {
                            product.ImageUrl = existingProduct.ImageUrl;
                        }

                        // Cập nhật các ảnh khác: nếu tải lên loạt ảnh mới thì lưu mới, ngược lại giữ nguyên ảnh cũ
                        if (imageUrls != null && imageUrls.Count > 0)
                        {
                            // Xóa các file ảnh phụ cũ trên đĩa
                            if (existingProduct.ImageUrls != null)
                            {
                                foreach (var oldUrl in existingProduct.ImageUrls)
                                {
                                    DeleteOldImageFile(oldUrl);
                                }
                            }

                            product.ImageUrls = new List<string>();
                            foreach (var file in imageUrls)
                            {
                                product.ImageUrls.Add(await SaveImage(file));
                            }
                        }
                        else
                        {
                            product.ImageUrls = existingProduct.ImageUrls;
                        }
                    }

                    _productRepository.Update(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể lưu hình ảnh cập nhật lên máy chủ. Chi tiết lỗi: " + ex.Message);
                }
            }

            // Nạp lại danh sách danh mục nếu dữ liệu cập nhật không hợp lệ
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        /// <summary>
        /// Hiển thị trang xác nhận xóa sản phẩm (GET)
        /// </summary>
        /// <param name="id">Id của sản phẩm cần xóa</param>
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Xử lý xóa sản phẩm sau khi người dùng xác nhận (POST)
        /// </summary>
        /// <param name="id">Id của sản phẩm cần xóa</param>
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                // Xóa ảnh đại diện chính khỏi đĩa
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    DeleteOldImageFile(product.ImageUrl);
                }
                // Xóa danh sách ảnh phụ khỏi đĩa
                if (product.ImageUrls != null)
                {
                    foreach (var imgUrl in product.ImageUrls)
                    {
                        DeleteOldImageFile(imgUrl);
                    }
                }
                _productRepository.Delete(id);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hàm phụ trợ lưu hình ảnh vào thư mục wwwroot/images của dự án
        /// </summary>
        /// <param name="image">Đối tượng IFormFile đại diện cho ảnh được tải lên</param>
        /// <returns>Đường dẫn tương đối đến tệp ảnh đã lưu</returns>
        private async Task<string> SaveImage(IFormFile image)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            
            var savePath = Path.Combine(webRootPath, "images");
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            
            // Sử dụng GUID để đảm bảo tên tệp tin là duy nhất, tránh ghi đè dữ liệu
            var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
            var filePath = Path.Combine(savePath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của tệp hình ảnh tải lên (Định dạng & Dung lượng)
        /// Theo lưu ý trang 40 của giáo trình học tập.
        /// </summary>
        /// <param name="file">Tệp tin cần kiểm tra</param>
        /// <param name="errorMessage">Thông báo lỗi trả về nếu tệp không hợp lệ</param>
        /// <returns>True nếu hợp lệ, ngược lại trả về False</returns>
        private bool IsValidImage(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (file == null) return true;

            // 1. Kiểm tra phần mở rộng (Định dạng ảnh hợp lệ)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                errorMessage = $"Định dạng tệp '{file.FileName}' không hợp lệ. Chỉ chấp nhận tệp hình ảnh (.jpg, .jpeg, .png, .gif, .webp).";
                return false;
            }

            // 2. Kiểm tra dung lượng tệp tin (Tối đa 2MB)
            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
            {
                errorMessage = $"Dung lượng tệp '{file.FileName}' vượt quá mức giới hạn cho phép (Tối đa 2MB).";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Xóa file ảnh cũ khỏi đĩa để tránh tồn đọng file rác (Tránh trùng lặp hình ảnh)
        /// </summary>
        private void DeleteOldImageFile(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath)) return;
                
                var fileName = Path.GetFileName(relativePath);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                // Bỏ qua lỗi nếu không xóa được (file đang bị lock, v.v.)
            }
        }
    }
}

// Da hoan thanh cau hinh toan bo chuc nang CRUD nang cao cho ProductController theo giao trinh hoc tap.

