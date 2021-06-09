﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpotifyStalker.Data;

namespace SpotifyStalker.Data.Migrations
{
    [DbContext(typeof(SpotifyStalkerDbContext))]
    [Migration("20210609154229_Track")]
    partial class Track
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SpotifyStalker.Data.Artist", b =>
                {
                    b.Property<string>("ArtistId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ArtistName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasMaxLength(4080)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Popularity")
                        .HasColumnType("int");

                    b.HasKey("ArtistId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("SpotifyStalker.Data.ArtistQueryLog", b =>
                {
                    b.Property<string>("SearchTerm")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("QueriedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ResultCount")
                        .HasColumnType("int");

                    b.HasKey("SearchTerm");

                    b.HasIndex("SearchTerm");

                    b.ToTable("ArtistQueryLogs");
                });

            modelBuilder.Entity("SpotifyStalker.Data.Track", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double?>("Acousticness")
                        .HasColumnType("float");

                    b.Property<string>("ArtistId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double?>("Danceability")
                        .HasColumnType("float");

                    b.Property<double?>("DurationMs")
                        .HasColumnType("float");

                    b.Property<double?>("Energy")
                        .HasColumnType("float");

                    b.Property<double?>("Instrumentalness")
                        .HasColumnType("float");

                    b.Property<double?>("Key")
                        .HasColumnType("float");

                    b.Property<double?>("Liveness")
                        .HasColumnType("float");

                    b.Property<double?>("Loudness")
                        .HasColumnType("float");

                    b.Property<double?>("Mode")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double?>("Popularity")
                        .HasColumnType("float");

                    b.Property<double?>("Speechiness")
                        .HasColumnType("float");

                    b.Property<double?>("Tempo")
                        .HasColumnType("float");

                    b.Property<double?>("TimeSignature")
                        .HasColumnType("float");

                    b.Property<double?>("Valence")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Tracks");
                });
#pragma warning restore 612, 618
        }
    }
}