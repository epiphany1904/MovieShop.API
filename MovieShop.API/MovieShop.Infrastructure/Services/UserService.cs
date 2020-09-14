using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly IMovieService _movieService;

        public UserService(IMovieService movieService,IAsyncRepository<Favorite> favoriteRepository,IUserRepository userRepository, ICryptoService cryptoService,
            IPurchaseRepository purchaseRepository, IMapper mapper,ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _purchaseRepository = purchaseRepository;
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _movieService = movieService;
        }


        public async Task<User> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            var hashedPassword = _cryptoService.HashPassword(password, user.Salt);
            if (hashedPassword == user.HashedPassword)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel requestModel)
        {
            // 1. Call GetUserByEmail  with  requestModel.Email to check if the email exists in the User Table or not
            // if user/email exists return Email already exists and throw an Conflict exception
            // if email does not exists then we can proceed in creating the User record
            // 1. var salt =  Genreate a random salt
            // 2. var hashedPassword =  we take requestModel.Password and add Salt from above step and Hash them to generate Unique Hash
            // 3. Save Email, Salt, hashedPassword along with other details that user sent like FirstName, LastName etc
            // 4. return the UserRegisterResponseModel object with newly craeted Id for the User
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null)
            {
                throw new Exception("Email already exists");
            }

            var salt = _cryptoService.CreateSalt();
            var hashedPassword = _cryptoService.HashPassword(requestModel.Password, salt);
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            var createdUser = await _userRepository.AddAsync(user);
            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                Email = requestModel.Email,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            return response;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<PurchaseResponseModel> GetAllPurchasedMoviesByUser(int id)
        {
            return await _purchaseRepository.GetAllPurchasedMoviesByUser(id);
        }

        public async Task<PagedResultSet<User>> GetAllUsersByPagination(int pageSize = 20, int page = 0,
            string lastName = "")
        {

            IEnumerable<User> users;
            int count;
            if (string.IsNullOrEmpty(lastName))
            {
                users = await _userRepository.ListAllAsync();
                count = await _userRepository.GetCountAsync();
            }
            else
            {
                users = await _userRepository.ListAsync(u => u.LastName == lastName);
                count = await _userRepository.GetCountAsync(u => u.LastName == lastName);
            }

            users = users.Skip(page * pageSize).Take(pageSize).ToList();
            PagedResultSet<User> pagedResultSet = new PagedResultSet<User>(users, page, pageSize, count);
            return pagedResultSet;
        }

    //        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
    //      {
    //          if (_currentUserService.UserId != favoriteRequest.UserId)
    //              throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to purchase");
    //          // See if Movie is already Favorited.
    //          if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId))
    //              throw new ConflictException("Movie already Favorited");
    //     
    //          var favorite = _mapper.Map<Favorite>(favoriteRequest);
    //          await _favoriteRepository.AddAsync(favorite);
    // }
    //        public async Task<bool> FavoriteExists(int id, int movieId)
    //        {
    //            return await _favoriteRepository.GetExistsAsync(f => f.MovieId == movieId &&
    //                                                                 f.UserId == id);
    //        }
    //        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
    //        {
    //            var dbFavorite =
    //                await _favoriteRepository.ListAsync(r => r.UserId == favoriteRequest.UserId &&
    //                                                         r.MovieId == favoriteRequest.MovieId);
    //            // var favorite = _mapper.Map<Favorite>(favoriteRequest);
    //            await _favoriteRepository.DeleteAsync(dbFavorite.First());
    //        }
    //        
    //        public async Task<FavoriteResponseModel> GetAllFavoritesForUser(int id)
    //        {
    //            if (_currentUserService.UserId != id)
    //                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to View Favorites");
    //
    //            var favoriteMovies = await _favoriteRepository.ListAllWithIncludesAsync(
    //                p => p.UserId == _currentUserService.UserId,
    //                p => p.Movie);
    //            return _mapper.Map<FavoriteResponseModel>(favoriteMovies);
    //        }

           // public async Task PurchaseMovie(PurchaseRequestModel purchaseRequest)
           // {
           //     if (_currentUserService.UserId != purchaseRequest.UserId)
           //         throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to purchase");
           //
           //     // See if Movie is already purchased.
           //     if (await IsMoviePurchased(purchaseRequest))
           //         throw new ConflictException("Movie already Purchased");
           //     // Get Movie Price from Movie Table
           //     var movie = await _movieService.GetMovieById(purchaseRequest.MovieId);
           //     purchaseRequest.TotalPrice = movie.Price;
           //
           //     var purchase = _mapper.Map<Purchase>(purchaseRequest);
           //     await _purchaseRepository.AddAsync(purchase);
           // }
           
           // public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest)
           // {
           //     return await _purchaseRepository.GetExistsAsync(p =>
           //         p.UserId == purchaseRequest.UserId && p.MovieId == purchaseRequest.MovieId);
           // }
           // public async Task<PurchaseResponseModel> GetAllPurchasesForUser(int id)
           // {
           //     if (_currentUserService.UserId != id)
           //         throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to View Purchases");
           //
           //     var purchasedMovies = await _purchaseRepository.ListAllWithIncludesAsync(
           //         p => p.UserId == _currentUserService.UserId,
           //         p => p.Movie);
           //     return _mapper.Map<PurchaseResponseModel>(purchasedMovies);
           // }
    } 
}
