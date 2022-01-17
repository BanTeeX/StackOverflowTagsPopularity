using StackOverflowTagsPopularity.Models;

namespace StackOverflowTagsPopularity.Services.Interfaces
{
	public interface ITagService
	{
		IEnumerable<TagModel> Tags { get; }
	}
}