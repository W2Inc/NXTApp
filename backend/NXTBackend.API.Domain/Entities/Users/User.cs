﻿// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Joins;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Users;

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_user")]
[Index(nameof(Login), IsUnique = true)]
[Index(nameof(Login), nameof(DisplayName))]
public class User : BaseEntity
{
    public User()
    {
        Login = string.Empty;
        DisplayName = null;
        AvatarUrl = null;
        DetailsId = null;
        Details = null;
        UserFeed = null;
    }

    [Column("login")]
    public string Login { get; set; }

    [Column("display_name")]
    public string? DisplayName { get; set; }

    [Column("avatar_url"), Url]
    public string? AvatarUrl { get; set; }

    [JsonIgnore, Column("details_id")]
    public Guid? DetailsId { get; set; }

    [ForeignKey(nameof(DetailsId))]
    public virtual Details? Details { get; set; }

    [JsonIgnore, Column("user_feed_id")]
    public Guid? UserFeedId { get; set; }

    [ForeignKey(nameof(UserFeedId))]
    public virtual UserFeed? UserFeed { get; set; }

    //= Relations =//

    /// <summary>
    /// The user was the evaluator on the following reviews.
    /// </summary>
    public virtual ICollection<Review> Rubricer { get; set; }

    /// <summary>
    /// The projects the user created.
    /// </summary>
    public virtual ICollection<Project> CreatedProjects { get; set; }

    /// <summary>
    /// The rubrics the user created.
    /// </summary>
    public virtual ICollection<Rubric> CreatedRubrics { get; set; }

    /// <summary>
    /// The Learning goals the user created.
    /// </summary>
    public virtual ICollection<LearningGoal> CreatedGoals { get; set; }

    /// <summary>
    /// The cursi the user created.
    /// </summary>
    public virtual ICollection<Cursus> CreatedCursus { get; set; }

    /// <summary>
    /// The user goals of the user.
    /// </summary>
    public virtual ICollection<UserGoal> UserGoals { get; set; }

    /// <summary>
    /// The user goals of the user.
    /// </summary>
    public virtual ICollection<UserCursus> UserCursi { get; set; }

    /// <summary>
    /// The comments the user made.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// All membership instances towards a user project.
    /// </summary>
    public virtual ICollection<Member> ProjectMember { get; set; }

    /// <summary>
    /// Feeds where the user is the actor
    /// </summary>
    // public virtual ICollection<Feed> CreatedFeeds { get; set; }

    //= Collaborations =//

    public virtual ICollection<CursusCollaborator> CollaboratedCursi { get; set; }
    public virtual ICollection<GoalCollaborator> CollaboratedGoals { get; set; }


    // NOTE(W2): Project collaboration is tracked via the external git source control.
    // That way we can sync the state more conveniently in case user gets added as a collaborator
    // via the frontend or directly from Gitea.
    //
    // Therefor this Many-To-Many relationship is not necessary.
    // public virtual ICollection<Project> CollaboratesOnProjects { get; set; }
}
