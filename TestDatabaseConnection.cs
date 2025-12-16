using System;
using Lab05.BUS;

class TestDatabaseConnection
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Testing Entity Framework connection to SQL Server...");
            
            var studentService = new StudentService();
            var facultyService = new FacultyService();
            
            Console.WriteLine("Loading faculties...");
            var faculties = facultyService.GetAll();
            Console.WriteLine($"Found {faculties.Count} faculties:");
            foreach (var faculty in faculties)
            {
                Console.WriteLine($"- {faculty.FacultyName} (ID: {faculty.FacultyID})");
            }
            
            Console.WriteLine("\nLoading students...");
            var students = studentService.GetAll();
            Console.WriteLine($"Found {students.Count} students:");
            foreach (var student in students)
            {
                Console.WriteLine($"- {student.FullName} (Faculty: {student.Faculty?.FacultyName}, Major: {student.Major?.Name ?? "None"})");
            }
            
            Console.WriteLine("\nLoading students without major...");
            var studentsWithoutMajor = studentService.GetAllHasNoMajor();
            Console.WriteLine($"Found {studentsWithoutMajor.Count} students without major:");
            foreach (var student in studentsWithoutMajor)
            {
                Console.WriteLine($"- {student.FullName} (Faculty: {student.Faculty?.FacultyName})");
            }
            
            Console.WriteLine("\n✅ Database connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}