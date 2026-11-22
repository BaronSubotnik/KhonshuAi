using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KhonshuFront.General;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
namespace KhonshuFront.Auth;

public sealed class JwtAuthState(ProtectedLocalStorage storage) : AuthenticationStateProvider
{
    private readonly AuthenticationState anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));
    public Int32? UserId { get; private set; }
    public String? UserName { get; private set; }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ProtectedBrowserStorageResult<String> tokenProvider;
        try
        {
           tokenProvider =  await storage.GetAsync<String>(GeneralConstant.JwtTokenStorageKey);
            if (!tokenProvider.Success || String.IsNullOrWhiteSpace(tokenProvider.Value))
                return SetAnonimus();
        }
        catch
        {
            Console.WriteLine("Первый рендер");
            return SetAnonimus();
            
        }


        JwtSecurityToken jwt;

        try
        {
            jwt =  new JwtSecurityTokenHandler().ReadJwtToken(tokenProvider.Value);
        }
        catch 
        {
            return await DropTokenAsync();
        }

        
        if (jwt.ValidTo < DateTime.Now)
            return await DropTokenAsync();
        
        String? idStr = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        String? name = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (!Int32.TryParse(idStr, out Int32 idValue) || name is null)
            return await DropTokenAsync();

        UserId = idValue;
        UserName = name;

        var indent = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, idStr), new Claim(ClaimTypes.Name, name)], "jwt");
        Console.WriteLine($"id : {idStr}, name : {name}, jwt: {tokenProvider.Value}");
        return new AuthenticationState(new ClaimsPrincipal(indent));
    }

    public async Task LoginAsync(String token)
    {
        await storage.SetAsync(GeneralConstant.JwtTokenStorageKey, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    public async Task LogoutAsync()
    {
        await storage.DeleteAsync(GeneralConstant.JwtTokenStorageKey);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<AuthenticationState> DropTokenAsync()
    {
        await storage.DeleteAsync(GeneralConstant.JwtTokenStorageKey);
        return SetAnonimus();
    }

    private AuthenticationState SetAnonimus()
    {
        UserId = null;
        UserName = null;
        return anonymous;
    }
}
