# Dự án Bài 2: Quản Lý Sản Phẩm (Product Management) - ASP.NET Core MVC

Chào mừng bạn đến với dự án **Quản Lý Sản Phẩm** được xây dựng trên nền tảng **ASP.NET Core 8.0 MVC**. Đây là bài tập thực hành nâng cao tích hợp đầy đủ các chức năng CRUD, tải lên hình ảnh, xử lý nhiều ảnh phụ đi kèm, kiểm thực dữ liệu (Model Validation) và giao diện thiết kế chuẩn Premium, phản hồi linh hoạt với Bootstrap.

---

## 🚀 Tính Năng Chính

Dự án cung cấp một hệ thống quản lý sản phẩm hoàn chỉnh với các tính năng:

1. **Hiển thị danh sách sản phẩm (`Index`)**:
   - Giao diện dạng lưới (Grid) hiện đại kết hợp bảng biểu trực quan.
   - Hiển thị đầy đủ thông tin: Ảnh đại diện, Tên sản phẩm, Giá bán, Danh mục.
   - Hỗ trợ các nút thao tác nhanh (Xem chi tiết, Sửa, Xóa).

2. **Xem chi tiết sản phẩm (`Display`)**:
   - Giao diện chi tiết thiết kế cao cấp, trình bày hình ảnh sản phẩm nổi bật.
   - Hiển thị danh sách các ảnh phụ đi kèm (Carousel / Grid) sinh động.
   - Thể hiện đầy đủ mô tả chi tiết, giá tiền và phân loại danh mục sản phẩm.

3. **Thêm mới sản phẩm (`Add`)**:
   - Form nhập liệu thông minh với các trường: Tên, Giá, Mô tả, Danh mục.
   - Hỗ trợ tải lên **Ảnh đại diện** duy nhất và **Loạt ảnh phụ** (Multiple Images) cùng lúc.
   - Rà soát tính hợp lệ của tệp tải lên (Chỉ nhận file ảnh `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp` và giới hạn dung lượng `< 2MB` theo chuẩn tài liệu học tập).

4. **Cập nhật sản phẩm (`Update`)**:
   - Tự động điền dữ liệu cũ của sản phẩm cần sửa.
   - Cho phép thay đổi ảnh đại diện mới hoặc giữ nguyên ảnh cũ.
   - Cho phép cập nhật lại danh sách ảnh phụ đi kèm một cách trực quan.
   - Tự động xóa file ảnh cũ trên máy chủ khi người dùng thay thế bằng ảnh mới, tránh tồn đọng file rác.

5. **Xóa sản phẩm (`Delete`)**:
   - Trang xác nhận xóa an toàn để tránh thao tác nhầm lẫn.
   - Sau khi xác nhận xóa, hệ thống sẽ tự động dọn dẹp toàn bộ file ảnh liên quan của sản phẩm đó trong thư mục lưu trữ `wwwroot/images/`.

---

## 🛠️ Công Nghệ Sử Dụng

- **Backend**: ASP.NET Core 8.0 MVC C#
- **Frontend**: HTML5, CSS3, Bootstrap 5, Razor View Engine
- **Mẫu Thiết Kế (Design Pattern)**: Repository Pattern (`IProductRepository`, `MockProductRepository`, `ICategoryRepository`)
- **Quản lý dữ liệu**: Mock Data (Khởi tạo sẵn danh sách sản phẩm cao cấp gồm iPhone 17 Pro Max và MacBook Neo 13")

---

## 📂 Cấu Trúc Thư Mục Dự Án

```text
Bai2/
├── Controllers/
│   └── ProductController.cs       # Xử lý toàn bộ logic CRUD & Upload ảnh
├── Models/
│   ├── Product.cs                 # Định nghĩa Model sản phẩm & ràng buộc Validation
│   └── Category.cs                # Định nghĩa Model danh mục sản phẩm
├── Repositories/
│   ├── IProductRepository.cs      # Interface cho Product Repository
│   ├── MockProductRepository.cs   # Dữ liệu mẫu & triển khai các hàm xử lý sản phẩm
│   ├── ICategoryRepository.cs     # Interface quản lý Danh mục
│   └── MockCategoryRepository.cs  # Khởi tạo danh sách danh mục
├── Views/
│   ├── Home/
│   │   └── Index.cshtml           # Trang chủ giới thiệu
│   ├── Product/
│   │   ├── Index.cshtml           # Danh sách sản phẩm
│   │   ├── Add.cshtml             # Form thêm mới
│   │   ├── Update.cshtml          # Form chỉnh sửa
│   │   ├── Delete.cshtml          # Xác nhận xóa sản phẩm
│   │   └── Display.cshtml         # Chi tiết sản phẩm & ảnh phụ
│   └── Shared/
│       └── _Layout.cshtml         # Giao diện khung dùng chung (Navbar, Footer)
├── wwwroot/
│   ├── css/
│   │   └── site.css               # Phong cách CSS tùy chỉnh
│   └── images/                    # Nơi lưu trữ hình ảnh sản phẩm tải lên
└── Program.cs                     # Cấu hình DI và luồng Middleware của ứng dụng
```

---

## 💻 Hướng Dẫn Cài Đặt & Chạy Ứng Dụng

### Yêu cầu hệ thống:
- Cài đặt **.NET 8.0 SDK** trở lên.
- Cài đặt **Visual Studio 2022** hoặc **VS Code**.

### Các bước thực hiện:

1. **Khôi phục các Package Nuget**:
   ```bash
   dotnet restore
   ```

2. **Chạy ứng dụng bằng dòng lệnh**:
   ```bash
   dotnet run
   ```

3. **Truy cập ứng dụng**:
   Mở trình duyệt và truy cập theo đường dẫn hiển thị ở terminal (thông thường là `http://localhost:5000` hoặc `https://localhost:5001`).

---

## 📝 Giấy Phép & Tác Giả

Dự án được thực hiện bởi thành viên lớp lập trình web. Mọi thông tin đóng góp vui lòng liên hệ tác giả qua email hoặc mở Issue trên GitHub.
