using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedQueryable<Product>> GetAllAsync(bool statement, int pageIndex, int pageSize);
        Task<ICollection<ProductDTO>> GetAllProductsByTranslationAsync(string lang);
        Task<ICollection<Product>> GetProductsByTheirIdsAsync(List<int> productIds);
        Task<ICollection<Product>> GetAllProductsByCategoryAsync(int categoryId);
        Task<ICollection<Product>> GetNotDeletedProductsAsync();
        Task<int> GetProductIdBySearchAsync(string code);
        Task<Product> CreateAsync(CreateProductDTO dto);
        Task<Product> UpdateAsync(UpdateProductDTO dto);
        Task<Product> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}
