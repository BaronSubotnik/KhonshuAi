using Khonshu.Constants;
using Khonshu.ObamaGemini;
using Khonshu.ObamaGemini.ObamaAgentTools;
using KhonshuFront.Auth;
using KhonshuFront.Components;
using KhonshuFront.General;
using KhonshuFront.Interfaces;
using KhonshuFront.Repositoryes;
using KhonshuProgramingAiShared.Constatnt;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.AI;


var builder = WebApplication.CreateBuilder(args);

String? apiKey = builder.Configuration["CloudFareApiKey"]!;
String? exaSearchToolKey = builder.Configuration["ExaTools"]!;
String? cloudFareAccauntId = builder.Configuration["CloudFareAccauntId"]!;

if(apiKey is null ||  exaSearchToolKey is null || cloudFareAccauntId is null)
    throw new ArgumentNullException(nameof(apiKey));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<JwtAuthState>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<JwtAuthState>());

builder.Services.AddHttpClient<IAuthRepo, AuthRepository>(options => {
    options.BaseAddress = new Uri(SharedConstant.BaseApiUrl);
});

builder.Services.AddHttpClient<IChatRepo, ChatRepository>(options => {
    options.BaseAddress = new Uri(SharedConstant.BaseApiUrl);
});

builder.Services.AddHttpClient<IPersonRepo, PersonRepository>(options => {
    options.BaseAddress = new Uri(SharedConstant.BaseApiUrl);
});


builder.Services.AddSingleton<InitialObamaAgentOptions>(options => new()
{
    AgentName = "Khonshu",
    AiModel = "@cf/openai/gpt-oss-120b",
    ApiKey =  apiKey,
    ApiServerUrl = new Uri(GeneralConstant.GetCloudFareEndpoint(cloudFareAccauntId)),
    SystemInstruction = GeneralConstant.AiSystemInstruction,
    ReasoningEffort = ReasoningEffort.High,
    Temperature = 0.3f,
    RetryPolicy = 0,
    IsWebSearch = true,
    MaxOutputToken = Int32.MaxValue
});

builder.Services.AddHttpClient<ExaWebSearchTool>(options => {
    options.BaseAddress = new Uri(ExaWebSearchTool.BaseApiUrl);
    options.DefaultRequestHeaders.Add(ExaWebSearchTool.ApiKeyHeaders, exaSearchToolKey);
});

builder.Services.AddScoped<ObamaAgentII>(options =>  {
    var optionsAi = options.GetRequiredService<InitialObamaAgentOptions>();
    var exaTools = options.GetRequiredService<ExaWebSearchTool>();
    return new ObamaAgentII(optionsAi);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// main: https://khonshu-ai.cloudpub.ru/
// api: https://khonshu-api.cloudpub.ru/