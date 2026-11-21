using KhonshuFront.Interfaces;
namespace KhonshuFront.Repositoryes;

public sealed class ChatRepository : IChatRepo
{
    private readonly ILogger<ChatRepository> logger;
    private readonly HttpClient client;
    
    public ChatRepository(ILogger<ChatRepository> logger, HttpClient client)
    {
        this.logger = logger;
        this.client = client;
    }
}
