using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao
{
    public class UserDao
    {
        VaccineDbContext db = null;
        public UserDao()
        {
            db = new VaccineDbContext();
        }
        public User GetByEmail(string email)
        {
            return db.Users.SingleOrDefault(x => x.Email == email);
        }
        public int Login(string passWord, string email)
        {
            var result = db.Users.SingleOrDefault(x => x.Email == email);
            if (result == null) return 0; // tài khoản ko tồn tại
            if (result.Status == 0) return -1; // tài khoản bị xoá
            if (result.Password != passWord) return -2; // sai mật khẩu
            return 1;
        }
    }
}
