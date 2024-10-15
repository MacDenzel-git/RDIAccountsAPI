using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

public partial class EasyAccountDbContext : DbContext
{
    public EasyAccountDbContext()
    {
    }

    public EasyAccountDbContext(DbContextOptions<EasyAccountDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<GroupAccount> GroupAccounts { get; set; }

    public virtual DbSet<GroupDetail> GroupDetails { get; set; }

    public virtual DbSet<GroupGorveningBody> GroupGorveningBodies { get; set; }

    public virtual DbSet<InterestAccount> InterestAccounts { get; set; }

    public virtual DbSet<InterestAccountJournalEntry> InterestAccountJournalEntries { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<LoanAccount> LoanAccounts { get; set; }

    public virtual DbSet<LoanConfiguration> LoanConfigurations { get; set; }

    public virtual DbSet<MailingList> MailingLists { get; set; }

    public virtual DbSet<MemberAccount> MemberAccounts { get; set; }

    public virtual DbSet<MemberAccountJournalEntry> MemberAccountJournalEntries { get; set; }

    public virtual DbSet<MemberDetail> MemberDetails { get; set; }

    public virtual DbSet<TransIdCounter> TransIdCounters { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EasyAccountDB;TrustServerCertificate=True; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.ToTable("AuditTrail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Operator).HasMaxLength(50);
            entity.Property(e => e.RecordTranId).HasMaxLength(50);
            entity.Property(e => e.ServiceName).HasMaxLength(50);
            entity.Property(e => e.TableAffected).HasMaxLength(50);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<GroupAccount>(entity =>
        {
            entity.HasKey(e => e.GroupAccountId).HasName("PK_MainAccounts");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Ggbid).HasColumnName("GGBId");
            entity.Property(e => e.GroupAccountName).HasMaxLength(50);
            entity.Property(e => e.GroupAccountNumber).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Ggb).WithMany(p => p.GroupAccounts)
                .HasForeignKey(d => d.Ggbid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupAccounts_GroupGorveningBody");
        });

        modelBuilder.Entity<GroupDetail>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.Property(e => e.GroupInitials).HasMaxLength(50);
            entity.Property(e => e.GroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<GroupGorveningBody>(entity =>
        {
            entity.HasKey(e => e.Ggbid);

            entity.ToTable("GroupGorveningBody");

            entity.Property(e => e.Ggbid).HasColumnName("GGBId");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<InterestAccount>(entity =>
        {
            entity.HasKey(e => e.InterestAccountId).HasName("PK_InterestAccount");

            entity.Property(e => e.InterestAccountName).HasMaxLength(50);
            entity.Property(e => e.InterestAccountNumber).HasMaxLength(300);
        });

        modelBuilder.Entity<InterestAccountJournalEntry>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.AssociatedLoanAccountNumber).HasMaxLength(300);
            entity.Property(e => e.BankTransferTransId).HasMaxLength(50);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DateLoanSent).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.DrCr)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FromAccountNumber).HasMaxLength(50);
            entity.Property(e => e.InterestAccountJournalEntryTransId).ValueGeneratedOnAdd();
            entity.Property(e => e.MemberBankAccount).HasMaxLength(50);
            entity.Property(e => e.MemberBankName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PayMode).HasMaxLength(50);
            entity.Property(e => e.ProcessDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProcessedStatus)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReceiptNo).HasMaxLength(50);
            entity.Property(e => e.RequestMethod).HasMaxLength(50);
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");
            entity.Property(e => e.Rev)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Revreq)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ToAccountNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionIdentifier).HasMaxLength(50);
            entity.Property(e => e.TransactionStatus).HasMaxLength(50);
            entity.Property(e => e.TransactionTypeDescription).HasMaxLength(50);

            entity.HasOne(d => d.TranscationType).WithMany()
                .HasForeignKey(d => d.TranscationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InterestAccountJournalEntries_InterestAccountJournalEntries");
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.JournalEntryTransId).HasName("PK_ReceiptNo");

            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.AssociatedLoanAccountNumber).HasMaxLength(300);
            entity.Property(e => e.BankTransferTransId).HasMaxLength(50);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DateLoanSent).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.DrCr)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FromAccountNumber).HasMaxLength(50);
            entity.Property(e => e.MemberBankAccount).HasMaxLength(50);
            entity.Property(e => e.MemberBankName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PayMode).HasMaxLength(50);
            entity.Property(e => e.ProcessDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProcessedStatus)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReceiptNo).HasMaxLength(50);
            entity.Property(e => e.RequestMethod).HasMaxLength(50);
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");
            entity.Property(e => e.Rev)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Revreq)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ToAccountNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionIdentifier).HasMaxLength(50);
            entity.Property(e => e.TransactionStatus).HasMaxLength(50);
            entity.Property(e => e.TransactionTypeDescription).HasMaxLength(50);

            entity.HasOne(d => d.TranscationType).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.TranscationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntries_JournalEntries");
        });

        modelBuilder.Entity<LoanAccount>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualPaymentDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpectedFinalRepaymentDate).HasColumnType("datetime");
            entity.Property(e => e.LoanAcccountStatus).HasMaxLength(50);
            entity.Property(e => e.LoanAccountNumber).HasMaxLength(50);
            entity.Property(e => e.LoanRequestedDate).HasColumnType("datetime");
            entity.Property(e => e.LoanStatus).HasMaxLength(50);
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<LoanConfiguration>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Createdby).HasMaxLength(50);
            entity.Property(e => e.LoanDescription).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MailingList>(entity =>
        {
            entity.ToTable("MailingList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(400);
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.MemberAccountId).HasName("PK_New_MemberAccounts");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MemberAccountName).HasMaxLength(50);
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MemberAccountJournalEntry>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.AssociatedLoanAccountNumber).HasMaxLength(300);
            entity.Property(e => e.BankTransferTransId).HasMaxLength(50);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DateLoanSent).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.DrCr)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FromAccountNumber).HasMaxLength(50);
            entity.Property(e => e.MemberAccountJournalEntryTransId).ValueGeneratedOnAdd();
            entity.Property(e => e.MemberBankAccount).HasMaxLength(50);
            entity.Property(e => e.MemberBankName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PayMode).HasMaxLength(50);
            entity.Property(e => e.ProcessDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProcessedStatus)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReceiptNo).HasMaxLength(50);
            entity.Property(e => e.RequestMethod).HasMaxLength(50);
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");
            entity.Property(e => e.Rev)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Revreq)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ToAccountNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionIdentifier).HasMaxLength(50);
            entity.Property(e => e.TransactionStatus).HasMaxLength(50);
            entity.Property(e => e.TransactionTypeDescription).HasMaxLength(50);

            entity.HasOne(d => d.TranscationType).WithMany()
                .HasForeignKey(d => d.TranscationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAccountJournalEntries_MemberAccountJournalEntries");
        });

        modelBuilder.Entity<MemberDetail>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_MemberAccounts");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(400);
            entity.Property(e => e.MemberName).HasMaxLength(100);
            entity.Property(e => e.Occupation).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);

            entity.HasOne(d => d.Group).WithMany(p => p.MemberDetails)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberDetails_GroupDetails");
        });

        modelBuilder.Entity<TransIdCounter>(entity =>
        {
            entity.ToTable("TransIdCounter");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.Property(e => e.TransactionTypeDescription).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
