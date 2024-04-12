using ecommerce.Models;

namespace ecommerce.ecoomerceAccessLayer.DataLayer
{
    public interface IecommerceRepository
    {
        List<CategoryModel> GetPaginatedCategory(int pageIndex, int pageSize,string searchValue);
        int GetTotalCategoryCount(string searchValue);
        int DeleteCategory(int Id);
        int AddCategoryDetails(CategoryModel category);
        CategoryModel GetCategoryById(int Id);
        List<CategoryModel> ActiveCategories();
        int AddProductDetails(ProductViewModel product);

        ProductModelMapper GetProductById(int Id);
        List<ProductModelMapper> GetPaginatedProduct(int pageIndex, int pageSize, string searchValue);
        List<ProductModelMapper> GetPaginatedProductCId(int pageIndex, int pageSize, string searchValue,int CId);

        int GetTotalProductCount(string searchValue);
        int DeleteProduct(int Id);
    }
}
