using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalInspectionApp
{
    class StatusType
    {
        public static string DriverLicense { get; set; } = "Истек срок вод. удостов.";
        public static string TechnicalInspection { get; set; } = "Истек срок тех. осмотра";
        public static string Insuranse { get; set; } = "Истек срок страховки";
    }
}
