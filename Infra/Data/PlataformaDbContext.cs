using Domain.Entities;
using Domain.Entities.Aluno;
using Domain.Entities.Conteudo;
using Domain.Entities.Financeiro;
using Domain.Entities.Questoes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PlataformaDbContext : DbContext
    {
        public PlataformaDbContext(DbContextOptions<PlataformaDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Resumo> Resumos { get; set; }
        public DbSet<BancoQuestao> BancoQuestoes { get; set; }
        public DbSet<AlternativaQuestao> AlternativasQuestao { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<ProgressoAula> ProgressosAulas { get; set; }
        public DbSet<RespostaQuestao> RespostasQuestoes { get; set; }
        public DbSet<Duvida> Duvidas { get; set; }
        public DbSet<RespostaDuvida> RespostasDuvidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações das tabelas EXATAMENTE como estão no banco
            ConfigurarUsuario(modelBuilder);
            ConfigurarSerie(modelBuilder);
            ConfigurarModulo(modelBuilder);
            ConfigurarAula(modelBuilder);
            ConfigurarResumo(modelBuilder);
            ConfigurarBancoQuestao(modelBuilder);
            ConfigurarAlternativaQuestao(modelBuilder);
            ConfigurarMatricula(modelBuilder);
            ConfigurarPagamento(modelBuilder);
            ConfigurarProgressoAula(modelBuilder);
            ConfigurarRespostaQuestao(modelBuilder);
            ConfigurarDuvida(modelBuilder);
            ConfigurarRespostaDuvida(modelBuilder);
        }

        private void ConfigurarUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
                entity.Property(e => e.SenhaHash).HasColumnName("senhahash").HasMaxLength(255).IsRequired();
                entity.Property(e => e.TipoUsuario).HasColumnName("tipousuario").IsRequired();
                entity.Property(e => e.Telefone).HasColumnName("telefone").HasMaxLength(20);
                entity.Property(e => e.DataNascimento).HasColumnName("datanascimento");
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");
                entity.Property(e => e.UltimoLogin).HasColumnName("ultimologin");
                entity.Property(e => e.Ativo).HasColumnName("ativo");

                entity.HasIndex(e => e.Email).IsUnique();
            });
        }

        private void ConfigurarSerie(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Serie>(entity =>
            {
                entity.ToTable("series");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(255);
                entity.Property(e => e.Preco).HasColumnName("preco").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Ordem).HasColumnName("ordem").IsRequired();
                entity.Property(e => e.Ativo).HasColumnName("ativo");
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");

                entity.HasIndex(e => e.Ordem).IsUnique();
            });
        }

        private void ConfigurarModulo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.ToTable("modulos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SerieId).HasColumnName("serieid").IsRequired();
                entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(500);
                entity.Property(e => e.Ordem).HasColumnName("ordem").IsRequired();
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");

                entity.HasOne<Serie>()
                      .WithMany()
                      .HasForeignKey(e => e.SerieId)
                      .HasConstraintName("fk_modulo_serie");

                entity.HasIndex(e => new { e.SerieId, e.Ordem }).IsUnique();
            });
        }

        private void ConfigurarAula(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aula>(entity =>
            {
                entity.ToTable("aulas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ModuloId).HasColumnName("moduloid").IsRequired();
                entity.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(500);
                entity.Property(e => e.UrlVideo).HasColumnName("urlvideo").HasMaxLength(255);
                entity.Property(e => e.DuracaoMinutos).HasColumnName("duracaominutos");
                entity.Property(e => e.Ordem).HasColumnName("ordem").IsRequired();
                entity.Property(e => e.Ativo).HasColumnName("ativo");
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");
                entity.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

                entity.HasOne<Modulo>()
                      .WithMany()
                      .HasForeignKey(e => e.ModuloId)
                      .HasConstraintName("fk_aula_modulo");

                entity.HasIndex(e => new { e.ModuloId, e.Ordem }).IsUnique();
            });
        }

        private void ConfigurarResumo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resumo>(entity =>
            {
                entity.ToTable("resumos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ModuloId).HasColumnName("moduloid").IsRequired();
                entity.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(150).IsRequired();
                entity.Property(e => e.ConteudoResumo).HasColumnName("conteudoresumo").IsRequired();
                entity.Property(e => e.UrlArquivoPdf).HasColumnName("urlarquivopdf").HasMaxLength(255);
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");
                entity.Property(e => e.Ativo).HasColumnName("ativo");
                entity.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

                entity.HasOne<Modulo>()
                      .WithMany()
                      .HasForeignKey(e => e.ModuloId)
                      .HasConstraintName("fk_resumo_modulo");
            });
        }

        private void ConfigurarBancoQuestao(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BancoQuestao>(entity =>
            {
                entity.ToTable("bancoquestoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ModuloId).HasColumnName("moduloid").IsRequired();
                entity.Property(e => e.Enunciado).HasColumnName("enunciado").IsRequired();
                entity.Property(e => e.TipoQuestao).HasColumnName("tipoquestao").IsRequired();
                entity.Property(e => e.Dificuldade).HasColumnName("dificuldade").IsRequired();
                entity.Property(e => e.Explicacao).HasColumnName("explicacao");
                entity.Property(e => e.DataImportacao).HasColumnName("dataimportacao");
                entity.Property(e => e.Ativo).HasColumnName("ativo");
                entity.Property(e => e.QuestaoExternaId).HasColumnName("questaoexternaid");
                entity.Property(e => e.FonteOrigem).HasColumnName("fonteorigem").HasMaxLength(100);
                entity.Property(e => e.AnoVestibular).HasColumnName("anovestibular");
                entity.Property(e => e.PontosValor).HasColumnName("pontosvalor");
                entity.Property(e => e.TagsConteudo).HasColumnName("tagsconteudo").HasMaxLength(255);

                entity.HasOne<Modulo>()
                      .WithMany()
                      .HasForeignKey(e => e.ModuloId)
                      .HasConstraintName("fk_questao_modulo");
            });
        }

        private void ConfigurarAlternativaQuestao(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlternativaQuestao>(entity =>
            {
                entity.ToTable("alternativasquestao");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.QuestaoId).HasColumnName("questaoid").IsRequired();
                entity.Property(e => e.Letra).HasColumnName("letra").HasMaxLength(1).IsRequired();
                entity.Property(e => e.Texto).HasColumnName("texto").IsRequired();
                entity.Property(e => e.Correta).HasColumnName("correta");

                entity.HasOne<BancoQuestao>()
                      .WithMany()
                      .HasForeignKey(e => e.QuestaoId)
                      .HasConstraintName("fk_alternativa_questao");
            });
        }

        private void ConfigurarMatricula(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.ToTable("matriculas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AlunoId).HasColumnName("alunoid").IsRequired();
                entity.Property(e => e.SerieId).HasColumnName("serieid").IsRequired();
                entity.Property(e => e.DataMatricula).HasColumnName("datamatricula");
                entity.Property(e => e.DataExpiracao).HasColumnName("dataexpiracao");
                entity.Property(e => e.StatusMatricula).HasColumnName("statusmatricula").IsRequired();
                entity.Property(e => e.ValorPago).HasColumnName("valorpago").HasColumnType("decimal(10,2)");
                entity.Property(e => e.TipoPlano).HasColumnName("tipoplano").IsRequired();

                entity.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(e => e.AlunoId)
                      .HasConstraintName("fk_matricula_usuario");

                entity.HasOne<Serie>()
                      .WithMany()
                      .HasForeignKey(e => e.SerieId)
                      .HasConstraintName("fk_matricula_serie");

                entity.HasIndex(e => new { e.AlunoId, e.SerieId }).IsUnique();
            });
        }

        private void ConfigurarPagamento(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("pagamentos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MatriculaId).HasColumnName("matriculaid").IsRequired();
                entity.Property(e => e.ValorPagamento).HasColumnName("valorpagamento").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.FormaPagamento).HasColumnName("formapagamento").IsRequired();
                entity.Property(e => e.StatusPagamento).HasColumnName("statuspagamento").IsRequired();
                entity.Property(e => e.TransacaoId).HasColumnName("transacaoid").HasMaxLength(100);
                entity.Property(e => e.GatewayPagamento).HasColumnName("gatewaypagamento").HasMaxLength(50);
                entity.Property(e => e.DataPagamento).HasColumnName("datapagamento");
                entity.Property(e => e.DataVencimento).HasColumnName("datavencimento");
                entity.Property(e => e.DataConfirmacao).HasColumnName("dataconfirmacao");
                entity.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(255);

                entity.HasOne<Matricula>()
                      .WithMany()
                      .HasForeignKey(e => e.MatriculaId)
                      .HasConstraintName("fk_pagamento_matricula");
            });
        }

        private void ConfigurarProgressoAula(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgressoAula>(entity =>
            {
                entity.ToTable("progressoaulas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AlunoId).HasColumnName("alunoid").IsRequired();
                entity.Property(e => e.AulaId).HasColumnName("aulaid").IsRequired();
                entity.Property(e => e.PercentualAssistido).HasColumnName("percentualassistido").HasColumnType("decimal(5,2)");
                entity.Property(e => e.Concluida).HasColumnName("concluida");
                entity.Property(e => e.TempoAssistido).HasColumnName("tempoassistido");
                entity.Property(e => e.UltimoAcesso).HasColumnName("ultimoacesso");

                entity.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(e => e.AlunoId)
                      .HasConstraintName("fk_progresso_usuario");

                entity.HasOne<Aula>()
                      .WithMany()
                      .HasForeignKey(e => e.AulaId)
                      .HasConstraintName("fk_progresso_aula");

                entity.HasIndex(e => new { e.AlunoId, e.AulaId }).IsUnique();
            });
        }

        private void ConfigurarRespostaQuestao(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RespostaQuestao>(entity =>
            {
                entity.ToTable("respostasquestoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AlunoId).HasColumnName("alunoid").IsRequired();
                entity.Property(e => e.QuestaoId).HasColumnName("questaoid").IsRequired();
                entity.Property(e => e.RespostaEscolhida).HasColumnName("respostaescolhida").HasMaxLength(1);
                entity.Property(e => e.Correto).HasColumnName("correto");
                entity.Property(e => e.PontosObtidos).HasColumnName("pontosobtidos");
                entity.Property(e => e.DataResposta).HasColumnName("dataresposta");
                entity.Property(e => e.TempoResposta).HasColumnName("temporesposta");

                entity.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(e => e.AlunoId)
                      .HasConstraintName("fk_resposta_usuario");

                entity.HasOne<BancoQuestao>()
                      .WithMany()
                      .HasForeignKey(e => e.QuestaoId)
                      .HasConstraintName("fk_resposta_questao");
            });
        }

        private void ConfigurarDuvida(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Duvida>(entity =>
            {
                entity.ToTable("duvidas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AlunoId).HasColumnName("alunoid").IsRequired();
                entity.Property(e => e.AulaId).HasColumnName("aulaid");
                entity.Property(e => e.ModuloId).HasColumnName("moduloid");
                entity.Property(e => e.QuestaoId).HasColumnName("questaoid");
                entity.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Pergunta).HasColumnName("pergunta").IsRequired();
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
                entity.Property(e => e.Prioridade).HasColumnName("prioridade").IsRequired();
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");
                entity.Property(e => e.DataResposta).HasColumnName("dataresposta");

                entity.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(e => e.AlunoId)
                      .HasConstraintName("fk_duvida_usuario");

                entity.HasOne<Aula>()
                      .WithMany()
                      .HasForeignKey(e => e.AulaId)
                      .HasConstraintName("fk_duvida_aula");

                entity.HasOne<BancoQuestao>()
                      .WithMany()
                      .HasForeignKey(e => e.QuestaoId)
                      .HasConstraintName("fk_duvida_questao");
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        private void ConfigurarRespostaDuvida(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RespostaDuvida>(entity =>
            {
                entity.ToTable("respostasduvidas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DuvidaId).HasColumnName("duvidaid").IsRequired();
                entity.Property(e => e.ProfessorId).HasColumnName("professorid").IsRequired();
                entity.Property(e => e.Resposta).HasColumnName("resposta").IsRequired();
                entity.Property(e => e.DataResposta).HasColumnName("dataresposta");
                entity.Property(e => e.Editada).HasColumnName("editada");
                entity.Property(e => e.DataEdicao).HasColumnName("dataedicao");

                entity.HasOne<Duvida>()
                      .WithMany()
                      .HasForeignKey(e => e.DuvidaId)
                      .HasConstraintName("fk_resposta_duvida");

                entity.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorId)
                      .HasConstraintName("fk_resposta_professor");
            });
        }
    }
}