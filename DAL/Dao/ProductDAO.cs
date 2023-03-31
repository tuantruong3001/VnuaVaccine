using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao
{
    public class ProductDAO
    {
        VaccineDbContext db = null;
        public ProductDAO()
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
