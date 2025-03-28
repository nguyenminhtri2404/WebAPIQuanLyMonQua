﻿// <auto-generated />
using System;
using GiftManagement_Version2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GiftManagement_Version2.Migrations
{
    [DbContext(typeof(MyDBContext))]
    [Migration("20240904030850_Fix_DB3")]
    partial class Fix_DB3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GiftManagement_Version2.Data.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GiftId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GiftId");

                    b.HasIndex("UserId", "GiftId")
                        .IsUnique();

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Gift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("GiftType")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Order", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<int>("GiftId")
                        .HasColumnType("int");

                    b.Property<int?>("MainGiftId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderType")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("GiftId");

                    b.HasIndex("MainGiftId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Permission", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("char(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Code");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Code = "CreateUser",
                            Name = "CreateUser"
                        },
                        new
                        {
                            Code = "ViewUser",
                            Name = "ViewUser"
                        },
                        new
                        {
                            Code = "UpdateteUser",
                            Name = "UpdateteUser"
                        },
                        new
                        {
                            Code = "DeleteUser",
                            Name = "DeleteUser"
                        });
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BonusGiftId")
                        .HasColumnType("int");

                    b.Property<int>("MainGiftId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BonusGiftId");

                    b.HasIndex("MainGiftId", "BonusGiftId")
                        .IsUnique();

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IsUseAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("PermissionCode")
                        .HasColumnType("char(25)");

                    b.HasKey("RoleId", "PermissionCode");

                    b.HasIndex("PermissionCode");

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionCode = "CreateUser"
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionCode = "ViewUser"
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionCode = "UpdateteUser"
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionCode = "DeleteUser"
                        });
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("Point")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@example.com",
                            Password = "$2a$11$Yb6RrP6P/CCzpfHu96EkTunkG3Bg8z8i0/SWBlf6iOLfttcdxXAfW",
                            Phone = "0345567810",
                            Point = 0,
                            RoleId = 1,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Cart", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.Gift", "Gift")
                        .WithMany("Carts")
                        .HasForeignKey("GiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GiftManagement_Version2.Data.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gift");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Order", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.Gift", "Gift")
                        .WithMany("Orders")
                        .HasForeignKey("GiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GiftManagement_Version2.Data.Order", "MainGift")
                        .WithMany()
                        .HasForeignKey("MainGiftId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GiftManagement_Version2.Data.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gift");

                    b.Navigation("MainGift");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Promotion", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.Gift", "BonusGift")
                        .WithMany()
                        .HasForeignKey("BonusGiftId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GiftManagement_Version2.Data.Gift", "MainGift")
                        .WithMany("Promotions")
                        .HasForeignKey("MainGiftId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BonusGift");

                    b.Navigation("MainGift");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.RefreshToken", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.RolePermission", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GiftManagement_Version2.Data.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.User", b =>
                {
                    b.HasOne("GiftManagement_Version2.Data.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Gift", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");

                    b.Navigation("Promotions");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("GiftManagement_Version2.Data.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
