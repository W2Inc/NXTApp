// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Notification;

/*
model Event {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    title          String  @unique @db.VarChar(64)
    description    String  @db.VarChar(256)
    action_text    String  @db.VarChar(64)
    href           String
    background_url String?

    @@map("event")
}
*/

[Table("tbl_notification")]
public class Notification : BaseEntity
{
    public Notification()
    {
        Title = string.Empty;
        Description = string.Empty;
        ActionText = string.Empty;
        Href = string.Empty;
        BackgroundUrl = string.Empty;
    }

    /// <summary>
    /// The title of the event
    /// </summary>
    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("action_text")]
    public string ActionText { get; set; }

    [Column("href"), Url]
    public string Href { get; set; }

    [Column("background_url"), Url]
    public string BackgroundUrl { get; set; }
}
