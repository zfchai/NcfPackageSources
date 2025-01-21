﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Senparc.Xncf.AgentsManager.Models;

#nullable disable

namespace Senparc.Xncf.AgentsManager.Domain.Migrations.Dm
{
    [DbContext(typeof(AgentsManagerSenparcEntities_Dm))]
    [Migration("20250120143944_Add_AgentTemplate_FunctionCallNames")]
    partial class Add_AgentTemplate_FunctionCallNames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Domain.Models.DatabaseModel.ChatTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("AiModelId")
                        .HasColumnType("INT");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("INT");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<int>("HookPlatform")
                        .HasColumnType("INT");

                    b.Property<string>("HookPlatformParameter")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<bool>("IsPersonality")
                        .HasColumnType("BIT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR2(150)");

                    b.Property<string>("PromptCommand")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<string>("ResultComment")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<bool>("Score")
                        .HasColumnType("BIT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<int>("Status")
                        .HasColumnType("INT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.ToTable("Senparc_AgentsManager_ChatTask");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<string>("Avastar")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<bool>("Enable")
                        .HasColumnType("BIT");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<string>("FunctionCallNames")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("HookRobotParameter")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<int>("HookRobotType")
                        .HasColumnType("INT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("PromptCode")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<string>("SystemMessage")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.ToTable("Senparc_AgentsManager_AgentTemplate");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<int>("AdminAgentTemplateId")
                        .HasColumnType("INT");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<bool>("Enable")
                        .HasColumnType("BIT");

                    b.Property<int>("EnterAgentTemplateId")
                        .HasColumnType("INT");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("State")
                        .HasColumnType("INT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("AdminAgentTemplateId");

                    b.HasIndex("EnterAgentTemplateId");

                    b.ToTable("Senparc_AgentsManager_ChatGroup");
                });

            modelBuilder.Entity("Senparc.Xncf.AgentsManager.Models.DatabaseModel.Models.ChatGroupHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("INT");

                    b.Property<int>("ChatTaskId")
                        .HasColumnType("INT");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<int?>("FromAgentTemplateId")
                        .HasColumnType("INT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

                    b.Property<int>("MessageType")
                        .HasColumnType("INT");

                    b.Property<int>("MyProperty")
                        .HasColumnType("INT");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("Status")
                        .HasColumnType("INT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.Property<int?>("ToAgentTemplateId")
                        .HasColumnType("INT");

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
                        .HasColumnType("INT")
                        .HasAnnotation("Dm:ValueGenerationStrategy", DmValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("AgentTemplateId")
                        .HasColumnType("INT");

                    b.Property<int>("ChatGroupId")
                        .HasColumnType("INT");

                    b.Property<bool>("Flag")
                        .HasColumnType("BIT");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int>("TenantId")
                        .HasColumnType("INT");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(32767)");

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
                        .IsRequired();

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
                        .IsRequired();

                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "FromAgentTemplate")
                        .WithMany("FromChatGroupHistories")
                        .HasForeignKey("FromAgentTemplateId");

                    b.HasOne("Senparc.Xncf.AgentsManager.Models.DatabaseModel.AgentTemplate", "ToAgentTemplate")
                        .WithMany("ToChatGroupHistoies")
                        .HasForeignKey("ToAgentTemplateId");

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
