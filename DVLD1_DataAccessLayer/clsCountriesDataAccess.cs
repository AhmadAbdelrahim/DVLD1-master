using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_DataAccessLayer
{
    public class clsCountriesDataAccess
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from Countries where CountryID = @CountryID";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    CountryName = (string)Reader["CountryName"];                    
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }

                Reader.Close();
            }
            catch (Exception ex1)
            {
                //Console.WriteLine(ex.Message);
                IsFound = false;
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from Countries where CountryName = @CountryName";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    ID = (int)Reader["CountryID"];                   
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception ex1)
            {
                //Console.WriteLine(ex.Message);
                IsFound = false;
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }

        public static int AddNewCountry(string CountryName)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int CountryID = -1;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"insert into Countries (CountryName)
                            Values (@CountryName);
                            select Scope_Identity();";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryName", CountryName);            

            try
            {
                Connect.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    CountryID = insertedID;
                }
            }
            catch (Exception ex2)
            {
                //Console.WriteLine(ex.Message);
                CountryID = -1;
            }
            finally
            {
                Connect.Close();
            }
            return CountryID;
        }

        public static bool UpdateCountry(int ID, string CountryName)
        {
            int Result = 0;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "update Countries set CountryName = @CountryName where CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryID", ID);
            Command.Parameters.AddWithValue("@CountryName", CountryName);
            
            try
            {
                Connect.Open();

                Result = Command.ExecuteNonQuery();
            }
            catch (Exception ex3)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Connect.Close();
            }
            return (Result > 0); // true إذا كان الريزلت اكبر من صفر معناته 
        }

        public static bool DeleteCountry(int ID)
        {
            int Result = 0;

            SqlConnection connection = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "DELETE FROM Countries WHERE CountryID = @CountryID";

            //string Query = "DELETE FROM Countries " +
            //                "WHERE CountryID = @CountryID";

            //string Query = @"DELETE FROM Countries
            //                 WHERE CountryID = @CountryID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CountryID", ID);
            try
            {
                connection.Open();

                Result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return Result > 0;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select * from Countries order by CountryName";

            SqlCommand Command = new SqlCommand(Query, Connect);

            try
            {
                Connect.Open();

                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows) // إذا كان فيه بيانات
                {
                    dt.Load(Reader); // DataTable حملهم في الـ 
                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Connect.Close();
            }
            return dt;
        }

        public static bool IsCountryExist(int ID)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from Countries where CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                IsFound = false;
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }

        public static bool IsCountryExist(string CountryName)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from Countries where CountryName = @CountryName";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                IsFound = false;
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }
    }
}
