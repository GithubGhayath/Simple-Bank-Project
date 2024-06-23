using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BankDataAccessLayer;
using Syncfusion.Windows.Forms.Diagram;

namespace BankBusinessLayer
{
    internal class Program
    {
        static public void PrintAllPeople()
        {
            DataTable dt = clsPeopleBusinessLayer.GetAllPeople();

            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine($"{dr["PersonID"]} , {dr["FirstName"]} ,{dr["MidName"]} ,{dr["LastName"]} ,{dr["PhoneNumber"]} ,{dr["AccountNumber"]} ,{dr["PINCode"]} ,{dr["AccountBalance"]}");
            }
        }

        static public void TestAddNewPerson()
        {
            clsPeopleBusinessLayer person = new clsPeopleBusinessLayer();

            person.firstName = "Maher";
            person.midName = "Ali";
            person.lastName = "Alian";
            person.pINCode = "2222";
            person.accountBalance = 3652.3m;
            person.phoneNumber = "987-654-3210";
            person.accountNumber = "ACCT54321";

            if (person.Save())
            {
                Console.WriteLine("Person added successfully with id : " + person.PersonID);
            }
            else
                Console.WriteLine("Error");
        }

        static void TestExitstPerson(int PersonID)
        {


            if(clsPeopleBusinessLayer.IsExistsPerson(PersonID))
            {

                Console.WriteLine("Person Here");
            }
            else
                Console.WriteLine("Not found");
        }

        static void TestFindPerson(int PersonID)
        {
            clsPeopleBusinessLayer p = clsPeopleBusinessLayer.Find(PersonID);

            if (p != null)
            {
                Console.WriteLine($"{p.firstName} , {p.midName}, {p.lastName}, {p.accountNumber}, {p.accountBalance}, {p.pINCode}, {p.phoneNumber}, {p.PersonID}");
            }
            else
                Console.WriteLine("Not found");
        }

        static void TestUpdate(int personID)
        {
            clsPeopleBusinessLayer p = clsPeopleBusinessLayer.Find(personID);

            if (p != null)
            {
                p.firstName = "Ghayath";
                p.midName = "Alali";
                p.lastName = "Alrazj";
                p.phoneNumber = "+963 517 056 025";
                p.accountBalance = 7777.7m;
                p.accountNumber = "TRX12369";
                p.pINCode = "0000";

                if (p.Save())
                    Console.WriteLine("Person Updated successfuly");
                else
                    Console.WriteLine("Error");
            }
            else
                Console.WriteLine("perosn not found");
        }

        static void TestDelete(int personid)
        {
            if (clsPeopleBusinessLayer.DeletePerson(personid))
                Console.WriteLine("Deleted Successfully ");
            else
                Console.WriteLine("Error");
        }

        static void TestAddClient()
        {
            clsClientsBusinessLayer c = new clsClientsBusinessLayer();

            c.PersonInfo.firstName = "Jamal";
            c.PersonInfo.midName = "Al";
            c.PersonInfo.lastName = "AlBeash";
            c.PersonInfo.phoneNumber = "+963 258 741 258";
            c.PersonInfo.accountNumber = "YTR123690";
            c.PersonInfo.accountBalance = 12585.3m;
            c.PersonInfo.pINCode = "0000";
            c.PersonInfo.CreatedBy = 1;

            if (c.Save())
            {
                Console.WriteLine("Cleint added Successfuly with id : " + c.ClientID);
            }
            else
                Console.WriteLine("Error");

        }

        static void TestGetClients()
        {
            DataTable t = clsClientsBusinessLayer.GetAllClients();

            foreach(DataRow r  in t.Rows) {
                Console.WriteLine($"{r["ClientID"]} , {r["FullName"]} , {r["AccountBalance"]}");
            }
        }

        static void TestFindTestUpdateClient(int ClientID)
        {
            clsClientsBusinessLayer c = clsClientsBusinessLayer.Find(ClientID);

            if (c != null)
            {
                c.PersonInfo.firstName = "Ola";
                c.PersonInfo.midName = "Mahmood";
                c.PersonInfo.lastName = "Malak";
                c.PersonInfo.accountNumber = "TREX78963";
                c.PersonInfo.accountBalance = 11111.1m;
                c.PersonInfo.phoneNumber = "+963 222 222 222 222";
                c.PersonInfo.pINCode = "0000";

                if (c.Save())
                {
                    Console.WriteLine("Client updated");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            else
                Console.WriteLine("Client not found");
        }

        static void TestDeleteClient(int ClientID)
        {
            if(clsClientsBusinessLayer.DeleteClient(ClientID))
            {
                Console.WriteLine("Client Deleted Successfuly"); 
            }
            else
            { Console.WriteLine("Error"); }
        }

        static void TestGEtAllUsers()
        {
            DataTable t = clsUsersBusinessLayer.GetAllUsers();

            foreach(DataRow r in t.Rows)
            {
                Console.WriteLine($"{r["UserID"]} , {r["PersonID"]}, {r["FullName"]}, {r["UserName"]}, {r["Password"]}");
            }
        }

        static void TestAddNewUser()
        {
            clsUsersBusinessLayer u = new clsUsersBusinessLayer();

            u.UserName = "Test";
            u.Permissions = 12;
            u.Password = "1452";
            u.PersonInfo.firstName = "Maya";
            u.PersonInfo.midName = "Ola";
            u.PersonInfo.lastName = "Ali";
            u.PersonInfo.phoneNumber = "+963 258 741 258";
            u.PersonInfo.accountNumber = "TYRE85296";
            u.PersonInfo.accountBalance = 8526.3m;
            u.PersonInfo.CreatedBy = 1;
            u.PersonInfo.pINCode = "1111";

            if (u.Save())
                Console.WriteLine("User added successfully with id : " + u.UserID);
            else
            { Console.WriteLine("Error"); }
        }

        static void TestFindUser(int UserID) 
        {
            clsUsersBusinessLayer u = clsUsersBusinessLayer.Find(UserID);

            if (u != null)
                Console.WriteLine($"{u.UserName} , {u.UserID}, {u.PersonID}, {u.Permissions}, {u.Password}, {u.PersonInfo.firstName}, {u.PersonInfo.midName}, {u.PersonInfo.lastName}, {u.PersonInfo.accountNumber}");
            else
            { Console.WriteLine("Error"); }
        }

        static void TestFindUser(string UserName)
        {
            clsUsersBusinessLayer u = clsUsersBusinessLayer.Find(UserName);

            if (u != null)
                Console.WriteLine($"{u.UserName} , {u.UserID}, {u.PersonID}, {u.Permissions}, {u.Password}, {u.PersonInfo.firstName}, {u.PersonInfo.midName}, {u.PersonInfo.lastName}, {u.PersonInfo.accountNumber}");
            else
            { Console.WriteLine("Error"); }
        }

        static void TEstFindAndUpdateUser(int UserID)
        {
            clsUsersBusinessLayer u = clsUsersBusinessLayer.Find(UserID);

            u.UserName = "MMM";
            u.Password = "0000";
            u.Permissions = 12;

            if (u.Save())
                Console.WriteLine("updated successfuly");
            else
            { Console.WriteLine("Error"); }
        }

        static void testUserExists(int userid)
        {
            if (clsUsersBusinessLayer.IsUserExists(userid))
                Console.WriteLine("User here");
            else
                Console.WriteLine("Error");
        }

        static void TestGetTotlaBalances()
        {
            Console.WriteLine("Total : " + clsClientsBusinessLayer.GetTotlaBalances().ToString());
        }

        static void ShwoTotalBalances()
        {
            DataTable d = clsClientsBusinessLayer.ShowTotalBalance();

            foreach(DataRow r in d.Rows)
            {
                Console.WriteLine($"{r["AccountNumber"]} , {r["FullName"]} , {r["AccountBalance"]}");
            }
        }

        static public string GetNewAccountNumber()
        {
            string AccountNum = string.Empty;

            Guid NewGuid = Guid.NewGuid();

            AccountNum = NewGuid.ToString().Substring(0, 8);
            AccountNum += NewGuid.ToString().Substring(10,11);

            return AccountNum.Substring(0,9);
        }

        static void TestIsClientExsts(int ClientID)
        {
            if(clsClientsBusinessLayer.IsExists(ClientID))
            {
                Console.WriteLine("Client Here");
            }
            else
            { Console.WriteLine("Error"); }
        }
        static void testUserActiv()
        {
            DataTable t = clsUsersBusinessLayer.GetUsersActive();

            foreach(DataRow dt in t.Rows)
            {
                Console.WriteLine($"{dt["Active"]} , {dt["UserID"]}");
            }
        }

        static void TestSelectUsrFromView(int UserID)
        {
            DataTable user = clsUsersBusinessLayer.SelectUserFromView(UserID);
            foreach(DataRow u in user.Rows)
                Console.WriteLine($"{u[0]} , {u[1]}, {u[2]}, {u[3]}, {u[4]}");
           
        }

        static void TestDeleteUser(int UserID)
        {
            if (clsUsersBusinessLayer.DeleteUser(UserID))
                Console.WriteLine("User Deleted");
            else
            { Console.WriteLine("Error"); }
        }
        static void TestFindByAccnand(string Accou,string pin)
        {
            clsClientsBusinessLayer c = clsClientsBusinessLayer.Find(Accou, pin);

            if (c != null)
                Console.WriteLine("Found");
            else
            { Console.WriteLine("Error"); }
        }
        static void GetTransfer10Days()
        {
            DataTable d = clsTransactionDataAccessLayer.GetNumberOfTransferLasstTenDays();

            foreach (DataRow r in d.Rows)
                Console.WriteLine($", {r["RowID"]}, {r["NumberOfTransferProcess"]}, {r["Date"]}");
        }
        static void GetWithdrawfer10Days()
        {
            DataTable d = clsTransactionDataAccessLayer.GetNumberOfWithdrawalLasstTenDays();

            foreach (DataRow r in d.Rows)
                Console.WriteLine($", {r["RowID"]}, {r["NumberOfWithdraw"]}, {r["Date"]}");
        }

        static void GetDepositfer10Days()
        {
            DataTable d = clsTransactionDataAccessLayer.GetNumberOfDepositLasstTenDays();

            foreach (DataRow r in d.Rows)
                Console.WriteLine($", {r["RowID"]}, {r["NumberOfDeposit"]}, {r["Date"]}");
        }

        //This function Get commission from all transfer process
        static void TestCalaculateCommission()
        {
            Console.WriteLine($"The commission of transfers : {clsTransactionsBusinessLayer.CalculateCommissionOfTransfers()}");
        }

        //The avarage of commission last 10 days
        static void TestGetCommission10Days()
        {
            Console.WriteLine($"The commission of transfers last 10 days : {clsTransactionsBusinessLayer.CalculateAVGOfCommissionLast_10_Days()}");

        }
        static void TestGetSUMCommission10Days()
        {
            Console.WriteLine($"The commission of transfers last 10 days : {clsTransactionsBusinessLayer.CalculateSUMOfCommissionLast_10_Days()}");

        }
        static void Test()
        {
            (int, int) Result = clsTransactionsBusinessLayer.GetTheLastTwoValueFromCommissionTransfer10Days();
            Console.WriteLine($"The item 1 {Result.Item1} , item 2 {Result.Item2}");
        }
        static void TestGetWithdraw10Days()
        {
            Console.WriteLine($"The Withdrawal last 10 days : {clsTransactionsBusinessLayer.CalculateWithdrawLast_10_Days()}");

        }
        static void TestGetDeposit10Days()
        {
            Console.WriteLine($"The deposit last 10 days : {clsTransactionsBusinessLayer.CalculateDepositLast_10_Days()}");

        }

        static void testGetTopClientTTT()
        {
            DataTable d = clsTransactionsBusinessLayer.GetTheTop6ClientDoTransfers();
            foreach (DataRow r in d.Rows)
            {
                if (!string.IsNullOrEmpty( r["FullName"].ToString()))
                    Console.WriteLine($", {r["NumberOfTransfers"]}, {r["FullName"]}");
            }
        }
        static void testGetTopClientWWW()
        {
            DataTable d = clsTransactionsBusinessLayer.GetTheTopClientDoWithdrawal();
            foreach (DataRow r in d.Rows)
            {
                    Console.WriteLine($", {r["NUMBEROFWITHDRAWAL"]}, {r["CLIENTNAME"]}");
            }
        }
        static void testGetTopClientDDD()
        {
            DataTable d = clsTransactionsBusinessLayer.GetTheTopClientDoDeposit();
            foreach (DataRow r in d.Rows)
            {
                Console.WriteLine($", {r["NUMBEROFDEPOSIT"]}, {r["CLIENTNAME"]}");
            }
        }
        static string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        static void TestFindCurrency(string C)
        {
            clsCurrenciesBusinessLayer c = clsCurrenciesBusinessLayer.Find(C);
            if (c == null)
                Console.WriteLine("Not Found");
            else
                Console.WriteLine($"Currency Code : {c.Code}\nCurrency Value : {c.Value}");
        }
        static void GetAllCurrenceies()
        {
            string value = "Value_In_" + DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = clsCurrenciesBusinessLayer.GetCurrencies();

            foreach (DataRow d in dt.Rows)
                Console.WriteLine(d["CurrencyCode"] + "    " + d[value]);
        }
        static void Main(string[] args)
        {
            Console.WriteLine(ComputeHash("1234"));
            //TestFindCurrency("MCOIN");
            //TestFindCurrency("shdfskdl");
            //Console.WriteLine(DateTime.Now.Date.Year + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Day);
            //Console.WriteLine("Value_In_" + DateTime.Now.ToString("yyyy-MM-dd"));
            //GetAllCurrenceies();
            //testGetTopClientDDD();
            //testGetTopClientWWW();
            //testGetTopClient();
            //TestGetWithdraw10Days();
            //TestGetDeposit10Days();
            //Test();
            //TestGetSUMCommission10Days();
            //TestGetCommission10Days();
            //TestCalaculateCommission();
            //GetDepositfer10Days();
            //GetWithdrawfer10Days();
            //GetTransfer10Days();
            //TestFindByAccnand("ACCT88888", "9876");
            //TestFindByAccnand("ACCT88888", "9876435");
            //TestDeleteUser(16);
            //TestDeleteUser(6516);
            //TestSelectUsrFromView(1);
            //TestSelectUsrFromView(11464);

            //testUserActiv();
            //TestIsClientExsts(1);
            //TestIsClientExsts(11);
            //Console.WriteLine(GetNewAccountNumber())
;            //ShwoTotalBalances();
            //TestGetTotlaBalances();
            //testUserExists(1);
            //testUserExists(164);
            //TEstFindAndUpdateUser(5);
            //TestFindUser("Ghayath");
            //TestAddNewUser();
            //TestGEtAllUsers();
            //TestDeleteClient(10);
            //TestDeleteClient(1541);

            //TestFindTestUpdateClient(7);
            //TestFindTestUpdateClient(5614);

            // TestGetClients();

            //TestAddClient();

            //PrintAllPeople();

            //TestAddNewPerson();

            //TestExitstPerson(1);
            //TestExitstPerson(515);

            //TestFindPerson(2);
            //TestFindPerson(1651);

            //TestUpdate(12);
            //TestUpdate(1246);

            //TestDelete(12);
            //TestDelete(12);

            Console.ReadKey();
        }
    }
}
