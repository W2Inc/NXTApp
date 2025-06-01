using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Domain.Joins;

[Table("rel_goalproject")]
[PrimaryKey(nameof(ProjectId), nameof(GoalId))]
public class GoalProject
{
    [Column(Order = 0)]
    public Guid GoalId { get; set; }
    
    [Column(Order = 1)]
    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; }
    public virtual LearningGoal Goal { get; set; }
}