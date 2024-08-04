using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore.Storage;
using RDIAccountsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnityOfWork : IDisposable
    {
        private EasyAccountDbContext context = new EasyAccountDbContext();
        private GenericRepository<MemberDetail> memberRepository;
        private GenericRepository<MainAccount> mainAccountRepository;
        private GenericRepository<GroupDetail> groupDetailRepository;
        private GenericRepository<MailingList> mailingListRepository;
        private IDbContextTransaction _transaction;

        public GenericRepository<MemberDetail> MemberRepository
        {
            get
            {
                if (this.memberRepository == null)
                {
                    this.memberRepository = new GenericRepository<MemberDetail>(context);
                }
                return memberRepository;
            }
        }

        public GenericRepository<GroupDetail> GroupDetailRepository
        {
            get
            {
                if (this.groupDetailRepository == null)
                {
                    this.groupDetailRepository = new GenericRepository<GroupDetail>(context);
                }
                return groupDetailRepository;
            }
        }

        public GenericRepository<MailingList> MailingListRepository
        {
            get
            {
                if (this.mailingListRepository == null)
                {
                    this.mailingListRepository = new GenericRepository<MailingList>(context);
                }
                return mailingListRepository;
            }
        }

        public GenericRepository<MainAccount> MainAccountRepository
        {
            get
            {
                if (this.mainAccountRepository == null)
                {
                    this.mainAccountRepository = new GenericRepository<MainAccount>(context);
                }
                return mainAccountRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = context.Database.BeginTransaction();
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
            context.Dispose();
        }


    }
}

