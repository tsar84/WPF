using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalInspectionApp.Model.Entities;

namespace TechnicalInspectionApp.Model.EF
{
    class EfDbContext : DbContext
    {
        public EfDbContext() : base("DBConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<TechInspection> TechInspections { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
