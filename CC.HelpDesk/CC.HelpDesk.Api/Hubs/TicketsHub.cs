using CC.HelpDesk.Domain;
using Microsoft.AspNetCore.SignalR;

namespace CC.HelpDesk.Api.Hubs
{
    public class TicketsHub : Hub
    {
        private readonly ILogger<TicketsHub> _logger;

        public TicketsHub(ILogger<TicketsHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Connected {ConnectionId}", Context.ConnectionId);

            return base.OnConnectedAsync();
        }

       

        public async Task SendTicketStatusChanged(Ticket ticket)
        {
            await Clients.Others.SendAsync("TicketStatusChanged", ticket);
        }
    }
}
