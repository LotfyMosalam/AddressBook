using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AddressBook.Infrastructure.Persistence.Seeders;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(ApplicationDbSeeder));

        try
        {
            await context.Database.MigrateAsync();

            await SeedJobsAsync(context, logger);
            await SeedDepartmentsAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task SeedJobsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Jobs.AnyAsync())
            return;

        var jobs = new[]
        {
            "Software Engineer",
            "Senior Software Engineer",
            "Technical Lead",
            "Product Manager",
            "UX Designer",
            "Data Analyst",
            "QA Engineer",
            "DevOps Engineer",
            "Business Analyst",
            "Project Manager"
        }.Select(Job.Create).ToList();

        await context.Jobs.AddRangeAsync(jobs);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} jobs.", jobs.Count);
    }

    private static async Task SeedDepartmentsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Departments.AnyAsync())
            return;

        var departments = new[]
        {
            "Engineering",
            "Product",
            "Design",
            "Data & Analytics",
            "Quality Assurance",
            "Operations",
            "Human Resources",
            "Finance"
        }.Select(Department.Create).ToList();

        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} departments.", departments.Count);
    }
}
