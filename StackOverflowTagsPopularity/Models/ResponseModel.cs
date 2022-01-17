using System.Text.Json.Serialization;

namespace StackOverflowTagsPopularity.Models
{
	public class ResponseModel<T>
	{
		[JsonPropertyName("items")] public IEnumerable<T>? Items { get; set; }
		[JsonPropertyName("has_more")] public bool? HasMore { get; set; }
		[JsonPropertyName("error_id")] public int? ErrorId { get; set; }
		[JsonPropertyName("error_name")] public string? ErrorName { get; set; }
		[JsonPropertyName("error_message")] public string? ErrorMessage { get; set; }
	}
}
