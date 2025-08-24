using Solution.Application.Dtos.Auth;
using static Solution.Application.Dtos.Auth.RegisterDto;


namespace Solution.Application.Interfaces
{
    internal interface IAuthService
    {
        Task<LoginResponseDto> AuthenticateAsync(LoginDto dto);
        Task<bool> RegisterPacienteAsync(RegisterPacienteDto dto);
        Task<bool> RegisterMedicoAsync(RegisterMedicoDto dto);
        Task ConfirmRegisterAsync(RegisterResponseDto dto);
    }
}
