using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetMoviesByPagination([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1, string title = "")
        {
            var movies = await _movieService.GetMoviesByPagination(pageSize, pageIndex, title);
            return Ok(movies);
        }

        [HttpGet("revenue")]
       
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            var movies = await _movieService.GetTopRevenueMovies();
            return Ok(movies);
        }
        
        [HttpGet("{movieId}")]
       
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            var movie = await _movieService.GetMovieById(movieId);
            return Ok(movie);
        }
        
        [HttpGet]
        [Route("genre/{genreId}")]
        public async Task<IActionResult> GetMoviesByGenreId(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenreId(genreId);
            return Ok(movies);
        }
        
        [HttpGet]
        [Route("cast/{castId}")]
        [Authorize]
        public async Task<IActionResult> GetMoviesForCast(int castId)
        {
            var movies = await _movieService.GetMoviesForCast(castId);
            return Ok(movies);
        }
        
    }
}