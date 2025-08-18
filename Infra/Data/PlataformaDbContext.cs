using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PlataformaDbContext : DbContext
    {
        public PlataformaDbContext(DbContextOptions<PlataformaDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Administrador> Administrador { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
                entity.Property(e => e.SenhaHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.Cidade).HasMaxLength(100);
                entity.Property(e => e.TipoUsuario).HasConversion<int>();
                entity.Property(e => e.DataCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.ToTable("alunos");
                entity.HasKey(e => e.UsuarioId);

                entity.Property(e => e.PlanoAtivo).HasMaxLength(30);
                entity.Property(e => e.StatusPagamento).HasMaxLength(20).HasDefaultValue("pendente");
                entity.Property(e => e.DataMatricula).HasDefaultValueSql("CURRENT_DATE");

                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Aluno)
                      .HasForeignKey<Aluno>(e => e.UsuarioId);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.ToTable("professores");
                entity.HasKey(e => e.UsuarioId);

                entity.Property(e => e.Materia).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DataContratacao).HasDefaultValueSql("CURRENT_DATE");

                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Professor)
                      .HasForeignKey<Professor>(e => e.UsuarioId);
            });

            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.ToTable("admins");
                entity.HasKey(e => e.UsuarioId);

                entity.Property(e => e.NivelAcesso).HasDefaultValue(1);
                entity.Property(e => e.DataPromocao).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Administrador)
                      .HasForeignKey<Administrador>(e => e.UsuarioId);
            });
        }
    }
}