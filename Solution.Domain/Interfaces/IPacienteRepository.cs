using Solution.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Domain.Interfaces
{
    public interface IPacienteRepository
        : IBaseRepository<Paciente>
    {
    }
}
