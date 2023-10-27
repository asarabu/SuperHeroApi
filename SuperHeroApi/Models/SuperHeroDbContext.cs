using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SuperHeroApi.Models;

public partial class SuperHeroDbContext : DbContext
{
    public SuperHeroDbContext()
    {
    }

    public SuperHeroDbContext(DbContextOptions<SuperHeroDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CharacterStory> CharacterStories { get; set; }

    public virtual DbSet<SuperHero> SuperHeros { get; set; }

    public virtual DbSet<SuperVillain> SuperVillains { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ADITYA;Database=SuperHeroDB;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterStory>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Story)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UrlHandle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.SuperHero).WithMany()
                .HasForeignKey(d => d.SuperHeroId)
                .HasConstraintName("FK_CharacterStories_SuperHeros");

            entity.HasOne(d => d.SuperVillain).WithMany()
                .HasForeignKey(d => d.SuperVillainId)
                .HasConstraintName("FK_CharacterStories_SuperVillains");
        });

        modelBuilder.Entity<SuperHero>(entity =>
        {
            entity.HasKey(e => e.SuperHeroId).HasName("PK__SuperHer__5D7B134218E24EE0");

            entity.ToTable(tb => tb.HasTrigger("TR_SuperHeros_update_CharacterStory"));

            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HeroName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SuperPowers)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SuperVillain>(entity =>
        {
            entity.HasKey(e => e.SuperVillainId).HasName("PK__SuperVil__9A8F76E7351EBBBC");

            entity.ToTable(tb => tb.HasTrigger("TR_SuperVillains_update_CharacterStory"));

            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SuperPowers)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VillainName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.SuperHero).WithMany(p => p.SuperVillains)
                .HasForeignKey(d => d.SuperHeroId)
                .HasConstraintName("FK__SuperVill__Super__2F10007B");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserInfo__1788CC4C5500FC2F");

            entity.ToTable("UserInfo");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
