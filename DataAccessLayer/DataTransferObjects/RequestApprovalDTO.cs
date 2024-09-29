using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataTransferObjects
{
    public class RequestApprovalDTO
    {
        public int LoanAccountId { get; set; }
        public string LoanRequestedDate { get; set; }
        public string RequestMethod { get; set; }

        public string ApprovedBy { get; set; }

        public string PayMode { get; set; }
    }
}
