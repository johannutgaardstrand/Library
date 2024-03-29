﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20191108095342_V2")]
    partial class V2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IDataInterface.Bill", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<double>("AmountPayed")
                        .HasColumnType("float");

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("CustumerID")
                        .HasColumnType("int");

                    b.HasKey("BillID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("IDataInterface.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Condition")
                        .HasColumnType("tinyint");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<DateTime>("DateOfPurchase")
                        .HasColumnType("datetime2");

                    b.Property<long>("ISBN")
                        .HasColumnType("bigint");

                    b.Property<int>("ShelfID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WasteListID")
                        .HasColumnType("int");

                    b.HasKey("BookID");

                    b.HasIndex("ShelfID");

                    b.HasIndex("WasteListID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("IDataInterface.Borrow", b =>
                {
                    b.Property<int>("BorrowID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BillID")
                        .HasColumnType("int");

                    b.Property<byte>("BookConditionDecreased")
                        .HasColumnType("tinyint");

                    b.Property<int>("BookID")
                        .HasColumnType("int");

                    b.Property<bool>("BookReturned")
                        .HasColumnType("bit");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("CustumerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBorrow")
                        .HasColumnType("datetime2");

                    b.HasKey("BorrowID");

                    b.HasIndex("BillID")
                        .IsUnique();

                    b.HasIndex("BookID")
                        .IsUnique();

                    b.HasIndex("CustomerID");

                    b.ToTable("Borrows");
                });

            modelBuilder.Entity("IDataInterface.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DamageToBooks")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCustomerID")
                        .HasColumnType("int");

                    b.HasKey("CustomerID");

                    b.HasIndex("ParentCustomerID");

                    b.ToTable("Custumers");
                });

            modelBuilder.Entity("IDataInterface.Hall", b =>
                {
                    b.Property<int>("HallID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("HallID");

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("IDataInterface.Shelf", b =>
                {
                    b.Property<int>("ShelfID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HallID")
                        .HasColumnType("int");

                    b.Property<int>("ShelfNumber")
                        .HasColumnType("int");

                    b.HasKey("ShelfID");

                    b.HasIndex("HallID");

                    b.ToTable("Shelves");
                });

            modelBuilder.Entity("IDataInterface.WasteList", b =>
                {
                    b.Property<int>("WasteListID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("WasteListID");

                    b.ToTable("WasteLists");
                });

            modelBuilder.Entity("IDataInterface.Bill", b =>
                {
                    b.HasOne("IDataInterface.Customer", "Customer")
                        .WithMany("Bills")
                        .HasForeignKey("CustomerID");
                });

            modelBuilder.Entity("IDataInterface.Book", b =>
                {
                    b.HasOne("IDataInterface.Shelf", "Shelf")
                        .WithMany("Books")
                        .HasForeignKey("ShelfID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IDataInterface.WasteList", "WasteList")
                        .WithMany("Books")
                        .HasForeignKey("WasteListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IDataInterface.Borrow", b =>
                {
                    b.HasOne("IDataInterface.Bill", "Bill")
                        .WithOne("borrow")
                        .HasForeignKey("IDataInterface.Borrow", "BillID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IDataInterface.Book", "Book")
                        .WithOne("Borrow")
                        .HasForeignKey("IDataInterface.Borrow", "BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IDataInterface.Customer", "Customer")
                        .WithMany("Borrows")
                        .HasForeignKey("CustomerID");
                });

            modelBuilder.Entity("IDataInterface.Customer", b =>
                {
                    b.HasOne("IDataInterface.Customer", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentCustomerID");
                });

            modelBuilder.Entity("IDataInterface.Shelf", b =>
                {
                    b.HasOne("IDataInterface.Hall", "Hall")
                        .WithMany("Shelves")
                        .HasForeignKey("HallID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
