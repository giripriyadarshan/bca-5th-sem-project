using System.Data.Entity;

namespace PYQ_Papers.Models
{
    internal class Context  : DbContext
    {
        public Context() : base("PYQ-DB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migrations.Configuration>());
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
