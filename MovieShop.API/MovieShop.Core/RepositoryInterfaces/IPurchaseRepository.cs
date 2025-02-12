using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IPurchaseRepository: IAsyncRepository<Purchase>
    {
        Task<PurchaseResponseModel> GetAllPurchasedMoviesByUser(int userId);
    }
}