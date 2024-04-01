﻿// <auto-generated />
using System;
using AccessManagementService.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccessManagementService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240401201559_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccessManagementService.Persistence.Entities.EligibilityMetadataEntity", b =>
                {
                    b.Property<int>("EmployerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployerId"));

                    b.Property<string>("EmployerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EmployerId");

                    b.HasIndex("EmployerName")
                        .IsUnique();

                    b.ToTable("EligibilityMetadata");
                });

            modelBuilder.Entity("AccessManagementService.Persistence.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("EmployerId")
                        .HasColumnType("integer");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmployerId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccessManagementService.Persistence.Entities.UserEntity", b =>
                {
                    b.HasOne("AccessManagementService.Persistence.Entities.EligibilityMetadataEntity", "EligibilityMetadataEntity")
                        .WithMany("Users")
                        .HasForeignKey("EmployerId");

                    b.Navigation("EligibilityMetadataEntity");
                });

            modelBuilder.Entity("AccessManagementService.Persistence.Entities.EligibilityMetadataEntity", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
