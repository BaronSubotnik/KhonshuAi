using KhonshuFront.General.Dto;
using KhonshuProgramingAiShared.Dto;
namespace KhonshuFront.Interfaces;

public interface IAuthRepo
{
    Task<AuthUserDto> LoginAsync(LoginDto loginDto);
    Task<AuthUserDto> RegisterAsync(RegisterDto registerDto);
}
