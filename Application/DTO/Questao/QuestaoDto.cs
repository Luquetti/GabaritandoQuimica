using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Questao
{
    public class QuestaoDto
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string NomeModulo { get; set; } = string.Empty;
        public string? QuestaoExternaId { get; set; }
        public string? FonteOrigem { get; set; }
        public int? AnoVestibular { get; set; }
        public string Enunciado { get; set; } = string.Empty;
        public int TipoQuestao { get; set; }
        public int Dificuldade { get; set; }
        public int PontosValor { get; set; }
        public string? Explicacao { get; set; }
        public string? TagsConteudo { get; set; }
        public List<AlternativaDto> Alternativas { get; set; } = new();

        // Para o aluno
        public bool? JaRespondida { get; set; }
        public bool? RespostaCorreta { get; set; }
    }
}
