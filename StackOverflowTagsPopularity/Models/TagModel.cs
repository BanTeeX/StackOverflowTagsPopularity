using System.Text.Json.Serialization;

namespace StackOverflowTagsPopularity.Models
{
	public class TagModel
	{
		[JsonPropertyName("name")] public string Name { get; set; } = "";
		[JsonPropertyName("count")] public int Count { get; set; }
		[JsonIgnore] public float Procentage { get; set; }
	}
}
