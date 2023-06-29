using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Dao
{
    public class VaccineDAO
    {
        private readonly VaccineDbContext db = null;
        public VaccineDAO()
        {
            db = new VaccineDbContext();
        }
        public int Insert(Vaccine vaccine)
        {
            db.Vaccines.Add(vaccine);
            db.SaveChanges();
            return vaccine.ID;
        }
        public bool Update(Vaccine vaccine)
        {
            try
            {
                var vaccineUpdate = db.Vaccines.Find(vaccine.ID);
                if (vaccine != null)
                {
                    vaccineUpdate.NameVaccine = vaccine.NameVaccine;
                    vaccineUpdate.Munafacturer = vaccine.Munafacturer;
                    vaccineUpdate.Description = vaccine.Description;
                    vaccineUpdate.QuantityStock = vaccine.QuantityStock;
                    vaccineUpdate.Path = vaccine.Path;
                    vaccineUpdate.ProductionDate = vaccine.ProductionDate;
                    vaccineUpdate.ExpirationData = vaccine.ExpirationData;
                    vaccineUpdate.Note = vaccine.Note;
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }
        public bool Delete(int id)
        {
            try
            {
                using (var db = new VaccineDbContext())
                {
                    // Tìm Vaccine có ID tương ứng
                    Vaccine vaccine = db.Vaccines.Find(id);
                    if (vaccine == null)
                    {
                        return false;
                    }
                    db.Vaccines.Remove(vaccine);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Đã có lỗi xảy ra, vui lòng thử lại sau", ex);
            }
        }

        public Vaccine GetByID(int id)
        {
            return db.Vaccines.Find(id);
        }
        public IEnumerable<Vaccine> ListAllPaging(string searchString, int page, int pageSize)
        {

            IQueryable<Vaccine> model = db.Vaccines;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NameVaccine.Contains(searchString) || x.Munafacturer.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }
            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
        public Vaccine ViewDetail(int id)
        {
            return db.Vaccines.Find(id);
        }
        public IEnumerable<Vaccine> GetAll()
        {
            return db.Vaccines.ToList();
        }
    }
}
