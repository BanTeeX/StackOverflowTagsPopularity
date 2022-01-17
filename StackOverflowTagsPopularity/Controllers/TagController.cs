using Microsoft.AspNetCore.Mvc;
using StackOverflowTagsPopularity.Models;
using StackOverflowTagsPopularity.Services.Interfaces;
using System.Diagnostics;

namespace StackOverflowTagsPopularity.Controllers
{
	public class TagController : Controller
	{
		private readonly ITagService _tagService;

		public TagController(ITagService tagService)
		{
			_tagService = tagService;
		}

		public IActionResult Index()
		{
			IEnumerable<TagModel>? tags;
			try
			{
				tags = _tagService.Tags;
			}
			catch (AggregateException ex)
			{
				string message = "";
				foreach (Exception exception in ex.InnerExceptions)
				{
					message += exception.Message + '\n';
				}
				return View("ErrorMessage", message);
			}
			return View(tags);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}