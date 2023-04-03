using DAL.EF;
using PagedList;
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
        public IEnumerable<Vaccine> ListAllPaging(string searchString,  int page, int pageSize)
        {

            IQueryable<Vaccine> model = db.Vaccines;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NameVaccine.Contains(searchString) || x.Description.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }       
            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
