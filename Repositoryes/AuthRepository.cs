using System.Net;
using KhonshuFront.General.Dto;
using KhonshuFront.General.Enums;
using KhonshuFront.Interfaces;
using KhonshuProgramingAiShared.Dto;
namespace KhonshuFront.Repositoryes;

public sealed class AuthRepository : IAuthRepo
{
    private readonly ILogger<AuthRepository> logger;
    private readonly HttpClient client;
    
    public AuthRepository(ILogger<AuthRepository> logger, HttpClient client)
    {
        this.logger = logger;
        this.client = client;
    }


    public async Task<AuthUserDto> LoginAsync(LoginDto loginDto)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return new ()
            {
                User = null,
                Status = AuthStatus.NoUser
            };

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return new ()
            {
                User = null,
                Status = AuthStatus.WrongPassword
            };

        if (response.StatusCode == HttpStatusCode.OK)
            return new()
            {
                Status = AuthStatus.Success,
                User = await response.Content.ReadFromJsonAsync<RegisteredUserDto>()
            };
        
        return new()
        {
            User = null,
            Status = AuthStatus.UnknownError
        };
    }

    public async Task<AuthUserDto> RegisterAsync(RegisterDto registerDto)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", registerDto);
        if (response.StatusCode == HttpStatusCode.OK)
            return new()
            {
                Status = AuthStatus.Success,
                User = await response.Content.ReadFromJsonAsync<RegisteredUserDto>()
            };

        return new()
        {
            Status = AuthStatus.UnknownError,
            User = null
        };
    }
}
