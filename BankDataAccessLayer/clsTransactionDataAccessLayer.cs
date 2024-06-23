using Syncfusion.Windows.Forms.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using static Humanizer.In;

namespace BankDataAccessLayer
{
    public class clsTransactionDataAccessLayer
    {
        static public bool Transfer(string FromAccount,string ToAccount,decimal Amount)
        {
            bool IsDone = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @" DECLARE @ROWID INT

                        EXEC SP_AddTransfer
	                    @RowID OUTPUT,
	                    @FromAccoutNumber = @FromAccount,
	                    @ToAccoutNumber = @ToAccount,
	                    @Amount = @Amount

                    SELECT @RowID";

                    using(SqlCommand command= new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@FromAccount", FromAccount);
                        command.Parameters.AddWithValue("@ToAccount", ToAccount);
                        command.Parameters.AddWithValue("@Amount", Amount);

                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Convert.ToInt32(Result) > 0)
                            IsDone = true;
                        
                    }
                }
            }
            catch(Exception e) { }

            return IsDone;
        }
        static public DataTable GetAllTransfers()
        {
            DataTable dt = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT * FROM TransferLog";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        connection.Open();
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch(Exception e) { }
            return dt;
        }
        static public bool Withdraw(int ClientID,decimal Amount)
        {
            bool IsDone = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"DECLARE @ISPROCESSDONE BIT;

                        EXEC @ISPROCESSDONE = SP_withdraw
					                        @ClientID = @ClientID,
					                        @Amount = @Amount

                        SELECT @ISPROCESSDONE";

                    using(SqlCommand command = new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@Amount", Amount);
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Convert.ToBoolean(Result) == true)
                            IsDone = true;
                    }
                }
            }
            catch(Exception e) { }

            return IsDone;
        }
        static public bool Deposit(int ClientID,decimal Amount)
        {
            bool IsDone = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @" DECLARE @ISTHEREDEPOSIT BIT;

                            EXEC @ISTHEREDEPOSIT = SP_Deposit
	                            @ClientID = @ClientID,
	                            @Amount = @Amount
                            SELECT @ISTHEREDEPOSIT";

                    using(SqlCommand command=new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@Amount", Amount);
                        connection.Open();
                        object Result = command.ExecuteScalar();

                        if(Convert.ToBoolean(Result) == true)
                            IsDone = true;
                    }

                }
            }
            catch(Exception e) { }
            return IsDone;
        }

        static public DataTable GetNumberOfTransferLasstTenDays()
        {
            DataTable dt = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT * FROM  dbo.GetNumberOfTransferOperationLast_10_DayFromNow()";

                    using( SqlCommand command=new SqlCommand(Query, connection))
                    {
                        connection.Open();

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch(Exception e) { }

            return dt;
        }
        static public DataTable GetNumberOfWithdrawalLasstTenDays()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT* FROM  dbo.GetNumberOfWithdrawOperationLast_10_DayFromNow()";

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
            catch (Exception e) { }

            return dt;
        }
        static public DataTable GetNumberOfDepositLasstTenDays()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT* FROM  dbo.GetNumberOfDepositOperationLast_10_DayFromNow()";

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
            catch (Exception e) { }

            return dt;
        }

        static public decimal CalculateCommissionOfTransfers()
        {
            decimal Commission = .0m;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"select dbo.CalclulateCommission()";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        object obj = command.ExecuteScalar();

                        Commission = Convert.ToDecimal(obj);
                    }
                }
            }
            catch (Exception e) { }

            return Commission;
        }
       
        static public decimal CalculateAVGOfCommissionLast_10_Days()
        {
            decimal Total = .0m;

            try
            {
                using(SqlConnection connection  = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT dbo.CalculateAVGOfCommissionLast_10_DaysFromNow()";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        connection.Open();
                        object obj = command.ExecuteScalar();
                        Total = Convert.ToDecimal(obj);
                    }
                }
            }
            catch(Exception e) { }
            return Total;
        }

        static public decimal CalculateSUMOfCommissionLast_10_Days()
        {
            decimal Total = .0m;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT dbo.CalculateSUMOfCommissionLast_10_DaysFromNow()";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object obj = command.ExecuteScalar();
                        Total = Convert.ToDecimal(obj);
                    }
                }
            }
            catch (Exception e) { }
            return Total;
        }

        static public (int,int) GetTheLastTwoValueFromCommissionTransfer10Days()
        {
            int LastValue = 0;
            int lastLastValue = 0;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
		                        DECLARE @LAST INT;
		                        DECLARE @LASTOFLAST INT;

		                        EXEC SP_DownTransferOrUp
			                        @LastValueAdded = @LAST OUTPUT,
			                        @TheSecondLastValueAdded = @LASTOFLAST OUTPUT
		                        SELECT @LAST AS LastValueAdded , @LASTOFLAST LastLastValueAdded ";
                    using( SqlCommand command = new SqlCommand(query,connection))
                    {
                        connection.Open();

                        DataTable dt = new DataTable();
                        using(SqlDataReader  reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }

                        foreach(DataRow row in dt.Rows)
                        {
                            LastValue = (int)row["LastValueAdded"];
                            lastLastValue = (int)row["LastLastValueAdded"];
                        }
                    }
                }
            }
            catch (Exception e) { }
            return (LastValue, lastLastValue);
        }
        static public int CalculateWithdrawLast_10_Days()
        {
            int Total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"SELECT dbo.CalculateWithdrawalLast_10_Days()";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object obj = command.ExecuteScalar();
                        Total = Convert.ToInt32(obj);
                    }
                }
            }
            catch (Exception e) { }
            return Total;
        }
        static public int CalculateDepositLast_10_Days()
        {
            int Total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"  SELECT dbo.CalculateDepositLast_10_Days()";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object obj = command.ExecuteScalar();
                        Total = Convert.ToInt32(obj);
                    }
                }
            }
            catch (Exception e) { }
            return Total;
        }

        static public DataTable GetTheTop6ClientDoTransfers()
        {
            DataTable table = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT * FROM dbo.GetTheTop6ClientsDoTransfers()";
                    using(SqlCommand command= new SqlCommand(Query, connection))
                    {
                        connection.Open();
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch (Exception e) { }

            return table;
        }

        static public DataTable GetTheTopClientDoWithdrawal()
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @" SELECT* FROM dbo.GetTheTop6ClientsDoWithdrawal()";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch (Exception e) { }

            return table;
        }

        

        static public DataTable GetTheTopClientDoDeposit()
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Query = @"SELECT* FROM dbo.GetTheTop6ClientsDoDeposit()";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch (Exception e) { }

            return table;
        }

    }
}
