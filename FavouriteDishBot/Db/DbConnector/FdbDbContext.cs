using System;
using System.Collections.Generic;
using FavouriteDishBot.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FavouriteDishBot.Db.DbConnector;

public partial class FdbDbContext : DbContext
{
    public FdbDbContext()
    {
    }

    public FdbDbContext(DbContextOptions<FdbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishesList> DishesLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:28432;Database=fdb_db;Username=fdb_user;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dishes_pk");

            entity.ToTable("dishes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(150)
                .HasColumnName("comment");
            entity.Property(e => e.Ingredients)
                .HasColumnType("character varying")
                .HasColumnName("ingredients");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Recipe)
                .HasColumnType("character varying")
                .HasColumnName("recipe");
            entity.Property(e => e.Title)
                .HasMaxLength(25)
                .HasColumnName("title");

            entity.HasMany(d => d.DishesLists).WithMany(p => p.Dishes)
                .UsingEntity<Dictionary<string, object>>(
                    "DishesInDishesList",
                    r => r.HasOne<DishesList>().WithMany()
                        .HasForeignKey("DishesListId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("dishes_in_dishes_list_dishes_list_id_fk"),
                    l => l.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("dishes_in_dishes_list_dishes_id_fk"),
                    j =>
                    {
                        j.HasKey("DishId", "DishesListId").HasName("dishes_in_dishes_list_pk");
                        j.ToTable("dishes_in_dishes_list");
                        j.IndexerProperty<int>("DishId").HasColumnName("dish_id");
                        j.IndexerProperty<int>("DishesListId").HasColumnName("dishes_list_id");
                    });
        });

        modelBuilder.Entity<DishesList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dishes_list_pk");

            entity.ToTable("dishes_list");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
