using WebApplicationHW4.Dtos;
using WebApplicationHW4.Models;

namespace WebApplicationHW4.Abstraction
{
    public interface IUserRepository
    {
        int AddUser(UserDto user);
        RoleId CheckUser(LoginDto login);
    }
}
