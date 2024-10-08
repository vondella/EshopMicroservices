﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using discount.Grpc.Data;

#nullable disable

namespace discount.Grpc.Migrations
{
    [DbContext(typeof(DiscountContext))]
    [Migration("20240506091144_initialcreate")]
    partial class initialcreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("discount.Grpc.Models.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("coupons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 150,
                            Description = "IPhone x Discount",
                            ProductName = "IPhone X"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 150,
                            Description = "Samsung Discount",
                            ProductName = "Samsung"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
