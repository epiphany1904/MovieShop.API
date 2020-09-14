using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            this._castRepository = castRepository;
        }
        public async Task<Cast> GetCastById(int id)
        {
            return await _castRepository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<Object>> GetCastsForMovie(int movieId)
        {
            return await _castRepository.GetCastsForMovie(movieId);
        }
        
    }
}