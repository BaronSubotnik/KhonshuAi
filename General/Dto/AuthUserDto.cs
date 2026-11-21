using KhonshuFront.General.Enums;
using KhonshuProgramingAiShared.Dto;
namespace KhonshuFront.General.Dto;

public sealed record AuthUserDto
{
    public AuthStatus Status { get; init; }
    public RegisteredUserDto? User { get; init; }
}
