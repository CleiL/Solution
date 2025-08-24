using Solution.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solution.Application.Dtos.Paciente.PacienteDto;

namespace Solution.Application.Mapping
{
    public static class PacienteMapper
    {
        public static PacienteResponseDto ToDto(this Paciente e)
            => new PacienteResponseDto
            {
                PacienteId = e.PacienteId,
                Nome = e.Nome,
                CPF = e.CPF,
                Email = e.Email,
                Phone = e.Phone
            };

        public static Paciente ToEntity(this PacienteCreateDto dto)
            => new Paciente
            {
                PacienteId = Guid.NewGuid(),
                Nome = dto.Nome.Trim(),
                CPF = dto.CPF.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim()
            };

        public static void Apply(this Paciente entity, PacienteUpdateDto dto)
        {
            entity.Nome = dto.Nome.Trim();
            entity.CPF = dto.CPF.Trim();
            entity.Email = dto.Email.Trim();
            entity.Phone = dto.Phone.Trim();
        }
    }
}
