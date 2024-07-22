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

    public virtual DbSet<InterestAccount> InterestAccounts { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<LoanAccount> LoanAccounts { get; set; }

    public virtual DbSet<LoanConfiguration> LoanConfigurations { get; set; }

    public virtual DbSet<MainAccount> MainAccounts { get; set; }

    public virtual DbSet<MemberAccount> MemberAccounts { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=EasyAccountDB;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InterestAccount>(entity =>
        {
            entity.ToTable("InterestAccount");

            entity.Property(e => e.InterestTitle).HasMaxLength(50);
            entity.Property(e => e.LoanAccountId).HasMaxLength(50);
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(50);
            entity.Property(e => e.Rate)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.JournalEntryTransId).HasName("PK_ReceiptNo");

            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.ChqNo).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DrCr)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.LoanAccountNumber).HasMaxLength(50);
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(50);
            entity.Property(e => e.MemberId).HasMaxLength(50);
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
            entity.Property(e => e.ExpectedRepaymentDate).HasColumnType("datetime");
            entity.Property(e => e.LoanAccountNumber).HasMaxLength(50);
            entity.Property(e => e.LoanDate).HasColumnType("datetime");
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<LoanConfiguration>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Createdby).HasMaxLength(50);
            entity.Property(e => e.LoanDescription).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RepaymentPeriod).HasMaxLength(50);
        });

        modelBuilder.Entity<MainAccount>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AccountName).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.Property(e => e.AccountName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MemberAccountNumber).HasMaxLength(100);
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.Property(e => e.TransactionTypeDescription).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
