using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankDataAccessLayer
{
    static class clsUtility
    {
        public struct stClientInfo
        {
            public string FirstName;
            public string MidName;
            public string LastName;
            public string PhoneNumber;
            public string AccountNumber;
            public string PinCode;
            public decimal AccountBalance;
        }
        //Environment.NewLine

        static public string CompareAndFormatStringChanges(stClientInfo Old, stClientInfo New)
        {
            string Detail = string.Empty;
            int Counter = 0;

            if(Old.FirstName.Trim() != New.FirstName.Trim())
            {
                Detail = $"Change First Name : {Environment.NewLine}{Old.FirstName} => {New.FirstName}{Environment.NewLine}";
                Counter++;
            }
            if (Old.MidName.Trim() != New.MidName.Trim())
            {
                Detail += $"Change Mid Name : {Environment.NewLine}{Old.MidName} => {New.MidName}{Environment.NewLine}";
                Counter++;
            }
            if (Old.LastName.Trim() != New.LastName.Trim())
            {
                Detail += $"Change Last Name : {Environment.NewLine}{Old.LastName} => {New.LastName}{Environment.NewLine}";
                Counter++;
            }
            if (Old.PhoneNumber.Trim() != New.PhoneNumber.Trim())
            {
                Detail += $"Change Phone Number : {Environment.NewLine}{Old.PhoneNumber} => {New.PhoneNumber}{Environment.NewLine}";
                Counter++;
            }
            if (Old.AccountNumber.Trim() != New.AccountNumber.Trim())
            {
                Detail += $"Change Account Number : {Environment.NewLine}{Old.AccountNumber} => {New.AccountNumber} {Environment.NewLine}";
                Counter++;
            }
            if (Old.PinCode.Trim() != New.PinCode.Trim())
            {
                Detail += $"Change Pin Code : {Environment.NewLine}{Old.PinCode} => {New.PinCode} {Environment.NewLine}";
                Counter++;
            }
            if (Old.AccountBalance != New.AccountBalance)
            {
                Detail += $"Change Account Balance : {Environment.NewLine}{Old.AccountBalance} => {New.AccountBalance} {Environment.NewLine}";
                Counter++;
            }
            Detail += $"{Environment.NewLine}Number Of Changes : {Counter}";

            return Detail;
        }
        static public bool Archive(int ClientID,int UserID,string Details)
        {
            bool IsSaved = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsSittings.ConnectionString))
                {
                    string query = @"
                                                DECLARE @RECORDIDD INT;

                                                EXEC SP_ArchiveChange
                                                    @RecordID = @RECORDIDD OUTPUT,
                                                    @ClientID = @ClientID,
                                                    @UserID = @UserID,
                                                    @Details = @Details;

                                                SELECT @RECORDIDD;";

                    using(SqlCommand command=new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        if (Details != null)
                            command.Parameters.AddWithValue("@Details", Details);
                        else
                            command.Parameters.AddWithValue("@Details", DBNull.Value);


                        connection.Open();

                        object RecordID = command.ExecuteScalar();

                        IsSaved = Convert.ToInt32(RecordID) > 0;

                    }
                }
            }
            catch (Exception ex) { }
            return IsSaved;
        }


    }
}
