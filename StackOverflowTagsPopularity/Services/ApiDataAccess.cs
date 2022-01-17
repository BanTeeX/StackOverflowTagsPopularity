using StackOverflowTagsPopularity.Exceptions;
using StackOverflowTagsPopularity.Models;
using StackOverflowTagsPopularity.Services.Interfaces;
using System.IO.Compression;
using System.Text.Json;

namespace StackOverflowTagsPopularity.Services
{
	public class ApiDataAccess : IDataAccess
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly ILogger<ApiDataAccess> _logger;

		public ApiDataAccess(IHttpClientFactory clientFactory, ILogger<ApiDataAccess> logger)
		{
			_clientFactory = clientFactory;
			_logger = logger;
		}

		private async Task<ResponseModel<T>?> GetRequestAsync<T>(string path)
		{
			try
			{
				using HttpClient client = _clientFactory.CreateClient("StackApi");
				using HttpResponseMessage responseMessage = await client.GetAsync(path);
				using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();
				using GZipStream zipStream = new(responseStream, CompressionMode.Decompress);
				using StreamReader responseReader = new(zipStream);
				return JsonSerializer.Deserialize<ResponseModel<T>>(responseReader.ReadToEnd());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while requesting data");
				throw new ErrorException("Error while requesting data", ex);
			}
		}

		public async Task<IEnumerable<TagModel>> GetTagsAsync(int amount)
		{
			int pageSize = 100;
			List<TagModel> tags = new();
			for (int i = 0; i <= amount / pageSize; i++)
			{
				ResponseModel<TagModel>? responseModel = await GetRequestAsync<TagModel>($"tags?page={i + 1}&pagesize={pageSize}&odrer=desc&sort=popular&site=stackoverflow");
				if (responseModel is null)
				{
					continue;
				}
				if (responseModel.ErrorId is not null)
				{
					_logger.LogError($"Api error {responseModel.ErrorId} {responseModel.ErrorName}: {responseModel.ErrorMessage}");
					throw new ErrorException("Api error");
				}
				if (responseModel.Items is null)
				{
					continue;
				}
				tags.AddRange(responseModel.Items);
				if (responseModel.HasMore is null || !(bool)responseModel.HasMore)
				{
					break;
				}
			}
			return tags.GetRange(0, amount > tags.Count ? tags.Count : amount);
		}
	}
}
