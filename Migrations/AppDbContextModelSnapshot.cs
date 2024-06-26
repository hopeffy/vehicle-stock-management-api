﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using vehicle_stock_management_api.Models;

#nullable disable

namespace vehicle_stock_management_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("vehicle_stock_management_api.Models.Domain.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<int>("modelYear")
                        .HasColumnType("integer");

                    b.Property<DateTime>("muayeneTarihi")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("plaka")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Vehicle");
                });
#pragma warning restore 612, 618
        }
    }
}
