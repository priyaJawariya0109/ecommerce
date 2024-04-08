using ecommerce.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Data;
using System.Data.SqlClient;
using System.Net.Sockets;
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

        public bool AddCategoryDetails(CategoryModel category)
        {
            using (SqlCommand cmd = new SqlCommand("AddCategoryDetails", _connection))
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
        }
        public List<CategoryModel> GetPaginatedCategory(int pageIndex, int pageSize)
        {

            try
            {
                Serviceparameters sp = new Serviceparameters();
                sp.ProcedureName = "GetPaginatedCategory";
                sp.ParameterList = new List<Param>() {
                    new Param { Name = "@PageIndex", Value = pageIndex },
                    new Param { Name = "@PageSize", Value = pageSize },
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
        }


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



        public CategoryModel GetCategoryById(int CId)
        {
            CategoryModel Category = new CategoryModel();

            SqlCommand cmd = new SqlCommand("GetCategoryById", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param;
            cmd.Parameters.Add(new SqlParameter("@CId", CId));

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {



                Category = new CategoryModel()
                {
                    CId = Convert.ToInt32(dr["CId"]),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),

                    Active = Convert.ToBoolean(dr["Active"]),

                };
            }
            return Category;
        }
        public bool DeleteCategory(int Id)
        {
            using (SqlCommand cmd = new SqlCommand("DeleteCategory", _connection))
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
        }

        public List<CategoryModel> ActiveCategories()
        {
            var categories = new List<CategoryModel>();


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
        }


        public bool AddProductDetails(ProductModel product)
        {
            using (SqlCommand cmd = new SqlCommand("AddProductDetails", _connection))
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
        }

        public int GetTotalCategoryCount()
        {
            try
            {
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Category", _connection))
                {
                    _connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine("Error occurred while getting total category count: " + ex.Message);
                return -1; // Return a default value or throw an exception based on your error handling strategy
            }
            finally
            {
                // Ensure the connection is always closed, regardless of success or failure
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

    }
}
