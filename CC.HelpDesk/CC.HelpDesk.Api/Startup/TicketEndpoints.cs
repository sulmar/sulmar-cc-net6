using AutoMapper;
using CC.HelpDesk.Api.Hubs;
using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CC.HelpDesk.Api;

public static class TicketEndpoints
{
    public static WebApplication MapTicketEndpoints(this WebApplication app)
    {
        app.MapPut("api/tickets/{id}", async (int id, 
            [FromBody] TicketStatus status,             
            ITicketRepository ticketRepository,
            IHubContext<TicketsHub> hub) =>
        {
            var ticket = ticketRepository.Get(id);

            await hub.Clients.All.SendAsync("TicketStatusChanged", ticket);

        });

        return app;

    }
}
