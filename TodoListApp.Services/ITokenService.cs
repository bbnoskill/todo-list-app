using Microsoft.AspNetCore.Identity;

namespace TodoListApp.Services;

public interface ITokenService
{
    string CreateToken(IdentityUser user);
}
