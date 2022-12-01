using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.HelpDesk.Domain
{
    public interface ISalaryCalculator
    {
        decimal Calculate(IEnumerable<Ticket> tickets);
    }
}
