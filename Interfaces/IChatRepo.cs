using KhonshuProgramingAiShared.Dto;
namespace KhonshuFront.Interfaces;

public interface IChatRepo
{
    Task<ChatMessagesDto> GetAllMessagesInChatAsync(Int32 chatId);
    Task SendMessageAsync(NewMessageDto message);
    Task<ChatDto?> CreateChatAsync(CreateChatDto dto);
    Task<List<ChatDto>> GetAllChatsAsync();
}
