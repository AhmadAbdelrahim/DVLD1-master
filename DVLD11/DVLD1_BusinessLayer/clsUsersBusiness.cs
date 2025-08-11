using DataAccessLayer;
using DVLD1_BusinessLayer;
using DVLD1_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class Session
    {
        public static string Username { get; set; }
    }
    public class clsUsersBusiness
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode mode = enMode.AddNew;
        
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
   

        public clsUsersBusiness()
        {
            
            this.UserID = -1;
            this.PersonID = -1;
            this.Username = "";
            this.Password = "";
            this.IsActive = true;

            mode = enMode.AddNew;
        }

        private clsUsersBusiness(int UserID, int personID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = personID;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;

            mode = enMode.Update;
        }

        public static clsUsersBusiness Find(int UserID)
        {
            int PersonID = -1; string Username = ""; string Password = ""; bool IsActive = true;

            bool IsFound = clsUsersDataAccess.GetUserDataByID (UserID, ref PersonID, ref Username, ref Password, ref IsActive);

            if (IsFound)
            {
                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsUsersBusiness(UserID, PersonID, Username, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static clsUsersBusiness Find(string Username, string Password)
        {
            int UserID = -1; int PersonID = -1; bool IsActive = false;

            if (clsUsersDataAccess.User(ref UserID, ref PersonID, Username, Password, ref IsActive))
            {
                // هنا بنعمل كاستنج للـ PermissionValue إلى نوع الـ enum اللي عندنا
                //enPermissions Permission = (enPermissions)PermissionValue;

                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsUsersBusiness(UserID, PersonID, Username, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewUser()
        {
            //call DataAccessLayer 
            this.UserID = clsUsersDataAccess.AddNewUser
                            (
                                this.Username, this.Password, this.IsActive
                            );

            return (this.UserID != -1);
        }

        private bool _UpdateUserData()
        {
            //call DataAccessLayer
            return clsUsersDataAccess.UpdateUserData
                    (
                        this.UserID, this.Username, this.Password, this.IsActive
                    );
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewUser())
                        {
                            mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case enMode.Update:
                    return _UpdateUserData();
            }
            return false;
        }

        // أيضاً نستخدم للحذف دالة استاتيك على مستوى الكلاس وليس من خلال الاوبجكت
        // حيث من خلال الاوبجكت يتم حذف الأوبجكت ولكن تظل المعلومات في الميموري
        public static bool DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersDataAccess.IsUserExist(UserID);
        }

        public static bool IsUserExist(string Username)
        {
            return clsUsersDataAccess.IsUserExist(Username);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password);
        }
    }
}
