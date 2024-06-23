using BankDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankBusinessLayer
{
    public class clsPeopleBusinessLayer
    {
        enum enMode { AddNew, Update }

        public int PersonID { get; set; }
        private enMode _Mode { get; set; }
        public string firstName { get; set; }
        public string midName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string accountNumber { get; set; }
        public string pINCode { get; set; }
        public decimal accountBalance { get; set; }
        public int CreatedBy { get; set; }

        private clsPeopleBusinessLayer(int PersonID,string firstName, string midName, string lastName, string phoneNumber, string accountNumber, string pINCode, decimal accountBalance, int createdBy)
        {
            this.PersonID = PersonID;
            this.firstName = firstName;
            this.midName = midName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.accountNumber = accountNumber;
            this.pINCode = pINCode;
            this.accountBalance = accountBalance;
            this.CreatedBy = createdBy;
            this._Mode = enMode.Update;
            CreatedBy = createdBy;
        }

        public clsPeopleBusinessLayer()
        {
            this.PersonID = PersonID;
            this.firstName = string.Empty;
            this.midName = string.Empty;
            this.lastName = string.Empty;
            this.phoneNumber = string.Empty;
            this.accountNumber = string.Empty;
            this.pINCode = string.Empty;
            this.accountBalance = .0m;
            this.CreatedBy = 0;
            this._Mode = enMode.AddNew;
        }

        static public DataTable GetAllPeople()
        {
            return clsPeopleDataAccessLayer.GetAllPeople();
        }

        private bool _AddPerson()
        {
            int Temp = clsPeopleDataAccessLayer.AddNewPerson(this.firstName, this.midName, this.lastName, this.phoneNumber, this.accountNumber, this.pINCode, this.accountBalance,this.CreatedBy);

            if (Temp > 0)
            {
                this.PersonID = Temp;
                return true;
            }
            else
                return false;
        }

        static public bool IsExistsPerson(int PersonID)
        {
            if (clsPeopleDataAccessLayer.IsExistsPerson(PersonID))
                return true;
            
            else
                return false;
        }

        static public clsPeopleBusinessLayer Find(int PersonIDd)
        {
            string firstName = string.Empty;
            string midName = string.Empty;
            string lastName = string.Empty;
            string phoneNumber = string.Empty;
            string accountNumber = string.Empty;
            string pINCode = string.Empty;
            decimal accountBalance = .0m;
            int CreateBy = 0;
            if (clsPeopleDataAccessLayer.Find(PersonIDd, ref firstName, ref midName, ref lastName, ref phoneNumber, ref accountNumber, ref pINCode, ref accountBalance, ref CreateBy))
                return new clsPeopleBusinessLayer(PersonIDd, firstName, midName, lastName, phoneNumber, accountNumber, pINCode, accountBalance, CreateBy);
            else
                return null;
        }

        private bool _UpdateInfo()
        {
            if (clsPeopleDataAccessLayer.UpdatePersonInfo(this.PersonID, this.firstName, this.midName, this.lastName, this.phoneNumber, this.accountNumber, this.pINCode, this.accountBalance))
                return true;
            else
                return false;

        }

        public bool Save()
        {
            switch (this._Mode)
            {
                case enMode.AddNew:
                
                    if (_AddPerson())
                    {
                        this._Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                
                case enMode.Update:
                    
                    if (_UpdateInfo())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        static public bool DeletePerson(int PersonID)
        {
            return clsPeopleDataAccessLayer.DeletePerson(PersonID);
        }
    }
}       
