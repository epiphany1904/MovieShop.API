using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        private readonly ICastService _castService;

        public CastsController(ICastService castService)
        {
            _castService = castService;
           
        }
        [HttpGet("Cast/{castId}")]
        
        public async Task<IActionResult>  GetCastById( int castId)
        {
            var cast = await _castService.GetCastById(castId);
            return Ok(cast);
        }
        
        [HttpGet]
        [Route("movie/{movieId}")]
        public async Task<IActionResult> GetCastsForMovie(int movieId)
        {
            var casts = await _castService.GetCastsForMovie(movieId);
            return Ok(casts);
        }
    }
}