using System.Data.Entity;

namespace notes_app_csharp_wpf.Models
{
    public class Context : DbContext
    {
        public Context() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, notes_app_csharp_wpf.Migrations.Configuration>());
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<File> Files { get; set; }
    }
}
