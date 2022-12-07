using CC.HelpDesk.Domain;

namespace CC.HelpDesk.IRepositories;

public interface ITicketRepository
{
    Ticket Get(int id);
}
