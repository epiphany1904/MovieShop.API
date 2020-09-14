using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;

namespace MovieShop.Infrastructure.Repositories
{
       public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
         public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<IEnumerable<Movie>> GetTopRatedMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Reviews.Average(r => r.Rating)).Take(20).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            var movies = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(mg => mg.Movie)
                                         .Select(m => m.Movie)
                                         .ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetHighestGrossingMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(50).ToListAsync();

            return movies;
        }

        public async Task<IEnumerable<Review>> GetMovieReviews(int id)
        {
            var reviews = await _dbContext.Reviews.Where(r => r.MovieId == id).Include(r => r.User)
                                          .Select(r => new Review
                                                       {
                                                           UserId = r.UserId,
                                                           Rating = r.Rating,
                                                           MovieId = r.MovieId,
                                                           ReviewText = r.ReviewText,
                                                           User = new User
                                                                  {
                                                                      Id = r.UserId,
                                                                      FirstName = r.User.FirstName,
                                                                      LastName = r.User.LastName
                                                                  }
                                                       }).ToListAsync();
            return reviews;
        }

        public async Task<IEnumerable<Movie>> GetMoviesForCast(int castId)
        {
            var movies = await _dbContext.MovieCasts.Where(g => g.CastId == castId).Include(mg => mg.Movie)
                .Select(m => m.Movie)
                .ToListAsync();
            return movies;
        }

        public override async Task<Movie> GetByIdAsync(int id)
        {  //Get Movie by Id and also include Average Rating of that Movie
            var movie = await _dbContext.Movies
                .Include(m=>m.MovieCasts).ThenInclude(m=>m.Cast).Include(m=>m.MovieGenres).ThenInclude(m=>m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).AverageAsync(r => r.Rating);
            if (movieRating > 0) movie.Rating = movieRating;

            return movie;
        }
    }
}