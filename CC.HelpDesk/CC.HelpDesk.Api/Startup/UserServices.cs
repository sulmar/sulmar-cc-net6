using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Infrastructure;
using CC.HelpDesk.Domain;

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
        services.AddSingleton(sp => new List<User>
        {
            new User(1, "John", "Smith") { Email = "john.smith@domain.com" },
            new User(2, "Kate", "Smith") { Email = "kate.smith@domain.com" },
            new User(3, "Mark", "Spider") { Email = "mark.spider@domain.com" },
        });

        return services;
    }

    public static IServiceCollection AddTicketRepositories(this IServiceCollection services)
    {
        // TODO: add tickets

        return services;
    }
}