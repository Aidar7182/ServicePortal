using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Holiday
    {
        public int ID { set; get; }
        public int EmployeeID { set; get; }

        public string FromDate { set; get; }
        public int Day { set; get; }
        public string Status { set; get; }
        public List<Holiday> holidays { set; get; }

    }
}