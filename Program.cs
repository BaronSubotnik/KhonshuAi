using KhonshuFront.Auth;
using KhonshuFront.Components;
using KhonshuFront.Interfaces;
using KhonshuFront.Repositoryes;
using KhonshuProgramingAiShared.Constatnt;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

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
