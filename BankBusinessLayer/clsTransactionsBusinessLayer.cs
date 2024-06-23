using BankDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BankBusinessLayer
{
    abstract public class clsTransactionsBusinessLayer
    {

        static public DataTable GetAllTransfers()
        {
            return clsTransactionDataAccessLayer.GetAllTransfers();
        }
        static public DataTable GetNumberOfTransferLasstTenDays()
        {
            return clsTransactionDataAccessLayer.GetNumberOfTransferLasstTenDays();
        }
        public bool Transfer(string FromAccount, string ToAccount, decimal Amount)
        {
            return clsTransactionDataAccessLayer.Transfer(FromAccount, ToAccount, Amount);
        }

        public bool Withdraw(int ClientID, decimal Amount)
        {
            return clsTransactionDataAccessLayer.Withdraw(ClientID, Amount);
        }

        static public DataTable GetNumberOfWithdrawalLasstTenDays()
        {
            return clsTransactionDataAccessLayer.GetNumberOfWithdrawalLasstTenDays();
        }
        public bool Deposit(int ClientID, decimal Amount)
        {
            return clsTransactionDataAccessLayer.Deposit(ClientID, Amount);
        }

        static public DataTable GetNumberOfDepositLasstTenDays()
        {
            return clsTransactionDataAccessLayer.GetNumberOfDepositLasstTenDays();
        }

        /// <summary>
        /// Calculates the total commission for all transfers.(The data from Transfer Table)
        /// </summary>
        /// <returns>The total commission as a decimal number.</returns>
        static public decimal CalculateCommissionOfTransfers()
        {
            return clsTransactionDataAccessLayer.CalculateCommissionOfTransfers();
        }

        /// <summary>
        /// Calculates the average commission over the last 10 days.(The data from Commission History Table)
        /// </summary>
        /// <returns>The average commission as a decimal number.</returns>
        static public decimal CalculateAVGOfCommissionLast_10_Days()
        {
            return clsTransactionDataAccessLayer.CalculateAVGOfCommissionLast_10_Days();
        }

        /// <summary>
        /// Calculates the sum commission over the last 10 days.(The data from Commission History Table)
        /// </summary>
        /// <returns>The sum commission as a decimal number.</returns>
        static public decimal CalculateSUMOfCommissionLast_10_Days()
        {
            return clsTransactionDataAccessLayer.CalculateSUMOfCommissionLast_10_Days();
        }
        /// <summary>
        /// Get the last two Number of Transfers Done Based on Date
        /// </summary>
        /// <returns>Item 1 : Last Added, Item 2 : The privous</returns>
        static public (int, int) GetTheLastTwoValueFromCommissionTransfer10Days()
        {
            (int, int) Result = clsTransactionDataAccessLayer.GetTheLastTwoValueFromCommissionTransfer10Days();
            return (Result.Item1, Result.Item2);
        }

        static public int CalculateWithdrawLast_10_Days()
        {
            return clsTransactionDataAccessLayer.CalculateWithdrawLast_10_Days();
        }
        static public int CalculateDepositLast_10_Days()
        {
            return clsTransactionDataAccessLayer.CalculateDepositLast_10_Days();
        }
        static public DataTable GetTheTop6ClientDoTransfers()
        {
            return clsTransactionDataAccessLayer.GetTheTop6ClientDoTransfers();
        }
        static public DataTable GetTheTopClientDoWithdrawal()
        {
            return clsTransactionDataAccessLayer.GetTheTopClientDoWithdrawal();
        }
        static public DataTable GetTheTopClientDoDeposit()
        {
            return clsTransactionDataAccessLayer.GetTheTopClientDoDeposit();
        }
    }
}
