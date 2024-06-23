using BankDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBusinessLayer
{
    public class clsClientsBusinessLayer : clsTransactionsBusinessLayer
    {
        enum enMode { AddNew,Update}
        private enMode _Mode { get; set; }

        public clsPeopleBusinessLayer PersonInfo;
        public int ClientID { get; set; }

        public clsClientsBusinessLayer()
        {
            PersonInfo = new clsPeopleBusinessLayer();
            this.ClientID = -1;
            this._Mode = enMode.AddNew;
        }

        private clsClientsBusinessLayer(int clientid,int personid)
        {
            this.ClientID = clientid;
            this.PersonInfo = clsPeopleBusinessLayer.Find(personid);

            this._Mode = enMode.Update;
        }

        static public clsClientsBusinessLayer Find(int ClientID)
        {
            int personID = -1;

            if (clsClientDataAccessLayer.Find(ClientID, ref personID))
                return new clsClientsBusinessLayer(ClientID, personID);
            else
                return null;
        }
        static public clsClientsBusinessLayer Find(string AccountNumber,string PINCode)
        {
            int personID = -1;
            int ClientID = -1;

            if (clsClientDataAccessLayer.Find(AccountNumber, PINCode, ref personID,ref ClientID))
            {

                return new clsClientsBusinessLayer(ClientID, personID);
            }
            else
                return null;
        }

        static public bool IsExists(int ClientID)
        {
            return clsClientDataAccessLayer.IsClientExists(ClientID);
        }
        private bool _AddNewClient()
        {
            int ClientID = clsClientDataAccessLayer.AddNewClient(PersonInfo.firstName, PersonInfo.midName,
                PersonInfo.lastName, PersonInfo.phoneNumber, PersonInfo.accountNumber, PersonInfo.pINCode, PersonInfo.accountBalance,PersonInfo.CreatedBy);

            if (ClientID > 0)
            {
                this.ClientID = ClientID;
                return true;
            }
            else
                return false;
        }

        private bool _UpdateClientInfo(int ClientIDForArchive = 1)
        {
            if (clsClientDataAccessLayer.UpdateClientInfo(this.ClientID, this.PersonInfo.firstName, 
                this.PersonInfo.midName, this.PersonInfo.lastName, this.PersonInfo.phoneNumber, this.PersonInfo.accountNumber,
                this.PersonInfo.pINCode, this.PersonInfo.accountBalance, ClientIDForArchive))
            {
                return true;
            }
            else
                return false;
        }

        public static DataTable GetAllClients()
        {
            return clsClientDataAccessLayer.GetAllClients();
        }
        public bool Save(int ClientIDForArchive=1)
        {
            switch (this._Mode)
            {
                case enMode.AddNew:
                    if (_AddNewClient())
                    {
                        this._Mode = enMode.Update;
                        int personID = -1;

                        if (clsClientDataAccessLayer.Find(this.ClientID, ref personID))
                        {
                            PersonInfo = clsPeopleBusinessLayer.Find(personID);
                        }
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    if (_UpdateClientInfo(ClientIDForArchive))
                        return true;
                    else
                        return false;
            }
            return false;
        }

        static public DataTable ShowTotalBalance()
        {
            return clsClientDataAccessLayer.ShowTotalBalance();
        }
        static public decimal GetTotlaBalances()
        {
            return clsClientDataAccessLayer.GetTotlaBalances();
        }
        public static bool DeleteClient(int ClientID)
        {
            return clsClientDataAccessLayer.DeleteClient(ClientID);
        }

        public bool Withdraw(int Amount)
        {
            return base.Withdraw(this.ClientID, Amount);
        }
        public bool Deposit(int Amount)
        {
            return base.Deposit(this.ClientID, Amount);
        }

        public bool Transfer(string ToAccountNum,decimal Amount)
        {
            return base.Transfer(this.PersonInfo.accountNumber, ToAccountNum, Amount);
        }
    }
}
