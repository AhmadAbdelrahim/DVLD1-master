using DVLD1_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_BusinessLayer
{
    public class clsPeopleBusiness
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }
        public DateTime DateOfBirth { get; set; }
        public short Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Nationality { get; set; }

        public clsCountriesBusiness CountryInfo;
        
        private string _ImagePath;
        public string ImagePath 
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        public clsPeopleBusiness()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.Nationality = -1;
            this.ImagePath = "";

            mode = enMode.AddNew;
        }

        private clsPeopleBusiness(int personID, string nationalNo, string firstname, string secondname, string thirdname, string lastname, DateTime dateofbirth, short gender, string address, string phone, string email, int nationality, string imagepath)
        {
            this.PersonID = personID;
            this.NationalNo = nationalNo;
            this.FirstName = firstname;
            this.SecondName = secondname;
            this.ThirdName = thirdname;
            this.LastName = lastname;
            this.DateOfBirth = dateofbirth;
            this.Gender = gender;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.Nationality = nationality;
            this.ImagePath = imagepath;
            this.CountryInfo = clsCountriesBusiness.Find(nationality);

            mode = enMode.Update;
        }

        public static clsPeopleBusiness Find(int PersonID)
        {
            string NationalNo = ""; string FirstName = ""; string SecondName = ""; string ThirdName = ""; string LastName = ""; 
            DateTime DateOfBirth = DateTime.Now; short Gender = 0; string Address = ""; string Phone = ""; string Email = ""; 
            int Nationality = -1; string ImagePath = "";

            bool IsFound = clsPeopleDataAccess.GetPersonDataByID
                            (
                                PersonID, ref NationalNo, ref FirstName, ref SecondName, 
                                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, 
                                ref Address, ref Phone, ref Email, ref Nationality, ref ImagePath
                            );

            if (IsFound)
            {
                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsPeopleBusiness(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, Nationality, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPeopleBusiness Find(string NationalNo)
        {
            int PersonID = -1; string FirstName = ""; string SecondName = ""; string ThirdName = ""; string LastName = "";
            DateTime DateOfBirth = DateTime.Now; short Gender = 0; string Address = ""; string Phone = ""; string Email = "";
            int Nationality = -1; string ImagePath = "";

            bool IsFound = clsPeopleDataAccess.GetPersonDataByNationalNo
                            (
                                ref PersonID, NationalNo, ref FirstName, ref SecondName,
                                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender,
                                ref Address, ref Phone, ref Email, ref Nationality, ref ImagePath
                            );

            if (IsFound)
            {
                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsPeopleBusiness(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, Nationality, ImagePath);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewPerson()
        {
            //call DataAccessLayer 
            this.PersonID = clsPeopleDataAccess.AddNewPerson
                            (
                                this.NationalNo, this.FirstName, this.SecondName, 
                                this.ThirdName, this.LastName, this.DateOfBirth, 
                                this.Gender, this.Address, this.Phone, this.Email, 
                                this.Nationality, this.ImagePath
                            );

            return (this.PersonID != -1);
        }

        private bool _UpdatePersonData()
        {
            //call DataAccessLayer
            return clsPeopleDataAccess.UpdatePersonData
                    (
                        this.PersonID, this.NationalNo, this.FirstName, 
                        this.SecondName, this.ThirdName, this.LastName, 
                        this.DateOfBirth, this.Gender, this.Address, 
                        this.Phone, this.Email, this.Nationality, this.ImagePath
                    );
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewPerson())
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
                    return _UpdatePersonData();
            }
            return false;
        }

        // أيضاً نستخدم للحذف دالة استاتيك على مستوى الكلاس وليس من خلال الاوبجكت
        // حيث من خلال الاوبجكت يتم حذف الأوبجكت ولكن تظل المعلومات في الميموري
        public static bool DeletePerson(int PersonID)
        {
            return clsPeopleDataAccess.DeletePerson(PersonID);
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleDataAccess.GetAllPeople();
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPeopleDataAccess.IsPersonExist(PersonID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPeopleDataAccess.IsPersonExist(NationalNo);
        }
    }
}
