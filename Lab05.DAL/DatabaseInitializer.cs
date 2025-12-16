using Lab05.DAL.Entities;
using System.Data.Entity;

namespace Lab05.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<StudentModel>
    {
        protected override void Seed(StudentModel context)
        {
            // Thêm dữ liệu Faculty
            var faculty1 = new Faculty { FacultyID = 1, FacultyName = "Công nghệ thông tin" };
            var faculty2 = new Faculty { FacultyID = 2, FacultyName = "Kinh tế" };
            var faculty3 = new Faculty { FacultyID = 3, FacultyName = "Ngoại ngữ" };

            context.Faculties.Add(faculty1);
            context.Faculties.Add(faculty2);
            context.Faculties.Add(faculty3);

            // Thêm dữ liệu Major
            var major1 = new Major { MajorID = 1, Name = "Công nghệ phần mềm", FacultyID = 1 };
            var major2 = new Major { MajorID = 2, Name = "Hệ thống thông tin", FacultyID = 1 };
            var major3 = new Major { MajorID = 3, Name = "Quản trị kinh doanh", FacultyID = 2 };
            var major4 = new Major { MajorID = 4, Name = "Tiếng Anh", FacultyID = 3 };

            context.Majors.Add(major1);
            context.Majors.Add(major2);
            context.Majors.Add(major3);
            context.Majors.Add(major4);

            // Thêm dữ liệu Student
            var student1 = new Student { StudentID = 1001, FullName = "Nguyễn Văn An", FacultyID = 1, MajorID = 1, AverageScore = 8.5 };
            var student2 = new Student { StudentID = 1002, FullName = "Trần Thị Bình", FacultyID = 1, MajorID = null, AverageScore = 7.8 };
            var student3 = new Student { StudentID = 1003, FullName = "Lê Văn Cường", FacultyID = 2, MajorID = 3, AverageScore = 9.0 };
            var student4 = new Student { StudentID = 1004, FullName = "Phạm Thị Dung", FacultyID = 2, MajorID = null, AverageScore = 6.5 };
            var student5 = new Student { StudentID = 1005, FullName = "Hoàng Văn Em", FacultyID = 3, MajorID = null, AverageScore = 7.2 };

            context.Students.Add(student1);
            context.Students.Add(student2);
            context.Students.Add(student3);
            context.Students.Add(student4);
            context.Students.Add(student5);

            context.SaveChanges();
        }
    }
}