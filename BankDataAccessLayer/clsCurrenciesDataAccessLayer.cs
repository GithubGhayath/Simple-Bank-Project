using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankDataAccessLayer
{
    public class clsCurrenciesDataAccessLayer
    {
        public static DataTable GetCurrencies()
        {
            DataTable dt = new DataTable();
            try
            {
                using(SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"EXEC dbo.GetCurrencies";
                    using(SqlCommand command = new SqlCommand(query, connection))
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

        public static bool Find(string CurrencyCode,ref double Value)
        {
            bool IsFound = false;
           
            try
            {
                using(SqlConnection connection  = new SqlConnection(clsSittings.ConnectionString))
                {
                    string Querey = @"SELECT * FROM dbo.GetCurrencyByCode(@CurrencyCode)";
                    using(SqlCommand command = new SqlCommand(Querey, connection))
                    {
                        command.Parameters.AddWithValue("@CurrencyCode", CurrencyCode);
                        connection.Open();

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Value = (double)reader["Value_In_2024-06-21"];
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch(Exception e) { }

            return IsFound;
        }
    }
}
