using MudBlazor.Services;
using ProfiraClinicRME.Components;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Infra;
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

builder.Services.AddScoped<INavigationRedirector, NavigationRedirector>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<AuthRedirectHandler>();
builder.Services.AddTransient<BearerTokenHandler>();


var apiBaseAddress = builder.Configuration["ApiSettings:BaseAddress"];

builder.Services.AddHttpClient("std", httpClient =>
{
    httpClient.Timeout = TimeSpan.FromSeconds(15);

}).AddHttpMessageHandler<AuthRedirectHandler>();

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMasterDiagnosaService, MasterDiagnosaService>();
builder.Services.AddScoped<IDiagnosaService, DiagnosaService>();
builder.Services.AddScoped<IPemeriksaanUmumService, PemeriksaanUmumService>();
builder.Services.AddScoped<ICPPTService, CPPTService>();



builder.Services.AddHttpClient<AuthService>(client =>
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
