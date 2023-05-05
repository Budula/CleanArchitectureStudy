using MediatR;
using Movies.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Queries
{
    public class GetMovieByDirectorNameQuery : IRequest<IEnumerable<MovieResponse>>
    {
        public string DirectorName { get; set; }
        public GetMovieByDirectorNameQuery(string directorName)
        {
            DirectorName = directorName;
        }
    }
}
