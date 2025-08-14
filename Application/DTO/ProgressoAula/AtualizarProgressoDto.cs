using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ProgressoAula
{
    public class AtualizarProgressoDto
    {
        public int AulaId { get; set; }
        public decimal PercentualAssistido { get; set; }
        public int TempoAssistido { get; set; }
        public bool Concluida { get; set; }
    }
}
