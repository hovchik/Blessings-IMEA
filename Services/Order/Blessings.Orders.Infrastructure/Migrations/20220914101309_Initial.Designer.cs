﻿// <auto-generated />
using System;
using Blessings.Orders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blessings.Orders.Infrastructure.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20220914101309_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Blessings.Domain.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Materials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "995",
                            Name = "Gold"
                        },
                        new
                        {
                            Id = 2,
                            Description = "850",
                            Name = "Silver"
                        },
                        new
                        {
                            Id = 3,
                            Description = "908",
                            Name = "Copper"
                        });
                });

            modelBuilder.Entity("Blessings.Domain.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Blessings.Domain.ProductItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("ProductItems");
                });

            modelBuilder.Entity("Blessings.Domain.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId")
                        .IsUnique();

                    b.ToTable("Sets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaterialId = 1,
                            Name = "Gold set for Woman"
                        },
                        new
                        {
                            Id = 2,
                            MaterialId = 2,
                            Name = "Silver set for Child"
                        },
                        new
                        {
                            Id = 3,
                            MaterialId = 3,
                            Name = "Copper set for Dogs"
                        });
                });

            modelBuilder.Entity("Blessings.Domain.Order", b =>
                {
                    b.HasOne("Blessings.Domain.Set", "Set")
                        .WithMany("Orders")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Blessings.Domain.ProductItem", b =>
                {
                    b.HasOne("Blessings.Domain.Set", null)
                        .WithMany("ProductItems")
                        .HasForeignKey("SetId");
                });

            modelBuilder.Entity("Blessings.Domain.Set", b =>
                {
                    b.HasOne("Blessings.Domain.Material", "Material")
                        .WithOne("Set")
                        .HasForeignKey("Blessings.Domain.Set", "MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");
                });

            modelBuilder.Entity("Blessings.Domain.Material", b =>
                {
                    b.Navigation("Set")
                        .IsRequired();
                });

            modelBuilder.Entity("Blessings.Domain.Set", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ProductItems");
                });
#pragma warning restore 612, 618
        }
    }
}