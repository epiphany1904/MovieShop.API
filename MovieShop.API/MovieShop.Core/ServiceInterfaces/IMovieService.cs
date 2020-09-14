using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface IMovieService
    {
        //Get Top Revenue Movies
        Task<IEnumerable<Movie>> GetTopRevenueMovies();
        // GetMovieById
        Task<MovieDetailsResponseModel> GetMovieById(int id);

        Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId);
        
        Task<IEnumerable<Movie>> GetMoviesForCast(int castId);
        
        Task<PagedResultSet<MovieResponseModel>> GetMoviesByPagination(int pageSize = 20, int page = 0, string title = "");
    }
}