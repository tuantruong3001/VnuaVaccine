using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao
{
    public class UserDAO
    {
        VaccineDbContext db = null;
        public UserDAO()
        {
            db = new VaccineDbContext();
        }
        // Add user
        public int Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }
        public User GetById(int id)
        {
            return db.Users.Find(id);
        }
        public bool CheckUserName(string userName)
        {
            var name = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (name == null) return true;
            return false;
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
        // Check Register account
        public int RegisterCheck(string email, string user)
        {

            var emailExists = db.Users.SingleOrDefault(x => x.Email == email);
            var usernameExists = db.Users.SingleOrDefault(x => x.UserName == user);
            if (emailExists == null && usernameExists == null)
            {
                return 1;
            }
            else if (emailExists != null)
            {
                return 0; // trùng email
            }
            else
            {
                return -1; // trùng user
            }
        }
        public bool Update(User user)
        {
            try
            {
                var userUpdate = db.Users.Find(user.ID);
                if (user != null)
                {
                    userUpdate.UserName = user.UserName;
                    userUpdate.Password = user.Password;
                    userUpdate.Email = user.Email;
                   
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }
    }
}
