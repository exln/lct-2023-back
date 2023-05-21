﻿// <auto-generated />
using System;
using MediWingWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20230521125456_23")]
    partial class _23
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MediWingWebAPI.Models.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Mkb10", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Chapter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<char>("Litera")
                        .HasColumnType("character(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int?>("Subnumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Mkb10s");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Mkb10Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Chapter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sub")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Mkb10Chapters");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Mkb10Standart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EsiliName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("boolean");

                    b.Property<string>("Mkb10Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Mkb10Standarts");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.MskEsili", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Ambulatory")
                        .HasColumnType("boolean");

                    b.Property<int?>("IdCode")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsChild")
                        .HasColumnType("boolean");

                    b.Property<int?>("LdpCode")
                        .HasColumnType("integer");

                    b.Property<string>("Modalities")
                        .HasColumnType("text");

                    b.Property<string>("MskEsiliCode")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("Stationary")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MskEsilis");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.MskEsiliAnalog", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("MskEsiliId")
                        .HasColumnType("integer");

                    b.HasKey("Name");

                    b.HasIndex("MskEsiliId");

                    b.ToTable("MskEsiliAnalog");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.MskEsiliType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MskEsiliTypes");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<long?>("Insurance")
                        .HasColumnType("bigint");

                    b.Property<string>("Lastname")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phonenumber")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.RusEsili", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Block")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<char>("Section")
                        .HasColumnType("character(1)");

                    b.Property<int>("Subnumber")
                        .HasColumnType("integer");

                    b.Property<int?>("Subsubnumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RusEsilis");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.RusEsiliBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Block")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<char>("Section")
                        .HasColumnType("character(1)");

                    b.HasKey("Id");

                    b.ToTable("RusEsiliBlocks");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.RusEsiliNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Block")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<char>("Section")
                        .HasColumnType("character(1)");

                    b.HasKey("Id");

                    b.ToTable("RusEsiliNumbers");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.RusEsiliSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<char>("Section")
                        .HasColumnType("character(1)");

                    b.HasKey("Id");

                    b.ToTable("RusEsiliSections");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Staff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StaffId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.UserDiagnosticInput", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Diagnosis")
                        .HasColumnType("text");

                    b.Property<string>("DoctorPost")
                        .HasColumnType("text");

                    b.Property<Guid>("InputId")
                        .HasColumnType("uuid");

                    b.Property<string>("MKBCode")
                        .HasColumnType("text");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer");

                    b.Property<string>("Recomendation")
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserDiagnosticInputs");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.UserInputRelation", b =>
                {
                    b.Property<Guid>("InputId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("InputName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("InputId");

                    b.ToTable("UserInputRelations");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.MskEsiliAnalog", b =>
                {
                    b.HasOne("MediWingWebAPI.Models.MskEsili", null)
                        .WithMany("Analogs")
                        .HasForeignKey("MskEsiliId");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.Staff", b =>
                {
                    b.HasOne("MediWingWebAPI.Models.User", "User")
                        .WithOne("Staff")
                        .HasForeignKey("MediWingWebAPI.Models.Staff", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.MskEsili", b =>
                {
                    b.Navigation("Analogs");
                });

            modelBuilder.Entity("MediWingWebAPI.Models.User", b =>
                {
                    b.Navigation("Staff");
                });
#pragma warning restore 612, 618
        }
    }
}
