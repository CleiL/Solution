namespace Solution.Domain.Entitites
{
    public class Consulta
    {
        public Guid ConsultaId { get; set; }
        public DateTime DataHora { get; set; }
        public string Local { get; set; } = default!;
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
    }
}
