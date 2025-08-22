namespace Solution.Domain.Entitites
{
    public class Agenda
    {
        public Guid AgendaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataAtendimento { get; set; }
        public string Local { get; set; } = default!;
    }
}
