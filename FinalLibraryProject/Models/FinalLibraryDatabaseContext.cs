using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using FinalLibraryProject.Models;

namespace FinalLibraryProject.Models
{
    public partial class FinalLibraryDatabaseContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public virtual DbSet<Checkouts> Checkouts { get; set; }
        public virtual DbSet<CurrentlyPresentLookup> CurrentlyPresentLookup { get; set; }
        public virtual DbSet<CurrentlyReservedLookup> CurrentlyReservedLookup { get; set; }
        public virtual DbSet<CurrentlySuspendedLookup> CurrentlySuspendedLookup { get; set; }
        public virtual DbSet<GenreTypeLookup> GenreTypeLookup { get; set; }
        public virtual DbSet<ItemTypeLookup> ItemTypeLookup { get; set; }
        public virtual DbSet<LibraryItem> LibraryItem { get; set; }
        public virtual DbSet<LibraryMember> LibraryMember { get; set; }
        public virtual DbSet<MemberTypeLookup> MemberTypeLookup { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<WaitList> WaitList { get; set; }

        public FinalLibraryDatabaseContext(DbContextOptions<FinalLibraryDatabaseContext> options)
    : base(options)
        { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fix for auth error
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Checkouts>(entity =>
            {
                entity.HasKey(e => e.CheckoutId);

                entity.Property(e => e.CheckoutId)
                    .HasColumnName("CheckoutID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.CheckoutTime).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.LibraryId).HasColumnName("LibraryID");

                entity.Property(e => e.TimeReturned).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Checkouts)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Checkouts_LibraryItem");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.Checkouts)
                    .HasForeignKey(d => d.LibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Checkouts_LibraryMember");
            });

            modelBuilder.Entity<CurrentlyPresentLookup>(entity =>
            {
                entity.HasKey(e => e.CurrentlyPresentId);

                entity.Property(e => e.CurrentlyPresentId)
                    .HasColumnName("CurrentlyPresentID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CurrentlyPresentValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentlyReservedLookup>(entity =>
            {
                entity.HasKey(e => e.CurrentlyReservedId);

                entity.Property(e => e.CurrentlyReservedId)
                    .HasColumnName("CurrentlyReservedID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CurrentlyReservedValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentlySuspendedLookup>(entity =>
            {
                entity.HasKey(e => e.CurrentlySuspendedId);

                entity.Property(e => e.CurrentlySuspendedId)
                    .HasColumnName("CurrentlySuspendedID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CurrentlySuspendedValue)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenreTypeLookup>(entity =>
            {
                entity.HasKey(e => e.GenreTypeId);

                entity.Property(e => e.GenreTypeId)
                    .HasColumnName("GenreTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GenreTypeValue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemTypeLookup>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.Property(e => e.ItemTypeId)
                    .HasColumnName("ItemTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ItemTypeValue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LibraryItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.AuthorFirstName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorLastName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.DateAddedToSystem).HasColumnType("datetime");

                entity.Property(e => e.DetailDescription).IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnName("ISBN")
                    .HasColumnType("char(13)");

                entity.Property(e => e.LibrarySection)
                    .IsRequired()
                    .HasColumnType("char(5)");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CurrentlyPresentNavigation)
                    .WithMany(p => p.LibraryItem)
                    .HasForeignKey(d => d.CurrentlyPresent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LibraryItem_CurrentlyPresentLookup");

                entity.HasOne(d => d.CurrentlyPresent1)
                    .WithMany(p => p.LibraryItem)
                    .HasForeignKey(d => d.CurrentlyPresent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LibraryItem_CurrentlyReservedLookup");

                entity.HasOne(d => d.GenreNavigation)
                    .WithMany(p => p.LibraryItem)
                    .HasForeignKey(d => d.Genre)
                    .HasConstraintName("FK_LibraryItem_GenreTypeLookup");

                entity.HasOne(d => d.ItemTypeNavigation)
                    .WithMany(p => p.LibraryItem)
                    .HasForeignKey(d => d.ItemType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LibraryItem_ItemTypeLookup");
            });

            modelBuilder.Entity<LibraryMember>(entity =>
            {
                entity.HasKey(e => e.LibraryId);

                entity.Property(e => e.LibraryId)
                    .HasColumnName("LibraryID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.CityAddress)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StateAddress)
                    .IsRequired()
                    .HasColumnType("char(2)");

                entity.Property(e => e.StreetAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeAccountCreated).HasColumnType("datetime");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CurrentlySuspendedNavigation)
                    .WithMany(p => p.LibraryMember)
                    .HasForeignKey(d => d.CurrentlySuspended)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LibraryMember_CurrentlySupsendedLookup");

                entity.HasOne(d => d.MemberTypeNavigation)
                    .WithMany(p => p.LibraryMember)
                    .HasForeignKey(d => d.MemberType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LibraryMember_MemberTypeLookup");
            });

            modelBuilder.Entity<MemberTypeLookup>(entity =>
            {
                entity.HasKey(e => e.MemberTypeId);

                entity.Property(e => e.MemberTypeId)
                    .HasColumnName("MemberTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MemberTypeValue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.CurrentAccountBalance).HasComputedColumnSql("(([PreviousAccountBalance]+[CurrentFine])-[CurrentPayment])");

                entity.Property(e => e.LibraryId).HasColumnName("LibraryID");

                entity.Property(e => e.TransactionTime).HasColumnType("datetime");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.LibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_LibraryMember");
            });

            modelBuilder.Entity<WaitList>(entity =>
            {
                entity.Property(e => e.WaitListId)
                    .HasColumnName("WaitListID");
                    //.ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.LibraryId).HasColumnName("LibraryID");

                entity.Property(e => e.TimeWaitListed).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.WaitList)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaitList_LibraryItem");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.WaitList)
                    .HasForeignKey(d => d.LibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaitList_LibraryMember");
            });
        }



        public DbSet<FinalLibraryProject.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
