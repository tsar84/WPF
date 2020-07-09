using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalInspectionApp.Model.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public string StateNumber { get; set; }
        public bool Enabled { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public DateTime TechnicalInspectionEndDate { get; set; }
        public DateTime InsuranseEndDate { get; set; }

        public ICollection<TechInspection> TechInspections { get; set; }
    }
}
