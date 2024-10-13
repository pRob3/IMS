﻿// <auto-generated />
using System;
using IMS.Plugins.EFCoreSqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IMS.Plugins.EFCoreSqlServer.Migrations
{
    [DbContext(typeof(IMSContext))]
    [Migration("20241012175246_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IMS.CoreBusiness.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Inventories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bike Seat",
                            Price = 2.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 2,
                            Name = "Bike Body",
                            Price = 15.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bike Wheel",
                            Price = 8.0,
                            Quantity = 20
                        },
                        new
                        {
                            Id = 4,
                            Name = "Bike Pedal",
                            Price = 1.0,
                            Quantity = 20
                        });
                });

            modelBuilder.Entity("IMS.CoreBusiness.InventoryTransaction", b =>
                {
                    b.Property<int>("InventoryTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InventoryTransactionId"));

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<string>("DoneBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<string>("PONumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductionNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuantityAfter")
                        .HasColumnType("int");

                    b.Property<int>("QuantityBefore")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("InventoryTransactionId");

                    b.HasIndex("InventoryId");

                    b.ToTable("InventoryTransactions");
                });

            modelBuilder.Entity("IMS.CoreBusiness.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bike",
                            Price = 150.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 2,
                            Name = "Car",
                            Price = 25000.0,
                            Quantity = 10
                        });
                });

            modelBuilder.Entity("IMS.CoreBusiness.ProductInventory", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<int>("InventoryQuantity")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "InventoryId");

                    b.HasIndex("InventoryId");

                    b.ToTable("ProductInventories");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            InventoryId = 1,
                            InventoryQuantity = 1
                        },
                        new
                        {
                            ProductId = 1,
                            InventoryId = 2,
                            InventoryQuantity = 1
                        },
                        new
                        {
                            ProductId = 1,
                            InventoryId = 3,
                            InventoryQuantity = 2
                        },
                        new
                        {
                            ProductId = 1,
                            InventoryId = 4,
                            InventoryQuantity = 2
                        });
                });

            modelBuilder.Entity("IMS.CoreBusiness.ProductTransaction", b =>
                {
                    b.Property<int>("ProductTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductTransactionId"));

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<string>("DoneBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ProductionNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuantityAfter")
                        .HasColumnType("int");

                    b.Property<int>("QuantityBefore")
                        .HasColumnType("int");

                    b.Property<string>("SONumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("ProductTransactionId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductTransactions");
                });

            modelBuilder.Entity("IMS.CoreBusiness.InventoryTransaction", b =>
                {
                    b.HasOne("IMS.CoreBusiness.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("IMS.CoreBusiness.ProductInventory", b =>
                {
                    b.HasOne("IMS.CoreBusiness.Inventory", "Inventory")
                        .WithMany("ProductInventories")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IMS.CoreBusiness.Product", "Product")
                        .WithMany("ProductInventories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IMS.CoreBusiness.ProductTransaction", b =>
                {
                    b.HasOne("IMS.CoreBusiness.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("IMS.CoreBusiness.Inventory", b =>
                {
                    b.Navigation("ProductInventories");
                });

            modelBuilder.Entity("IMS.CoreBusiness.Product", b =>
                {
                    b.Navigation("ProductInventories");
                });
#pragma warning restore 612, 618
        }
    }
}
