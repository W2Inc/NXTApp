﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NXTBackend.API.Infrastructure.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240701092956_RelationShipsTest")]
    partial class RelationShipsTest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("FeedbackId")
                        .HasColumnType("uuid")
                        .HasColumnName("feedback_id");

                    b.Property<string>("Markdown")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("markdown");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("FeedbackId");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_comment");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Cursus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("creator_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<int>("Kind")
                        .HasColumnType("integer")
                        .HasColumnName("kind");

                    b.Property<string>("Markdown")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)")
                        .HasColumnName("markdown");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("Public")
                        .HasColumnType("boolean")
                        .HasColumnName("public");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("tbl_cursus");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.CursusVertex", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("CursusId")
                        .HasColumnType("uuid")
                        .HasColumnName("cursus_id");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("CursusId");

                    b.HasIndex("ParentId");

                    b.ToTable("tbl_cursus_vertex");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Details", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("bio");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("GithubUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("github_url");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("LinkedinUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("linkedin_url");

                    b.Property<string>("TwitterUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("twitter_url");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("WebsiteUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("website_url");

                    b.HasKey("Id");

                    b.ToTable("tbl_user_details");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ActionText")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("action_text");

                    b.Property<string>("BackgroundUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("background_url");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("Href")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("href");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("title");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("tbl_event");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.EventAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<bool>("IsDismissed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_dismissed");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("tbl_event_action");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean")
                        .HasColumnName("is_public");

                    b.Property<string>("Markdown")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("markdown");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("tbl_feature");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("ReviewId")
                        .HasColumnType("uuid")
                        .HasColumnName("review_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.ToTable("tbl_feedback");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Git", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("GitBranch")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("git_branch");

                    b.Property<string>("GitCommit")
                        .HasColumnType("text")
                        .HasColumnName("git_commit");

                    b.Property<string>("GitUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("git_url");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("tbl_git");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.LearningGoal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_learning_goal");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<Guid>("GitInfoId")
                        .HasColumnType("uuid")
                        .HasColumnName("git_info_id");

                    b.Property<string>("Markdown")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("markdown");

                    b.Property<int>("MaxMembers")
                        .HasColumnType("integer")
                        .HasColumnName("max_members");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<bool>("Public")
                        .HasColumnType("boolean")
                        .HasColumnName("public");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<string[]>("Tags")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("tags");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("text")
                        .HasColumnName("thumbnail_url");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("GitInfoId");

                    b.HasIndex("OwnerId");

                    b.ToTable("tbl_project");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("FeedbackId")
                        .HasColumnType("uuid")
                        .HasColumnName("feedback_id");

                    b.Property<int>("Kind")
                        .HasColumnType("integer")
                        .HasColumnName("name");

                    b.Property<Guid?>("ReviewerId")
                        .HasColumnType("uuid")
                        .HasColumnName("reviewer_id");

                    b.Property<Guid?>("RubricId")
                        .HasColumnType("uuid")
                        .HasColumnName("rubric_id");

                    b.Property<int>("State")
                        .HasMaxLength(256)
                        .HasColumnType("integer")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UserProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_project_id");

                    b.Property<bool>("Validated")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ReviewerId");

                    b.ToTable("tbl_review");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Rubric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_rubric");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("DetailsId")
                        .HasColumnType("uuid")
                        .HasColumnName("details_id");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("DetailsId");

                    b.ToTable("tbl_user");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserGoal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_user_goal");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserProject.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("invite_state");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UserGoalId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_goal_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_project_id");

                    b.HasKey("Id");

                    b.HasIndex("UserGoalId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserProjectId");

                    b.ToTable("tbl_user_project_member");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserProject.UserProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("GitInfoId")
                        .HasColumnType("uuid")
                        .HasColumnName("git_info_id");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<Guid>("RubricId")
                        .HasColumnType("uuid")
                        .HasColumnName("rubric_id");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("GitInfoId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RubricId");

                    b.ToTable("tbl_user_project");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Comment", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Feedback", "Feedback")
                        .WithMany()
                        .HasForeignKey("FeedbackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NXTBackend.API.Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feedback");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Cursus", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.User", "Creator")
                        .WithMany("CreatedCursus")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.CursusVertex", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Cursus", "Cursus")
                        .WithMany()
                        .HasForeignKey("CursusId");

                    b.HasOne("NXTBackend.API.Domain.Entities.CursusVertex", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Cursus");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Feedback", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Review", "Review")
                        .WithMany()
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Review");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.LearningGoal", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.User", null)
                        .WithMany("CreatedGoals")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Project", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Git", "GitInfo")
                        .WithMany()
                        .HasForeignKey("GitInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NXTBackend.API.Domain.Entities.User", "Owner")
                        .WithMany("CreatedProjects")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GitInfo");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Review", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Feedback", "Feedback")
                        .WithMany()
                        .HasForeignKey("ReviewerId");

                    b.HasOne("NXTBackend.API.Domain.Entities.Rubric", "Rubric")
                        .WithMany()
                        .HasForeignKey("ReviewerId");

                    b.HasOne("NXTBackend.API.Domain.Entities.User", "Reviewer")
                        .WithMany("Rubricer")
                        .HasForeignKey("ReviewerId");

                    b.HasOne("NXTBackend.API.Domain.Entities.UserProject.UserProject", "UserProject")
                        .WithMany()
                        .HasForeignKey("ReviewerId");

                    b.Navigation("Feedback");

                    b.Navigation("Reviewer");

                    b.Navigation("Rubric");

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.Rubric", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.User", null)
                        .WithMany("CreatedRubrics")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.User", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Details", "Details")
                        .WithMany()
                        .HasForeignKey("DetailsId");

                    b.Navigation("Details");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserGoal", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.User", null)
                        .WithMany("UserGoals")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserProject.Member", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.UserGoal", "UserGoal")
                        .WithMany()
                        .HasForeignKey("UserGoalId");

                    b.HasOne("NXTBackend.API.Domain.Entities.User", "User")
                        .WithMany("ProjectMember")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NXTBackend.API.Domain.Entities.UserProject.UserProject", "UserProject")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserGoal");

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserProject.UserProject", b =>
                {
                    b.HasOne("NXTBackend.API.Domain.Entities.Git", "GitInfo")
                        .WithMany()
                        .HasForeignKey("GitInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NXTBackend.API.Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NXTBackend.API.Domain.Entities.Rubric", "Rubric")
                        .WithMany()
                        .HasForeignKey("RubricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GitInfo");

                    b.Navigation("Project");

                    b.Navigation("Rubric");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("CreatedCursus");

                    b.Navigation("CreatedGoals");

                    b.Navigation("CreatedProjects");

                    b.Navigation("CreatedRubrics");

                    b.Navigation("ProjectMember");

                    b.Navigation("Rubricer");

                    b.Navigation("UserGoals");
                });

            modelBuilder.Entity("NXTBackend.API.Domain.Entities.UserProject.UserProject", b =>
                {
                    b.Navigation("ProjectMembers");
                });
#pragma warning restore 612, 618
        }
    }
}