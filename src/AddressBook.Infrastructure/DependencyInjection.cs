using AddressBook.Application.Common.Interfaces;
using AddressBook.Infrastructure.Identity;
using AddressBook.Infrastructure.Persistence;
using AddressBook.Infrastructure.Persistence.Repositories;
using AddressBook.Infrastructure.Services;
using AddressBook.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped<IAddressEntryRepository, AddressEntryRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IExcelExportService, ExcelExportService>();

        services.Configure<FileUploadSettings>(configuration.GetSection(nameof(FileUploadSettings)));
        services.AddScoped<IFileUploadService, FileUploadService>();

        // Identity
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
