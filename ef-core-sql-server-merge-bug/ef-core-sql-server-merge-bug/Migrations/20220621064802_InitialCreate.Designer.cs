﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ef_core_sql_server_merge_bug;

#nullable disable

namespace ef_core_sql_server_merge_bug.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20220621064802_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ef_core_sql_server_merge_bug.ImpactValue", b =>
                {
                    b.Property<Guid>("ImpactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ImpactValueTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("DATE");

                    b.Property<int>("ImpactPeriodId")
                        .HasColumnType("int");

                    b.Property<decimal>("NormalizedValue")
                        .HasPrecision(38, 10)
                        .HasColumnType("decimal(38,10)");

                    b.Property<DateTime>("ValidFrom")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2(7)");

                    b.Property<DateTime>("ValidTo")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2(7)");

                    b.Property<decimal>("Value")
                        .HasPrecision(38, 10)
                        .HasColumnType("decimal(38,10)");

                    b.HasKey("ImpactId", "ImpactValueTypeId", "Date", "ImpactPeriodId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("ImpactId", "ImpactValueTypeId", "Date", "ImpactPeriodId"), false);

                    b.ToTable("ImpactValue", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
