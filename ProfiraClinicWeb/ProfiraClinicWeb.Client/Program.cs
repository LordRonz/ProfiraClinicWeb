using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();
builder.Services.AddBootstrapBlazor();
builder.Services.AddMudServices();
