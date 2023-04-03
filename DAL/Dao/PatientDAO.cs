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
        public Patient GetByUserName(string userName)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    var patient = db.Patients.FirstOrDefault(p => p.IdUserName == user.ID);
                    return patient;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool Update(Patient patient)
        {
            try
            {
                var patientUpdate = db.Patients.FirstOrDefault(p => p.IdUserName == patient.IdUserName);
                if (patientUpdate != null)
                {
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
