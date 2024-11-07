// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Users;

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_user")]
public class User : BaseEntity
{
    public User()
    {
        Login = string.Empty;
        DisplayName = null;
        AvatarUrl = null;
        DetailsId = null;
        Details = null;
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

    //[JsonIgnore]
    //public ICollection<UserCursus> Cursi { get; set; }

    /// <summary>
    /// The user was the evaluator on the following reviews.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Review> Rubricer { get; set; }

    /// <summary>
    /// The projects the user created.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Project> CreatedProjects { get; set; }

    /// <summary>
    /// The rubrics the user created.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Rubric> CreatedRubrics { get; set; }

    /// <summary>
    /// The Learning goals the user created.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<LearningGoal> CreatedGoals { get; set; }

    /// <summary>
    /// The cursi the user created.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Cursus> CreatedCursus { get; set; }

    /// <summary>
    /// The user goals of the user.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<UserGoal> UserGoals { get; set; }

    /// <summary>
    /// The user goals of the user.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<UserCursus> UserCursi { get; set; }

    /// <summary>
    /// The comments the user made.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// All membership instances towards a user project.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Member> ProjectMember { get; set; }
}
