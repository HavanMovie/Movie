using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grp8MovieShop.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DeptId { get; set; }
        //public Department Dept { get; set; }
        public ICollection<Department> Departments { get; set; }
        public Employee()
        {
            Departments = new List<Department>();
        }
    }
}