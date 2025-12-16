using System;
using Lab05.BUS;

class TestConnection
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Testing database connection...");
            
            var studentService = new StudentService();
            var students = studentService.GetAll();
            
            Console.WriteLine($"Found {students.Count} students:");
            foreach (var student in students)
            {
                Console.WriteLine($"- {student.FullName} ({student.Faculty?.FacultyName})");
            }
            
            var studentsWithoutMajor = studentService.GetAllHasNoMajor();
            Console.WriteLine($"\nStudents without major: {studentsWithoutMajor.Count}");
            
            Console.WriteLine("\nDatabase connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}