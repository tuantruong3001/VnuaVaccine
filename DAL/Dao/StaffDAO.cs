using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
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
        public int Insert(MedicalStaff newPatient)
        {
            db.MedicalStaffs.Add(newPatient);
            db.SaveChanges();
            return newPatient.ID;
        }       
        public MedicalStaff GetByID(int id)
        {
            return db.MedicalStaffs.Find(id);
        }
        public bool DeleteByUserId(int userId)
        {
            try
            {
                // Lấy danh sách bệnh nhân có IdUser là userId
                List<MedicalStaff> patients = db.MedicalStaffs.Where(p => p.IdUserName == userId).ToList();

                db.MedicalStaffs.RemoveRange(patients);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update(MedicalStaff patient)
        {
            try
            {
                var patientUpdate = db.MedicalStaffs.Find(patient.ID);
                if (patientUpdate != null)
                {
                    patientUpdate.Sex = patient.Sex;
                    patientUpdate.PhoneNumber = patient.PhoneNumber;
                    patientUpdate.Name = patient.Name;
                    patientUpdate.Birthday = patient.Birthday;
                    patientUpdate.Address = patient.Address;
                    patientUpdate.Age = CalculateAge((DateTime)patient.Birthday);
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
        public IEnumerable<MedicalStaff> ListPatientPaging(string searchString, int page, int pageSize)
        {

            IQueryable<MedicalStaff> model = db.MedicalStaffs;
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
        public MedicalStaff ViewDetail(int id)
        {
            return db.MedicalStaffs.Find(id);
        }
    }
}
