namespace Lab05.DAL.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public double AverageScore { get; set; }
        public int FacultyID { get; set; }
        public int? MajorID { get; set; }
        public string Avatar { get; set; }
        
        // Navigation properties
        public virtual Faculty Faculty { get; set; }
        public virtual Major Major { get; set; }
    }
}