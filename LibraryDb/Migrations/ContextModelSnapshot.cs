﻿// <auto-generated />
using System;
using LibraryDb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryDb.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthorISBN", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("ISBNsId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "ISBNsId");

                    b.HasIndex("ISBNsId");

                    b.ToTable("AuthorISBN");
                });

            modelBuilder.Entity("LibraryDb.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LibraryDb.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Borrowed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("BorrowedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("ISBNId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ISBNId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryDb.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PIN")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("LibraryDb.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderHistoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId")
                        .IsUnique();

                    b.HasIndex("OrderHistoryId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("LibraryDb.Models.ISBN", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderHistoryId")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderHistoryId");

                    b.ToTable("ISBNs");
                });

            modelBuilder.Entity("LibraryDb.Models.OrderHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("OrderHistories");
                });

            modelBuilder.Entity("AuthorISBN", b =>
                {
                    b.HasOne("LibraryDb.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryDb.Models.ISBN", null)
                        .WithMany()
                        .HasForeignKey("ISBNsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryDb.Models.Book", b =>
                {
                    b.HasOne("LibraryDb.Models.Customer", "Customer")
                        .WithMany("Books")
                        .HasForeignKey("CustomerId");

                    b.HasOne("LibraryDb.Models.ISBN", "ISBN")
                        .WithMany("Books")
                        .HasForeignKey("ISBNId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("ISBN");
                });

            modelBuilder.Entity("LibraryDb.Models.Customer", b =>
                {
                    b.HasOne("LibraryDb.Models.Card", "Card")
                        .WithOne("Customer")
                        .HasForeignKey("LibraryDb.Models.Customer", "CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryDb.Models.OrderHistory", null)
                        .WithMany("Customers")
                        .HasForeignKey("OrderHistoryId");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("LibraryDb.Models.ISBN", b =>
                {
                    b.HasOne("LibraryDb.Models.OrderHistory", null)
                        .WithMany("ISBNs")
                        .HasForeignKey("OrderHistoryId");
                });

            modelBuilder.Entity("LibraryDb.Models.Card", b =>
                {
                    b.Navigation("Customer")
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryDb.Models.Customer", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryDb.Models.ISBN", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryDb.Models.OrderHistory", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("ISBNs");
                });
#pragma warning restore 612, 618
        }
    }
}
