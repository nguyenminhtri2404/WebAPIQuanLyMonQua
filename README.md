## 📌 Giới thiệu
APIQuanLyMonQua là một Web API được phát triển bằng **ASP.NET Core 6** nhằm quản lý các chức năng phát quà theo lịch, bảng xếp hạng người dùng

## 🚀 Công nghệ sử dụng
- **.NET 6 (ASP.NET Core Web API)**
- **Entity Framework Core** (Code-First, Fluent API)
- **Microsoft SQL Server** (Lưu trữ dữ liệu)
- **Fluent Validation** (Kiểm tra ràng buộc dữ liệu đầu vào)
- **Background job** (Đặt lịch thực hiện cho services)
- **AutoMapper** (Chuyển đổi dữ liệu giữa DTO và Model)
- **JWT Authentication** (Xác thực API)
- **Swagger UI** (Tài liệu API)
- **Repository Pattern** (Quản lý dữ liệu)


## 📚 Cấu trúc thư mục
```
GiftManagement/
│-- Controllers/         # Xử lý request và trả về response cho client
│-- Data/                # DbContext, DOmain Model và cấu hình EF Core
│-- Heplers/             # Các hàm tiện ích
│-- Model/               # Data Transfer Objects
│-- Mapping/             # Cấu hình AutoMapper
│-- Middleware/          # Xác thực và phân quyền
│-- Migrations/          # Lưu trữ các migration của database
│-- Properties/          # Cấu hình dự án
│-- Repository/          # Repository Pattern
│-- Services/            # Chứa logic nghiệp vụ
│-- Validators/          # Kiểm tra dữ liệu đầu vào
│-- Program.cs           # Cấu hình ứng dụng
│-- appsettings.json     # Cấu hình database và JWT
```

## 🔑 Chức năng chính
✅ **Quản lý quà**: Tạo, đọc, cập nhật và xóa các quà chính, quà khuyến mãi. Thiết lập quà khuyế mãi kèm theo quà chính         
✅ **Tính năng mua quà**: Cho phép người dùng mua quà bằng điểm.   
✅ **Tính năng phát quà**: Phát quà khi còn quà khuyến mãi, phát quà khi hết quà khuyến mãi, tự động phát quà theo ngày chọn trước.                         
✅ **Quản lý người dùng**: Xác thực, phân quyền, quản lý thông tin người dùng.                                    
✅ **Bảng xếp hàng tháng**: Cho phép xếp hạng người dùng mua quà hàng tháng.                                  
✅ **Xác thực & Phân quyền**: JWT Authentication, Refresh Token, Custom Authorization sử dụng Middlleware                            
