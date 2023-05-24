using DAL.EF;
using PagedList;
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

        public IEnumerable<VaccinationSchedule> ListAllPaging(int page, int pageSize)
        {
            IQueryable<VaccinationSchedule> model = db.VaccinationSchedules;           
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }

        public VaccinationSchedule GetByID(int id)
        {
            return db.VaccinationSchedules.Find(id);
        }
        /*public Patient GetPatientName(int patientId)
        {
            return db.Set<Patient>().FirstOrDefault(p => p.ID == patientId);
        }

        public Vaccine GetVaccineName(int vaccineId)
        {
            return db.Set<Vaccine>().FirstOrDefault(v => v.ID == vaccineId);
        }*/

    }
}
