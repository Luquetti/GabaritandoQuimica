using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Modulo
{
    public class AtualizarModuloDto
    {
        public int Id { get; set; }
        public int SerieId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
    }
}
