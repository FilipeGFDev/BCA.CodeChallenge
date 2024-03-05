﻿// <auto-generated />
using System;
using Car.Auction.Management.System.SqlServer.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Car.Auction.Management.System.SqlServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Auction.Auction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ClosedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("VehicleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId")
                        .IsUnique()
                        .HasFilter("[VehicleId] IS NOT NULL");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Bid.Bid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("AuctionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("StartingBid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("VehicleType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");

                    b.HasDiscriminator<string>("VehicleType").HasValue("Vehicle");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Vehicle.Hatchback", b =>
                {
                    b.HasBaseType("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle");

                    b.Property<int>("DoorsNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DoorsNumber");

                    b.HasDiscriminator().HasValue("Hatchback");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Vehicle.Sedan", b =>
                {
                    b.HasBaseType("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle");

                    b.Property<int>("DoorsNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DoorsNumber");

                    b.HasDiscriminator().HasValue("Sedan");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Vehicle.Suv", b =>
                {
                    b.HasBaseType("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle");

                    b.Property<int>("SeatsNumber")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Suv");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Vehicle.Truck", b =>
                {
                    b.HasBaseType("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle");

                    b.Property<int>("LoadCapacity")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Truck");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Auction.Auction", b =>
                {
                    b.HasOne("Car.Auction.Management.System.Models.Aggregates.Vehicle.Vehicle", "Vehicle")
                        .WithOne()
                        .HasForeignKey("Car.Auction.Management.System.Models.Aggregates.Auction.Auction", "VehicleId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Bid.Bid", b =>
                {
                    b.HasOne("Car.Auction.Management.System.Models.Aggregates.Auction.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("Car.Auction.Management.System.Models.Aggregates.Auction.Auction", b =>
                {
                    b.Navigation("Bids");
                });
#pragma warning restore 612, 618
        }
    }
}