using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;

namespace MovieShop.Infrastructure.Repositories
{
    public class GenreRepository : EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieShopDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Genre>> GetGenresByMovieId(int movieId)
        {
            var genres = await _dbContext.MovieGenres.Where(mg => mg.MovieId == movieId)
                .Include(mg => mg.Genre)
                .Select(mg => mg.Genre)
                .ToListAsync();
            return genres;
        }
    }
}