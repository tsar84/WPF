using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalInspectionApp.Model.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public int TechInspectionId { get; set; }
        public string Status { get; set; }

        public TechInspection TechInspection { get; set; }
    }
}
