using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;

namespace BankDataAccessLayer
{
    public class clsClientDataAccessLayer
    {
        static public int AddNewClient(string firstName
                                       , string midName
                                       , string lastName
                                       , string phoneNumber
                                       , string accountNumber
                                       , string pINCode
                                       , decimal accountBalance
                                       ,int CreateBy)
        {
            int ClientID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"DECLARE @CLIENTID INT;

                                                EXEC SP_AddNewClient 
				                                                @FirstName = @firstName,
				                                                @MidName = @midName,
				                                                @LastName = @lastName,
				                                                @PhoneNumber = @phoneNumber,
				                                                @AccountNumber = @accountNumber,
				                                                @PINCODE = @pINCode,
				                                                @ACCOUNTBALANCE = @accountBalance,
				                                                @ClientID = @ClientID OUTPUT,
                                                                @CreateBy = @CreateBy

                                                SELECT @ClientID;
                                                ";

                    using (SqlCommand command = new SqlCommand (query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@midName", midName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@accountNumber", accountNumber);
                        command.Parameters.AddWithValue("@pINCode", pINCode);
                        command.Parameters.AddWithValue("@accountBalance", accountBalance);
                        command.Parameters.AddWithValue("@CreateBy", CreateBy);


                        connection.Open();

                        object obj = command.ExecuteScalar();

                        if(int.TryParse(obj.ToString(),out int result))
                        {
                            ClientID = result;
                        }
                    }
                }
            }
            catch (Exception ex) { }

                return ClientID;
        }

        static public DataTable GetAllClients()
        {
            DataTable Clients = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT * FROM ClientInformations";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                Clients.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return Clients;
        }

        static public bool UpdateClientInfo(int ClientID, string firstName
                                       , string midName
                                       , string lastName
                                       , string phoneNumber
                                       , string accountNumber
                                       , string pINCode
                                       , decimal accountBalance,int UserIDForArchive=1)
        {
            clsUtility.stClientInfo NewInfo;
            clsUtility.stClientInfo OldInfo;
            bool IsUpdated = false;

            //Fill New Info
            NewInfo.FirstName = firstName;
            NewInfo.MidName = midName;
            NewInfo.LastName = lastName;
            NewInfo.PhoneNumber = phoneNumber;
            NewInfo.AccountBalance = accountBalance;
            NewInfo.AccountNumber = accountNumber;
            NewInfo.PinCode = pINCode;

            //Fill Old Info
            int personid = 0;
            string F = string.Empty; //firstName
            string M = string.Empty; //midName
            string L = string.Empty; //lastName
            string PH = string.Empty; //phoneNumber
            string AN = string.Empty; //accountNumber
            string PI = string.Empty; //pINCode
            decimal AB = .0m; //accountBalance
            int CreatedBy = 0;

            Find(ClientID, ref personid);
            clsPeopleDataAccessLayer.Find(personid,ref F, ref M, ref L, ref PH, ref AN, ref PI, ref AB, ref CreatedBy);

            OldInfo.FirstName = F;
            OldInfo.MidName = M;
            OldInfo.LastName = L;
            OldInfo.PhoneNumber = PH;
            OldInfo.AccountBalance = AB;
            OldInfo.AccountNumber = AN;
            OldInfo.PinCode = PI;

            string Details = clsUtility.CompareAndFormatStringChanges(OldInfo, NewInfo);

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
                                DECLARE @ISUPDATED BIT;

                                EXEC @ISUPDATED = SP_UpdateClientInfo
		                                @ClientID = @ClientID,
		                                @FirstName = @firstName,
		                                @MidName = @midName,
		                                @LastName = @lastName,
		                                @PhoneNumber = @phoneNumber,
		                                @AccountNumber = @accountNumber,
		                                @PINCODE = @pINCode,
		                                @ACCOUNTBALANCE = @accountBalance
		

		                                select @ISUPDATED
                                ";
                    using (SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@midName", midName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@accountNumber", accountNumber);
                        command.Parameters.AddWithValue("@pINCode", pINCode);
                        command.Parameters.AddWithValue("@accountBalance", accountBalance);

                        connection.Open();

                        object obj = command.ExecuteScalar();

                        IsUpdated = Convert.ToBoolean(obj);
                        if (IsUpdated)
                            clsUtility.Archive(ClientID, UserIDForArchive, Details);
                    }

                }
            }
            catch (Exception ex) { }

            return IsUpdated;
        }

        static public bool Find(int ClientID, ref int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT * FROM FindClient(@ClientID)";

                    using (SqlCommand commmand= new SqlCommand(query, connection))
                    {
                        commmand.Parameters.AddWithValue("@ClientID", ClientID);
                        connection.Open();
                        using(SqlDataReader reader = commmand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
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

        static public bool DeleteClient(int ClientID)
        {
            bool isdeleted = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
	                                DECLARE @ISDELETE INT

	                                EXEC @ISDELETE = SP_DeleteClient
	                                @CLIENTID = @ClientID,
	                                @ISDELETED = @ISDELETE OUTPUT

	                                SELECT @ISDELETE";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);

                        connection.Open();

                        object obj = command.ExecuteScalar();

                        if(int.TryParse(obj.ToString(),out int result))
                        {
                            isdeleted = (result <= 3 && result > 0);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return isdeleted;
        }

        static public decimal GetTotlaBalances()
        {
            decimal Totle = .0m;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT dbo.TotalBalance()";

                    using(SqlCommand command= new SqlCommand(Query, connection))
                    {
                        connection.Open();

                        object obj = command.ExecuteScalar();

                        Totle = Convert.ToDecimal(obj);
                    }
                }

            }
            catch (Exception ex) { }

            return Totle;
        }

        static public DataTable ShowTotalBalance()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT *FROM ShowTotalBalances";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return dt;
        }

        static public bool IsClientExists(int ClientID)
        {
            bool IsExists = false;

            try
            {
                using(SqlConnection connection = new SqlConnection (clsSittings.ConnectionString))
                {
                    string Query = @"SELECT dbo.IsClientExists(@ClientID)";

                    using(SqlCommand command= new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);

                        connection.Open();

                        IsExists = Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex) { }

            return IsExists;
        }

        static public bool Find(string AccountNumber,string PINCode,ref int PersonID,ref int ClientID)
        {
            bool IsFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT * FROM dbo.FindClientByAccountNumberPINCode(@AccountNumber,@PINCode)";
                    using(SqlCommand command = new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        command.Parameters.AddWithValue("@PINCode", PINCode);

                        connection.Open();
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                if (reader["PERSONID"] != null)
                                    PersonID = (int)reader["PERSONID"];
                                if (reader["ClientID"] != null)
                                {
                                    ClientID = (int)reader["ClientID"];
                                    IsFound = true;
                                }
                                else
                                    return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return IsFound;
        }
    }
}
