using CC.HelpDesk.Domain;

namespace CC.HelpDesk.Application
{
    public class SalaryCalculator : ISalaryCalculator
    {
        private readonly decimal ratePerHour;

        public SalaryCalculator(decimal ratePerHour)
        {
            this.ratePerHour = ratePerHour;
        }

        public decimal Calculate(IEnumerable<Ticket> tickets)
        {            
            return tickets.Sum(t => t.Duration.HasValue ? t.Duration.Value.Hours * ratePerHour : default);
        }
    }
}