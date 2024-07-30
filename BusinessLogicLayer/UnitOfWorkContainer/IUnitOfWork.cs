using BusinessLogicLayer.Services.MailingListServiceContainer;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWorkContainer
{
    public interface IUnitOfWork : IDisposable
    {
        IMainAccountService _iMainAccountService { get; }
        IMemberDetailService _iMemberDetailService { get; }
        IMailingListService _iMailingListService { get; }

        void SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
