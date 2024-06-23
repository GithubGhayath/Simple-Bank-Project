using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BankDataAccessLayer
{
    public class clsPeopleDataAccessLayer
    {

        /*
         * 
                        --PersonID
                        --FirstName
                        --MidName
                        --LastName
                        --PhoneNumber
                        --AccountNumber
                        --PINCode
                        --AccountBalance
         */

        static public int AddNewPerson(string firstName
                                       , string midName
                                       , string lastName
                                       , string phoneNumber
                                       , string accountNumber
                                       , string pINCode
                                       , decimal accountBalance
                                       , int CreatedBy)
        {
            int result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"
                                         DECLARE @SelectedValue INT;

                                         EXEC @SelectedValue= SP_AddNewPerson
			                                        @FirstName=@firstName,
			                                        @MidName=@midName,
			                                        @LastName=@lastName,
			                                        @PhoneNumber=@phoneNumber,
			                                        @AccountNumber=@accountNumber,
			                                        @PINCode=@pINCode,
			                                        @AccountBalance=@accountBalance,
                                                    @CreateBy = @CreatedBy

			                                        SELECT @SelectedValue";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@midName", midName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@accountNumber", accountNumber);
                        command.Parameters.AddWithValue("@pINCode", pINCode);
                        command.Parameters.AddWithValue("@accountBalance", accountBalance);
                        command.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                        connection.Open();

                        object obj = command.ExecuteScalar();

                        if (int.TryParse(obj.ToString(), out int re))
                        {
                            result = re;
                        }
                    }
                }
            }
            catch (Exception e) { }
            return result;
        }
        static public DataTable GetAllPeople()
        {
            DataTable People = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT *FROM People";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                People.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return People;
        }

        static public bool IsExistsPerson(int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT dbo.IsExistsPerson(@PersonID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        object obj = command.ExecuteScalar();

                        if (int.TryParse(obj.ToString(), out int Result))
                        {
                            IsFound = (Result == 1);
                        }

                    }
                }
            }
            catch (Exception ex) { }

            return IsFound;
        }

        static public bool Find(int PersonID
                                       , ref string firstName
                                       , ref string midName
                                       , ref string lastName
                                       , ref string phoneNumber
                                       , ref string accountNumber
                                       , ref string pINCode
                                       , ref decimal accountBalance,ref int CreatedBy)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
            {
                string query = @"EXEC SP_FindPersonByID @PERSONID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            firstName = (string)reader["FirstName"];
                            midName = (string)reader["MidName"];
                            lastName = (string)reader["LastName"];
                            phoneNumber = (string)reader["PhoneNumber"];
                            accountNumber = (string)reader["AccountNumber"];
                            pINCode = (string)reader["pINCode"];
                            accountBalance = (decimal)reader["AccountBalance"];
                            CreatedBy = (int)reader["CreateBy"];

                            IsFound = true;
                        }
                    }
                }
            }
            return IsFound;
        }

        static public bool UpdatePersonInfo(int PersonID, string firstName
                                                       , string midName
                                                        , string lastName
                                                        , string phoneNumber
                                                        , string accountNumber
                                                         , string pINCode
                                                          , decimal accountBalance)
        {
            bool IsUpdate = false;

            using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
            {
                string Query = @"
                                        DECLARE @IsUpdate BIT;

                                        EXEC @IsUpdate = SP_UpdatePersonINformation
				                                        @PersonID = @PersonID
				                                        ,@FirstName = @firstName
				                                        ,@MidNamin = @midName
				                                        ,@LastName = @lastName
				                                        ,@PhoneNumber = @phoneNumber
				                                        ,@AccountNumber = @accountNumber
				                                        ,@PinCode = @pINCode
				                                        ,@AccountBalance = @accountBalance
		                                        SELECT @IsUpdate";

                using(SqlCommand command = new SqlCommand(Query,connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@midName", midName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@accountNumber", accountNumber);
                    command.Parameters.AddWithValue("@pINCode", pINCode);
                    command.Parameters.AddWithValue("@accountBalance", accountBalance);

                    connection.Open();

                    object obj = command.ExecuteScalar();


                    IsUpdate = Convert.ToBoolean(obj);
                    
                }
            }

            return IsUpdate;
        }

        static public bool DeletePerson(int personid)
        {
            bool IsDeleted = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"
                                    DECLARE @ISDeleted INT;

                                    EXEC @ISDeleted = SP_DeletePerson @PersonID = @personid

                                    SELECT @ISDeleted";

                    using(SqlCommand command= new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue(@"personid", personid);


                        connection.Open();

                        object obj = command.ExecuteScalar();

                        IsDeleted = Convert.ToBoolean(obj);
                    }

                }
            }
            catch (Exception ex) { }

            return IsDeleted;
        }
    }
}