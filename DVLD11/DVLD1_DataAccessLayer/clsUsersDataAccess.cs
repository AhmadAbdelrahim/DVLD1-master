using DVLD1_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsUsersDataAccess
    {
        public static bool User(ref int UserID, ref int PersonID, string Username, string Password, ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from Users where Username = @Username and Password = @password";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@Username", Username);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    UserID = (int)Reader["UserID"];
                    PersonID = (int)Reader["PersonID"];
                    Username = (string)Reader["Username"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];

                    Reader.Close();
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
            }
            catch (Exception ex1)
            {
                //Console.WriteLine(ex.Message);
                //IsFound = false;
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }

        public static bool GetUserDataByID(int UserID, ref int PersonID, ref string Username, ref string Password,
           ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from Users where UserID = @UserID";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    Username = (string)Reader["Username"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];               

                    Reader.Close();
                }
                else
                {
                    // The record was not found
                    IsFound = false;
                }
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

        public static int AddNewUser(string Username, string Password, bool IsActive)
        {
            //this function will return the new Person id if succeeded and -1 if not.
            int UserID = -1;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"insert into People (Username, Password, IsActive)
                            Values (@Username, @Password, @IsActive);
                            select Scope_Identity();";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@Username", Username);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            
            try
            {
                Connect.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                Connect.Close();
            }
            return UserID;
        }

        public static bool UpdateUserData(int UserID, string Username, string Password, bool IsActive)
        {
            int Result = 0;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"update Users set
                            Username = @Username,
                            Password = @Password,
                            IsActive = @IsActive
                            where UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@Username", Username);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                Connect.Open();
                Result = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
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

        public static bool DeleteUser(int UserID)
        {
            int Result = 0;

            SqlConnection connection = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "DELETE FROM Users WHERE UserID = @UserID";

            //string Query = "DELETE FROM Users " +
            //                "WHERE UserID = @UserID";

            //string Query = @"DELETE FROM Users
            //                 WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
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

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"SELECT * from Users";

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

        public static DataTable GetAllUsersByUsername(string username)
        {
            DataTable dataTable = new DataTable();

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string query = @"SELECT * from Users
                            WHERE Username LIKE '%' + @Username + '%'";

            SqlCommand Command = new SqlCommand(query, Connect);
            Command.Parameters.AddWithValue("@Username", username);

            try
            {
                Connect.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows) // إذا كان فيه بيانات
                {
                    dataTable.Load(Reader); // DataTable حملهم في الـ 
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
            return dataTable;
        }

        public static bool IsUserExist(int UserID)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from Users where UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@UserID", UserID);

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
            }
            finally
            {
                Connect.Close();
            }
            return IsFound;
        }

        public static bool IsUserExist(string username)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from Users where Username = @Username";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@Username", username);

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
