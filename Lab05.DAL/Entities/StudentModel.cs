using System.Data.Entity;

namespace Lab05.DAL.Entities
{
    public class StudentModel : DbContext
    {
        public StudentModel() : base("name=StudentModel")
        {
            Database.SetInitializer<StudentModel>(null);
        }

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .ToTable("Students")
                .HasKey(s => s.StudentID);

            modelBuilder.Entity<Faculty>()
                .ToTable("Faculties")
                .HasKey(f => f.FacultyID);

            modelBuilder.Entity<Major>()
                .ToTable("Majors")
                .HasKey(m => m.MajorID);

            modelBuilder.Entity<Student>()
                .HasRequired(s => s.Faculty)
                .WithMany()
                .HasForeignKey(s => s.FacultyID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasOptional(s => s.Major)
                .WithMany()
                .HasForeignKey(s => s.MajorID)
                .WillCascadeOnDelete(false);
        }
    }
}