﻿// <auto-generated />
using System;
using Catalog.Api.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Services.Catalog.Api.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Catalog.Api.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("category_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryOrder")
                        .HasColumnType("integer")
                        .HasColumnName("category_order");

                    b.Property<bool>("IsMiniTruck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_mini_truck");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("CategoryId")
                        .HasName("pk_categories");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Catalog.Api.Entities.Photo", b =>
                {
                    b.Property<Guid>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("photo_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("text")
                        .HasColumnName("photo_path");

                    b.Property<Guid>("TruckId")
                        .HasColumnType("uuid")
                        .HasColumnName("truck_id");

                    b.HasKey("PhotoId")
                        .HasName("pk_photos");

                    b.HasIndex("TruckId")
                        .HasDatabaseName("ix_photos_truck_id");

                    b.ToTable("photos");
                });

            modelBuilder.Entity("Catalog.Api.Entities.Truck", b =>
                {
                    b.Property<Guid>("TruckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("truck_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<bool>("Damaged")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("damaged");

                    b.Property<string>("DefaultPhotoPath")
                        .HasColumnType("text")
                        .HasColumnName("default_photo_path");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("Hidden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("hidden");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal?>("PreviousPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("previous_price");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1)
                        .HasColumnName("quantity");

                    b.Property<int>("Year")
                        .HasColumnType("integer")
                        .HasColumnName("year");

                    b.HasKey("TruckId")
                        .HasName("pk_trucks");

                    b.ToTable("trucks");
                });

            modelBuilder.Entity("CategoryTruck", b =>
                {
                    b.Property<int>("CategoriesCategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("categories_category_id");

                    b.Property<Guid>("TrucksTruckId")
                        .HasColumnType("uuid")
                        .HasColumnName("trucks_truck_id");

                    b.HasKey("CategoriesCategoryId", "TrucksTruckId")
                        .HasName("pk_category_truck");

                    b.HasIndex("TrucksTruckId")
                        .HasDatabaseName("ix_category_truck_trucks_truck_id");

                    b.ToTable("category_truck");
                });

            modelBuilder.Entity("Catalog.Api.Entities.Photo", b =>
                {
                    b.HasOne("Catalog.Api.Entities.Truck", "Truck")
                        .WithMany("Photos")
                        .HasForeignKey("TruckId")
                        .HasConstraintName("fk_photos_trucks_truck_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("CategoryTruck", b =>
                {
                    b.HasOne("Catalog.Api.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesCategoryId")
                        .HasConstraintName("fk_category_truck_categories_categories_category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.Api.Entities.Truck", null)
                        .WithMany()
                        .HasForeignKey("TrucksTruckId")
                        .HasConstraintName("fk_category_truck_trucks_trucks_truck_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catalog.Api.Entities.Truck", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
