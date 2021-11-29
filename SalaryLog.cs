using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class SalaryLog
    {
        public int ID { set; get; }
        public int EmployeeID { set; get; }
        public int Year { set; get; }
        public string Mounth { set; get; }
        public int Day { set; get; }
        public int TaxSum { set; get; }
        public int TotalSum { set; get; }
        public int SalarySum { set; get; }

    }
}