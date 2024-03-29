﻿using DAL.EF;
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

        public VaccinationSchedule GetByID(int id)
        {
            return db.VaccinationSchedules.Find(id);
        }
        public Patient GetPatient(int patientId)
        {
            return db.Patients.FirstOrDefault(v => v.ID == patientId);
        }
        public string GetUserEmailByPatientId(int patientId)
        {
            var idUserName = db.Patients
                .Where(p => p.ID == patientId)
                .Select(p => p.IdUserName)
                .FirstOrDefault();

            var email = db.Users
                .Where(u => u.ID == idUserName)
                .Select(u => u.Email)
                .FirstOrDefault();
            return email;
        }
       
        public Vaccine GetVaccine(int vaccineId)
        {
            return db.Vaccines.FirstOrDefault(v => v.ID == vaccineId);
        }
        public List<VaccinationSchedule> GetAllSchedules()
        {
            var allSchedules = db.VaccinationSchedules.ToList();

            return allSchedules.Select(infor => new VaccinationSchedule
            {
                ID = infor.ID,
                IdPatient = infor.IdPatient,
                IdVaccine = infor.IdVaccine,
                Quantity = infor.Quantity,
                Status = infor.Status,
                CreateAt = infor.CreateAt,
                Time = infor.Time
            }).ToList();
        }

        public void UpdateAllSchedules(List<VaccinationSchedule> schedules)
        {
            foreach (var infor in schedules)
            {
                var scheduleUpdate = db.VaccinationSchedules.Find(infor.ID);
                if (scheduleUpdate != null)
                {
                    scheduleUpdate.Status = infor.Status;
                    //scheduleUpdate.Quantity = infor.Quantity;
                }
            }

            db.SaveChanges();
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
        public int GetUserNameId(int loggedInUserId)
        {

            var patient = db.Patients.FirstOrDefault(p => p.IdUserName == loggedInUserId);
            if (patient != null)
            {
                return (int)patient.ID;
            }
            return 0;
        }

        public List<VaccinationSchedule> GetPatientSchedule(int patientId)
        {

            var patientSchedule = db.VaccinationSchedules.Where(vs => vs.IdPatient == patientId).ToList();
            return patientSchedule;
        }
        public List<Patient> SearchByName(string name)
        {
            var nameMatches = db.Patients.Where(vs => vs.Name.Contains(name)).ToList();
            return nameMatches;

        }
        public List<Vaccine> SearchByNameVaccine(string name)
        {
            var nameMatches = db.Vaccines.Where(vs => vs.NameVaccine.Contains(name)).ToList();
            return nameMatches;
        }

        public List<VaccinationSchedule> GetScheduleByPatientAndVaccine(int patientId, int vaccineId)
        {
            var schedule = db.VaccinationSchedules
                .Where(s => s.IdPatient == patientId && s.IdVaccine == vaccineId).ToList();
            return schedule;

        }
    }
}
