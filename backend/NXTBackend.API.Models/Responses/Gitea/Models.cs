// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Responses.Gitea;

// TODO: For now we follow the GITEA API, later on we want to abstract this
// in such a way that no matter what GIT Provider we use the internal API
// is the same and just map the objects to each other.

public class CreateRepoOption
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("private")]
	public bool Private { get; set; } = true;

	[JsonPropertyName("auto_init")]
	public bool AutoInit { get; set; } = true;

	[JsonPropertyName("has_actions")]
	public bool HasActions { get; set; } = false;

	[JsonPropertyName("has_issues")]
	public bool HasIssues { get; set; } = true;

	[JsonPropertyName("has_packages")]
	public bool HasPackages { get; set; } = false;

	[JsonPropertyName("has_projects")]
	public bool HasProjects { get; set; } = false;

	[JsonPropertyName("has_pull_requests")]
	public bool HasPullRequests { get; set; } = true;

	[JsonPropertyName("has_releases")]
	public bool HasReleases { get; set; } = false;

	[JsonPropertyName("has_wiki")]
	public bool HasWiki { get; set; } = false;

	[JsonPropertyName("default_branch")]
	public string DefaultBranch { get; set; } = "main";
}

public class EditRepoOption
{
    [JsonPropertyName("archived")]
    public bool Archived { get; set; }
}

public class FileContentRequest
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("branch")]
    public string Branch { get; set; } = "main";

    [JsonPropertyName("sha")]
    public string? Sha { get; set; }

    [JsonPropertyName("author")]
    public GitUserInfo? Author { get; set; }

    [JsonPropertyName("committer")]
    public GitUserInfo? Committer { get; set; }
}

public class GitUserInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class FileContentResponse
{
    [JsonPropertyName("commit")]
    public CommitInfo? Commit { get; set; }

    [JsonPropertyName("content")]
    public ContentInfo? Content { get; set; }
}

public class CommitInfo
{
    [JsonPropertyName("sha")]
    public string Sha { get; set; } = string.Empty;

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
}

public class ContentInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("sha")]
    public string Sha { get; set; } = string.Empty;

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;

    [JsonPropertyName("git_url")]
    public string GitUrl { get; set; } = string.Empty;

    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("encoding")]
    public string Encoding { get; set; } = string.Empty;
}
