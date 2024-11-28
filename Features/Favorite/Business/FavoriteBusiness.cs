using Coffee_Ecommerce.API.Features.Favorite.GetByUser;
using Coffee_Ecommerce.API.Features.Favorite.Delete;
using Coffee_Ecommerce.API.Features.Favorite.Exceptions;
using Coffee_Ecommerce.API.Features.Favorite.Repository;
using Coffee_Ecommerce.API.Features.Product;
using Coffee_Ecommerce.API.Features.Product.Exceptions;
using Coffee_Ecommerce.API.Features.Product.Repository;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Favorite.Business
{
    public sealed class FavoriteBusiness : IFavoriteBusiness
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;

        public FavoriteBusiness(IFavoriteRepository repository, IProductRepository productRepository)
        {
            _favoriteRepository = repository;
            _productRepository = productRepository;
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var validationResult = CreateValidator.CheckForErrors(command);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            try
            {
                await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
            }
            catch (ProductNotFoundException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }

            var entity = new FavoriteEntity
            {
                UserId = command.UserId,
                ProductId = command.ProductId
            };

            try
            {
                var result = await _favoriteRepository.CreateAsync(entity, cancellationToken);

                return new CreateResult { Data = result };
            }
            catch (FavoriteAlreadyExistsException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (ArgumentNullException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult != null)
                return new DeleteResult { Error = validationResult };

            try
            {
                var result = await _favoriteRepository.DeleteAsync(id, cancellationToken);

                return new DeleteResult { Data = result };
            }
            catch (FavoriteNotFoundException ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetByUserResult> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var validationResult = GetByUserValidator.CheckForErrors(userId);

            if (validationResult != null)
                return new GetByUserResult { Error = validationResult };

            var result = await _favoriteRepository.GetByUserAsync(userId, cancellationToken);

            List<FavoriteData> favoriteData = new List<FavoriteData>();

            foreach (var item in result)
            {
                try
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

                    FavoriteData data = new FavoriteData
                    {
                        FavoriteId = item.Id,
                        Product = product
                    };

                    favoriteData.Add(data);
                }
                catch (ProductNotFoundException)
                {
                    await _favoriteRepository.DeleteAsync(item.Id, cancellationToken);
                }
            }

            return new GetByUserResult { Data = favoriteData };
        }
    }
}