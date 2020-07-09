using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalInspectionApp.Model.Entities
{
    public class TechInspection
    {
        public int TechInspectionId { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public bool IsExpired { get; set; }
        public bool Blocked { get; set; }
        public DateTime Date { get; set; }

        public Car Car { get; set; }
        public Driver Driver { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
