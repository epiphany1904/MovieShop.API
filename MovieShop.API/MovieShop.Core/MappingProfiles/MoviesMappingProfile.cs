
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;

namespace MovieShop.Core.MappingProfiles
{
    public class MoviesMappingProfile : Profile
    {
        public MoviesMappingProfile()
        {
            
            CreateMap<IEnumerable<Favorite>, FavoriteResponseModel>()
                .ForMember(p => p.FavoriteMovies, opt => opt.MapFrom(src => GetFavoriteMovies(src)))
                .ForMember(p => p.UserId, opt => opt.MapFrom(src => src.FirstOrDefault().UserId));
            CreateMap<FavoriteRequestModel, Favorite>();
            CreateMap<PurchaseRequestModel, Purchase>();

        }
        private List<FavoriteResponseModel.FavoriteMovieResponseModel> GetFavoriteMovies(
            IEnumerable<Favorite> favorites)
        {
            var favoriteResponse = new FavoriteResponseModel
            {
                FavoriteMovies = new List<FavoriteResponseModel.FavoriteMovieResponseModel>()
            };
            foreach (var favorite in favorites)
                favoriteResponse.FavoriteMovies.Add(new FavoriteResponseModel.FavoriteMovieResponseModel
                {
                    PosterUrl = favorite.Movie.PosterUrl,
                    Id = favorite.MovieId,
                    Title = favorite.Movie.Title
                });

            return favoriteResponse.FavoriteMovies;
        }
        
        private List<Genre> GetMovieGenres(IEnumerable<MovieGenre> srcGenres)
        {
            var movieGenres = new List<Genre>();
            foreach (var genre in srcGenres)
            {
                movieGenres.Add( new Genre{ Id = genre.GenreId, Name = genre.Genre.Name});
            }

            return movieGenres;
        }
    }
}