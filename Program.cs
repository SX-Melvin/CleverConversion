using CleverConversion.Configurations;
using CleverConversion.Services;
using CleverConversion.Services.REST;
using NLog.Extensions.Logging;

var config = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .Build();

NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000", "http://localhost")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.Configure<OTCSConfiguration>(builder.Configuration.GetSection("OTCS"));
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("App"));

// Add services to the container.
builder.Services.AddScoped<ViewService>();
builder.Services.AddSingleton<OTCSRestService>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
        .AddGroupDocsViewerUI();

builder.Services
        .AddControllers()
        .AddGroupDocsViewerSelfHostApi()
        .AddLocalStorage("./Files")
        .AddLocalCache("./Cache");

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
    
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapGroupDocsViewerUI(options =>
    {
        options.UIPath = "/viewer";
        options.ApiEndpoint = "/viewer-api";
    });
    endpoints.MapGroupDocsViewerApi(options =>
    {
        options.ApiPath = "/viewer-api";
    });
});

await app.RunAsync();
