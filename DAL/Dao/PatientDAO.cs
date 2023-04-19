using DAL.EF;
using PagedList;
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
        public int Insert(Patient newPatient)
        {
            db.Patients.Add(newPatient);
            db.SaveChanges();
            return newPatient.ID;
        }
        public bool UpdateProfile(Patient patient)
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
        public bool Update(Patient patient)
        {
            try
            {
                var patientUpdate = db.Patients.Find(patient.ID);
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
        public Patient GetByID(int id)
        {
            return db.Patients.Find(id);
        }
        public IEnumerable<Patient> ListAllPaging(string searchString, int page, int pageSize)
        {

            IQueryable<Patient> model = db.Patients;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Address.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
    }
}
