-- Tạo database
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentDB')
    DROP DATABASE StudentDB;
GO

CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

-- Tạo bảng Faculties
CREATE TABLE Faculties (
    FacultyID INT PRIMARY KEY,
    FacultyName NVARCHAR(100) NOT NULL
);

-- Tạo bảng Majors
CREATE TABLE Majors (
    MajorID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    FacultyID INT,
    FOREIGN KEY (FacultyID) REFERENCES Faculties(FacultyID)
);

-- Tạo bảng Students
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    AverageScore FLOAT NOT NULL,
    FacultyID INT NOT NULL,
    MajorID INT NULL,
    Avatar NVARCHAR(255) NULL,
    FOREIGN KEY (FacultyID) REFERENCES Faculties(FacultyID),
    FOREIGN KEY (MajorID) REFERENCES Majors(MajorID)
);

-- Thêm dữ liệu mẫu
INSERT INTO Faculties (FacultyID, FacultyName) VALUES
(1, N'Công nghệ thông tin'),
(2, N'Kinh tế'),
(3, N'Ngoại ngữ');

INSERT INTO Majors (MajorID, Name, FacultyID) VALUES
(1, N'Công nghệ phần mềm', 1),
(2, N'Hệ thống thông tin', 1),
(3, N'Quản trị kinh doanh', 2),
(4, N'Tiếng Anh', 3);

INSERT INTO Students (StudentID, FullName, AverageScore, FacultyID, MajorID, Avatar) VALUES
(1001, N'Nguyễn Văn An', 8.5, 1, 1, NULL),
(1002, N'Trần Thị Bình', 7.8, 1, NULL, NULL),
(1003, N'Lê Văn Cường', 9.0, 2, 3, NULL),
(1004, N'Phạm Thị Dung', 6.5, 2, NULL, NULL),
(1005, N'Hoàng Văn Em', 7.2, 3, NULL, NULL);

SELECT 'Database created successfully' as Result;