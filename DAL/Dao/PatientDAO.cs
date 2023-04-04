using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao
{
    public class PatientDAO : UserDAO
    {
        private readonly VaccineDbContext db = null;
        public PatientDAO()
        {
            db = new VaccineDbContext();
        }
       
        public bool Update(Patient patient)
        {
            try
            {
                var patientUpdate = db.Patients.FirstOrDefault(getPatient => getPatient.ID == patient.IdUserName);
                if (patientUpdate != null)
                {
                    patientUpdate.Sex = patient.Sex;
                    patientUpdate.PhoneNumber = patient.PhoneNumber;
                    patientUpdate.Name = patient.Name;
                    patientUpdate.Birthday = patient.Birthday;
                    patientUpdate.Address = patient.Address;
                    patientUpdate.Age = patient.Age;                   
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

    }
}
