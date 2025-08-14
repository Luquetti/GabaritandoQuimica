using Domain.Entities.Aluno;
using Domain.Enum;

namespace Domain.Entities.Financeiro
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int MatriculaId { get; set; }
        public decimal ValorPagamento { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; } 
        public string? TransacaoId { get; set; }
        public string? GatewayPagamento { get; set; }
        public DateTime DataPagamento { get; set; } 
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public string? Observacoes { get; set; }

        public Matricula Matricula { get; set; } 
    }
}
