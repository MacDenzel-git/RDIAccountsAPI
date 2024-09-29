using AllinOne.DataHandlers;
using AllinOne.DataHandlers.ErrorHandler;
using DataAccessLayer.DataTransferObjects;
using DataAccessLayer.Models;
using RDIAccountsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.AuditTrailServiceContainer
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly GenericRepository<AuditTrail> _repository;

        public async Task<OutputHandler> Add(AuditTrailDTO auditTrail)
        {

            try
            {
                var mapped = new AutoMapper<AuditTrailDTO, AuditTrail>().MapToObject(auditTrail);
                var result = await _repository.Create(mapped);
                return result;
            }
            catch (Exception ex)
            {
                return StandardMessages.getExceptionMessage(ex);
            }


        }
    }
}
