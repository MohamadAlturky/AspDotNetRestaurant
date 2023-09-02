﻿// <auto-generated />
using System;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    [Migration("20230830233404_sadfg")]
    partial class sadfg
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Customers.Aggregate.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<string>("BelongsToDepartment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eligible")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRegular")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Domain.Customers.Entities.AccountTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("By")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("AccountTransaction");
                });

            modelBuilder.Entity("Domain.Customers.Entities.Feedback", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("Domain.MealEntries.Aggregate.MealEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("AtDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("ConsumedReservations")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("CustomerCanCancel")
                        .HasColumnType("bit");

                    b.Property<int>("LastNumberInQueue")
                        .HasColumnType("int");

                    b.Property<long>("MealInformationId")
                        .HasColumnType("bigint");

                    b.Property<int>("PreparedCount")
                        .HasColumnType("int");

                    b.Property<int>("ReservationsCount")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AtDay")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MealInformationId");

                    b.ToTable("MealEntry");
                });

            modelBuilder.Entity("Domain.MealInformations.Aggregate.MealInformation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfCalories")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Meal", (string)null);
                });

            modelBuilder.Entity("Domain.Pricing.Aggregate.PricingRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerTypeValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MealTypeValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MealTypeValue", "CustomerTypeValue")
                        .IsUnique();

                    b.ToTable("PricingRecord");
                });

            modelBuilder.Entity("Domain.Reservations.Aggregate.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("AtDay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<long>("MealEntryId")
                        .HasColumnType("bigint");

                    b.Property<int>("NumberInQueue")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ReservationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AtDay");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MealEntryId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permission");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "RegisterCustomer"
                        },
                        new
                        {
                            Id = 4,
                            Name = "OrderContent"
                        },
                        new
                        {
                            Id = 3,
                            Name = "CreateSystemInformation"
                        },
                        new
                        {
                            Id = 1,
                            Name = "ReadContent"
                        },
                        new
                        {
                            Id = 5,
                            Name = "ReadSystemInformation"
                        },
                        new
                        {
                            Id = 6,
                            Name = "ConsumeReservations"
                        },
                        new
                        {
                            Id = 7,
                            Name = "EditBalances"
                        },
                        new
                        {
                            Id = 8,
                            Name = "SeePublicContent"
                        });
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "User"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Consumer"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Accountant"
                        });
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 8
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 3
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 5
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 7
                        },
                        new
                        {
                            RoleId = 4,
                            PermissionId = 6
                        },
                        new
                        {
                            RoleId = 4,
                            PermissionId = 8
                        },
                        new
                        {
                            RoleId = 3,
                            PermissionId = 7
                        },
                        new
                        {
                            RoleId = 3,
                            PermissionId = 8
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 8
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 4
                        });
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HiastMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Infrastructure.DataAccess.Tests.DummyMealInformation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfCalories")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DummyMealInformation");
                });

            modelBuilder.Entity("Infrastructure.ForgetPasswordHandling.Models.ForgetPasswordEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("AtDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("ValidationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ForgetPasswordEntry");
                });

            modelBuilder.Entity("Infrastructure.Notification.Model.NotificationMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageSubject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("NotificationMessage");
                });

            modelBuilder.Entity("Domain.Customers.Entities.AccountTransaction", b =>
                {
                    b.HasOne("Domain.Customers.Aggregate.Customer", "Customer")
                        .WithMany("AccountTransactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Customers.Entities.Feedback", b =>
                {
                    b.HasOne("Domain.Customers.Aggregate.Customer", "Customer")
                        .WithMany("Feedbacks")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.MealEntries.Aggregate.MealEntry", b =>
                {
                    b.HasOne("Domain.MealInformations.Aggregate.MealInformation", "MealInformation")
                        .WithMany("MealEntries")
                        .HasForeignKey("MealInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealInformation");
                });

            modelBuilder.Entity("Domain.Reservations.Aggregate.Reservation", b =>
                {
                    b.HasOne("Domain.Customers.Aggregate.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.MealEntries.Aggregate.MealEntry", "MealEntry")
                        .WithMany("Reservations")
                        .HasForeignKey("MealEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("MealEntry");
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.RolePermission", b =>
                {
                    b.HasOne("Infrastructure.Authentication.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Authentication.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.User", b =>
                {
                    b.HasOne("Domain.Customers.Aggregate.Customer", "Customer")
                        .WithOne()
                        .HasForeignKey("Infrastructure.Authentication.Models.User", "Id")
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Infrastructure.Authentication.Models.UserRole", b =>
                {
                    b.HasOne("Infrastructure.Authentication.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Authentication.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.ForgetPasswordHandling.Models.ForgetPasswordEntry", b =>
                {
                    b.HasOne("Infrastructure.Authentication.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Customers.Aggregate.Customer", b =>
                {
                    b.Navigation("AccountTransactions");

                    b.Navigation("Feedbacks");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Domain.MealEntries.Aggregate.MealEntry", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Domain.MealInformations.Aggregate.MealInformation", b =>
                {
                    b.Navigation("MealEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
