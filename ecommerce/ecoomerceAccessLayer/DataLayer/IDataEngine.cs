using ecommerce.Models;
using System.Data.SqlClient;
using System.Data;

namespace ecommerce.ecoomerceAccessLayer.DataLayer
{
    public interface IDataEngine
    {
        DataTable ExecuteProcedureDatatable(string storedProc, SqlConnection Conn, List<Param> param);
    }
}
