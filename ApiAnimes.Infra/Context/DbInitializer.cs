
using ApiAnimes.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace ApiAnimes.Infra.Context;
public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetRequiredService<SqlDbContext>());
    }

    private static void SeedData(SqlDbContext context)
    {   
        context.Database.Migrate();

        // Seed data
        if (!context.Animes.Any())
        {
            context.Animes.AddRange(
                new Anime { Name = "Naruto", Description = "Naruto Uzumaki é um menino que vive em Konohagakure no Sato ou simplesmente Konoha ou Vila Oculta da Folha, a vila ninja do País do Fogo.", Author = "Masashi Kishimoto" },
                new Anime { Name = "Dragon Ball", Description = "Dragon Ball é uma série de anime e mangá criada por Akira Toriyama.", Author = "Akira Toriyama" },
                new Anime { Name = "One Piece", Description = "One Piece é uma série de mangá escrita e ilustrada por Eiichiro Oda.", Author = "Eiichiro Oda" }
            );

            context.SaveChanges();
        }
    }
}
