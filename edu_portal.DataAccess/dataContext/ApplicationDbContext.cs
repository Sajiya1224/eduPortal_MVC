using eduPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace eduPortal.DataAccess.dataContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Course> courses { get; set; }
        public DbSet<Category> Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().HasData(

              new Course
              {
                  Id = 1,
                  Title = "Introduction to Programming with C#",
                  Description = "A beginner-friendly course covering the basics of C# programming, syntax, and simple projects.",
                  Price = 199,
                  Duration = "8h 30m",
                  Level = "Beginner",
                  IsPublished = false
              },
                new Course
                {
                    Id = 2,
                    Title = "ASP.NET Core MVC for Web Development",
                    Description = "Learn how to build modern web applications using ASP.NET Core MVC, Razor Pages, and Entity Framework.",
                    Price = 299,
                    Duration = "12h 15m",
                    Level = "Intermediate",
                    IsPublished = true
                },
                new Course
                {
                    Id = 3,
                    Title = "Advanced Database Management with SQL Server",
                    Description = "Master advanced SQL queries, stored procedures, and database optimization techniques using Microsoft SQL Server.",
                    Price = 249,
                    Duration = "10h 45m",
                    Level = "Advanced",
                    IsPublished = false
                }
            
            );

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
