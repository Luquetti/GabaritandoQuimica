using Domain.Entities.Financeiro;
using Domain.Enum;

namespace Domain.Entities.Aluno
{
    public class Matricula
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int SerieId { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public StatusMatricula StatusMatricula { get; set; } 
        public decimal? ValorPago { get; set; }
        public TipoPlano TipoPlano { get; set; }

        public Usuario Aluno { get; set; } 
        public Serie Serie { get; set; } 
        public List<Pagamento> Pagamentos { get; set; }
    }
}
