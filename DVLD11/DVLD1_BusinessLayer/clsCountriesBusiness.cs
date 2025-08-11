using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD1_DataAccessLayer;

namespace DVLD1_BusinessLayer
{
    public class clsCountriesBusiness
    {
        public enum enMode { AddNew = 1, UpdateNew = 2 }
        public enMode mode = enMode.AddNew;

        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountriesBusiness()
        {
            this.CountryID = -1;
            this.CountryName = "";
            mode = enMode.AddNew;
        }

        private clsCountriesBusiness(int countryID, string countryName)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
            mode = enMode.UpdateNew;
        }

        public static clsCountriesBusiness Find(int countryID)
        {
            string CountryName = "";

            if (clsCountriesDataAccess.GetCountryInfoByID(countryID, ref CountryName))
            {
                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsCountriesBusiness(countryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountriesBusiness Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountriesDataAccess.GetCountryInfoByName(CountryName, ref CountryID))
            {
                // with parameters إذا بيلاقيه بيرجع الكونستراكتر الكامل 
                return new clsCountriesBusiness(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewCountry()
        {
            //call DataAccess Layer 
            this.CountryID = clsCountriesDataAccess.AddNewCountry(this.CountryName);

            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {
            //call DataAccess Layer
            return clsCountriesDataAccess.UpdateCountry(this.CountryID, this.CountryName);
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewCountry())
                        {
                            mode = enMode.UpdateNew;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case enMode.UpdateNew:
                    return _UpdateCountry();
            }
            return false;
        }

        // أيضاً نستخدم للحذف دالة استاتيك على مستوى الكلاس وليس من خلال الاوبجكت
        // حيث من خلال الاوبجكت يتم حذف الأوبجكت ولكن تظل المعلومات في الميموري
        public static bool DeleteCountry(int ID)
        {
            return clsCountriesDataAccess.DeleteCountry(ID);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountriesDataAccess.GetAllCountries();
        }

        public static bool IsCountryExist(int ID)
        {
            return clsCountriesDataAccess.IsCountryExist(ID);
        }

        public static bool IsCountryExist(string CountryName)
        {
            return clsCountriesDataAccess.IsCountryExist(CountryName);
        }
    } 
}
