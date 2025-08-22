namespace Solution.Domain.Entitites
{
    public class Paciente
    {
        public Guid PacienteId { get; set; }
        public string Nome { get; set; } = default!;
        public string CPF { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

    }
}
