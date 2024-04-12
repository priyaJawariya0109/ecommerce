using ecommerce.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.VisualStudio.TextTemplating;
using System.Buffers;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;

namespace ecommerce.ecoomerceAccessLayer.DataLayer
{
    public class ecommerceRepository : IecommerceRepository
    {
        //private readonly SqlConnection _connection;
        public string errorMessage;
        public string errorDescription;
        private readonly IDataEngine _engine;
        //DataEngine engine = new DataEngine();
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlConnection _connection = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public ecommerceRepository(IDataEngine engine)
        {
            _engine = engine;
        }
        //public ecommerceRepository(IConfiguration configuration)
        //{

        //    _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        //    //conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        //}

        public int AddCategoryDetails(CategoryModel category)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "AddCategoryDetails";
                sp.ParameterList = new List<Param>() {
                    new Param { Name = "@CId", Value = category.CId },
                    new Param { Name = "@Name", Value =  category.Name },
                    new Param { Name = "@Description", Value =  category.Description },
                    new Param { Name = "@Active", Value =  category.Active }
                };
                return _engine.ExecuteProcedureInt(sp.ProcedureName, conn, sp.ParameterList);
            }
            catch (SqlException sqlEx)
            {
                // Iterate through each SqlError object in the SqlException.Errors collection
                foreach (SqlError error in sqlEx.Errors)
                {
                    // Check if the error message contains the specific error condition
                    if (error.Message.Contains("Category with this name already exists."))
                    {
                        // Handle the duplicate category name error
                        // For example, return a custom error code indicating duplicate name
                        return -2;
                    }
                }

                // If the specific error message is not found, handle other exceptions
                return -1;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // Log or handle the exception appropriately
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }

            /*            using (SqlCommand cmd = new SqlCommand("AddCategoryDetails", _connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CId", category.CId);
                            cmd.Parameters.AddWithValue("@Name", category.Name);
                            cmd.Parameters.AddWithValue("@Description", category.Description);
                            cmd.Parameters.AddWithValue("@Active", category.Active);

                            try
                            {
                                _connection.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();

                                return rowsAffected >= 1;
                            }
                            catch (Exception ex)
                            {
                                // Handle exceptions (log, throw, etc.)
                                // Example: Log the exception
                                Console.WriteLine($"Error inserting customer: {ex.Message}");
                                return false;
                            }
                            finally
                            {
                                _connection.Close();
                            }
                        }
               */
        }
        public List<CategoryModel> GetPaginatedCategory(int pageIndex, int pageSize, string searchValue)
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetPaginatedCategory";
                sp.ParameterList = new List<Param>() {
                    new Param { Name = "@PageIndex", Value = pageIndex },
                    new Param { Name = "@PageSize", Value = pageSize },
                    new Param { Name = "@SearchValue", Value = searchValue }
                };
                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataNamesMapper<CategoryModel> project = new DataNamesMapper<CategoryModel>();
                    return project.Map(dt).ToList();
                }
                return new List<CategoryModel>();
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return new List<CategoryModel>();
            }

            /*
                        var categories = new List<CategoryModel>();


                        using (var command = new SqlCommand("GetPaginatedCategory", _connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@PageIndex", pageIndex);
                            command.Parameters.AddWithValue("@PageSize", pageSize);

                            _connection.Open();

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var category = new CategoryModel
                                    {
                                        CId = (int)reader["CId"],
                                        Name = reader["Name"].ToString(),
                                        Description = reader["Description"].ToString(),

                                        Active = Convert.ToBoolean(reader["Active"]),


                                    };
                                    categories.Add(category);
                                }
                            }
                            _connection.Close();


                        }


                        return categories;
                    }*/
        }
        /*
                public List<CategoryModel> GetCategory()
                {
                    var categories = new List<CategoryModel>();


                    using (var command = new SqlCommand("GetCategory", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Operation", 1);


                        _connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var category = new CategoryModel
                                {
                                    CId = (int)reader["CId"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),

                                    Active = Convert.ToBoolean(reader["Active"]),


                                };
                                categories.Add(category);
                            }
                        }
                        _connection.Close();


                    }


                    return categories;
                }

        */

        public CategoryModel GetCategoryById(int CId)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetCategoryById";
                sp.ParameterList = new List<Param>() {
            new Param { Name = "@CId", Value = CId }
                 };

                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Assuming that DataNamesMapper<CategoryModel> has a method named MapRow(DataRow row) which maps DataRow to CategoryModel
                    DataNamesMapper<CategoryModel> project = new DataNamesMapper<CategoryModel>();
                    // Assuming that Map method returns a list of CategoryModel objects
                    var categories = project.Map(dt);
                    // Assuming that you only expect one category with the given ID, so returning the first one
                    return categories.FirstOrDefault();
                }
                // Return null if no data found
                return null;
            }
            catch (Exception ex)
            {
                // Log error
                errorMessage = "Message: " + ex.Message + ". Method: " + ex.TargetSite.Name + ". LineNumber: " + ex.StackTrace;
                Utility.WriteMsg(errorMessage);

                // It's generally better to rethrow the exception to let it bubble up, but for simplicity, returning null here
                return null;
            }
        }
        public List<CategoryModel> ActiveCategories()
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "ActiveCategories";
                sp.ParameterList = new List<Param>()
                {

                };
                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataNamesMapper<CategoryModel> project = new DataNamesMapper<CategoryModel>();
                    return project.Map(dt).ToList();
                }
                return new List<CategoryModel>();
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return new List<CategoryModel>();
            }
            /*   var categories = new List<CategoryModel>();


               using (var command = new SqlCommand("ActiveCategories", _connection))
               {
                   command.CommandType = CommandType.StoredProcedure;


                   _connection.Open();

                   using (var reader = command.ExecuteReader())
                   {
                       while (reader.Read())
                       {
                           var category = new CategoryModel
                           {
                               CId = (int)reader["CId"],
                               Name = reader["Name"].ToString(),
                               Description = reader["Description"].ToString(),

                               Active = Convert.ToBoolean(reader["Active"]),


                           };
                           categories.Add(category);
                       }
                   }
                   _connection.Close();


               }


               return categories;
           }*/


      

        }


        public int AddProductDetails(ProductViewModel product)
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "AddProductDetails";
                sp.ParameterList = new List<Param>() {
                        new Param { Name = "@CId", Value = product.CId },
                        new Param { Name = "@PId", Value = product.PId },
                        new Param { Name = "@PName", Value =  product.PName },
                        new Param { Name = "@PDescription", Value =  product.PDescription },
                        new Param { Name = "@PCostPrice", Value =  product.PCostPrice },
                        new Param { Name = "@PsellPrice", Value =  product.PsellPrice },
                        new Param { Name = "@PSavings", Value =  product.PSavings },
                        new Param { Name = "@StockQt", Value =  product.StockQt },
                        new Param { Name = "@RemainQt", Value =  product.RemainQt },
                        new Param { Name = "@PImage", Value =  product.PhotoPath },


                        //image

                    };
                int res= _engine.ExecuteProcedureInt(sp.ProcedureName, conn, sp.ParameterList);
                return res;
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return -1;
            }
/*            using (SqlCommand cmd = new SqlCommand("AddProductDetails", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CId", product.CId);
                cmd.Parameters.AddWithValue("@PId", product.PId); // Assuming PId is defined somewhere
                cmd.Parameters.AddWithValue("@PName", product.PName); // Assuming PName is defined somewhere
                cmd.Parameters.AddWithValue("@PDescription", product.PDescription); // Assuming PDescription is defined somewhere
                cmd.Parameters.AddWithValue("@PCostPrice", product.PCostPrice); // Assuming PCostPrice is defined somewhere
                cmd.Parameters.AddWithValue("@PsellPrice", product.PsellPrice); // Assuming PsellPrice is defined somewhere
                cmd.Parameters.AddWithValue("@PSavings", product.PSavings); // Assuming PSavings is defined somewhere
                cmd.Parameters.AddWithValue("@StockQt", product.StockQt); // Assuming StockQt is defined somewhere
                cmd.Parameters.AddWithValue("@RemainQt", product.RemainQt); // Assuming RemainQt is defined somewhere
                if (product.PImage != null && product.PImage.Length > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(product.PImage);

                }


                cmd.Parameters.AddWithValue("@PImage", product.PImage);


                try
                {
                    _connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected >= 1;
                }
                catch (Exception ex)
                {
                    // Handle exceptions (log, throw, etc.)
                    // Example: Log the exception
                    Console.WriteLine($"Error inserting customer: {ex.Message}");
                    return false;
                }
                finally
                {
                    _connection.Close();
                }
            }
     */   }

        public int GetTotalCategoryCount(string searchValue)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetTotalCategoryCount";
                // Since GetTotalCategoryCount doesn't require any parameters, you don't need to add any parameters to the ParameterList
                if (!string.IsNullOrEmpty(searchValue))
                {
                    sp.ParameterList = new List<Param>() {
                 new Param { Name = "@SearchValue", Value = searchValue }
                };
                }
                object result = _engine.ExecuteProcedureScalar(sp.ProcedureName, conn, sp.ParameterList);
                if (result != null)
                {
                    int categoryCount = Convert.ToInt32(result);
                    if (categoryCount > 0)
                    {
                        return categoryCount;
                    }
                }
                return 0; // Return 0 if there are no categories or if the result is null
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error message
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return 0; // Return 0 if there's an exception
            }

        }
        public int DeleteCategory(int Id)
        {
            /*            using (SqlCommand cmd = new SqlCommand("DeleteCategory", _connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CId", Id));

                            try
                            {
                                _connection.Open();
                                int i = cmd.ExecuteNonQuery();
                                return i >= 1;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                                return false;
                            }

                        }
                  */
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "DeleteCategory";
                sp.ParameterList = new List<Param>() {
                new Param { Name = "@CId", Value = Id }

            };

                return _engine.ExecuteProcedureInt(sp.ProcedureName, conn, sp.ParameterList);
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return -1;
            }

        }



        public ProductModelMapper GetProductById(int PId)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetProductById";
                sp.ParameterList = new List<Param>() {
            new Param { Name = "@PId", Value = PId }
                 };

                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Assuming that DataNamesMapper<CategoryModel> has a method named MapRow(DataRow row) which maps DataRow to CategoryModel
                    DataNamesMapper <ProductModelMapper> project = new DataNamesMapper<ProductModelMapper>();
                    // Assuming that Map method returns a list of CategoryModel objects
                    var products = project.Map(dt);
                    // Assuming that you only expect one category with the given ID, so returning the first one
                    return products.FirstOrDefault();
                }
                // Return null if no data found
                return null;
            }
            catch (Exception ex)
            {
                // Log error
                errorMessage = "Message: " + ex.Message + ". Method: " + ex.TargetSite.Name + ". LineNumber: " + ex.StackTrace;
                Utility.WriteMsg(errorMessage);

                // It's generally better to rethrow the exception to let it bubble up, but for simplicity, returning null here
                return null;
            }
        }
        public List<ProductModelMapper> GetPaginatedProduct(int pageIndex, int pageSize, string searchValue)
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetPaginatedProduct";
                sp.ParameterList = new List<Param>() {
                    new Param { Name = "@PageIndex", Value = pageIndex },
                    new Param { Name = "@PageSize", Value = pageSize },
                    new Param { Name = "@SearchValue", Value = searchValue }
                };
                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataNamesMapper<ProductModelMapper> project = new DataNamesMapper<ProductModelMapper>();
                    return project.Map(dt).ToList();
                }
                return new List<ProductModelMapper>();
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return new List<ProductModelMapper>();
            }

        }
        public int GetTotalProductCount(string searchValue)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetTotalProductCount";
                // Since GetTotalCategoryCount doesn't require any parameters, you don't need to add any parameters to the ParameterList
                if (!string.IsNullOrEmpty(searchValue))
                {
                    sp.ParameterList = new List<Param>() {
                 new Param { Name = "@SearchValue", Value = searchValue }
                };
                }
                object result = _engine.ExecuteProcedureScalar(sp.ProcedureName, conn, sp.ParameterList);
                if (result != null)
                {
                    int productCount = Convert.ToInt32(result);
                    if (productCount > 0)
                    {
                        return productCount;
                    }
                }
                return 0; // Return 0 if there are no categories or if the result is null
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error message
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return 0; // Return 0 if there's an exception
            }

        }

        public int DeleteProduct(int Id)
        {
            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "DeleteProduct";
                sp.ParameterList = new List<Param>() {
                new Param { Name = "@PId", Value = Id }

            };

                return _engine.ExecuteProcedureInt(sp.ProcedureName, conn, sp.ParameterList);
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return -1;
            }

        }
        public List<ProductModelMapper> GetPaginatedProductCId(int pageIndex, int pageSize, string searchValue,int CId)
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetPaginatedProductCId";
                sp.ParameterList = new List<Param>() {
                    new Param { Name = "@PageIndex", Value = pageIndex },
                    new Param { Name = "@PageSize", Value = pageSize },
                    new Param { Name = "@SearchValue", Value = searchValue },
                    new Param{ Name = "@CId", Value = CId }
                };
                dt = _engine.ExecuteProcedureDatatable(sp.ProcedureName, conn, sp.ParameterList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataNamesMapper<ProductModelMapper> project = new DataNamesMapper<ProductModelMapper>();
                    return project.Map(dt).ToList();
                }
                return new List<ProductModelMapper>();
            }
            catch (Exception ex)
            {
                errorMessage = "Message= " + ex.Message.ToString() + ". Method= " + ex.TargetSite.Name.ToString() + ". LineNumber= " + ex.LineNumber();
                errorDescription = " StackTrace : " + ex.StackTrace.ToString() + " Source = " + ex.Source.ToString();
                Utility.WriteMsg(errorMessage + " " + errorDescription);
                return new List<ProductModelMapper>();
            }

            /*
                        var categories = new List<CategoryModel>();


                        using (var command = new SqlCommand("GetPaginatedCategory", _connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@PageIndex", pageIndex);
                            command.Parameters.AddWithValue("@PageSize", pageSize);

                            _connection.Open();

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var category = new CategoryModel
                                    {
                                        CId = (int)reader["CId"],
                                        Name = reader["Name"].ToString(),
                                        Description = reader["Description"].ToString(),

                                        Active = Convert.ToBoolean(reader["Active"]),


                                    };
                                    categories.Add(category);
                                }
                            }
                            _connection.Close();


                        }


                        return categories;
                    }*/
        }

    }

}

