using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalInspectionApp.Model.Entities
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string FIO { get; set; }
        public bool Enabled { get; set; }
        public string DriverLicenseData { get; set; }
        public DateTime DriverLicenseEndDate { get; set; }

        public ICollection<TechInspection> TechInspections { get; set; }
    }
}
