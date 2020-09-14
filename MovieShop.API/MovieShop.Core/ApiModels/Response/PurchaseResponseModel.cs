using System;
using System.Collections.Generic;

namespace MovieShop.Core.ApiModels.Response
{
    public class PurchaseResponseModel
    {
        public int UserId { get; set; }
        public List<PurchasedMovedResponseModel> purchasedMovies { get; set; }
    }
    public class PurchasedMovedResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PurchasedDateTime { get; set; }
    }
}