using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ADT.UI;
using ADT.UI.Services;
using ADT.UI.Services.Contracts;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazorise(options =>
                              {
                                  options.Immediate = true;
                              })
                              .AddBootstrapProviders()
                              .AddFontAwesomeIcons();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7252/") });

builder.Services.AddScoped<IUserProfileService, UserProfileService>();

await builder.Build().RunAsync();