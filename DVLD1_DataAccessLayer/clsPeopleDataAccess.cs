using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_DataAccessLayer
{
    public class clsPeopleDataAccess
    {
        public static bool GetPersonDataByID(int PersonID, ref string NationalNo, ref string FirstName, 
            ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, 
            ref short Gender, ref string Address, ref string Phone, ref string Email, 
            ref int Nationality, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from People where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    NationalNo = (string)Reader["NationalNo"];
                    FirstName = (string)Reader["FirstName"];
                    
                    //SecondName and ThirdName: allows null in database so we should handle null
                    if (Reader["SecondName"] != DBNull.Value)
                        SecondName = (string)Reader["SecondName"];
                    else
                        SecondName = "";
                    
                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";
                    
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gender = Convert.ToByte(Reader["Gender"]);
                    
                    //Address: allows null in database so we should handle null
                    if (Reader["Address"] != DBNull.Value)
                        Address = (string)Reader["Address"];
                    else
                        Address = "";
                    
                    Phone = (string)Reader["Phone"];
                    
                    //Email: allows null in database so we should handle null
                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";
                    
                    Nationality = (int)Reader["Nationality"];
                    
                    //ImagePath: allows null in database so we should handle null
                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = string.Empty; // or ImagePath = "";                    

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

        public static bool GetPersonDataByNationalNo(ref int PersonID, string NationalNo, ref string FirstName,
          ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
          ref short Gender, ref string Address, ref string Phone, ref string Email,
          ref int Nationality, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);
            string Query = "select * from People where NationalNo = @NationalNo";
            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connect.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    // The record was found
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];

                    //SecondName and ThirdName: allows null in database so we should handle null
                    if (Reader["SecondName"] != DBNull.Value)
                        SecondName = (string)Reader["SecondName"];
                    else
                        SecondName = "";

                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";

                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gender = Convert.ToByte(Reader["Gender"]);

                    //Address: allows null in database so we should handle null
                    if (Reader["Address"] != DBNull.Value)
                        Address = (string)Reader["Address"];
                    else
                        Address = "";

                    Phone = (string)Reader["Phone"];

                    //Email: allows null in database so we should handle null
                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";

                    Nationality = (int)Reader["Nationality"];

                    //ImagePath: allows null in database so we should handle null
                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = string.Empty; // or ImagePath = "";                    

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

        public static int AddNewPerson(string NationalNo, string FirstName, 
            string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, 
            short Gender, string Address, string Phone, string Email, int Nationality, string ImagePath)
        {
            //this function will return the new Person id if succeeded and -1 if not.
            int PersonID = -1;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"insert into People (NationalNo, FirstName, SecondName, 
                                                ThirdName, LastName, DateOfBirth, Gender, Address, 
                                                Phone, Email, Nationality, ImagePath)
                            Values (@NationalNo, @FirstName, @SecondName, @ThirdName, 
                                    @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, 
                                    @Nationality, @ImagePath);
                            select Scope_Identity();";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);

            if(SecondName != "" && SecondName != null)
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gender", Gender);

            if (Address != "" && Address != null)
                Command.Parameters.AddWithValue("@Address", Address);
            else
                Command.Parameters.AddWithValue("@Address", DBNull.Value);            

            Command.Parameters.AddWithValue("@Phone", Phone);

            if(Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            Command.Parameters.AddWithValue("@Nationality", Nationality);

            if (ImagePath != "" && ImagePath != null)
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                Connect.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
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
            return PersonID;
        }

        public static bool UpdatePersonData(int PersonID, string NationalNo, string FirstName, 
            string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, short Gender, 
            string Address, string Phone, string Email, int Nationality, string ImagePath)
        {
            int Result = 0;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"update People set
                            NationalNo = @NationalNo,
                            FirstName = @FirstName,
                            SecondName = @SecondName,
                            ThirdName = @ThirdName,
                            LastName = @LastName,
                            DateOfBirth = @DateOfBirth,
                            Gender = @Gender,
                            Address = @Address,                             
                            Phone = @Phone,
                            Email = @Email,
                            Nationality = @Nationality,
                            ImagePath = @ImagePath
                            where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);

            if (SecondName != "" && SecondName != null)
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gender", Gender);

            if (Address != "" && Address != null)
                Command.Parameters.AddWithValue("@Address", Address);            
            else
                Command.Parameters.AddWithValue("@Address", DBNull.Value);
            
            Command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value); 

            Command.Parameters.AddWithValue("@Nationality", Nationality);

            if (ImagePath != "" && ImagePath != null)
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

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

        public static bool DeletePerson(int PersonID)
        {
            int Result = 0;

            SqlConnection connection = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "DELETE FROM People WHERE PersonID = @PersonID";

            //string Query = "DELETE FROM Persons " +
            //                "WHERE PersonID = @PersonID";

            //string Query = @"DELETE FROM Persons
            //                 WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
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

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = @"SELECT People.PersonID, People.NationalNo, People.FirstName, 
                                    People.SecondName, People.ThirdName, People.LastName, 
                                    People.DateOfBirth, People.Gender,  
                                    CASE WHEN People.Gender = 0 
                                    THEN 'Male'
                                    ELSE 'Female'
                                    END as GenderCaption,
                                    People.Address, People.Phone, People.Email, People.Nationality, 
                                    Countries.CountryName, People.ImagePath
                                    FROM People 
                                    INNER JOIN Countries ON People.Nationality = Countries.CountryID
                                    ORDER BY People.FirstName";

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
                //Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Connect.Close();
            }
            return dt;
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from People where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;

            SqlConnection Connect = new SqlConnection(clsConnectionSettings.ConnectionString);

            string Query = "select found = 1 from People where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connect);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
