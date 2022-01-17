using StackOverflowTagsPopularity.Services;
using StackOverflowTagsPopularity.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("StackApi", client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("StackApi")));
builder.Services.AddScoped<IDataAccess, ApiDataAccess>();
builder.Services.AddScoped<ITagService, TagService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Tag/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Tag}/{action=Index}/{id?}");

app.Run();
