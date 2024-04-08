using ecommerce.Models;

namespace ecommerce.ecoomerceAccessLayer.DataLayer
{
    public interface IecommerceRepository
    {
        List<CategoryModel> GetPaginatedCategory(int pageIndex, int pageSize);
    }
}
