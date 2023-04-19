﻿using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao
{
    public class StaffDAO
    {
        private readonly VaccineDbContext db = null;
        public StaffDAO()
        {
            db = new VaccineDbContext();
        }

        public bool Update(MedicalStaff staff)
        {
            try
            {
                var patientUpdate = db.MedicalStaffs.FirstOrDefault(getStaff => getStaff.ID == staff.IdUserName);
                if (patientUpdate != null)
                {
                    patientUpdate.Sex = staff.Sex;
                    patientUpdate.PhoneNumber = staff.PhoneNumber;
                    patientUpdate.Name = staff.Name;
                    patientUpdate.Address = staff.Address;
                    patientUpdate.Age = staff.Age;
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
    }
}