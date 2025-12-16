using Lab05.DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            using (var context = new StudentModel())
            {
                return context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Major)
                    .ToList();
            }
        }

        public List<Student> GetAllHasNoMajor()
        {
            using (var context = new StudentModel())
            {
                return context.Students
                    .Include(s => s.Faculty)
                    .Where(s => s.MajorID == null)
                    .ToList();
            }
        }

        public Student FindById(int studentId)
        {
            using (var context = new StudentModel())
            {
                return context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Major)
                    .FirstOrDefault(x => x.StudentID == studentId);
            }
        }

        public void InsertUpdate(Student student)
        {
            using (var context = new StudentModel())
            {
                try
                {
                    var existing = context.Students.FirstOrDefault(s => s.StudentID == student.StudentID);
                    if (existing == null)
                    {
                        // Thêm mới
                        var sql = @"INSERT INTO Students (StudentID, FullName, AverageScore, FacultyID, MajorID, Avatar) 
                                   VALUES ({0}, {1}, {2}, {3}, {4}, {5})";
                        context.Database.ExecuteSqlCommand(sql, 
                            student.StudentID, 
                            student.FullName, 
                            student.AverageScore, 
                            student.FacultyID, 
                            student.MajorID, 
                            student.Avatar);
                    }
                    else
                    {
                        // Cập nhật
                        var sql = @"UPDATE Students SET FullName = {0}, AverageScore = {1}, 
                                   FacultyID = {2}, MajorID = {3}, Avatar = {4} 
                                   WHERE StudentID = {5}";
                        context.Database.ExecuteSqlCommand(sql, 
                            student.FullName, 
                            student.AverageScore, 
                            student.FacultyID, 
                            student.MajorID, 
                            student.Avatar, 
                            student.StudentID);
                    }
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception($"Lỗi cập nhật: {ex.Message}. Inner: {ex.InnerException?.Message}");
                }
            }
        }

        public void Delete(int studentId)
        {
            using (var context = new StudentModel())
            {
                try
                {
                    var sql = "DELETE FROM Students WHERE StudentID = {0}";
                    context.Database.ExecuteSqlCommand(sql, studentId);
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception($"Lỗi xóa: {ex.Message}. Inner: {ex.InnerException?.Message}");
                }
            }
        }
    }
}