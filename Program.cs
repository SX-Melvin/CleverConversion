var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
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
