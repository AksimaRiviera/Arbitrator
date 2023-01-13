using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToilettenArbitrator;

public partial class MembersDataContext : DbContext
{
    public MembersDataContext()
    {
    }

    public MembersDataContext(DbContextOptions<MembersDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankCard> BankCards { get; set; }

    public virtual DbSet<HeroCard> HeroCards { get; set; }

    public virtual DbSet<ItemCard> ItemCards { get; set; }

    public virtual DbSet<MobCard> MobCards { get; set; }

    public virtual DbSet<ProfileCard> ProfileCards { get; set; }

    public virtual DbSet<QuestCard> QuestCards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=F:\\\\\\\\GitProjects\\\\\\\\Arbitrator\\\\\\\\ToilettenArbitrator\\\\\\\\DataBase\\\\\\\\MembersData.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankCard>(entity =>
        {
            entity.ToTable("BankCard");

            entity.HasIndex(e => e.Id, "IX_BankCard_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<HeroCard>(entity =>
        {
            entity.ToTable("HeroCard");

            entity.HasIndex(e => e.Id, "IX_HeroCard_Id").IsUnique();
        });

        modelBuilder.Entity<ItemCard>(entity =>
        {
            entity.ToTable("ItemCard");

            entity.HasIndex(e => e.Id, "IX_ItemCard_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
        });

        modelBuilder.Entity<MobCard>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MobCard");

            entity.HasIndex(e => e.Id, "IX_MobCard_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<ProfileCard>(entity =>
        {
            entity.ToTable("ProfileCard");

            entity.HasIndex(e => e.Id, "IX_ProfileCard_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<QuestCard>(entity =>
        {
            entity.ToTable("QuestCard");

            entity.HasIndex(e => e.Id, "IX_QuestCard_id").IsUnique();

            entity.HasIndex(e => e.QuestId, "IX_QuestCard_questID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.QuestData).HasColumnName("questData");
            entity.Property(e => e.QuestId).HasColumnName("questID");
            entity.Property(e => e.QuestPrize).HasColumnName("questPrize");
            entity.Property(e => e.QuestType).HasColumnName("questType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
