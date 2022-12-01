using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Infrastructure;
using CC.HelpDesk.Domain;
using Microsoft.EntityFrameworkCore;

namespace CC.HelpDesk.Api;

public static class HelpDeskServices
{
    public static IServiceCollection AddHelpDeskRepositories(this IServiceCollection services)
    {
        services.AddUserRepositories();
        services.AddTicketRepositories();

        return services;
    }

    public static IServiceCollection AddDbHelpDeskRepositories(this IServiceCollection services, string connectionString)
    {
        // dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.11
        services.AddDbContextPool<ApiDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDbUserRepositories();

        return services;
    }


}

public static class UserServices
{
    public static IServiceCollection AddDbUserRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, EFDbUserRepository>();

        return services;

    }

    public static IServiceCollection AddUserRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();     

        return services;
    }

    public static IServiceCollection AddTicketRepositories(this IServiceCollection services)
    {
        // TODO: add tickets

        return services;
    }
}