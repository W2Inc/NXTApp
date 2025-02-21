using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .Annotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .Annotation("Npgsql:Enum:notification_kind", "default,invite,system")
                .Annotation("Npgsql:Enum:notification_state", "none,read")
                .Annotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .Annotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .Annotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");

            migrationBuilder.CreateTable(
                name: "tbl_feature",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_feature", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_git",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    git_name = table.Column<string>(type: "text", nullable: false),
                    git_namespace = table.Column<string>(type: "text", nullable: false),
                    git_url = table.Column<string>(type: "text", nullable: false),
                    git_branch = table.Column<string>(type: "text", nullable: false),
                    git_provider = table.Column<int>(type: "integer", nullable: false),
                    git_owner = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_git", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_spotlight_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    action_text = table.Column<string>(type: "text", nullable: false),
                    href = table.Column<string>(type: "text", nullable: false),
                    background_url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_spotlight_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_spotlight_event_action",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    spotlight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_dismissed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_spotlight_event_action", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LearningGoalProject",
                columns: table => new
                {
                    GoalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningGoalProject", x => new { x.GoalsId, x.ProjectsId });
                });

            migrationBuilder.CreateTable(
                name: "tbl_comment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    feedback_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_cursus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    @public = table.Column<bool>(name: "public", type: "boolean", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    track = table.Column<byte[]>(type: "bytea", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cursus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    review_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_feedback", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_learning_goal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_learning_goal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: true),
                    @public = table.Column<bool>(name: "public", type: "boolean", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    max_members = table.Column<int>(type: "integer", nullable: false),
                    git_info_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags = table.Column<string[]>(type: "text[]", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_project_tbl_git_git_info_id",
                        column: x => x.git_info_id,
                        principalTable: "tbl_git",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    validated = table.Column<bool>(type: "boolean", nullable: false),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    rubric_id = table.Column<Guid>(type: "uuid", nullable: true),
                    feedback_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_review", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_review_tbl_feedback_feedback_id",
                        column: x => x.feedback_id,
                        principalTable: "tbl_feedback",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_rubric",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    markdown = table.Column<string>(type: "text", nullable: false),
                    @public = table.Column<bool>(name: "public", type: "boolean", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    git_info_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rubric", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_rubric_tbl_git_git_info_id",
                        column: x => x.git_info_id,
                        principalTable: "tbl_git",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_rubric_tbl_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    git_info_id = table.Column<Guid>(type: "uuid", nullable: true),
                    RubricId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_project_tbl_git_git_info_id",
                        column: x => x.git_info_id,
                        principalTable: "tbl_git",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_project_tbl_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_project_tbl_rubric_RubricId",
                        column: x => x.RubricId,
                        principalTable: "tbl_rubric",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    details_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_cursus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cursus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_cursus", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_cursus_tbl_cursus_cursus_id",
                        column: x => x.cursus_id,
                        principalTable: "tbl_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_cursus_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    bio = table.Column<string>(type: "character varying(16384)", maxLength: 16384, nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    github_url = table.Column<string>(type: "text", nullable: true),
                    linkedin_url = table.Column<string>(type: "text", nullable: true),
                    twitter_url = table.Column<string>(type: "text", nullable: true),
                    website_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_details_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_notifications_tbl_notifications_notification_id",
                        column: x => x.notification_id,
                        principalTable: "tbl_notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_notifications_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_project_member",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invite_state = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_project_member", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                        column: x => x.user_project_id,
                        principalTable: "tbl_user_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_project_member_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_goal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    goal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_cursus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_goal", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                        column: x => x.goal_id,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                        column: x => x.user_cursus_id,
                        principalTable: "tbl_user_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_goal_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningGoalProject_ProjectsId",
                table: "LearningGoalProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comment_feedback_id",
                table: "tbl_comment",
                column: "feedback_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comment_user_id",
                table: "tbl_comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_creator_id",
                table: "tbl_cursus",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_name",
                table: "tbl_cursus",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_slug",
                table: "tbl_cursus",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_feedback_review_id",
                table: "tbl_feedback",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_learning_goal_creator_id",
                table: "tbl_learning_goal",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_learning_goal_name",
                table: "tbl_learning_goal",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_learning_goal_slug",
                table: "tbl_learning_goal",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notifications_created_at",
                table: "tbl_notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notifications_kind",
                table: "tbl_notifications",
                column: "kind");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_creator_id",
                table: "tbl_project",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_git_info_id",
                table: "tbl_project",
                column: "git_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_name",
                table: "tbl_project",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_slug",
                table: "tbl_project",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_review_feedback_id",
                table: "tbl_review",
                column: "feedback_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_review_reviewer_id",
                table: "tbl_review",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_review_rubric_id",
                table: "tbl_review",
                column: "rubric_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_review_user_project_id",
                table: "tbl_review",
                column: "user_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rubric_creator_id",
                table: "tbl_rubric",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rubric_git_info_id",
                table: "tbl_rubric",
                column: "git_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rubric_name",
                table: "tbl_rubric",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rubric_project_id",
                table: "tbl_rubric",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user",
                column: "details_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_display_name",
                table: "tbl_user",
                column: "display_name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user",
                column: "login");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_cursus_cursus_id",
                table: "tbl_user_cursus",
                column: "cursus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_cursus_user_id",
                table: "tbl_user_cursus",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_details_user_id",
                table: "tbl_user_details",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goal_goal_id",
                table: "tbl_user_goal",
                column: "goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goal_user_cursus_id",
                table: "tbl_user_goal",
                column: "user_cursus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goal_user_id",
                table: "tbl_user_goal",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_created_at",
                table: "tbl_user_notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_notification_id",
                table: "tbl_user_notifications",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_status",
                table: "tbl_user_notifications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_user_id",
                table: "tbl_user_notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_user_id_notification_id",
                table: "tbl_user_notifications",
                columns: new[] { "user_id", "notification_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_RubricId",
                table: "tbl_user_project",
                column: "RubricId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_git_info_id",
                table: "tbl_user_project",
                column: "git_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_project_id",
                table: "tbl_user_project",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_member_user_id",
                table: "tbl_user_project_member",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_member_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningGoalProject_tbl_learning_goal_GoalsId",
                table: "LearningGoalProject",
                column: "GoalsId",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningGoalProject_tbl_project_ProjectsId",
                table: "LearningGoalProject",
                column: "ProjectsId",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment",
                column: "feedback_id",
                principalTable: "tbl_feedback",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_user_user_id",
                table: "tbl_comment",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cursus_tbl_user_creator_id",
                table: "tbl_cursus",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_feedback_tbl_review_review_id",
                table: "tbl_feedback",
                column: "review_id",
                principalTable: "tbl_review",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_learning_goal_tbl_user_creator_id",
                table: "tbl_learning_goal",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_project_tbl_user_creator_id",
                table: "tbl_project",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review",
                column: "rubric_id",
                principalTable: "tbl_rubric",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_user_project_user_project_id",
                table: "tbl_review",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_user_reviewer_id",
                table: "tbl_review",
                column: "reviewer_id",
                principalTable: "tbl_user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_rubric_tbl_user_creator_id",
                table: "tbl_rubric",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_tbl_user_details_details_id",
                table: "tbl_user",
                column: "details_id",
                principalTable: "tbl_user_details",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_rubric_tbl_project_project_id",
                table: "tbl_rubric");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_project_project_id",
                table: "tbl_user_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_feedback_feedback_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_details_tbl_user_user_id",
                table: "tbl_user_details");

            migrationBuilder.DropTable(
                name: "LearningGoalProject");

            migrationBuilder.DropTable(
                name: "tbl_comment");

            migrationBuilder.DropTable(
                name: "tbl_feature");

            migrationBuilder.DropTable(
                name: "tbl_spotlight_event");

            migrationBuilder.DropTable(
                name: "tbl_spotlight_event_action");

            migrationBuilder.DropTable(
                name: "tbl_user_goal");

            migrationBuilder.DropTable(
                name: "tbl_user_notifications");

            migrationBuilder.DropTable(
                name: "tbl_user_project_member");

            migrationBuilder.DropTable(
                name: "tbl_learning_goal");

            migrationBuilder.DropTable(
                name: "tbl_user_cursus");

            migrationBuilder.DropTable(
                name: "tbl_notifications");

            migrationBuilder.DropTable(
                name: "tbl_cursus");

            migrationBuilder.DropTable(
                name: "tbl_project");

            migrationBuilder.DropTable(
                name: "tbl_feedback");

            migrationBuilder.DropTable(
                name: "tbl_review");

            migrationBuilder.DropTable(
                name: "tbl_user_project");

            migrationBuilder.DropTable(
                name: "tbl_rubric");

            migrationBuilder.DropTable(
                name: "tbl_git");

            migrationBuilder.DropTable(
                name: "tbl_user");

            migrationBuilder.DropTable(
                name: "tbl_user_details");
        }
    }
}
