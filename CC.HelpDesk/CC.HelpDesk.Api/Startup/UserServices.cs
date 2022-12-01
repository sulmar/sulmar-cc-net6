using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Infrastructure;

namespace CC.HelpDesk.Api;

public static class HelpDeskServices
{
    public static IServiceCollection AddHelpDeskRepositories(this IServiceCollection services)
    {
        services.AddUserRepositories();
        services.AddTicketRepositories();

        return services;
    }
}

public static class UserServices
{
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