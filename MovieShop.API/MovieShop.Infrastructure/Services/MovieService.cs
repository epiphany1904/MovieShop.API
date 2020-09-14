using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.Exceptions;
using MovieShop.Core.Helpers;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
       
        private readonly IMovieRepository _movieRepository;
       

        public MovieService(IMovieRepository movieRepository)
        {
            
            this._movieRepository = movieRepository;
         
        }
        public async Task<IEnumerable<Movie>> GetTopRevenueMovies()
        {
           return await _movieRepository.GetHighestGrossingMovies();
        }

        public async Task<MovieDetailsResponseModel> GetMovieById(int id)
        {
           var movie =await _movieRepository.GetByIdAsync(id);
           List<CastResponseModel> casts = new List<CastResponseModel>();
           foreach (var cast in movie.MovieCasts)
           {
               var castResonse = new CastResponseModel
               {
                   Id = cast.Cast.Id,
                   Name = cast.Cast.Name,
                   Gender = cast.Cast.Gender,
                   ProfilePath = cast.Cast.ProfilePath,
                   TmdbUrl = cast.Cast.TmdbUrl,
                   Character = cast.Character
               };
               casts.Add(castResonse);
           }
           List<Genre> genres = new List<Genre>();
          foreach (var genre in movie.MovieGenres)
          {
              var g = new Genre
              {
                  Id = genre.Genre.Id,
                  Name = genre.Genre.Name
              };
              genres.Add(g);
          }
           var movieDetails = new MovieDetailsResponseModel
           {
               Id = movie.Id,
               Title = movie.Title,
               Overview = movie.Overview,
               Tagline = movie.Tagline,
               Budget = movie.Budget,
               Revenue = movie.Revenue,
               ImdbUrl = movie.ImdbUrl,
               TmdbUrl = movie.TmdbUrl,
               PosterUrl = movie.PosterUrl,
               BackdropUrl = movie.BackdropUrl,
               OriginalLanguage = movie.OriginalLanguage,
               ReleaseDate = movie.ReleaseDate,
               RunTime = movie.RunTime,
               Price = movie.Price,
               Casts = casts,
               Genres = genres
           };
           return movieDetails;
        }
        
        public async Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId)
        {
            return await _movieRepository.GetMoviesByGenre(genreId);
        }
        
        public async Task<IEnumerable<Movie>> GetMoviesForCast(int castId)
        {
            return await _movieRepository.GetMoviesForCast(castId);
        }

        public async Task<PagedResultSet<MovieResponseModel>> GetMoviesByPagination(int pageSize = 20, int page = 0, string title = "")
        {
            // check if title parameter is null or empty, if not then construct a Expression with Contains method
            // contains method will transalate to SQL like
            Expression<Func<Movie, bool>> filterExpression = null;
            if (!string.IsNullOrEmpty(title))
            {
                filterExpression = movie => title != null && movie.Title.Contains(title);
            }
            //  // we are gonna call GetPagedData method from repository;
            // pass the order by column, here we are ordering our result by movie title
            // pass the above filter expression
            var pagedMovies = await _movieRepository.GetPagedData(page, pageSize, movies => movies.OrderBy(m => m.Title), filterExpression);
            // once you get movies from repository , convert them in to MovieResponseModel List
            var pagedMovieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in pagedMovies)
            {
                pagedMovieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value
                });
            }
            // Pass the List of MovieResponseModel to our PagedResultSet class so that it can display the data along with page numbers
            var movies = new PagedResultSet<MovieResponseModel>(pagedMovieResponseModel, page, pageSize, pagedMovies.TotalCount);
            return movies;
        }
        
    }
}