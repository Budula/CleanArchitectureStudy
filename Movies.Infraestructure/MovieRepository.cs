using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities;
using Movies.Core.Repositories;
using Movies.Infraestructure.Data;
using Movies.Infraestructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infraestructure
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieContext movieContext):base(movieContext)
        {
            
        }
        public async Task<IEnumerable<Movie>> GetMoviesByDirectorName(string directorName)
        {
            return await _movieContext.Movies
                .Where(m => m.DirectorName.Equals(directorName))
                .ToListAsync();
        }
    }
}
