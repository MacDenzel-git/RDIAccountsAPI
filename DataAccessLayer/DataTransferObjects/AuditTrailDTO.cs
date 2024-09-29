using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataTransferObjects
{
    public class AuditTrailDTO
    {
        public string Operator { get; set; } = null!;

        public string Action { get; set; } = null!;

        public string ServiceName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime TimeStamp { get; set; }

        public string? TableAffected { get; set; }

        public string? RecordTranId { get; set; }
    }
}
