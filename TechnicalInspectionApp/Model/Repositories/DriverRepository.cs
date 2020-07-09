using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalInspectionApp.Model.EF;
using TechnicalInspectionApp.Model.Entities;

namespace TechnicalInspectionApp.Model.Repositories
{
    public class DriverRepository
    {
        public List<Driver> GetDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            using (EfDbContext context = new EfDbContext())
            {
                drivers = context.Drivers.Where(x => x.Enabled).Include("TechInspections").ToList();
            }
            return drivers;
        }
        public void AddDriver(Driver driver)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.Drivers.Where(x => x.DriverId == driver.DriverId).FirstOrDefault();
                if (item != null)
                {
                    item.FIO = driver.FIO;
                    item.DriverLicenseData = driver.DriverLicenseData;
                    item.DriverLicenseEndDate = driver.DriverLicenseEndDate;
                }
                else
                {
                    context.Drivers.Add(driver);
                }
                context.SaveChanges();
            }
        }
        public void DeleteDriver(Driver driver)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.Drivers.Where(x => x.DriverId == driver.DriverId).FirstOrDefault();
                if (item != null)
                {
                    item.Enabled = false;
                    context.SaveChanges();
                }
            }
        }
    }
}
