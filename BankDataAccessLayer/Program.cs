using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BankDataAccessLayer
{
    
    internal class Program
    {

        static void TestArchive()
        {
            if (clsUtility.Archive(1, 1, null))
                Console.WriteLine("Added");
            else
                Console.WriteLine("Error");
        }

        static void TestChangesString()
        {
            clsUtility.stClientInfo Old;
            clsUtility.stClientInfo New;

            Old.FirstName = "Test";
            Old.MidName = "Test1";
            Old.LastName = "Test2";
            Old.AccountBalance = 125m;
            Old.AccountNumber = "TRYE52369";
            Old.PhoneNumber = "+963 333 333 333 333";
            Old.PinCode = "0000";

            New.FirstName = "Ghaayath";
            New.MidName = "Alali";
            New.LastName = "maher";
            New.AccountBalance = 2525m;
            New.AccountNumber = "00000";
            New.PhoneNumber = "+963 111 111 111 111";
            New.PinCode = "7777";

            string Details = clsUtility.CompareAndFormatStringChanges(Old, New);

            Console.WriteLine(Details);
        }

        static void Main(string[] args)
        {
            //TestArchive();
            //TestChangesString();

            Console.ReadKey();
        }
    }
}
