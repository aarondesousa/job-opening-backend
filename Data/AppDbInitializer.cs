using JobOpeningBackend.Models;

namespace JobOpeningBackend.Data;

public static class AppDbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        // Seed departments
        if (!context.Departments.Any())
        {
            var departments = new Department[]
            {
                new() { Title = "Software Development" },
                new() { Title = "Project Management" },
                // Add more departments as needed
            };
            foreach (var department in departments)
            {
                context.Departments.Add(department);
            }
            context.SaveChanges();
        }

        // Seed locations
        if (!context.Locations.Any())
        {
            var locations = new Location[]
            {
                new()
                {
                    Title = "US Head Office",
                    City = "Baltimore",
                    State = "MD",
                    Country = "United States",
                    Zip = "21202"
                },
                new()
                {
                    Title = "India Office",
                    City = "Verna",
                    State = "Goa",
                    Country = "India",
                    Zip = "0832"
                },
            };
            foreach (var location in locations)
            {
                context.Locations.Add(location);
            }
            context.SaveChanges();
        }

        // Seed Jobs
        if (!context.Jobs.Any())
        {
            var jobs = new Job[]
            {
                new()
                {
                    Id = 101,
                    Code = "JOB-01",
                    Title = "Software Developer",
                    Description = "Job description here...",
                    LocationId = 1,
                    DepartmentId = 1,
                    ClosingDate = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = 102,
                    Code = "JOB-02",
                    Title = "Project Manager",
                    Description = "Job description here...",
                    LocationId = 2,
                    DepartmentId = 2,
                    ClosingDate = DateTimeOffset.UtcNow
                },
            };
            foreach (var job in jobs)
            {
                context.Jobs.Add(job);
            }
            context.SaveChanges();
        }

        // Seed UserTokens
        if (!context.UserToken.Any())
        {
            var userTokens = new UserToken[]
            {
                new()
                {
                    Username = "Department",
                    Password = BCrypt.Net.BCrypt.HashPassword("Department"),
                    Role = "Department"
                },
                new()
                {
                    Username = "Location",
                    Password = BCrypt.Net.BCrypt.HashPassword("Location"),
                    Role = "Location"
                },
            };
            foreach (var userToken in userTokens)
            {
                context.UserToken.Add(userToken);
            }
            context.SaveChanges();
        }
    }
}
