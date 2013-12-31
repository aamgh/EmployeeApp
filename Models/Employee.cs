using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime? DOB { get; set; }
        public int Dept { get; set; }
    }
}