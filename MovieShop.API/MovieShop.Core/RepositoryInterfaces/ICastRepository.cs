using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface ICastRepository : IAsyncRepository<Cast>
    {

        Task<IEnumerable<Object>> GetCastsForMovie(int movieId);
    }

}