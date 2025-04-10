using Hackaton.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infrastructure.Data
{
    public class HackatonDbContext : DbContext
    {
        public HackatonDbContext(DbContextOptions<HackatonDbContext> options) : base(options)
        {
        }
        
        public DbSet<Medico> Medicos { get; set; } = null!;
        public DbSet<Paciente> Pacientes { get; set; } = null!;
        public DbSet<Agenda> Agendas { get; set; } = null!;
        public DbSet<Consulta> Consultas { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da entidade Medico
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CRM).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Especialidade).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ValorConsulta).HasColumnType("decimal(18,2)");
            });
            
            // Configuração da entidade Paciente
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.DataNascimento).IsRequired();
            });
            
            // Configuração da entidade Agenda
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataHoraInicio).IsRequired();
                entity.Property(e => e.DataHoraFim).IsRequired();
                entity.Property(e => e.Disponivel).IsRequired();
                
                // Relacionamento com Medico
                entity.HasOne(e => e.Medico)
                      .WithMany(m => m.Agendas)
                      .HasForeignKey(e => e.MedicoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Configuração da entidade Consulta
            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataHora).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Justificativa).HasMaxLength(500);
                
                // Relacionamento com Paciente
                entity.HasOne(e => e.Paciente)
                      .WithMany(p => p.Consultas)
                      .HasForeignKey(e => e.PacienteId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Relacionamento com Medico
                entity.HasOne(e => e.Medico)
                      .WithMany(m => m.Consultas)
                      .HasForeignKey(e => e.MedicoId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Relacionamento com Agenda
                entity.HasOne(e => e.Agenda)
                      .WithMany()
                      .HasForeignKey(e => e.AgendaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}