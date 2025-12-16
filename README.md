# BAITAP6 - Hệ thống Quản lý Sinh viên

## Mô tả
Ứng dụng quản lý sinh viên sử dụng mô hình 3 lớp (3-Layer Architecture) với Entity Framework và SQL Server.

## Cấu trúc dự án
- **Lab05.DAL**: Data Access Layer - Kết nối cơ sở dữ liệu
- **Lab05.BUS**: Business Logic Layer - Xử lý logic nghiệp vụ  
- **Lab05.GUI**: Presentation Layer - Giao diện Windows Forms

## Công nghệ sử dụng
- .NET Framework 4.8
- Entity Framework 6.4.4
- SQL Server
- Windows Forms
- ADO.NET

## Chức năng
- ✅ Hiển thị danh sách sinh viên
- ✅ Thêm/sửa/xóa sinh viên
- ✅ Lọc sinh viên chưa có chuyên ngành
- ✅ Upload và quản lý avatar
- ✅ Quản lý khoa và chuyên ngành

## Cài đặt và chạy
1. Clone repository
2. Mở file `bt6.slnx` trong Visual Studio
3. Chạy script `CreateDatabase.sql` để tạo database
4. Build solution
5. Chạy `Lab05.GUI.exe`

## Database
- Database: StudentDB
- Tables: Students, Faculties, Majors
- Connection: SQL Server local instance

## Tác giả
Bài tập thực hành Entity Framework và mô hình 3 lớp