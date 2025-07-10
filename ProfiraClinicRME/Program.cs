using MudBlazor.Services;
using ProfiraClinicRME.Components;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.MessageHandlers;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<BrowserService>();

builder.Services.AddSingleton<INavigationRedirector, NavigationRedirector>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<AuthRedirectHandler>();
builder.Services.AddTransient<BearerTokenHandler>();

var apiBaseAddress = builder.Configuration["ApiSettings:BaseAddress"];

builder.Services.AddHttpClient<CustomerApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
})
    .AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<GroupUserApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<ClinicApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<CustomerRiwayatAsalService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<GroupPaketApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<GroupPerawatanApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<GroupBarangApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<DokterApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<PaketHeaderApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();
builder.Services.AddHttpClient<PaketDetailApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<PerawatanHeaderApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<BarangHeaderApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<ImagesApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<BearerTokenHandler>()
    .AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
