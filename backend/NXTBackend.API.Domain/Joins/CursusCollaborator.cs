using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Domain.Joins;

[Table("rel_cursuscollaborator")]
[PrimaryKey(nameof(UserId), nameof(CursusId))]
public class CursusCollaborator
{
    [Column(Order = 0)]
    public Guid UserId { get; set; }
    [Column(Order = 1)]
    public Guid CursusId { get; set; }

    public virtual User User { get; set; }
    public virtual Cursus Cursus { get; set; }
}