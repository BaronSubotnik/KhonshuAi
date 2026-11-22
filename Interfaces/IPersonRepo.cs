namespace KhonshuFront.Interfaces;

public interface IPersonRepo
{
    Task<Boolean> CheckExistPersonByNameAsync(String name);
    Task<Boolean> CheckExistPersonByEmailAsync(String email);
}
