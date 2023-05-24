using DAL.EF;
using System;
using System.Linq;

namespace DAL.Dao
{
    public class StaffDAO
    {
        private readonly VaccineDbContext db = null;
        public StaffDAO()
        {
            db = new VaccineDbContext();
        }       

        public bool UpdateProfile(MedicalStaff staff)
        {
            try
            {
                var patientUpdate = db.MedicalStaffs.FirstOrDefault(getStaff => getStaff.ID == staff.IdUserName);
                if (patientUpdate != null)
                {
                    patientUpdate.Sex = staff.Sex;
                    patientUpdate.Birthday = staff.Birthday;
                    patientUpdate.PhoneNumber = staff.PhoneNumber;
                    patientUpdate.Name = staff.Name;
                    patientUpdate.Address = staff.Address;
                    patientUpdate.Age = CalculateAge((DateTime)staff.Birthday);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private int CalculateAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
