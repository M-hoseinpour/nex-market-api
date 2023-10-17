﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using market;

#nullable disable

namespace market.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231017182242_fixPanel")]
    partial class fixPanel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("market.Data.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(120)
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Slug = "customer",
                            Title = "کاربر"
                        },
                        new
                        {
                            Id = 2,
                            Slug = "staff",
                            Title = "کارمند"
                        },
                        new
                        {
                            Id = 3,
                            Slug = "manager",
                            Title = "مدیر"
                        });
                });

            modelBuilder.Entity("market.Models.Domain.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Latitude")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Longitude")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("market.Models.Domain.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("market.Models.Domain.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DocumentUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("NationalId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("market.Models.Domain.Panel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId")
                        .IsUnique();

                    b.ToTable("Panel");
                });

            modelBuilder.Entity("market.Models.Domain.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApproverId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("NationalId")
                        .HasColumnType("integer");

                    b.Property<int>("PanelId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PanelId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("market.Models.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarLogo")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailVerifiedAt")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Setting")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateMoment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserState")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("market.Models.Domain.Address", b =>
                {
                    b.HasOne("market.Models.Domain.Customer", "Customer")
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("market.Models.Domain.Customer", b =>
                {
                    b.HasOne("market.Models.Domain.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("market.Models.Domain.Customer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("market.Models.Domain.Manager", b =>
                {
                    b.HasOne("market.Models.Domain.User", "User")
                        .WithOne("Manager")
                        .HasForeignKey("market.Models.Domain.Manager", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("market.Models.Domain.Panel", b =>
                {
                    b.HasOne("market.Models.Domain.Manager", "Manager")
                        .WithOne("Panel")
                        .HasForeignKey("market.Models.Domain.Panel", "ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("market.Models.Domain.Staff", b =>
                {
                    b.HasOne("market.Models.Domain.Panel", "Panel")
                        .WithMany("Staffs")
                        .HasForeignKey("PanelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("market.Models.Domain.User", "User")
                        .WithOne("Staff")
                        .HasForeignKey("market.Models.Domain.Staff", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Panel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("market.Models.Domain.User", b =>
                {
                    b.HasOne("market.Data.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("market.Models.Domain.Customer", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("market.Models.Domain.Manager", b =>
                {
                    b.Navigation("Panel");
                });

            modelBuilder.Entity("market.Models.Domain.Panel", b =>
                {
                    b.Navigation("Staffs");
                });

            modelBuilder.Entity("market.Models.Domain.User", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("Manager");

                    b.Navigation("Staff");
                });
#pragma warning restore 612, 618
        }
    }
}
