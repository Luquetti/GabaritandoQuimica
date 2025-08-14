using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Matricula
{
    public class MatriculaDto
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string NomeAluno { get; set; }
        public int SerieId { get; set; }
        public string NomeSerie { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int StatusMatricula { get; set; }
        public decimal? ValorPago { get; set; }
        public int TipoPlano { get; set; }
        public bool AcessoAtivo { get; set; }
    }
}