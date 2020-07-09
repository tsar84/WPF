using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalInspectionApp.Model.EF;
using TechnicalInspectionApp.Model.Entities;

namespace TechnicalInspectionApp.Model.Repositories
{
    public class TechInspectionRep
    {
        public List<TechInspection> GetTechInspections()
        {
            List<TechInspection> techInspections = new List<TechInspection>();
            using (EfDbContext context = new EfDbContext())
            {
                techInspections = context.TechInspections.Include("Car").Include("Driver").ToList();
            }
            return techInspections;
        }
        public void AddTechInspection(TechInspection techInspection)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.TechInspections.Add(techInspection);
                context.SaveChanges();
            }
        }
        public void ChangeStatus(TechInspection techInspection)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.TechInspections.Where(x => x.TechInspectionId == techInspection.TechInspectionId).FirstOrDefault();
                if(item != null)
                {
                    item.IsExpired = techInspection.IsExpired;
                    context.SaveChanges();
                }            
            }
        }
        public void Blocked(TechInspection techInspection)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var item = context.TechInspections.Where(x => x.TechInspectionId == techInspection.TechInspectionId).FirstOrDefault();
                if (item != null)
                {
                    item.Blocked = techInspection.Blocked;
                    context.SaveChanges();
                }
            }
        }
    }
}
