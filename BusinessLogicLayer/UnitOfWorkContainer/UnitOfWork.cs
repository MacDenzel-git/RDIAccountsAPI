using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using BusinessLogicLayer.Services.MailingListServiceContainer;
    using BusinessLogicLayer.Services.MainAccountsServiceContainer;
    using BusinessLogicLayer.Services.MemberDetailsServiceContainer; using Microsoft.EntityFrameworkCore.Storage;
    using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace BusinessLogicLayer.UnitOfWorkContainer
{
    
        //public class UnitOfWork : IUnitOfWork
        //{

        //    private readonly EasyAccountDbContext _context;
        //    private IDbContextTransaction _transaction;
        //    GenericRepository<MainAccount> IUnitOfWork._iMainAccountRepository;
        //    public UnitOfWork(EasyAccountDbContext context, IMailingListService iMailingListService,  IMainAccountService iMainAccountService, IMemberDetailService iMemberDetailService)
        //    {
        //        _context = context;
        //        _iMainAccountRepository = iMainAccountService;
        //        _iMemberDetailService = iMemberDetailService;
        //        _iMailingListRepository = iMailingListService;
        //    }

        //    public IMainAccountService _iMainAccountRepository { get; private set; }
        //    public IMemberDetailService _iMemberDetailService { get; private set; }
        //    public IMailingListService _iMailingListRepository { get; private set; }

       

        //public GenericRepository<MemberDetail> _memberDetailRepository => throw new NotImplementedException();

        //public GenericRepository<MailingList> IUnitOfWork._iMailingListRepository => throw new NotImplementedException();

        //public void SaveChanges()
        //    {
        //        _context.SaveChanges();
        //    }

        //    public void BeginTransaction()
        //    {
        //        _transaction = _context.Database.BeginTransaction();
        //    }

        //    public void CommitTransaction()
        //    {
        //        _transaction?.Commit();
        //    }

        //    public void RollbackTransaction()
        //    {
        //        _transaction?.Rollback();
        //    }

        //    public void Dispose()
        //    {
        //        _transaction?.Dispose();
        //        _context.Dispose();
        //    }
        //}
    
}
