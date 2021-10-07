namespace notes_app_csharp_wpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notesappv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        Year_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Years", t => t.Year_Id)
                .Index(t => t.Year_Id);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearNumber = c.Int(nullable: false),
                        Subject_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(nullable: false),
                        Semester_Sem = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semesters", t => t.Semester_Sem)
                .Index(t => t.Semester_Sem);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Sem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Sem);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Year_Id", "dbo.Years");
            DropForeignKey("dbo.Years", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "Semester_Sem", "dbo.Semesters");
            DropIndex("dbo.Subjects", new[] { "Semester_Sem" });
            DropIndex("dbo.Years", new[] { "Subject_Id" });
            DropIndex("dbo.Files", new[] { "Year_Id" });
            DropTable("dbo.Semesters");
            DropTable("dbo.Subjects");
            DropTable("dbo.Years");
            DropTable("dbo.Files");
            DropTable("dbo.Admins");
        }
    }
}
