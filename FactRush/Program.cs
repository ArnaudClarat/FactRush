using FactRush;
using FactRush.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddSingleton<LocalStorageService>();
builder.Services.AddScoped<TopScoreService>();
builder.Services.AddSingleton<GameState>();

await builder.Build().RunAsync();
