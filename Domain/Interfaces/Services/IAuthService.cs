using Domain.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
