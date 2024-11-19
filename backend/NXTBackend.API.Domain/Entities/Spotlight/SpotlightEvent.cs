// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
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

[Table("tbl_spotlight_event")]
public class SpotlightEvent : BaseEntity
{
    public SpotlightEvent()
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

    /// <summary>
    ///
    /// </summary>
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("action_text")]
    public string ActionText { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("href"), Url]
    public string Href { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("background_url"), Url]
    public string BackgroundUrl { get; set; }
}
