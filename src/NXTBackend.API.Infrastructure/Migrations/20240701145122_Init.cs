using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "tbl_event",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                action_text = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                href = table.Column<string>(type: "text", nullable: false),
                background_url = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_event", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tbl_event_action",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                event_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_dismissed = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_event_action", x => x.id);
            });

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
                git_url = table.Column<string>(type: "text", nullable: false),
                git_branch = table.Column<string>(type: "text", nullable: false),
                git_commit = table.Column<string>(type: "text", nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_git", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tbl_user_details",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                email = table.Column<string>(type: "text", nullable: false),
                bio = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                first_name = table.Column<string>(type: "text", nullable: false),
                last_name = table.Column<string>(type: "text", nullable: false),
                github_url = table.Column<string>(type: "text", nullable: false),
                linkedin_url = table.Column<string>(type: "text", nullable: false),
                twitter_url = table.Column<string>(type: "text", nullable: false),
                website_url = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_user_details", x => x.id);
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
                table.ForeignKey(
                    name: "FK_tbl_user_tbl_user_details_details_id",
                    column: x => x.details_id,
                    principalTable: "tbl_user_details",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "tbl_cursus",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                markdown = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                slug = table.Column<string>(type: "text", nullable: false),
                @public = table.Column<bool>(name: "public", type: "boolean", nullable: false),
                enabled = table.Column<bool>(type: "boolean", nullable: false),
                kind = table.Column<int>(type: "integer", nullable: false),
                creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_cursus", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_cursus_tbl_user_creator_id",
                    column: x => x.creator_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
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
                table.ForeignKey(
                    name: "FK_tbl_learning_goal_tbl_user_creator_id",
                    column: x => x.creator_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
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
                owner_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                table.ForeignKey(
                    name: "FK_tbl_project_tbl_user_owner_id",
                    column: x => x.owner_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
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
            name: "tbl_cursus_vertex",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                cursus_id = table.Column<Guid>(type: "uuid", nullable: true),
                LearningGoalId = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_cursus_vertex", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_cursus_vertex_tbl_cursus_cursus_id",
                    column: x => x.cursus_id,
                    principalTable: "tbl_cursus",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_cursus_vertex_tbl_cursus_vertex_parent_id",
                    column: x => x.parent_id,
                    principalTable: "tbl_cursus_vertex",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_cursus_vertex_tbl_learning_goal_LearningGoalId",
                    column: x => x.LearningGoalId,
                    principalTable: "tbl_learning_goal",
                    principalColumn: "id");
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
                table.ForeignKey(
                    name: "FK_LearningGoalProject_tbl_learning_goal_GoalsId",
                    column: x => x.GoalsId,
                    principalTable: "tbl_learning_goal",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_LearningGoalProject_tbl_project_ProjectsId",
                    column: x => x.ProjectsId,
                    principalTable: "tbl_project",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "rubric",
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
                table.PrimaryKey("PK_rubric", x => x.id);
                table.ForeignKey(
                    name: "FK_rubric_tbl_git_git_info_id",
                    column: x => x.git_info_id,
                    principalTable: "tbl_git",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_rubric_tbl_project_project_id",
                    column: x => x.project_id,
                    principalTable: "tbl_project",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_rubric_tbl_user_creator_id",
                    column: x => x.creator_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "tbl_user_goal",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                goal_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_cursus_id = table.Column<Guid>(type: "uuid", nullable: true),
                vertex_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_user_goal", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_user_goal_tbl_cursus_vertex_vertex_id",
                    column: x => x.vertex_id,
                    principalTable: "tbl_cursus_vertex",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                    column: x => x.goal_id,
                    principalTable: "tbl_learning_goal",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                    column: x => x.user_cursus_id,
                    principalTable: "tbl_user_cursus",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_user_goal_tbl_user_user_id",
                    column: x => x.user_id,
                    principalTable: "tbl_user",
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
                git_info_id = table.Column<Guid>(type: "uuid", nullable: false),
                rubric_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_user_project", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_user_project_rubric_rubric_id",
                    column: x => x.rubric_id,
                    principalTable: "rubric",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_user_project_tbl_git_git_info_id",
                    column: x => x.git_info_id,
                    principalTable: "tbl_git",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_user_project_tbl_project_project_id",
                    column: x => x.project_id,
                    principalTable: "tbl_project",
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
                user_goal_id = table.Column<Guid>(type: "uuid", nullable: true),
                user_project_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_user_project_member", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_user_project_member_tbl_user_goal_user_goal_id",
                    column: x => x.user_goal_id,
                    principalTable: "tbl_user_goal",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                    column: x => x.user_project_id,
                    principalTable: "tbl_user_project",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_user_project_member_tbl_user_user_id",
                    column: x => x.user_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
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
                table.ForeignKey(
                    name: "FK_tbl_comment_tbl_user_user_id",
                    column: x => x.user_id,
                    principalTable: "tbl_user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
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
            name: "tbl_review",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<int>(type: "integer", nullable: false),
                description = table.Column<int>(type: "integer", maxLength: 256, nullable: false),
                Validated = table.Column<bool>(type: "boolean", nullable: false),
                reviewer_id = table.Column<Guid>(type: "uuid", nullable: true),
                rubric_id = table.Column<Guid>(type: "uuid", nullable: true),
                feedback_id = table.Column<Guid>(type: "uuid", nullable: true),
                user_project_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_review", x => x.id);
                table.ForeignKey(
                    name: "FK_tbl_review_rubric_reviewer_id",
                    column: x => x.reviewer_id,
                    principalTable: "rubric",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_review_tbl_feedback_reviewer_id",
                    column: x => x.reviewer_id,
                    principalTable: "tbl_feedback",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_review_tbl_user_project_reviewer_id",
                    column: x => x.reviewer_id,
                    principalTable: "tbl_user_project",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_tbl_review_tbl_user_reviewer_id",
                    column: x => x.reviewer_id,
                    principalTable: "tbl_user",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_LearningGoalProject_ProjectsId",
            table: "LearningGoalProject",
            column: "ProjectsId");

        migrationBuilder.CreateIndex(
            name: "IX_rubric_creator_id",
            table: "rubric",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "IX_rubric_git_info_id",
            table: "rubric",
            column: "git_info_id");

        migrationBuilder.CreateIndex(
            name: "IX_rubric_project_id",
            table: "rubric",
            column: "project_id");

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
            name: "IX_tbl_cursus_vertex_LearningGoalId",
            table: "tbl_cursus_vertex",
            column: "LearningGoalId");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_cursus_vertex_cursus_id",
            table: "tbl_cursus_vertex",
            column: "cursus_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_cursus_vertex_parent_id",
            table: "tbl_cursus_vertex",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_feedback_review_id",
            table: "tbl_feedback",
            column: "review_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_learning_goal_creator_id",
            table: "tbl_learning_goal",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_project_git_info_id",
            table: "tbl_project",
            column: "git_info_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_project_owner_id",
            table: "tbl_project",
            column: "owner_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_review_reviewer_id",
            table: "tbl_review",
            column: "reviewer_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_details_id",
            table: "tbl_user",
            column: "details_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_cursus_cursus_id",
            table: "tbl_user_cursus",
            column: "cursus_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_cursus_user_id",
            table: "tbl_user_cursus",
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
            name: "IX_tbl_user_goal_vertex_id",
            table: "tbl_user_goal",
            column: "vertex_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_git_info_id",
            table: "tbl_user_project",
            column: "git_info_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_project_id",
            table: "tbl_user_project",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_rubric_id",
            table: "tbl_user_project",
            column: "rubric_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_member_user_goal_id",
            table: "tbl_user_project_member",
            column: "user_goal_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_member_user_id",
            table: "tbl_user_project_member",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_user_project_member_user_project_id",
            table: "tbl_user_project_member",
            column: "user_project_id");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_comment_tbl_feedback_feedback_id",
            table: "tbl_comment",
            column: "feedback_id",
            principalTable: "tbl_feedback",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_feedback_tbl_review_review_id",
            table: "tbl_feedback",
            column: "review_id",
            principalTable: "tbl_review",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_rubric_tbl_project_project_id",
            table: "rubric");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_user_project_tbl_project_project_id",
            table: "tbl_user_project");

        migrationBuilder.DropForeignKey(
            name: "FK_rubric_tbl_git_git_info_id",
            table: "rubric");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_user_project_tbl_git_git_info_id",
            table: "tbl_user_project");

        migrationBuilder.DropForeignKey(
            name: "FK_rubric_tbl_user_creator_id",
            table: "rubric");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_review_tbl_user_reviewer_id",
            table: "tbl_review");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_review_tbl_feedback_reviewer_id",
            table: "tbl_review");

        migrationBuilder.DropTable(
            name: "LearningGoalProject");

        migrationBuilder.DropTable(
            name: "tbl_comment");

        migrationBuilder.DropTable(
            name: "tbl_event");

        migrationBuilder.DropTable(
            name: "tbl_event_action");

        migrationBuilder.DropTable(
            name: "tbl_feature");

        migrationBuilder.DropTable(
            name: "tbl_user_project_member");

        migrationBuilder.DropTable(
            name: "tbl_user_goal");

        migrationBuilder.DropTable(
            name: "tbl_cursus_vertex");

        migrationBuilder.DropTable(
            name: "tbl_user_cursus");

        migrationBuilder.DropTable(
            name: "tbl_learning_goal");

        migrationBuilder.DropTable(
            name: "tbl_cursus");

        migrationBuilder.DropTable(
            name: "tbl_project");

        migrationBuilder.DropTable(
            name: "tbl_git");

        migrationBuilder.DropTable(
            name: "tbl_user");

        migrationBuilder.DropTable(
            name: "tbl_user_details");

        migrationBuilder.DropTable(
            name: "tbl_feedback");

        migrationBuilder.DropTable(
            name: "tbl_review");

        migrationBuilder.DropTable(
            name: "tbl_user_project");

        migrationBuilder.DropTable(
            name: "rubric");
    }
}
