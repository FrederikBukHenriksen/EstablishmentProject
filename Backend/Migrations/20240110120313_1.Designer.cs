﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240110120313_1")]
    partial class _1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Data.DataModels.SalesItems", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SaleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("SaleId");

                    b.ToTable("SalesItems");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EstablishmentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Establishment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("InformationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("InformationId");

                    b.ToTable("Establishment");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.EstablishmentInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LocationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("information");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EstablishmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Item");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.OpeningHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EstablishmentInformationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("close")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("dayOfWeek")
                        .HasColumnType("integer");

                    b.Property<DateTime>("open")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentInformationId");

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EstablishmentId")
                        .HasColumnType("uuid");

                    b.Property<int?>("PaymentType")
                        .HasColumnType("integer");

                    b.Property<int?>("SaleType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TableId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("TimestampArrival")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TimestampPayment")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EstablishmentId");

                    b.HasIndex("TableId");

                    b.ToTable("Sale");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Table", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EstablishmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("Table");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EstablishmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId", "EstablishmentId");

                    b.HasIndex("EstablishmentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("WebApplication1.Data.DataModels.SalesItems", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Domain_Layer.Entities.Sale", "Sale")
                        .WithMany("SalesItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Employee", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Establishment", null)
                        .WithMany("Employees")
                        .HasForeignKey("EstablishmentId");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Establishment", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.EstablishmentInformation", "Information")
                        .WithMany()
                        .HasForeignKey("InformationId");

                    b.Navigation("Information");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.EstablishmentInformation", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Item", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Establishment", null)
                        .WithMany("Items")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("WebApplication1.Domain_Layer.Entities.Price", "Price", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<int>("Currency")
                                .HasColumnType("integer")
                                .HasColumnName("PriceCurrency");

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("PriceValue");

                            b1.HasKey("Id");

                            b1.ToTable("Price");

                            b1.WithOwner()
                                .HasForeignKey("Id");
                        });

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Location", b =>
                {
                    b.OwnsOne("WebApplication1.Domain_Layer.Entities.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.HasKey("LocationId");

                            b1.ToTable("Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.Navigation("Coordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.OpeningHours", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.EstablishmentInformation", null)
                        .WithMany("OpeningHours")
                        .HasForeignKey("EstablishmentInformationId");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Sale", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("WebApplication1.Domain_Layer.Entities.Establishment", "Establishment")
                        .WithMany("Sales")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Domain_Layer.Entities.Table", "Table")
                        .WithMany()
                        .HasForeignKey("TableId");

                    b.Navigation("Employee");

                    b.Navigation("Establishment");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Table", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Establishment", null)
                        .WithMany("Tables")
                        .HasForeignKey("EstablishmentId");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.UserRole", b =>
                {
                    b.HasOne("WebApplication1.Domain_Layer.Entities.Establishment", "Establishment")
                        .WithMany()
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Domain_Layer.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Establishment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Establishment", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Items");

                    b.Navigation("Sales");

                    b.Navigation("Tables");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.EstablishmentInformation", b =>
                {
                    b.Navigation("OpeningHours");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.Sale", b =>
                {
                    b.Navigation("SalesItems");
                });

            modelBuilder.Entity("WebApplication1.Domain_Layer.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}