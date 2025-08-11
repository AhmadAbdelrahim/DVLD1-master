using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsApplicationBusiness
    {
		int ApplicationID { get; set; }
		int ApplicationPersonID { get; set; }
		DateTime ApplicationDate { get; set; }
		int ApplicationType { get; set; }
		int ApplicationStatus { get; set; }
		DateTime LastStatusDate { get; set; }
		float PaidFees { get; set; }
		int CreatedByUserID { get; set; }

        public clsApplicationBusiness(int applicationID, int applicationPersonID, DateTime applicationDate, int applicationType, int applicationStatus, DateTime lastStatusDate, float paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicationPersonID = applicationPersonID;
            ApplicationDate = applicationDate;
            ApplicationType = applicationType;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
        }
    }
}
