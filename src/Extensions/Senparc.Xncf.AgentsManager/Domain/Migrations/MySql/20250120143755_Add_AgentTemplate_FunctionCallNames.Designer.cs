﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Senparc.Xncf.AgentsManager.Models;

#nullable disable

namespace Senparc.Xncf.AgentsManager.Domain.Migrations.MySql
{
    [DbContext(typeof(AgentsManagerSenparcEntities_MySql))]
    [Migration("20250120143755_Add_AgentTemplate_FunctionCallNames")]
    partial class Add_AgentTemplate_FunctionCallNames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Domain.Models.DatabaseModel.ChatTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("AiModelId")
                        .HasColumnType("int");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Flag")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("HookPlatform")
                        .HasColumnType("int");

                    b.Property<string>("HookPlatformParameter")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPersonality")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("PromptCommand")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("ResultComment")
                        .HasColumnType("longtext");

                    b.Property<bool>("Score")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Senparc_AgentsManager_ChatTask");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Avastar")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("Enable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Flag")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FunctionCallNames")
                        .HasColumnType("longtext");

                    b.Property<string>("HookRobotParameter")
                        .HasColumnType("longtext");

                    b.Property<int>("HookRobotType")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PromptCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("SystemMessage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Senparc_AgentsManager_AgentTemplate");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("AdminAgentTemplateId")
                        .HasColumnType("int");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("Enable")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("EnterAgentTemplateId")
                        .HasColumnType("int");

                    b.Property<bool>("Flag")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminAgentTemplateId");

                    b.HasIndex("EnterAgentTemplateId");

                    b.ToTable("Senparc_AgentsManager_ChatGroup");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroupHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("int");

                    b.Property<int>("ChatTaskId")
                        .HasColumnType("int");

                    b.Property<bool>("Flag")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("FromAgentTemplateId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<int>("MyProperty")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<int?>("ToAgentTemplateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatGroupId");

                    b.HasIndex("ChatTaskId");

                    b.HasIndex("FromAgentTemplateId");

                    b.HasIndex("ToAgentTemplateId");

                    b.ToTable("Senparc_AgentsManager_ChatGroupHistory");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("AgentTemplateId")
                        .HasColumnType("int");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("int");

                    b.Property<bool>("Flag")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AgentTemplateId");

                    b.ToTable("Senparc_AgentsManager_ChatGroupMember");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroup", b =>
                {
                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "AdminAgentTemplate")
                        .WithMany("AdminChatGroups")
                        .HasForeignKey("AdminAgentTemplateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "EnterAgentTemplate")
                        .WithMany("EnterAgentChatGroups")
                        .HasForeignKey("EnterAgentTemplateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Senparc_AgentsManager_ChatGroup_Senparc_AgentsManager_Agent~1");

                    b.Navigation("AdminAgentTemplate");

                    b.Navigation("EnterAgentTemplate");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroupHistory", b =>
                {
                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroup", "ChatGroup")
                        .WithMany()
                        .HasForeignKey("ChatGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Senparc.Xncf.AgentsManager.Domain.Models.DatabaseModel.ChatTask", "ChatTask")
                        .WithMany()
                        .HasForeignKey("ChatTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Senparc_AgentsManager_ChatGroupHistory_Senparc_AgentsManage~1");

                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "FromAgentTemplate")
                        .WithMany("FromChatGroupHistories")
                        .HasForeignKey("FromAgentTemplateId")
                        .HasConstraintName("FK_Senparc_AgentsManager_ChatGroupHistory_Senparc_AgentsManage~2");

                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "ToAgentTemplate")
                        .WithMany("ToChatGroupHistoies")
                        .HasForeignKey("ToAgentTemplateId")
                        .HasConstraintName("FK_Senparc_AgentsManager_ChatGroupHistory_Senparc_AgentsManage~3");

                    b.Navigation("ChatGroup");

                    b.Navigation("ChatTask");

                    b.Navigation("FromAgentTemplate");

                    b.Navigation("ToAgentTemplate");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroupMember", b =>
                {
                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "AgentTemplate")
                        .WithMany("ChatGroupMembers")
                        .HasForeignKey("AgentTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgentTemplate");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", b =>
                {
                    b.Navigation("AdminChatGroups");

                    b.Navigation("ChatGroupMembers");

                    b.Navigation("EnterAgentChatGroups");

                    b.Navigation("FromChatGroupHistories");

                    b.Navigation("ToChatGroupHistoies");
                });
#pragma warning restore 612, 618
        }
    }
}
