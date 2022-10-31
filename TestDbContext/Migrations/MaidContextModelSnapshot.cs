﻿// <auto-generated />
using System;
using MaidContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MaidContexts.Migrations
{
    [DbContext(typeof(MaidContext))]
    partial class MaidContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.CodeMaid.AttributeDefinition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");

                    b.Property<string>("Arguments")
                        .HasColumnType("longtext")
                        .HasComment("参数");

                    b.Property<string>("ArgumentsText")
                        .HasColumnType("longtext")
                        .HasComment("参数文本");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("Attribute名称");

                    b.Property<long>("PropertyDefinitionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("Attribute文本");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

                    b.HasKey("Id");

                    b.HasIndex("PropertyDefinitionId");

                    b.ToTable("AttributeDefinitions");

                    b.HasComment("类定义");
                });

            modelBuilder.Entity("Models.CodeMaid.ClassDefinition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");

                    b.Property<string>("Base")
                        .HasColumnType("longtext")
                        .HasComment("基类或者接口名称");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

                    b.Property<long?>("MaidId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("类名");

                    b.Property<string>("NameSpace")
                        .HasColumnType("longtext")
                        .HasComment("命名空间");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext")
                        .HasComment("注释");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

                    b.HasKey("Id");

                    b.HasIndex("MaidId");

                    b.ToTable("ClassDefinitions");

                    b.HasComment("类定义");
                });

            modelBuilder.Entity("Models.CodeMaid.Maid", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");

                    b.Property<bool>("Autonomous")
                        .HasColumnType("tinyint(1)")
                        .HasComment("是否自动修复");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

                    b.Property<string>("DestinationPath")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("目标路径");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

                    b.Property<int>("MaidWork")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("名称");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint");

                    b.Property<string>("SourcePath")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("原路径");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Maids");

                    b.HasComment("功能");
                });

            modelBuilder.Entity("Models.CodeMaid.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("项目名");

                    b.Property<string>("Path")
                        .HasColumnType("longtext")
                        .HasComment("项目路径");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

                    b.HasKey("Id");

                    b.ToTable("Projects");

                    b.HasComment("项目定义");
                });

            modelBuilder.Entity("Models.CodeMaid.PropertyDefinition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");

                    b.Property<long>("ClassDefinitionId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

                    b.Property<string>("FullText")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("完整文本内容");

                    b.Property<string>("Get")
                        .HasColumnType("longtext")
                        .HasComment("Get方法体");

                    b.Property<bool>("HasGet")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("HasSet")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Initializer")
                        .HasColumnType("longtext")
                        .HasComment("初始化器");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

                    b.Property<string>("LeadingTrivia")
                        .HasColumnType("longtext")
                        .HasComment("前导");

                    b.Property<string>("Modifiers")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("修饰符");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("属性名称");

                    b.Property<string>("Set")
                        .HasColumnType("longtext")
                        .HasComment("Set方法体");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext")
                        .HasComment("注释");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("数据类型");

                    b.Property<DateTimeOffset>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

                    b.HasKey("Id");

                    b.HasIndex("ClassDefinitionId");

                    b.ToTable("PropertyDefinitions");

                    b.HasComment("类定义");
                });

            modelBuilder.Entity("Models.CodeMaid.AttributeDefinition", b =>
                {
                    b.HasOne("Models.CodeMaid.PropertyDefinition", "PropertyDefinition")
                        .WithMany("Attributes")
                        .HasForeignKey("PropertyDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PropertyDefinition");
                });

            modelBuilder.Entity("Models.CodeMaid.ClassDefinition", b =>
                {
                    b.HasOne("Models.CodeMaid.Maid", null)
                        .WithMany("Classes")
                        .HasForeignKey("MaidId");
                });

            modelBuilder.Entity("Models.CodeMaid.Maid", b =>
                {
                    b.HasOne("Models.CodeMaid.Project", "Project")
                        .WithMany("Maids")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Models.CodeMaid.PropertyDefinition", b =>
                {
                    b.HasOne("Models.CodeMaid.ClassDefinition", "ClassDefinition")
                        .WithMany("Properties")
                        .HasForeignKey("ClassDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassDefinition");
                });

            modelBuilder.Entity("Models.CodeMaid.ClassDefinition", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("Models.CodeMaid.Maid", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Models.CodeMaid.Project", b =>
                {
                    b.Navigation("Maids");
                });

            modelBuilder.Entity("Models.CodeMaid.PropertyDefinition", b =>
                {
                    b.Navigation("Attributes");
                });
#pragma warning restore 612, 618
        }
    }
}
