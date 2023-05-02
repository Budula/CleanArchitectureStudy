using Movies.Core.Entities;
using Movies.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Repositories
{
    public interface IMovieRepository :IRepository<Movie>
    {
        //Here we can have all custom operations
        Task<IEnumerable<Movie>> GetMoviesByDirectorName(string directorName);

    }
}
