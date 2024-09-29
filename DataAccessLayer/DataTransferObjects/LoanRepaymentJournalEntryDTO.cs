using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataTransferObjects
{
    public class LoanRepaymentJournalEntryDTO: JournalEntryDTO
    {
        public string MemberAccount { get; set; }
        public string LoanAccount { get; set; }
        public string GroupAccount { get; set; }
        public string GroupInterestAccount { get; set; }
    }
}
