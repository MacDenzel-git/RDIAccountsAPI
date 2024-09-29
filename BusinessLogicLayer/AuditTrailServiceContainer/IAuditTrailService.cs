using AllinOne.DataHandlers;
using DataAccessLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.AuditTrailServiceContainer
{
    public interface IAuditTrailService
    {
        Task<OutputHandler> Add (AuditTrailDTO auditTrail);
    }
}
