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
        private GenericRepository<MemberAccount> memberAccountRepository;
        private GenericRepository<LoanAccount> loanAccountRepository;
        private GenericRepository<GroupAccount> groupAccountRepository;
        private GenericRepository<JournalEntry> journalEntryRepository;
        private GenericRepository<GroupDetail> groupDetailRepository;
        private GenericRepository<MailingList> mailingListRepository;
        private GenericRepository<TransIdCounter> transCounterRepository;
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



        public GenericRepository<LoanAccount> LoanAccountRepository
        {
            get
            {
                if (this.loanAccountRepository == null)
                {
                    this.loanAccountRepository = new GenericRepository<LoanAccount>(context);
                }
                return loanAccountRepository;
            }
        }


        public GenericRepository<TransIdCounter> TransCounterRepository
        {
            get
            {
                if (this.transCounterRepository == null)
                {
                    this.transCounterRepository = new GenericRepository<TransIdCounter>(context);
                }
                return transCounterRepository;
            }
        } 
        
        public GenericRepository<JournalEntry> JournalEntryRepository
        {
            get
            {
                if (this.journalEntryRepository == null)
                {
                    this.journalEntryRepository = new GenericRepository<JournalEntry>(context);
                }
                return journalEntryRepository;
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

        public GenericRepository<MemberAccount> MemberAccountRepository
        {
            get
            {
                if (this.memberAccountRepository == null)
                {
                    this.memberAccountRepository = new GenericRepository<MemberAccount>(context);
                }
                return memberAccountRepository;
            }
        }


        public GenericRepository<GroupAccount> GroupAccountRepository
        {
            get
            {
                if (this.groupAccountRepository == null)
                {
                    this.groupAccountRepository = new GenericRepository<GroupAccount>(context);
                }
                return groupAccountRepository;
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

