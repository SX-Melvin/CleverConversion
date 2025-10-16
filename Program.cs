using CleverConversion.Common.Annotation;
using CleverConversion.Common.Common.Config;
using CleverConversion.Common.Comparison.Comparison.Service;
using CleverConversion.Common.Viewer;
using CleverConversion.Common.Viewer.Caching;
using CleverConversion.Configurations;
using CleverConversion.Services;
using CleverConversion.Services.REST;
using NLog.Extensions.Logging;

var config = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .Build();

NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

GroupDocs.Total.License.SetLicense("./License/GroupDocs.Totalfor.NET.lic");

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OTCSConfiguration>(builder.Configuration.GetSection("OTCS"));
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("App"));

// Add services to the container.
builder.Services.AddScoped<ViewService>();
builder.Services.AddScoped<IViewer, CachingViewer>();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddScoped<UIConfig>();
builder.Services.AddScoped<OTCSService>();
builder.Services.AddSingleton<OTCSRestService>();

builder.Services.AddRazorPages();

builder.Services
        .AddGroupDocsViewerUI();

builder.Services
        .AddControllers()
        .AddGroupDocsViewerSelfHostApi()
        .AddLocalStorage("./Files")
        .AddLocalCache("./Cache");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAuthorized", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

});
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAuthorized");

app.UseAuthorization();

app.MapRazorPages();

app.MapGroupDocsViewerUI(options =>
{
    //options.AddCustomStylesheet("./wwwroot/css/custom.css");
    //options.UIPath = "/viewer";
    //options.ApiEndpoint = "/viewer-api";
});

app.MapGroupDocsViewerApi(options =>
{
    options.ApiPath = "/viewer-api";
});

await app.RunAsync();
