﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web_2.Data;

#nullable disable

namespace Web_2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240526185251_Update_9")]
    partial class Update_9
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Data")
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Web_2.Models.Carts.CartShoping", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CartId"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("CartId");

                    b.ToTable("CartShoping", "Data");
                });

            modelBuilder.Entity("Web_2.Models.Carts.ItemCart", b =>
                {
                    b.Property<int>("ItemCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemCartId"));

                    b.Property<int>("CartId")
                        .HasColumnType("integer");

                    b.Property<int>("CartShopingCartId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("ItemCartId");

                    b.HasIndex("CartShopingCartId");

                    b.HasIndex("ProductId");

                    b.ToTable("ItemCart", "Data");
                });

            modelBuilder.Entity("Web_2.Models.Product.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("Daycreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Decription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sellerid")
                        .HasColumnType("integer");

                    b.Property<int>("Stockquantity")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("product", "Data");
                });

            modelBuilder.Entity("Web_2.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("account")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("USER", "Data");
                });

            modelBuilder.Entity("Web_2.Models.Carts.ItemCart", b =>
                {
                    b.HasOne("Web_2.Models.Carts.CartShoping", "CartShoping")
                        .WithMany("CartItem")
                        .HasForeignKey("CartShopingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_2.Models.Product.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartShoping");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Web_2.Models.Carts.CartShoping", b =>
                {
                    b.Navigation("CartItem");
                });
#pragma warning restore 612, 618
        }
    }
}
