using System;
using Lab05.BUS;

class TestEF
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Testing Entity Framework...");
            
            var studentService = new StudentService();
            var facultyService = new FacultyService();
            
            var faculties = facultyService.GetAll();
            Console.WriteLine($"Faculties: {faculties.Count}");
            
            var students = studentService.GetAll();
            Console.WriteLine($"Students: {students.Count}");
            
            var noMajor = studentService.GetAllHasNoMajor();
            Console.WriteLine($"Students without major: {noMajor.Count}");
            
            Console.WriteLine("✅ Entity Framework working!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine($"Inner: {ex.InnerException?.Message}");
        }
        Console.ReadKey();
    }
}