using CV_Generator.Services;
using CV_Generator.Services.Implementation;
using CV_Generator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AiOptions>(builder.Configuration.GetSection("Ai"));
builder.Services.AddHttpClient<IAiService, OpenRouterAiService>(client =>
{
    var baseUrl = builder.Configuration["Ai:BaseUrl"]
                  ?? "https://openrouter.ai/api/v1/";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(90);
});
builder.Services.AddScoped<IGeneratePromptService, GeneratePromptService>();
builder.Services.AddScoped<CVBuilder>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Cv}/{action=Index}/{id?}");
app.Run();
