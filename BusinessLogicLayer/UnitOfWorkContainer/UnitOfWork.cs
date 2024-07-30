using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWorkContainer
{
    using System;
    using BusinessLogicLayer.Services.MailingListServiceContainer;
    using BusinessLogicLayer.Services.MainAccountsServiceContainer;
    using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
    using DataAccessLayer.Models;
    using DocumentFormat.OpenXml.Bibliography;
    using Microsoft.EntityFrameworkCore.Storage;
    using RDIAccountsAPI;

    namespace ContosoUniversity.DAL
    {
        public class UnitOfWork : IUnitOfWork
        {

            private readonly EasyAccountDbContext _context;
            private IDbContextTransaction _transaction;

            public UnitOfWork(EasyAccountDbContext context, IMailingListService iMailingListService,  IMainAccountService iMainAccountService, IMemberDetailService iMemberDetailService)
            {
                _context = context;
                _iMainAccountService = iMainAccountService;
                _iMemberDetailService = iMemberDetailService;
                _iMailingListService = iMailingListService;
            }

            public IMainAccountService _iMainAccountService { get; private set; }
            public IMemberDetailService _iMemberDetailService { get; private set; }
            public IMailingListService _iMailingListService { get; private set; }

            public void SaveChanges()
            {
                _context.SaveChanges();
            }

            public void BeginTransaction()
            {
                _transaction = _context.Database.BeginTransaction();
            }

            public void CommitTransaction()
            {
                _transaction?.Commit();
            }

            public void RollbackTransaction()
            {
                _transaction?.Rollback();
            }

            public void Dispose()
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
    }
}
