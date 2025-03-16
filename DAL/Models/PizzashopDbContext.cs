﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class PizzashopDbContext : DbContext
{
    public PizzashopDbContext()
    {
    }

    public PizzashopDbContext(DbContextOptions<PizzashopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assigntable> Assigntables { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Favouriteitem> Favouriteitems { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Itemtype> Itemtypes { get; set; }

    public virtual DbSet<Kot> Kots { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<Modifiergroup> Modifiergroups { get; set; }

    public virtual DbSet<Modifierorder> Modifierorders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Paymentstatustable> Paymentstatustables { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Permissionmanage> Permissionmanages { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Waitinglist> Waitinglists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=PizzashopDb;Username=postgres;     password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assigntable>(entity =>
        {
            entity.HasKey(e => e.AssignId).HasName("assigntable_pkey");

            entity.ToTable("assigntable");

            entity.Property(e => e.AssignId)
                .ValueGeneratedNever()
                .HasColumnName("assign_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.TableId).HasColumnName("table_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssigntableCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("assigntable_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Assigntables)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assigntable_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AssigntableModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("assigntable_modified_by_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Assigntables)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assigntable_table_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("category_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CategoryModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("category_modified_by_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("city_pkey");

            entity.ToTable("city");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(20)
                .HasColumnName("city_name");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_state_id_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(20)
                .HasColumnName("country_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.Phoneno).HasColumnName("phoneno");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("customers_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CustomerModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("customers_modified_by_fkey");
        });

        modelBuilder.Entity<Favouriteitem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("favouriteitem");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("favouriteitem_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany()
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouriteitem_customer_id_fkey");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouriteitem_item_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany()
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("favouriteitem_modified_by_fkey");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.CgstTax)
                .HasPrecision(5, 2)
                .HasColumnName("cgst_tax");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.GstTax)
                .HasPrecision(5, 2)
                .HasColumnName("gst_tax");
            entity.Property(e => e.InvoiceNo)
                .HasColumnType("character varying")
                .HasColumnName("invoice_no");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Other)
                .HasPrecision(5, 2)
                .HasColumnName("other");
            entity.Property(e => e.SgstTax)
                .HasPrecision(5, 2)
                .HasColumnName("sgst_tax");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InvoiceModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_modified_by_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_order_id_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("items_pkey");

            entity.ToTable("items");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Democol)
                .HasColumnType("char")
                .HasColumnName("democol");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isavailable)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isavailable");
            entity.Property(e => e.Isdefaulttax)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isdefaulttax");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ItemImage).HasColumnName("item_image");
            entity.Property(e => e.ItemName)
                .HasMaxLength(50)
                .HasColumnName("item_name");
            entity.Property(e => e.ItemTypeId).HasColumnName("item_type_id");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasPrecision(5, 2)
                .HasColumnName("rate");
            entity.Property(e => e.ShortCode)
                .HasMaxLength(20)
                .HasColumnName("short_code");
            entity.Property(e => e.TaxValue)
                .HasPrecision(5, 2)
                .HasColumnName("tax_value");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_category_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("items_created_by_fkey");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_item_type_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("items_modified_by_fkey");
        });

        modelBuilder.Entity<Itemtype>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId).HasName("itemtype_pkey");

            entity.ToTable("itemtype");

            entity.Property(e => e.ItemTypeId).HasColumnName("item_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.TypeImage).HasColumnName("type_image");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .HasColumnName("type_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemtypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("itemtype_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemtypeModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("itemtype_modified_by_fkey");
        });

        modelBuilder.Entity<Kot>(entity =>
        {
            entity.HasKey(e => e.KotId).HasName("kot_pkey");

            entity.ToTable("kot");

            entity.Property(e => e.KotId).HasColumnName("kot_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Isready).HasColumnName("isready");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.KotCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("kot_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.KotModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("kot_modified_by_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Kots)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kot_order_id_fkey");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.ModifierId).HasName("modifier_pkey");

            entity.ToTable("modifier");

            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifierGrpId).HasColumnName("modifier_grp_id");
            entity.Property(e => e.ModifierName)
                .HasMaxLength(50)
                .HasColumnName("modifier_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasPrecision(5, 2)
                .HasColumnName("rate");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ModifierCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("modifier_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ModifierModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("modifier_modified_by_fkey");

            entity.HasOne(d => d.ModifierGrp).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.ModifierGrpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifier_modifier_grp_id_fkey");
        });

        modelBuilder.Entity<Modifiergroup>(entity =>
        {
            entity.HasKey(e => e.ModifierGrpId).HasName("modifiergroup_pkey");

            entity.ToTable("modifiergroup");

            entity.Property(e => e.ModifierGrpId).HasColumnName("modifier_grp_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Desciption).HasColumnName("desciption");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifierGrpName)
                .HasMaxLength(50)
                .HasColumnName("modifier_grp_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ModifiergroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("modifiergroup_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ModifiergroupModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("modifiergroup_modified_by_fkey");
        });

        modelBuilder.Entity<Modifierorder>(entity =>
        {
            entity.HasKey(e => e.ModifierorderId).HasName("modifierorder_pkey");

            entity.ToTable("modifierorder");

            entity.Property(e => e.ModifierorderId).HasColumnName("modifierorder_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");
            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ModifierorderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("modifierorder_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ModifierorderModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("modifierorder_modified_by_fkey");

            entity.HasOne(d => d.Modifier).WithMany(p => p.Modifierorders)
                .HasForeignKey(d => d.ModifierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifierorder_modifier_id_fkey");

            entity.HasOne(d => d.Orderdetail).WithMany(p => p.Modifierorders)
                .HasForeignKey(d => d.OrderdetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifierorder_orderdetail_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentmethodId).HasColumnName("paymentmethod_id");
            entity.Property(e => e.PaymentstatusId).HasColumnName("paymentstatus_id");
            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(5, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("orders_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.OrderModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("orders_modified_by_fkey");

            entity.HasOne(d => d.Paymentmethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentmethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_paymentmethod_id_fkey");

            entity.HasOne(d => d.Paymentstatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_paymentstatus_id_fkey");

            entity.HasOne(d => d.Rating).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RatingId)
                .HasConstraintName("orders_rating_id_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_section_id_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_table_id_fkey");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.OrderdetailId).HasName("orderdetails_pkey");

            entity.ToTable("orderdetails");

            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ExtraInstruction).HasColumnName("extra_instruction");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderdetailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("orderdetails_created_by_fkey");

            entity.HasOne(d => d.Item).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_item_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.OrderdetailModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("orderdetails_modified_by_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_order_id_fkey");
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.HasKey(e => e.PaymentmethodId).HasName("paymentmethod_pkey");

            entity.ToTable("paymentmethod");

            entity.Property(e => e.PaymentmethodId).HasColumnName("paymentmethod_id");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Paymenttype)
                .HasMaxLength(20)
                .HasColumnName("paymenttype");
        });

        modelBuilder.Entity<Paymentstatustable>(entity =>
        {
            entity.HasKey(e => e.PaymentstatusId).HasName("paymentstatustable_pkey");

            entity.ToTable("paymentstatustable");

            entity.Property(e => e.PaymentstatusId).HasColumnName("paymentstatus_id");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .HasColumnName("paymentstatus");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.PermissionsName)
                .HasMaxLength(20)
                .HasDefaultValueSql("'canView'::character varying")
                .HasColumnName("permissions_name");
        });

        modelBuilder.Entity<Permissionmanage>(entity =>
        {
            entity.HasKey(e => e.PermissionmanageId).HasName("permissionmanage_pkey");

            entity.ToTable("permissionmanage");

            entity.Property(e => e.PermissionmanageId).HasColumnName("permissionmanage_id");
            entity.Property(e => e.Candelete).HasColumnName("candelete");
            entity.Property(e => e.Caneditadd).HasColumnName("caneditadd");
            entity.Property(e => e.Canview).HasColumnName("canview");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Permissioncheck).HasColumnName("permissioncheck");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.Permissionmanages)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permissionmanage_permission_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissionmanages)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permissionmanage_role_id_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.Ambience)
                .HasMaxLength(20)
                .HasColumnName("ambience");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Food)
                .HasMaxLength(20)
                .HasColumnName("food");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.Review).HasColumnName("review");
            entity.Property(e => e.Service)
                .HasMaxLength(20)
                .HasColumnName("service");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RatingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("rating_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RatingModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("rating_modified_by_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("sections_pkey");

            entity.ToTable("sections");

            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.SectionName)
                .HasMaxLength(50)
                .HasColumnName("section_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SectionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("sections_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SectionModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("sections_modified_by_fkey");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("states_pkey");

            entity.ToTable("states");

            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.StateName)
                .HasMaxLength(20)
                .HasColumnName("state_name");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("states_country_id_fkey");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("tables_pkey");

            entity.ToTable("tables");

            entity.Property(e => e.TableId)
                .ValueGeneratedNever()
                .HasColumnName("table_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TableName)
                .HasMaxLength(20)
                .HasColumnName("table_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TableCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("tables_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TableModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("tables_modified_by_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Tables)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tables_section_id_fkey");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.TaxId).HasName("tax_pkey");

            entity.ToTable("tax");

            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Isdefault)
                .HasDefaultValueSql("false")
                .HasColumnName("isdefault");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Isenable)
                .HasDefaultValueSql("true")
                .HasColumnName("isenable");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.TaxName)
                .HasMaxLength(20)
                .HasColumnName("tax_name");
            entity.Property(e => e.TaxType)
                .HasMaxLength(20)
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxValue).HasColumnName("tax_value");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaxCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("tax_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaxModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("tax_modified_by_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.ProfileImage, "users_profile_image_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.ProfileImage).HasColumnName("profile_image");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.UserloginId).HasColumnName("userlogin_id");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("users_city_id_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("users_country_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_role_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Users)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("users_state_id_fkey");

            entity.HasOne(d => d.Userlogin).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserloginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_userlogin_id_fkey");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.UserloginId).HasName("userlogin_pkey");

            entity.ToTable("userlogin");

            entity.HasIndex(e => e.Email, "userlogin_email_key").IsUnique();

            entity.Property(e => e.UserloginId).HasColumnName("userlogin_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userlogin_role_id_fkey");
        });

        modelBuilder.Entity<Waitinglist>(entity =>
        {
            entity.HasKey(e => e.WaitingId).HasName("waitinglist_pkey");

            entity.ToTable("waitinglist");

            entity.Property(e => e.WaitingId).HasColumnName("waiting_id");
            entity.Property(e => e.AssignedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assigned_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Isassign).HasColumnName("isassign");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.NoOfPerson).HasColumnName("no_of_person");
            entity.Property(e => e.TableId).HasColumnName("table_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WaitinglistCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("waitinglist_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Waitinglists)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waitinglist_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.WaitinglistModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("waitinglist_modified_by_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Waitinglists)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waitinglist_table_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
