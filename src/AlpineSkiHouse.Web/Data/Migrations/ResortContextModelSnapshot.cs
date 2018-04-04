﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlpineSkiHouse.Web.Data.Migrations
{
    [DbContext(typeof(ResortContext))]
    partial class ResortContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlpineSkiHouse.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("Altitude");

                    b.Property<decimal?>("Latitude");

                    b.Property<decimal?>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("ResortId");

                    b.HasKey("Id");

                    b.HasIndex("ResortId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("AlpineSkiHouse.Models.Resort", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Resorts");
                });

            modelBuilder.Entity("AlpineSkiHouse.Models.Location", b =>
                {
                    b.HasOne("AlpineSkiHouse.Models.Resort")
                        .WithMany("Locations")
                        .HasForeignKey("ResortId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
