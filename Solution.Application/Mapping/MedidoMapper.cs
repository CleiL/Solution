using Solution.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solution.Application.Dtos.Medicos.MedicosDto;

namespace Solution.Application.Mapping
{
    public static class MedidoMapper
    {
        public static MedicoResponseDto ToDto(this Medico e)
            => new MedicoResponseDto
            {
                MedicoId = e.MedicoId,
                Nome = e.Nome,
                CRM = e.CRM,
                Email = e.Email,
                Phone = e.Phone,
                Especialidade = e.Especialidade
            };

        public static Medico ToEntity(this MedicoCreateDto dto)
            => new Medico
            {
                MedicoId = Guid.NewGuid(),
                Nome = dto.Nome?.Trim() ?? string.Empty,
                CRM = dto.CRM?.Trim() ?? string.Empty,
                Email = dto.Email?.Trim() ?? string.Empty,
                Phone = dto.Phone?.Trim() ?? string.Empty,
                Especialidade = dto.Especialidade?.Trim() ?? string.Empty
            };
    }
}
