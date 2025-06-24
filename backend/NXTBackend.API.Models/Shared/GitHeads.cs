using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Shared;

public class GitRefDO
{
	[JsonPropertyName("ref")]
	public string Ref { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

	[JsonPropertyName("object")]
	public GitObjectDO Object { get; set; }

	public class GitObjectDO
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("sha")]
		public string Sha { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}
}