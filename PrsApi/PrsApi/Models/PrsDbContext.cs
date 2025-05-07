using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PrsApi.Models;

namespace PrsApi.Models;

public partial class PrsDbContext : DbContext
{
    public PrsDbContext()
    {
    }

    public PrsDbContext(DbContextOptions<PrsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User1> Users1 { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

public DbSet<PrsApi.Models.LogIn> LogIn { get; set; } = default!;

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0RUU8UE\\SQLEXPRESS;Initial Catalog=PRSproto;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<LineItem>(entity =>
    //    {
    //        entity.HasKey(e => e.Id).HasName("PK__LineItem__3214EC278BA7359E");

    //        entity.HasOne(d => d.Product).WithMany(p => p.LineItems).HasConstraintName("FK_ProductID2ProductID");

    //        entity.HasOne(d => d.Request).WithMany(p => p.LineItems).HasConstraintName("FK_RequestID2RequestID");
    //    });

//        modelBuilder.Entity<Product>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC2701B60EB4");

//            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_VendorID2VendorID");
//        });

//        modelBuilder.Entity<Request>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC27BA357123");

//            entity.Property(e => e.Status).HasDefaultValue("New");

//            entity.HasOne(d => d.User).WithMany(p => p.Requests)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_UserID2UserID");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__User__3214EC273435006B");
//        });

//        modelBuilder.Entity<Vendor>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Vendor__3214EC27E23405F7");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
