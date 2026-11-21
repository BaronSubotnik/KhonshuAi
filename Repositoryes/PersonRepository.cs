using KhonshuFront.Interfaces;
namespace KhonshuFront.Repositoryes;

public sealed class PersonRepository : IPersonRepo
{
    private readonly ILogger<PersonRepository> logger;
    private readonly HttpClient client;
    
    public PersonRepository(ILogger<PersonRepository> logger, HttpClient client)
    {
        this.logger = logger;
        this.client = client;
    }
}
