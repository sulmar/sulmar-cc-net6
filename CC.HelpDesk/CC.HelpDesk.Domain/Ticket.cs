using CC.Core.Domain;

namespace CC.HelpDesk.Domain
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public User AssignedTo { get; set; }
        public TimeSpan? Duration { get; set; }
    }

    public enum TicketStatus
    {
        New,
        Assigned,
        Fixed,
        Closed
    }
}