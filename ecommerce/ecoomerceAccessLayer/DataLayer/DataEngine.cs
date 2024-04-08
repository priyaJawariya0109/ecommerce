using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ecommerce.Models;
using Microsoft.Extensions.Configuration;


namespace ecommerce.ecoomerceAccessLayer.DataLayer
{
    public class DataEngine: IDataEngine
    {
        public string errorMessage;
        public string errorDescription;
        public int GetSec = 600;
        private readonly IConfiguration _configuration;

        public DataEngine(IConfiguration configuration)
        {
            //errorMessage = "Connection Error : Please try, after some time.";
            //errorDescription = "";
            _configuration = configuration;
        }

        /// <summary>
        /// Open connection of database with specified username and password
        /// </summary>
        /// <param name="Conn">SqlConnection</param>
        /// <returns>bool</returns>
        public bool OpenConnection(SqlConnection Conn)
        {
            //   System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()
            string connStr = _configuration.GetConnectionString("DefaultConnection").ToString();
                //System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();


            errorMessage = "";
            errorDescription = "";
            if (Conn == null)
            {
                errorMessage = "Connection Object doesn't exists. Internal Error";
                return false;
            }
            if (Conn.State != ConnectionState.Open)
            {
                try
                {
                    if (Conn == null)
                    {
                        Conn = new SqlConnection(); // Initialize the connection object if it's null
                    }

                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.ConnectionString = connStr; // Set the connection string
                        Conn.Open(); // Open the connection
                    }
                    return true;
                }
                catch (Exception objE)
                {
                    errorMessage = "Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString() + ". LineNumber= " + objE.LineNumber();
                    errorDescription = " StackTrace : " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                    Utility.WriteMsg(errorMessage + " " + errorDescription);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Close connection of database
        /// </summary>
        /// <param name="Conn">SqlConnection</param>
        /// <returns>bool</returns>
        public bool CloseConnection(SqlConnection Conn)
        {
            errorMessage = "";
            errorDescription = "";

            if (Conn.State == ConnectionState.Closed)
            {
                return true;
            }
            else
            {
                try
                {
                    Conn.Close();
                    return true;
                }
                catch (SqlException objE)
                {
                    errorMessage = "Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString() + ". LineNumber= " + objE.LineNumber();
                    errorDescription = " StackTrace : " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                    Utility.WriteMsg(errorMessage + " " + errorDescription);
                    return false;
                }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }
        /// <summary>
        /// Check given input is numeric or not
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>bool</returns>
        public bool IsNumeric(string id)
        {
            errorMessage = "";
            errorDescription = "";

            for (int i = 0; i < id.Length; i++)
            {
                if (Convert.ToInt32(id[i]) > 47 && Convert.ToInt32(id[i]) < 59)
                {
                    if (i == id.Length) return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Check given input is decimal or not
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>bool</returns>
        public bool IsDecimal(string id)
        {
            errorMessage = "";
            errorDescription = "";

            for (int i = 0; i < id.Length; i++)
            {
                if (Convert.ToInt32(id[i]) > 47 && Convert.ToInt32(id[i]) < 59 || Convert.ToInt32(id[i]) == 46)
                {
                    if (i == id.Length) return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check given input is valid email address or not
        /// </summary>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public bool IsValidEmailAddress(string sEmail)
        {
            errorMessage = "";
            errorDescription = "";

            if (sEmail == null)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(sEmail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", RegexOptions.IgnorePatternWhitespace);
            }
        }


        /// <summary>
        /// Execute stored procedure and return int value
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>int</returns>
        public int ExecuteProcedureInt(string storedProc, SqlConnection Conn, List<Param> param)
        {

            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;
            int rowsAffected;

            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                rowsAffected = dbCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    tran.Commit();
                    return rowsAffected;
                }
                else
                {
                    tran.Rollback();
                    return -1;
                }
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }

            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        /// <summary>
        /// Execute stored procedure with return integer value
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>int</returns>
        public int ExecuteProcedureReturn(string storedProc, SqlConnection Conn, List<Param> param)
        {

            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;
            int rowsAffected;

            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            SqlParameter retValue = dbCommand.Parameters.Add("return", SqlDbType.Int);
            retValue.Direction = ParameterDirection.ReturnValue;
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                rowsAffected = dbCommand.ExecuteNonQuery();

                if (int.Parse(retValue.Value.ToString()) > 0)
                {
                    tran.Commit();
                    return int.Parse(retValue.Value.ToString());
                }
                else
                {
                    tran.Rollback();
                    return int.Parse(retValue.Value.ToString());
                }
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }

            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        /// <summary>
        /// Execute stored procedure and get data in dataset
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteProcedureDataset(string storedProc, SqlConnection Conn, List<Param> param)
        {
            DataSet dsList = new DataSet();
            SqlTransaction tran;
            SqlCommand dbCommand = new SqlCommand();
            dbCommand.Connection = Conn;
            dbCommand.CommandTimeout = GetSec;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                SqlDataAdapter da = new SqlDataAdapter(dbCommand);
                da.Fill(dsList);
                tran.Commit();
                return dsList;
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        /// <summary>
        /// Execute stored procedure and get data in string
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>string</returns>
        public string ExecuteProcedureScalar(string storedProc, SqlConnection Conn, List<Param> param)
        {
            DataSet dsList = new DataSet();
            SqlTransaction tran;
            SqlCommand dbCommand = new SqlCommand();
            dbCommand.Connection = Conn;
            dbCommand.CommandTimeout = GetSec;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            string output = "";

            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;

            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                output = Convert.ToString(dbCommand.ExecuteScalar());
                tran.Commit();

            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }

            return output;
        }

        /// <summary>
        /// Execute stored procedure and get data in datatable
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteProcedureDatatable(string storedProc, SqlConnection Conn, List<Param> param)
        {
            DataTable dt = new DataTable();
            SqlTransaction tran;
            SqlCommand dbCommand = new SqlCommand();
            dbCommand.Connection = Conn;
            dbCommand.CommandTimeout = GetSec;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                SqlDataAdapter da = new SqlDataAdapter(dbCommand);
                da.Fill(dt);
                tran.Commit();
                return dt;
            }

            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        public int ExecuteStoredProcedureWithTable(string storedProc, SqlConnection Conn, List<Param> param, string dtparamName, DataTable dtParams)
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;
            int rowsAffected;
            //string ID = "0";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (!string.IsNullOrEmpty(dtparamName))
            {
                SqlParameter sqlParam = dbCommand.Parameters.Add(dtparamName, SqlDbType.Structured);
                sqlParam.Value = dtParams;
            }
            SqlParameter retValue = dbCommand.Parameters.Add("return", SqlDbType.Int);
            retValue.Direction = ParameterDirection.ReturnValue;
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                rowsAffected = dbCommand.ExecuteNonQuery();

                if (int.Parse(retValue.Value.ToString()) > 0)
                {
                    tran.Commit();
                    return int.Parse(retValue.Value.ToString());
                }
                else
                {
                    tran.Rollback();
                    return int.Parse(retValue.Value.ToString());
                }

            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                throw;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }


        }

        /// <summary>
        /// Execute stored procedure and return int value
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <returns>int</returns>
        public int ExecuteProcedureIntWithTable(string storedProc, SqlConnection Conn, List<Param> param, string dtparamName, DataTable dtParams)
        {

            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;
            int rowsAffected;

            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (!string.IsNullOrEmpty(dtparamName))
            {
                SqlParameter sqlParam = dbCommand.Parameters.Add(dtparamName, SqlDbType.Structured);
                sqlParam.Value = dtParams;
            }
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                rowsAffected = dbCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    tran.Commit();
                    return rowsAffected;
                }
                else
                {
                    tran.Rollback();
                    return -1;
                }
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }

            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        /// <summary>
        /// Save error log
        /// </summary>
        /// <param name="methodName">Method name </param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="errorDescription">Error description</param>
        /// <param name="userId">Current user's id</param>
        /// <param name="con">SqlConnection object</param>
        public void SaveErrorLog(string methodName, string errorMessage, string errorDescription, string userId, SqlConnection con)
        {
            Serviceparameters sp = new Serviceparameters();
            sp.ProcedureName = "SaveErrorLog";
            sp.ParameterList = new List<Param>() {
                    new Param { Name = "@METHOD_NAME", Value = methodName } ,
                    new Param { Name = "@ERROR_MESSAGE", Value = errorMessage } ,
                    new Param { Name = "@ERROR_DESCRIPTION", Value = errorDescription } ,
                    new Param { Name = "@USER_ID", Value = userId}

                };
            ExecuteProcedureInt(sp.ProcedureName, con, sp.ParameterList);
        }

        /// <summary>
        /// Convert date to specific format
        /// </summary>
        /// <returns>string</returns>
        public string DateConvert()
        {
            double timdiff = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["timdiff"]);
            DateTime i = DateTime.Now;
            string newdate = i.AddHours(timdiff).ToString();
            DateTime j = Convert.ToDateTime(newdate);
            newdate = j.ToString("yyyy-MM-dd") + " " + j.ToString("HH:mm:ss");
            return newdate;
        }
        /// <summary>
        /// Execute stored procedure and return int value
        /// </summary>
        /// <param name="storedProc">Procedure Name</param>
        /// <param name="Conn">SqlConnection object</param>
        /// <param name="param">List<Param></param>
        /// <param name="dtparamName">datatable list name</param>
        /// <param name="dtParams">datatable list</param>
        /// <returns>int</returns>
        public int ExecuteProcedureIntWithMultiTable(string storedProc, SqlConnection Conn, List<Param> param, string[] dtparamName, DataTable[] dtParams)
        {

            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;
            int rowsAffected;

            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            foreach (var item in param)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            for (int i = 0; i <= dtparamName.Count() - 1; i++)
            {
                if (!string.IsNullOrEmpty(dtparamName[i]))
                {
                    SqlParameter sqlParam = dbCommand.Parameters.Add(dtparamName[i], SqlDbType.Structured);
                    sqlParam.Value = dtParams[i];
                }
            }
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            SqlParameter retValue = dbCommand.Parameters.Add("return", SqlDbType.Int);
            retValue.Direction = ParameterDirection.ReturnValue;
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                rowsAffected = dbCommand.ExecuteNonQuery();
                if (int.Parse(retValue.Value.ToString()) > 0)
                {
                    tran.Commit();
                    return int.Parse(retValue.Value.ToString());
                }
                else
                {
                    tran.Rollback();
                    return int.Parse(retValue.Value.ToString());
                }
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                throw;
            }

            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        public string ExecuteStoredProcedureOutputWithMultiTable(string storedProc, SqlConnection Conn, string[] dtparamName, DataTable[] dtParams, List<Param> arrparam, string OutputParamName = "ID")
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;

            string v = "";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            dbCommand.CommandTimeout = 500;
            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            for (int p = 0; p < dtparamName.Length; p++)
            {
                SqlParameter sqlParam = dbCommand.Parameters.Add(dtparamName[p], SqlDbType.Structured);
                sqlParam.Value = dtParams[p];
            }
            dbCommand.Parameters.Add("@" + OutputParamName, SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();
                string ID = dbCommand.Parameters["@" + OutputParamName].Value.ToString();
                if (!string.IsNullOrEmpty(ID))
                {
                    tran.Commit();
                    return ID;
                }
                else
                {
                    tran.Rollback();
                    return ID;
                }
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }

            return v;

        }

        public string ExecuteStoredProcedureOutputWithTable(string storedProc, SqlConnection Conn, string dtparamName, DataTable dtParams, List<Param> arrparam, string OutputParamName = "ID")
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;

            string v = "";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }
            if (dtparamName != null && dtparamName != "")
            {
                SqlParameter sqlParam = dbCommand.Parameters.Add(dtparamName, SqlDbType.Structured);
                sqlParam.Value = dtParams;
            }

            dbCommand.Parameters.Add("@" + OutputParamName, SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();
                string ID = dbCommand.Parameters["@" + OutputParamName].Value.ToString();
                if (!string.IsNullOrEmpty(ID))
                {
                    tran.Commit();
                    return ID;
                }
                else
                {
                    tran.Rollback();
                    return ID;
                }
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }

            return v;

        }

        public string ExecuteStoredProcedureOutput(string storedProc, SqlConnection Conn, List<Param> arrparam, string OutputParamName = "ID")
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;

            string v = "";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }

            SqlParameter sqlParamOut = dbCommand.Parameters.Add("@" + OutputParamName, SqlDbType.VarChar, 500);
            sqlParamOut.Direction = ParameterDirection.Output;

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();
                string ID = dbCommand.Parameters["@" + OutputParamName].Value.ToString();
                if (!string.IsNullOrEmpty(ID))
                {
                    tran.Commit();
                    return ID;
                }
                else
                {
                    tran.Rollback();
                    return ID;
                }
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                Utility.WriteMsg("3. AddUser => " + errorMessage + " " + errorDescription);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
            return v;
        }


        public int getMaxid(string fieldname, string tablename)
        {
            SqlConnection Conn = new SqlConnection();
            int strResult = 0;
            try
            {

                if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
                DataSet dsLoadData = GetDataSet(" select Max(" + fieldname + ") as MaxNo from " + tablename + "", "Generic", Conn);
                if (dsLoadData.Tables.Count > 0 && dsLoadData.Tables[0].Rows.Count > 0)
                {
                    if (dsLoadData.Tables[0].Rows[0]["MaxNo"].ToString().Trim() == "" || dsLoadData.Tables[0].Rows[0]["MaxNo"].ToString().Trim() == "0")
                    { strResult = 1; }
                    else
                        strResult = int.Parse(dsLoadData.Tables["Generic"].Rows[0]["MaxNo"].ToString()) + 1;
                }
                else { strResult = 1; }
                //if (strResult.Trim() == "") strResult = 0;
                return strResult;
            }
            catch
            {
                return strResult;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }

        }

        public DataSet GetDataSet(string cmdStr, string tableName, SqlConnection Conn)
        {
            errorMessage = "";
            errorDescription = "";

            DataSet ds = new DataSet();
            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn;
            cmd.CommandTimeout = 600;
            //Boolean blnState= OpenConnection(Conn);
            //if (blnState == false)
            //    return null ;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmdStr, Conn);
                da.SelectCommand.CommandTimeout = 180;
                da.Fill(ds, tableName);
                return ds;
            }
            catch (Exception objE)
            {
                errorMessage = "<font Color=RED>Exception Occured :</Font>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " StackTrace : " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                ds = null;
                return ds;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }

        public string ExecuteStoredProcedureOutputList(string storedProc, SqlConnection Conn, List<Param> arrparam, ref List<Param> OutputParam)
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;

            string v = "";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }

            foreach (var item in OutputParam)
            {
                SqlParameter sqlParamOut = dbCommand.Parameters.Add("@" + item.Name, SqlDbType.VarChar, 1000);
                sqlParamOut.Direction = ParameterDirection.Output;
            }


            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();
                //v = rowsAffected.ToString();
                foreach (var item in OutputParam)
                {
                    string val = dbCommand.Parameters["@" + item.Name].Value.ToString();
                    item.Value = val;
                }
                tran.Commit();
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                Utility.WriteMsg("3. AddUser => " + errorMessage + " " + errorDescription);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }

            return v;

        }


        public int ExecuteStoredProcedureOutputListTrans(string storedProc, SqlConnection Conn, List<Param> arrparam, ref List<Param> OutputParam, SqlTransaction tran = null, bool TranReAssign = false)
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();

            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;

            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }

            foreach (var item in OutputParam)
            {
                SqlParameter sqlParamOut = dbCommand.Parameters.Add("@" + item.Name, SqlDbType.VarChar, 1000);
                sqlParamOut.Direction = ParameterDirection.Output;
            }

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);

            if (TranReAssign) tran = Conn.BeginTransaction();
            if (tran != null) dbCommand.Transaction = tran;

            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();

                foreach (var item in OutputParam)
                {
                    string val = dbCommand.Parameters["@" + item.Name].Value.ToString();
                    item.Value = val;
                }

                return rowsAffected;
                //tran.Commit();
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
                Utility.WriteMsg("3. AddUser => " + errorMessage + " " + errorDescription);

                return 0;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
        }


        public string ExecuteStoredProcedureOutputWithXML_Data(string xmlData, string storedProc, SqlConnection Conn, List<Param> arrparam, string OutputParamName = "Result")
        {
            DataSet dsList = new DataSet();
            SqlCommand dbCommand = new SqlCommand();
            SqlTransaction tran;

            string v = "";
            dbCommand.Connection = Conn;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = storedProc;
            dbCommand.CommandTimeout = 500;
            foreach (var item in arrparam)
            {
                SqlParameter sqlParam = dbCommand.Parameters.AddWithValue(item.Name, item.Value);
            }

            dbCommand.Parameters.AddWithValue("@xmlOrder_ID", xmlData);
            dbCommand.Parameters.Add("@" + OutputParamName, SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

            if (Conn.State == ConnectionState.Closed) OpenConnection(Conn);
            tran = Conn.BeginTransaction();
            dbCommand.Transaction = tran;
            try
            {
                int rowsAffected;
                rowsAffected = dbCommand.ExecuteNonQuery();
                string ID = dbCommand.Parameters["@" + OutputParamName].Value.ToString();
                if (!string.IsNullOrEmpty(ID))
                {
                    tran.Commit();
                    return ID;
                }
                else
                {
                    tran.Rollback();
                    return ID;
                }
            }
            catch (Exception objE)
            {
                tran.Rollback();
                errorMessage = "<B><font Color=RED>Exception Occured :</Font></B>  Message= " + objE.Message.ToString() + ". Method= " + objE.TargetSite.Name.ToString();
                errorDescription = " <B>StackTrace :</B> " + objE.StackTrace.ToString() + " Source = " + objE.Source.ToString();
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) CloseConnection(Conn);
            }
            return v;
        }
    }

}


