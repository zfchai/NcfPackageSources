﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Senparc.Xncf.FileManager.Models;

#nullable disable

namespace Senparc.Xncf.FileManager.Domain.Migrations.Dm
{
    [DbContext(typeof(FileManagerSenparcEntities_Dm))]
    partial class FileManagerSenparcEntities_DmModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("Senparc.Xncf.FileManager.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("AdditionNote")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("Blue")
                        .HasColumnType("INT");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<int>("Green")
                        .HasColumnType("INT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<int>("Red")
                        .HasColumnType("INT");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.ToTable("Senparc_FileManager_Color");
                });
#pragma warning restore 612, 618
        }
    }
}
