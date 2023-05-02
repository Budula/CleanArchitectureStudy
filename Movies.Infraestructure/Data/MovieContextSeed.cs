using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Movies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infraestructure.Data
{
    public class MovieContextSeed
    {
        public static async Task SeedAsync(MovieContext movieContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                await movieContext.Database.EnsureCreatedAsync();
                if (!movieContext.Movies.Any())
                {
                    movieContext.Movies.AddRange(entities: GetMovies());
                    await movieContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                retryForAvailability++;
                var log = loggerFactory.CreateLogger<MovieContextSeed>();
                log.LogError($"Exception ocurred while connecting: {ex.Message}");
                await SeedAsync(movieContext, loggerFactory, retryForAvailability);

            }
        }

        private static IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie{MovieName="Avatar", DirectorName="James Cameron", ReleaseYear="2009"},
                new Movie{MovieName="Titanic", DirectorName="James Cameron", ReleaseYear="1997"},
                new Movie{MovieName="Die Another Day", DirectorName="Lee Tamahori", ReleaseYear="2002"},
                new Movie{MovieName="Godizilla", DirectorName="Gareth Edward", ReleaseYear="2014"},

            };
        }
    }
}
