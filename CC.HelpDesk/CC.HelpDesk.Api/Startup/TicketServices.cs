using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Infrastructure;

namespace CC.HelpDesk.Api;

public static class TicketServices
{
    public static IServiceCollection AddDbTicketRepositories(this IServiceCollection services)
    {        
        services.AddScoped<ITicketRepository, EFDbTicketRepository>();

        return services;

    }
}
