using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface ICastService
    {
        //GetCastById
        //GetMoviesForCast(int castId)
        Task<Cast> GetCastById(int id);
        Task<IEnumerable<Object>> GetCastsForMovie(int movieId);
       
    }
}