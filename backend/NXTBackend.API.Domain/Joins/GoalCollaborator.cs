using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Domain.Joins;

[Table("rel_goalcollaborator")]
[PrimaryKey(nameof(UserId), nameof(GoalId))]
public class GoalCollaborator
{
    [Column(Order = 0)]
    public Guid UserId { get; set; }
    [Column(Order = 1)]
    public Guid GoalId { get; set; }

    public virtual User User { get; set; }
    public virtual LearningGoal Goal { get; set; }
}