using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementService.Data.Models;

namespace RestaurantManagementService.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menu> Menus { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LogUser> LogUsers { get; set; }

    public DbSet<RestaurantMenu> RB_RESTAURANTS_MENUS { get; set; }
    public DbSet<RestaurantItemMenu> RB_RESTAURANTS_ITEMSMENUS { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<RestaurantCategory> RestaurantCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

  //  public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=restaurantDB.mssql.somee.com;User ID=it22125866;Password=u1nmcs8493;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogUser>().HasNoKey();
        modelBuilder.Entity<RestaurantMenu>().HasNoKey();
        modelBuilder.Entity<RestaurantItemMenu>().HasNoKey();
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menus__C99ED230098F2526");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MenuName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Menus)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_Menus_Restaurant");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__MenuItem__8943F722BC151ABD");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
           // entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_MenuItems_Menu");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454C9502E65DE0");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.ImageUrl).HasMaxLength(150);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Restaurants_Category");

            entity.HasOne(d => d.Owner).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Restaurants_Owner");
        });

        modelBuilder.Entity<RestaurantCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Restaura__19093A0B3FCD926F");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ABCA6F888");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61604C82D6DA").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4914EB0E");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534BCB4983E").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A35EC49AF8E");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UQ_UserRole").IsUnique();

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
