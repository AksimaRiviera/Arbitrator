using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToilettenArbitrator
{
    public partial class MembersDataContext : DbContext
    {
        public MembersDataContext()
        {
        }

        public MembersDataContext(DbContextOptions<MembersDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BankCard> BankCards { get; set; } = null!;
        public virtual DbSet<HeroCard> HeroCards { get; set; } = null!;
        public virtual DbSet<ItemCard> ItemCards { get; set; } = null!;
        public virtual DbSet<MobCard> MobCards { get; set; } = null!;
        public virtual DbSet<ProfileCard> ProfileCards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=F:\\\\\\\\GitProjects\\\\\\\\Arbitrator\\\\\\\\ToilettenArbitrator\\\\\\\\DataBase\\\\\\\\MembersData.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankCard>(entity =>
            {
                entity.ToTable("BankCard");

                entity.HasIndex(e => e.Id, "IX_BankCard_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<HeroCard>(entity =>
            {
                entity.ToTable("HeroCard");

                entity.HasIndex(e => e.Id, "IX_HeroCard_Id")
                    .IsUnique();
            });

            modelBuilder.Entity<ItemCard>(entity =>
            {
                entity.ToTable("ItemCard");

                entity.HasIndex(e => e.Id, "IX_ItemCard_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");
            });

            modelBuilder.Entity<MobCard>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MobCard");

                entity.HasIndex(e => e.Id, "IX_MobCard_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<ProfileCard>(entity =>
            {
                entity.ToTable("ProfileCard");

                entity.HasIndex(e => e.Id, "IX_ProfileCard_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
