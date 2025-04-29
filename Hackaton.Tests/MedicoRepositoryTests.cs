using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using Hackaton.Infrastructure.Data;
using Hackaton.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hackaton.Tests
{
    public class MedicoRepositoryTests
    {
        private readonly DbContextOptions<HackatonDbContext> _options;
        private readonly HackatonDbContext _context;
        private readonly IMedicoRepository _repository;

        public MedicoRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<HackatonDbContext>()
                .UseInMemoryDatabase(databaseName: $"HackatonTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new HackatonDbContext(_options);
            _context.Database.EnsureCreated();

            // Inicializar com dados de teste
            SeedDatabase();

            _repository = new MedicoRepository(_context);
        }

        private void SeedDatabase()
        {
            _context.Medicos.Add(new Medico
            {
                Nome = "Dr. João Silva",
                CRM = "12345-SP",
                Especialidade = "Cardiologia",
                ValorConsulta = 200.00M,
                Senha = "senha123"
            });

            _context.Medicos.Add(new Medico
            {
                Nome = "Dra. Maria Santos",
                CRM = "67890-SP",
                Especialidade = "Neurologia",
                ValorConsulta = 250.00M,
                Senha = "senha456"
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosMedicos()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.CRM == "12345-SP");
            Assert.Contains(result, m => m.CRM == "67890-SP");
        }

        [Fact]
        public async Task GetByIdAsync_ComIdValido_DeveRetornarMedico()
        {
            // Arrange
            var medico = await _context.Medicos.FirstOrDefaultAsync();
            var id = medico.Id;

            // Act
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(medico.Nome, result.Nome);
            Assert.Equal(medico.CRM, result.CRM);
        }

        [Fact]
        public async Task GetByCRMAsync_ComCRMValido_DeveRetornarMedico()
        {
            // Act
            var result = await _repository.GetByCRMAsync("12345-SP");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("12345-SP", result.CRM);
            Assert.Equal("Dr. João Silva", result.Nome);
        }

        [Fact]
        public async Task CreateAsync_DeveAdicionarERetornarMedicoComId()
        {
            // Arrange
            var novoMedico = new Medico
            {
                Nome = "Dr. Roberto Pereira",
                CRM = "54321-SP",
                Especialidade = "Ortopedia",
                ValorConsulta = 180.00M,
                Senha = "senha789"
            };

            // Act
            var result = await _repository.CreateAsync(novoMedico);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id); // Deve ter um ID atribuído
            Assert.Equal(novoMedico.Nome, result.Nome);
            Assert.Equal(novoMedico.CRM, result.CRM);

            // Verificar se foi salvo no banco
            var medicoSalvo = await _context.Medicos.FindAsync(result.Id);
            Assert.NotNull(medicoSalvo);
            Assert.Equal(novoMedico.Nome, medicoSalvo.Nome);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarERetornarMedico()
        {
            // Arrange
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == "12345-SP");
            medico.Nome = "Dr. João Silva Atualizado";
            medico.ValorConsulta = 220.00M;

            // Act
            var result = await _repository.UpdateAsync(medico);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(medico.Id, result.Id);
            Assert.Equal("Dr. João Silva Atualizado", result.Nome);
            Assert.Equal(220.00M, result.ValorConsulta);

            // Verificar se foi atualizado no banco
            var medicoAtualizado = await _context.Medicos.FindAsync(medico.Id);
            Assert.NotNull(medicoAtualizado);
            Assert.Equal("Dr. João Silva Atualizado", medicoAtualizado.Nome);
            Assert.Equal(220.00M, medicoAtualizado.ValorConsulta);
        }

        [Fact]
        public async Task DeleteAsync_DeveRemoverMedico()
        {
            // Arrange
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == "67890-SP");
            var id = medico.Id;

            // Act
            var result = await _repository.DeleteAsync(id);

            // Assert
            Assert.True(result);

            // Verificar se foi removido do banco
            var medicoRemovido = await _context.Medicos.FindAsync(id);
            Assert.Null(medicoRemovido);
        }
        
        [Fact]
        public async Task AuthenticateAsync_ComCredenciaisValidas_DeveRetornarMedico()
        {
            // Act
            var result = await _repository.AuthenticateAsync("12345-SP", "senha123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("12345-SP", result.CRM);
            Assert.Equal("Dr. João Silva", result.Nome);
        }

        [Fact]
        public async Task AuthenticateAsync_ComCredenciaisInvalidas_DeveRetornarNull()
        {
            // Act
            var result = await _repository.AuthenticateAsync("12345-SP", "senhaerrada");

            // Assert
            Assert.Null(result);
        }
    }
} 