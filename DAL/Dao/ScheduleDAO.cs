using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DAL.Dao
{
    public class ScheduleDAO
    {
        private readonly VaccineDbContext db = null;
        public ScheduleDAO()
        {
            db = new VaccineDbContext();
        }

        public int Insert(VaccinationSchedule newSchedule)
        {
            db.VaccinationSchedules.Add(newSchedule);
            db.SaveChanges();
            return newSchedule.ID;
        }

        public IPagedList<VaccinationSchedule> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<VaccinationSchedule> model = db.VaccinationSchedules;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Patient.Name.Contains(searchString) || x.Vaccine.NameVaccine.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public IPagedList<VaccinationSchedule> ListWishlist(string searchString, int page, int pageSize)
        {
            IQueryable<VaccinationSchedule> model = db.VaccinationSchedules;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Vaccine.NameVaccine.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }
        
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public VaccinationSchedule GetByID(int id)
        {
            return db.VaccinationSchedules.Find(id);
        }
        public Patient GetPatientName(int patientId)
        {
            return db.Patients.FirstOrDefault(v => v.ID == patientId);
        }

        public Vaccine GetVaccineName(int vaccineId)
        {
            return db.Vaccines.FirstOrDefault(v => v.ID == vaccineId);
        }
        public bool Delete(int id)
        {
            try
            {
                using (var db = new VaccineDbContext())
                {                  
                    VaccinationSchedule vaccine = db.VaccinationSchedules.Find(id);                 
                    if (vaccine == null)
                    {
                        return false;
                    }
                   
                    db.VaccinationSchedules.Remove(vaccine);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Đã có lỗi xảy ra, vui lòng thử lại sau", ex);
            }
        }
        public bool Update(VaccinationSchedule vaccine)
        {
            try
            {
                var vaccineUpdate = db.VaccinationSchedules.Find(vaccine.ID);
                if (vaccine != null)
                {                   
                    vaccineUpdate.Status = vaccine.Status;
                    vaccineUpdate.Quantity = vaccine.Quantity;
                    vaccineUpdate.Time = vaccine.Time;                
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }
    }
}
