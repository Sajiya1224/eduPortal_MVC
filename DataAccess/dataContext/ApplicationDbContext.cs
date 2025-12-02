using eduPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eduPortal.DataAccess.dataContext
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Course> courses { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers{ get; set; }
        public DbSet<VideoCourse> videoCourses{ get; set; }
        public DbSet<QuizCourse> quizCourses { get; set; }
        public DbSet<Section> section { get; set; }
        public DbSet<Lecture> lecture { get; set; }
        public DbSet<Cart> Cart { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VideoCourse>()
                .HasOne(v => v.ApplicationUser)
                .WithMany()
                .HasForeignKey(v => v.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Course>().HasData(

            //  new Course
            //  {
            //      Id = 1,
            //      Title = "Introduction to Programming with C#",
            //      Description = "A beginner-friendly course covering the basics of C# programming, syntax, and simple projects.",
            //      Duration = "8h 30m",
            //      Level = "Beginner",
            //      IsPublished = false,
            //      CategoryId = 4
            //  },
            //    new Course
            //    {
            //        Id = 2,
            //        Title = "ASP.NET Core MVC for Web Development",
            //        Description = "Learn how to build modern web applications using ASP.NET Core MVC, Razor Pages, and Entity Framework.",
            //        Duration = "12h 15m",
            //        Level = "Intermediate",
            //        IsPublished = true,
            //        CategoryId = 4
            //    },
            //    new Course
            //    {
            //        Id = 3,
            //        Title = "Advanced Database Management with SQL Server",
            //        Description = "Master advanced SQL queries, stored procedures, and database optimization techniques using Microsoft SQL Server.",
            //        Duration = "10h 45m",
            //        Level = "Advanced",
            //        IsPublished = false,
            //        CategoryId = 4
            //    }

            //);

            modelBuilder.Entity<Category>().HasData(

              new Category
              {
                  Id = 1,
                  Name = "Database",
                  CreatedAt = DateTime.UtcNow,
              },
              new Category
               {
                  Id = 2,
                  Name = "ASP.net",
                  CreatedAt = DateTime.UtcNow,
              },
               new Category
                {
                 Id = 3,
                 Name = "Networking",
                 CreatedAt = DateTime.UtcNow,
                }
              );

              }
     }
}
