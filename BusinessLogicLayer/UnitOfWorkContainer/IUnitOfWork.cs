using BusinessLogicLayer.Services.MailingListServiceContainer;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using DataAccessLayer.Models;
using RDIAccountsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWorkContainer
{
    public interface IUnitOfWork : IDisposable
    {
        

        void SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
