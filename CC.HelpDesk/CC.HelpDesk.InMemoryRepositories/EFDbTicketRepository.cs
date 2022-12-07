using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;

namespace CC.HelpDesk.Infrastructure
{
    public class EFDbTicketRepository : ITicketRepository
    {
        private readonly ApiDbContext context;

        public EFDbTicketRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public Ticket Get(int id)
        {
            return context.Tickets.Find(id);   
        }
    }
}
