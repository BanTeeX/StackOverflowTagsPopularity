using StackOverflowTagsPopularity.Models;

namespace StackOverflowTagsPopularity.Services.Interfaces
{
	public interface IDataAccess
	{
		Task<IEnumerable<TagModel>> GetTagsAsync(int amount);
	}
}