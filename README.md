## 📌 Giới thiệu
APIQuanLyQua là một Web API được phát triển bằng **ASP.NET Core 6** nhằm quản lý các chức năng phát quà theo lịch, bảng xếp hạng người dùng

## 🚀 Công nghệ sử dụng
- **.NET 6 (ASP.NET Core Web API)**
- **Entity Framework Core** (Code-First, Fluent API)
- **Microsoft SQL Server** (Lưu trữ dữ liệu)
- **Fluent Validation** (Kiểm tra ràng buộc dữ liệu đầu vào)
- **AutoMapper** (Chuyển đổi dữ liệu giữa DTO và Model)
- **JWT Authentication** (Xác thực API)
- **Swagger UI** (Tài liệu API)
- **Repository Pattern** (Quản lý dữ liệu)

## 📚 Cấu trúc thư mục
```
FlightDocsSystem/
│-- Authorization/       # Xác thực và phân quyền
│-- Controllers/         # Xử lý request và trả về response cho client
│-- DTO/                 # Data Transfer Objects
│-- Data/                # DbContext, Models và cấu hình EF Core
│-- Mapper/              # Cấu hình AutoMapper
│-- Migrations/          # Lưu trữ các migration của database
│-- Properties/          # Cấu hình dự án
│-- Repository/          # Repository Pattern
│-- Services/            # Chứa logic nghiệp vụ
│-- Upload/              # Thư mục lưu trữ file upload
│-- Validation/          # Kiểm tra dữ liệu đầu vào
│-- Program.cs           # Cấu hình ứng dụng
│-- TheThanh_WebAPI_Flight.csproj  # File cấu hình dự án
│-- appsettings.json     # Cấu hình database và JWT
```

## 🔑 Chức năng chính
✅ **Quản lý chuyến bay**: Tạo, đọc, cập nhật và xóa thông tin chuyến bay.  
✅ **Quản lý tài liệu chuyến bay**: Thêm, sửa, xóa, cho phép upload và download các tài liệu chuyến bay  
✅ **Quản lý người dùng**: Xác thực, phân quyền, quản lý thông tin người dùng   
✅ **Phân quyền tài liệu**: Cho phép người dùng có quyền truy cập tài liệu theo vai trò    
✅ **Xác thực & Phân quyền**: JWT Authentication, Refresh Token, Custom Authorization  
