using Solution.Application.Dtos.Auth;
using Solution.Application.Interfaces;

namespace Solution.Application.Services
{
    public class AuthService
        : IAuthService
    {
        public Task<LoginResponseDto> AuthenticateAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task ConfirmRegisterAsync(RegisterDto.RegisterResponseDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterMedicoAsync(RegisterDto.RegisterMedicoDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterPacienteAsync(RegisterDto.RegisterPacienteDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
