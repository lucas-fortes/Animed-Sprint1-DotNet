using AnimedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimedApi.Data;

public class AnimedDbContext : DbContext
{
    public AnimedDbContext(DbContextOptions<AnimedDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tutor> Tutores => Set<Tutor>();

    public DbSet<Pet> Pets => Set<Pet>();

    public DbSet<Consulta> Consultas => Set<Consulta>();

    public DbSet<Vacina> Vacinas => Set<Vacina>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(tutor => tutor.Id);

            entity.Property(tutor => tutor.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(tutor => tutor.Cpf)
                .IsRequired()
                .HasMaxLength(14);

            entity.Property(tutor => tutor.Email)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(tutor => tutor.Telefone)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(tutor => tutor.Cpf)
                .IsUnique();
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(pet => pet.Id);

            entity.Property(pet => pet.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(pet => pet.Especie)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(pet => pet.Raca)
                .HasMaxLength(80);

            entity.Property(pet => pet.Peso)
                .HasPrecision(10, 2);

            entity.HasOne(pet => pet.Tutor)
                .WithMany(tutor => tutor.Pets)
                .HasForeignKey(pet => pet.TutorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(consulta => consulta.Id);

            entity.Property(consulta => consulta.Motivo)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(consulta => consulta.Diagnostico)
                .HasMaxLength(500);

            entity.Property(consulta => consulta.Tratamento)
                .HasMaxLength(500);

            entity.Property(consulta => consulta.Observacoes)
                .HasMaxLength(1000);

            entity.Property(consulta => consulta.NivelUrgencia)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(consulta => consulta.Tutor)
                .WithMany(tutor => tutor.Consultas)
                .HasForeignKey(consulta => consulta.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(consulta => consulta.Pet)
                .WithMany(pet => pet.Consultas)
                .HasForeignKey(consulta => consulta.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vacina>(entity =>
        {
            entity.HasKey(vacina => vacina.Id);

            entity.Property(vacina => vacina.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(vacina => vacina.Observacoes)
                .HasMaxLength(500);

            entity.HasOne(vacina => vacina.Pet)
                .WithMany(pet => pet.Vacinas)
                .HasForeignKey(vacina => vacina.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}