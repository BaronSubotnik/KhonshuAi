using System.Net;
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

    public async Task<Boolean> CheckExistPersonByNameAsync(String name)
    {
        var response = await client.GetAsync($"/api/Person/CheckPersonByName/{name}");
        if(response.StatusCode == HttpStatusCode.OK)
            return  true;

        return false;
    }
    public async Task<Boolean> CheckExistPersonByEmailAsync(String email)
    {
        var response = await client.GetAsync($"/api/Person/CheckPersonByEmail/{email}");
        if(response.StatusCode == HttpStatusCode.OK)
            return  true;

        return false;
    }
}
