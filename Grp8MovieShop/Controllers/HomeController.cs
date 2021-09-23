using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grp8MovieShop.Models;
namespace Grp8MovieShop.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void AddDeptsandEmps()
        {
            //Department dept = new Department();
            //dept.Name = "Development";
            //db.Departments.Add(dept);
            //db.SaveChanges();

            //dept = new Department();
            //dept.Name = "Tester";
            //db.Departments.Add(dept);
            //db.SaveChanges();

            //dept = new Department();
            //dept.Name = "Administration";
            //db.Departments.Add(dept);
            //db.SaveChanges();

            //Employee emp = new Employee();
            //emp.Name = "Employee 1";
            //db.Employees.Add(emp);
            //db.SaveChanges();

            //emp = new Employee();
            //emp.Name = "Employee 2";
            //db.Employees.Add(emp);
            //db.SaveChanges();

            //emp = new Employee();
            //emp.Name = "Employee 3";
            //db.Employees.Add(emp);
            //db.SaveChanges();

            //emp = new Employee();
            //emp.Name = "Employee 4";
            //db.Employees.Add(emp);
            //db.SaveChanges();

            //emp = new Employee();
            //emp.Name = "Employee 5";
            //db.Employees.Add(emp);
            //db.SaveChanges();

            Response.Write("Data added successfully!");
        }

        public ActionResult AssignDepartments(int DeptName = 0, int EmpName = 0)
        {
            SelectList departmentlist = new SelectList(db.Departments, "Id", "Name");
            SelectList employeelist = new SelectList(db.Employees, "Id", "Name");
          
            var TupleList = Tuple.Create<SelectList, SelectList>(departmentlist, employeelist);
            if (DeptName > 0 && EmpName > 0)
            {
                Department dept = new Department();
                dept = db.Departments.Find(DeptName);
                Employee emp = new Employee();
                emp = db.Employees.Find(EmpName);
                //emp.Departments.Add(dept);
                dept.Employees.Add(emp);
                db.SaveChanges();
            }
            return View(TupleList);
        }
    }
}