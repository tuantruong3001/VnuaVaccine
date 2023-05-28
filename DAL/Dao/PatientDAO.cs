using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Patient> GetAllPatients()
        {
            return db.Patients.ToList();
        }

        public bool UpdateProfile(Patient staff)
        {
            try
            {
                var patientUpdate = db.Patients.FirstOrDefault(getStaff => getStaff.IdUserName == staff.IdUserName);
                if (patientUpdate != null)
                {
                    patientUpdate.Sex = staff.Sex;
                    patientUpdate.Birthday = staff.Birthday;
                    patientUpdate.PhoneNumber = staff.PhoneNumber;
                    patientUpdate.Name = staff.Name;
                    patientUpdate.Address = staff.Address;
                    patientUpdate.Age = CalculateAge((DateTime)staff.Birthday);
                    patientUpdate.AgeMonth = CalculateAgeMonth((DateTime)staff.Birthday);
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
        public bool UpdateAllPatients(List<Patient> patients)
        {
            try
            {
                foreach (var patient in patients)
                {
                    var patientUpdate = db.Patients.Find(patient.ID);
                    if (patientUpdate != null)
                    {
                        patientUpdate.Sex = patient.Sex;
                        patientUpdate.PhoneNumber = patient.PhoneNumber;
                        patientUpdate.Name = patient.Name;
                        patientUpdate.NameParent = patient.NameParent;
                        patientUpdate.Birthday = patient.Birthday;
                        patientUpdate.Address = patient.Address;
                        patientUpdate.Age = CalculateAge((DateTime)patient.Birthday);
                        patientUpdate.AgeMonth = CalculateAgeMonth((DateTime)patient.Birthday);
                        patientUpdate.UpdateAt = patient.UpdateAt;
                    }
                }
               
                db.SaveChanges();
                return true;
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
                    patientUpdate.NameParent = patient.NameParent;
                    patientUpdate.Birthday = patient.Birthday;
                    patientUpdate.Address = patient.Address;
                    patientUpdate.Age = CalculateAge((DateTime)patient.Birthday);
                    patientUpdate.AgeMonth = CalculateAgeMonth((DateTime)patient.Birthday);
                    patientUpdate.UpdateAt = patient.UpdateAt;
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

        private int CalculateAgeMonth(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Month - birthday.Month;

            if (today.Day < birthday.Day)
            {
                age--;
            }

            if (age < 0)
            {
                age += 12;
            }

            return age;
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

        public Patient GetByID(int id)
        {
            return db.Patients.Find(id);
        }
        public bool DeleteByUserId(int userId)
        {
            try
            {              
                List<Patient> patients = db.Patients.Where(p => p.IdUserName == userId).ToList();               
                db.Patients.RemoveRange(patients);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Patient> ListPatientPaging(string searchString, int page, int pageSize)
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
        public Patient ViewDetail(int id)
        {
            return db.Patients.Find(id);
        }
    }
}
