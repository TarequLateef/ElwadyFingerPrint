using ElwadyFingerPirntFo_Blazor;
using ElwadyFingerPirntFo_Blazor.Services.EmpSchema.Services;
using IunitWork;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

/*builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://24.24.24.252:903/") });*/
builder.Services.AddScoped<EmplService>();
builder.Services.AddScoped<CertService>();
await builder.Build().RunAsync();
