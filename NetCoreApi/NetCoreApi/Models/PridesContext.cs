using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NetCoreApi.Models;

public partial class PridesContext : DbContext
{
    public PridesContext()
    {
    }

    public PridesContext(DbContextOptions<PridesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sesione> Sesiones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost; Database=Prides; Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__ROL__2A49584C5CD95B16");

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sesione>(entity =>
        {
            entity.HasKey(e => e.IdSesion);

            entity.Property(e => e.IdSesion).ValueGeneratedNever();
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Sesiones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Sesiones_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuarios_ROL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
