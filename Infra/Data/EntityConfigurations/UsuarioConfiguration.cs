using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.EntityConfigurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.SenhaHash)
                .HasColumnName("senhahash")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.TipoUsuario)
                .HasColumnName("tipousuario")
                .IsRequired();

            builder.Property(u => u.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);

            builder.Property(u => u.DataNascimento)
                .HasColumnName("datanascimento")
                .HasColumnType("date");

            builder.Property(u => u.DataCriacao)
                .HasColumnName("datacriacao")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(u => u.UltimoLogin)
                .HasColumnName("ultimologin");

            builder.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("ix_usuarios_email");

            builder.HasIndex(u => u.TipoUsuario)
                .HasDatabaseName("ix_usuarios_tipousuario");

            builder.HasIndex(u => u.Ativo)
                .HasDatabaseName("ix_usuarios_ativo");

            builder.HasMany(u => u.Matriculas)
                .WithOne(m => m.Aluno)
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ProgressoAulas)
                .WithOne(p => p.Aluno)
                .HasForeignKey(p => p.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Duvidas)
                .WithOne(d => d.Aluno)
                .HasForeignKey(d => d.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
