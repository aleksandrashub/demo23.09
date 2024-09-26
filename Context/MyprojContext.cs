using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Order.Models;

namespace Order.Context;

public partial class MyprojContext : DbContext
{
    public MyprojContext()
    {
    }

    public MyprojContext(DbContextOptions<MyprojContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PunktVydahi> PunktVydahis { get; set; }

    public virtual DbSet<SrokDostavki> SrokDostavkis { get; set; }

    public virtual DbSet<StatusZakaz> StatusZakazs { get; set; }

    public virtual DbSet<Zakaz> Zakazs { get; set; }

    public virtual DbSet<ZakazProduct> ZakazProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=myproj;Username=postgres;Password=18b22M02a");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.IdDiscount).HasName("discounts_pk");

            entity.ToTable("discounts", "shubenok23");

            entity.Property(e => e.IdDiscount).HasColumnName("id_discount");
            entity.Property(e => e.ValueDiscount).HasColumnName("value_discount");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.IdManufacturer).HasName("manufacturer_pk");

            entity.ToTable("manufacturer", "shubenok23");

            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.NameManufacturer)
                .HasColumnType("character varying")
                .HasColumnName("name_manufacturer");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("product_pk");

            entity.ToTable("product", "shubenok23");

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.AmountManufacturer).HasColumnName("amount_manufacturer");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Descriprion)
                .HasColumnType("character varying")
                .HasColumnName("descriprion");
            entity.Property(e => e.IdDiscount).HasColumnName("id_discount");
            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.NameProduct)
                .HasColumnType("character varying")
                .HasColumnName("name_product");

            entity.HasOne(d => d.IdDiscountNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdDiscount)
                .HasConstraintName("product_discounts_fk");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("product_manufacturer_fk");
        });

        modelBuilder.Entity<PunktVydahi>(entity =>
        {
            entity.HasKey(e => e.IdPunkt).HasName("punkt_vydahi_pk");

            entity.ToTable("punkt_vydahi", "shubenok23");

            entity.Property(e => e.IdPunkt).HasColumnName("id_punkt");
            entity.Property(e => e.NamePunkt)
                .HasColumnType("character varying")
                .HasColumnName("name_punkt");
        });

        modelBuilder.Entity<SrokDostavki>(entity =>
        {
            entity.HasKey(e => e.IdSrokDost).HasName("srok_dostavki_pk");

            entity.ToTable("srok_dostavki", "shubenok23");

            entity.Property(e => e.IdSrokDost).HasColumnName("id_srok_dost");
            entity.Property(e => e.ValueSrok)
                .HasColumnType("character varying")
                .HasColumnName("value_srok");
        });

        modelBuilder.Entity<StatusZakaz>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("status_zakaz_pk");

            entity.ToTable("status_zakaz", "shubenok23");

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasColumnType("character varying")
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<Zakaz>(entity =>
        {
            entity.HasKey(e => e.IdZakaz).HasName("zakaz_pk");

            entity.ToTable("zakaz", "shubenok23");

            entity.Property(e => e.IdZakaz)
                .HasIdentityOptions(null, null, null, null, true, null)
                .HasColumnName("id_zakaz");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdPunkt).HasColumnName("id_punkt");
            entity.Property(e => e.IdSrok).HasColumnName("id_srok");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");

            entity.HasOne(d => d.IdPunktNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdPunkt)
                .HasConstraintName("zakaz_punkt_vydahi_fk");

            entity.HasOne(d => d.IdSrokNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdSrok)
                .HasConstraintName("zakaz_srok_dostavki_fk");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("zakaz_status_zakaz_fk");
        });

        modelBuilder.Entity<ZakazProduct>(entity =>
        {
            entity.HasKey(e => new { e.IdZakaz, e.IdProduct }).HasName("zakaz_product_pk");

            entity.ToTable("zakaz_product", "shubenok23");

            entity.Property(e => e.IdZakaz).HasColumnName("id_zakaz");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ZakazProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("zakaz_product_product_fk");

            entity.HasOne(d => d.IdZakazNavigation).WithMany(p => p.ZakazProducts)
                .HasForeignKey(d => d.IdZakaz)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("zakaz_product_zakaz_fk");
        });
        modelBuilder.HasSequence("discounts_id_discount_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("manufacturer_id_manufacturer_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("newtable_1_id_user_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("newtable_id_role_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("newtable_id_srok_dost_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("newtable_id_visit_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("punkt_vydahi_id_punkt_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("status_zakaz_id_status_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("table1_id_seq", "shubenok23").HasMax(2147483647L);
        modelBuilder.HasSequence("zakaz_id_zakaz_seq", "shubenok23").HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
