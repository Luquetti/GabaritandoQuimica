using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Aula
{
    public class AtualizarAulaDto
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? UrlVideo { get; set; }
        public int? DuracaoMinutos { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
    }
}
