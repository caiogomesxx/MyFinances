using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyFinance.Infra.Context;

public partial class MyfinancesContext : DbContext
{
    public MyfinancesContext()
    {
    }

    public MyfinancesContext(DbContextOptions<MyfinancesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-44UF9P5;Initial Catalog=MYFINANCES;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__TB_USUAR__91136B906F140264");

            entity.ToTable("TB_USUARIO");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.DsEmail)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DS_EMAIL");
            entity.Property(e => e.DsNome)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DS_NOME");
            entity.Property(e => e.DsSenha)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("DS_SENHA");
            entity.Property(e => e.DsTelefone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DS_TELEFONE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
