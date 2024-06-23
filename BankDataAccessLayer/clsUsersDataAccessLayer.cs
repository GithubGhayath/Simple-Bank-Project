using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BankDataAccessLayer
{
    public class clsUsersDataAccessLayer
    {
        /*
      * 
                    --UserID
                    --UserName
                    --Password
                    --PersonID
                    --Permissions
      */
        static public DataTable GetAllUsers()
        {
            DataTable Users = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
						SELECT * FROM UsersInformations";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                Users.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return Users;
        }
        static public bool IsUserExists(int UserID)
        {
            bool IsUserExists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT dbo.IsUserExists(@UserID)";

                    using(SqlCommand command= new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();

                        IsUserExists = Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch(Exception ex) { }

            return IsUserExists;
        }
        public static int AddNewUser(string UserName, string Password, int Permissions, string firstname, string midName, string lastName,
            string phoneNumber, string accountNumber, string pINCode, decimal accountBalance, int createby)
        {
            int Userid = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"DECLARE @USERIDD INT;

	                    EXEC @USERIDD = SP_AddNewUser 
				                  	@USERNAME =@UserName,
	                                @PASSWORD =@Password,
	                                @PERMISSIONS = @Permissions ,
	                                @personID = @personid
				                    SELECT @USERIDD
";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int PersonIDD = clsPeopleDataAccessLayer.AddNewPerson(firstname, midName, lastName, phoneNumber, accountNumber, pINCode, accountBalance, createby);

                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Permissions", Permissions);
                        command.Parameters.AddWithValue("@personid", PersonIDD);
                        


                        object obj = command.ExecuteScalar();

                        if(obj != null)
                        {
                            Userid = Convert.ToInt32(obj);
                        }
              
                    }
                }
            }
            catch (Exception ex) { }
            return Userid;
        }
        static public bool Find(int UserID,ref string UserName,ref string Password,ref int Permissions,ref int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @" SELECT * FROM dbo.FindUser(@UserID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserName = (string)reader["UserName"];
                                Password = (string)reader["Password"];
                                Permissions = (int)reader["Permissions"];
                                PersonID = (int)reader["PersonID"];

                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch(Exception ex) { }

            return IsFound;
        }
        public static bool Update(int UserID,string UserName,string Password,int Permissions)
        {
            bool IsUpdated = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
	                                    DECLARE @ISUPDATED BIT;
		                                    EXEC @ISUPDATED = SP_UpdateUserInformation
								                                    @UserName = @UserName,
								                                    @Password = @Password,
								                                    @Permissions = @Permissions,
								                                    @UserID = @UserID
		                                    SELECT @ISUPDATED";

                    using(SqlCommand command=new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Permissions", Permissions);

                        connection.Open();

                        IsUpdated = Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch(Exception ex) { }
            return IsUpdated;
        }
        static public bool Find(ref int UserID, string UserName, ref string Password, ref int Permissions, ref int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT * FROM dbo.FindUserByUserName(@UserName)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserID = (int)reader["UserID"];
                                Password = (string)reader["Password"];
                                Permissions = (int)reader["Permissions"];
                                PersonID = (int)reader["PersonID"];

                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return IsFound;
        }
        static public DataTable GetUsersActive()
        {
            DataTable result = new DataTable();

            try
            {

                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT * FROM  dbo.GetUsersActive()";

                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                result.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return result;
        }
        static public DataTable SelectUserFromView(int UserID)
        {
            DataTable result = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"select *from UsersInformations WHERE USERID = @UserID";
                    using(SqlCommand command = new SqlCommand (Query,connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        connection.Open();

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                result.Load(reader);
                        }
                    }
                }
            }
            catch(Exception ex) { }

            return result;
        }

        static public bool DeleteUser(int UserID)
        {
            bool IsDeleted = false;

            try
            {
                using(SqlConnection connection = new SqlConnection (clsSittings.ConnectionString))
                {
                    string query = @"
                                    DECLARE @ROW INT;

                                    EXEC @ROW = SP_DeleteUser
	                                    @USERID = @UserID
	                                    SELECT @ROW

                                    ";
                    using(SqlCommand command =new SqlCommand (query,connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        connection.Open();

                        int Result = Convert.ToInt32(command.ExecuteScalar());

                        IsDeleted = (Result >= 1);
                    }
                }
            }
            catch(Exception ex) { }
            return IsDeleted;
        }
    }
}
