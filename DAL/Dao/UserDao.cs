using DAL.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Dao
{
    public class UserDAO
    {
        private readonly VaccineDbContext db = null;
        public UserDAO()
        {
            db = new VaccineDbContext();
        }
        // Add user
        public int Insert(User newUser)
        {         
            db.Users.Add(newUser);
            db.SaveChanges();
            return newUser.ID;
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
        public int RegisterCheck(string email)
        {
            var emailExists = db.Users.SingleOrDefault(x => x.Email == email);
           
            if (emailExists == null)
            {
                return 1;
            }
            else
            {
                return 0; // trùng email
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
                    userUpdate.Email = user.Email;
                    userUpdate.Role = user.Role;
                    userUpdate.Password = user.Password;
                    userUpdate.UpdateAt = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }
        public bool UpdateProfile(User user)
        {
            try
            {
                var userUpdate = db.Users.Find(user.ID);
                if (user != null)
                {
                    userUpdate.Email = user.Email;
                    userUpdate.UpdateAt = DateTime.Now;
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
                    
                    User user = db.Users.Find(id);
                    if (user == null)
                    {
                        return false;
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Đã có lỗi xảy ra, vui lòng thử lại sau", ex);
            }
        }

        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {

            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Email.Contains(searchString));
                if (model == null)
                {
                    return null;
                }
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
    }
}
