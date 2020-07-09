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
    public class CarRepository
    {
        public List<Car> GetCars()
        {
            List<Car> cars = new List<Car>();
            using (EfDbContext context = new EfDbContext())
            {
                cars = context.Cars.Where(x => x.Enabled).Include("TechInspections").ToList();
            }
            return cars;
        }
        public void AddCar(Car car)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.Cars.Where(x => x.CarId == car.CarId).FirstOrDefault();
                if(item != null)
                {
                    item.StateNumber = car.StateNumber;
                    item.Mark = car.Mark;
                    item.Model = car.Model;
                    item.TechnicalInspectionEndDate = car.TechnicalInspectionEndDate;
                    item.InsuranseEndDate = car.InsuranseEndDate;
                }
                else
                {
                    context.Cars.Add(car);
                }
                context.SaveChanges();
            }
        }

        public void DeleteCar(Car car)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.Cars.Where(x => x.CarId == car.CarId).FirstOrDefault();
                if (item != null)
                {
                    item.Enabled = false;
                    context.SaveChanges();
                }               
            }
        }
    }
}
