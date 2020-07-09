using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalInspectionApp.Model.EF;
using TechnicalInspectionApp.Model.Entities;

namespace TechnicalInspectionApp.Model.Repositories
{
    public class ReportRepository
    {
        public List<Report> GetReports()
        {
            List<Report> reports = new List<Report>();
            using (EfDbContext context = new EfDbContext())
            {
                reports = context.Reports.Include("TechInspection").
                    Include("TechInspection.Car").Include("TechInspection.Driver").ToList();
            }
            return reports;
        }
        public void AddReport(Report report)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Reports.Add(report);
                context.SaveChanges();
            }
        }
    }
}
