namespace Solution.Domain.Entitites
{
    public class Medico
    {
        public Guid MedicoId { get; set; }
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string CRM { get; set; } = default!;
    }
}
