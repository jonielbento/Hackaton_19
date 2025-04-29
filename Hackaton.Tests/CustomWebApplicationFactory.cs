using Hackaton.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Hackaton.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remover o DbContext registrado
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HackatonDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adicionar DbContext em memória para testes
                services.AddDbContext<HackatonDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Inicializar o banco de dados para cada teste
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<HackatonDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        // Inicializar dados de teste
                        SeedTestData.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Um erro ocorreu durante a inicialização do banco de dados para testes. Erro: {Message}", ex.Message);
                    }
                }
            });
        }
    }
} 