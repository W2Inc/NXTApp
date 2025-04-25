// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NXTBackend.API.Models.Validators;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Project;

/// <summary>
///
/// </summary>
public class ProjectPatchRequestDto : BaseRequestDTO
{
	/// <summary>
	/// The title of the project
	/// </summary>
	[StringLength(128, MinimumLength = 4)]
	public string? Name { get; set; }

	/// <summary>
	/// A short description of the project
	/// </summary>
	[StringLength(256, MinimumLength = 4)]
	public string? Description { get; set; }

	/// <summary>
	/// The markdown content of the project
	/// </summary>
	[StringLength(2048, MinimumLength = 128)]
	public string? Markdown { get; set; }

	/// <summary>
	/// The maximum amount of members that can join this project
	/// </summary>
	[Range(1, 5)]
	public int? MaxMembers { get; set; }

	/// <summary>
	/// Url to the thumbnail image of the project
	/// </summary>
	[Url]
	public string? ThumbnailUrl { get; set; }

	/// <summary>
	/// Is the project visible to the public or not
	/// </summary>
	public bool? Public { get; set; }

	/// <summary>
	/// Is the project enabled, allows for users to subscribe or not
	/// </summary>
	public bool? Enabled { get; set; }

	/// <summary>
	/// The git information (Url, branch, commit) of the project
	/// </summary>
	public GitInfoRequestDto? GitInfo { get; set; }

	/// <summary>
	/// Tags for the project
	/// /// </summary>
	// [MaxLength(24), StringLengthEnumerable(1, 64), JsonIgnore]
	public string[]? Tags { get; set; }
}
