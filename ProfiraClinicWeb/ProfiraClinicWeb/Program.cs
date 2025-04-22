using BlazorReports.Extensions;
using MudBlazor.Services;
using ProfiraClinicWeb.Client.Pages;
using ProfiraClinicWeb.Components;
using ProfiraClinicWeb.Data;
using ProfiraClinicWeb.Properties.Report;
using ProfiraClinicWeb.Services;
using ProfiraClinicWeb.Utils;

var builder = WebApplication.CreateBuilder(args);

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddBootstrapBlazor();
builder.Services.AddMudServices();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBlazorReports();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<BrowserService>();

var apiBaseAddress = builder.Configuration["ApiSettings:BaseAddress"];

builder.Services.AddHttpClient<CustomerApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddHttpClient<GroupUserApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});
builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ProfiraClinicWeb.Client._Imports).Assembly);
app.MapGroup("reports").MapBlazorReport<Report, HelloReportData>();

app.Run();
