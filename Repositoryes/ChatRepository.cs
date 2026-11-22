using System.Net;
using System.Net.Http.Headers;
using KhonshuFront.General;
using KhonshuFront.Interfaces;
using KhonshuProgramingAiShared.Dto;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
namespace KhonshuFront.Repositoryes;

public sealed class ChatRepository : IChatRepo
{
    private readonly ILogger<ChatRepository> logger;
    private readonly HttpClient client;
    private readonly ProtectedLocalStorage localStorage;
    
    public ChatRepository(ILogger<ChatRepository> logger, HttpClient client, ProtectedLocalStorage localStorage)
    {
        this.logger = logger;
        this.client = client;
        this.localStorage = localStorage;
    }


    public async Task<ChatMessagesDto> GetAllMessagesInChatAsync(Int32 chatId)
    {
        await AddJwtToken();
        var response = await client.GetAsync($"/api/messages/GetMessagesInChat/{chatId}");
        return (await response.Content.ReadFromJsonAsync<ChatMessagesDto>())!;
    }

    public async Task SendMessageAsync(NewMessageDto message)
    {
        await AddJwtToken();
        var response = await client.PostAsJsonAsync($"/api/messages/", message);
    }

    public async Task<ChatDto?> CreateChatAsync(CreateChatDto dto)
    {
        await AddJwtToken();
        var response = await client.PostAsJsonAsync($"/api/chats/newChat", dto);
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return null;
        
        return await response.Content.ReadFromJsonAsync<ChatDto>();
    }

    public async Task<List<ChatDto>> GetAllChatsAsync()
    {
        await AddJwtToken();
        var response = await client.GetAsync($"/api/chats/myChats");
        if(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return [];
        
        return (await response.Content.ReadFromJsonAsync<List<ChatDto>>())!;
    }

    private async Task AddJwtToken()
    {
        var tokenStorage = await localStorage.GetAsync<String>(GeneralConstant.JwtTokenStorageKey);
        if (tokenStorage.Success && tokenStorage.Value is not null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.Value);
        }
    }
}
