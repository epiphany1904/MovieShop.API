using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IGenreRepository : IAsyncRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetGenresByMovieId(int movieId);
        
    }
}