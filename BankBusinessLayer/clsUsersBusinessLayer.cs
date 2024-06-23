using BankDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BankBusinessLayer
{
    public class clsUsersBusinessLayer
    {
        enum enMode { AddNew, Update }
        private enMode _Mode { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Permissions { get; set; }
        public int PersonID { get; set; }

        public clsPeopleBusinessLayer PersonInfo{ get; set; }

        static public DataTable GetAllUsers()
        {
            return clsUsersDataAccessLayer.GetAllUsers();
        }

        public clsUsersBusinessLayer()
        {
            this.UserID= 0;
            this.Password= string.Empty;
            this.Permissions = 0;
            this.PersonID = 0;
            this.UserName = string.Empty;
            PersonInfo = new clsPeopleBusinessLayer();
            this._Mode = enMode.AddNew;
        }

        private clsUsersBusinessLayer(int userid,string userName,string password,int Permisstions,int PersonID)
        {
            this.Permissions= Permisstions;
            this.UserID= userid;
            this.UserName= userName;
            this.Password= password;
            this.PersonID = PersonID;
            PersonInfo = clsPeopleBusinessLayer.Find(PersonID);
            this._Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            int UserID = clsUsersDataAccessLayer.AddNewUser(this.UserName, this.Password, this.Permissions, this.PersonInfo.firstName
                , this.PersonInfo.midName, this.PersonInfo.lastName, this.PersonInfo.phoneNumber, this.PersonInfo.accountNumber, this.PersonInfo.pINCode, this.PersonInfo.accountBalance, this.PersonInfo.CreatedBy);

            if (UserID > 0)
            {
                this.UserID = UserID;
                return true;
            }
            else
                return false;
        }

        private bool _UpdateUser()
        {
            if (clsUsersDataAccessLayer.Update(this.UserID, this.UserName, this.Password, this.Permissions))
                return true;
            else
                return false;
        }
        public static clsUsersBusinessLayer Find(int UserID)
        {
            string UserName = string.Empty;
            string Password = string.Empty;
            int Permissions = 0;
            int PersonID = 0;

            if (clsUsersDataAccessLayer.Find(UserID, ref UserName, ref Password, ref Permissions, ref PersonID))
                return new clsUsersBusinessLayer(UserID, UserName, Password, Permissions, PersonID);
            else
                return null;

        }

        public static clsUsersBusinessLayer Find(string UserName)
        {
            int UserID = 0;
            string Password = string.Empty;
            int Permissions = 0;
            int PersonID = 0;

            if (clsUsersDataAccessLayer.Find(ref UserID, UserName, ref Password, ref Permissions, ref PersonID))
                return new clsUsersBusinessLayer(UserID, UserName, Password, Permissions, PersonID);
            else
                return null;

        }

        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        PersonInfo = clsPeopleBusinessLayer.Find(clsUsersBusinessLayer.Find(UserID).PersonID);
                        this._Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    if (_UpdateUser())
                    {
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        static public bool IsUserExists(int UserID)
        {
            return clsUsersDataAccessLayer.IsUserExists(UserID);
        }

        static public DataTable GetUsersActive()
        {
            return clsUsersDataAccessLayer.GetUsersActive();
        }

        static public DataTable SelectUserFromView(int UserID)
        {
            return clsUsersDataAccessLayer.SelectUserFromView(UserID);
        }
        static public bool DeleteUser(int UserID)
        {
            return clsUsersDataAccessLayer.DeleteUser(UserID);
        }
    }
}
