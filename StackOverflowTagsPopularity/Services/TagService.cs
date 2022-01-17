using StackOverflowTagsPopularity.Models;
using StackOverflowTagsPopularity.Services.Interfaces;

namespace StackOverflowTagsPopularity.Services
{
	public class TagService : ITagService
	{
		private const int UpdateTimeSpanInMinutes = 10;
		private const int NumberOfTags = 1000;
		private readonly IDataAccess _dataSource;
		private int _countSum = 0;
		private DateTime _lastUpdateTime = DateTime.MinValue;
		private TagModel[] _tags = Array.Empty<TagModel>();

		public IEnumerable<TagModel> Tags
		{
			get
			{
				if (ShouldUpdate)
				{
					Update().Wait();
				}
				return _tags;
			}
		}
		private bool ShouldUpdate => DateTime.Now - _lastUpdateTime > TimeSpan.FromMinutes(UpdateTimeSpanInMinutes);

		public TagService(IDataAccess dataSource)
		{
			_dataSource = dataSource;
		}

		private async Task Update()
		{
			_tags = (await _dataSource.GetTagsAsync(NumberOfTags)).ToArray();
			_lastUpdateTime = DateTime.Now;
			_countSum = _tags.Sum(tag => tag.Count);
			if (_countSum == 0)
			{
				return;
			}
			foreach (TagModel tag in _tags)
			{
				tag.Procentage = tag.Count / (float)_countSum * 100f;
			}
		}
	}
}
