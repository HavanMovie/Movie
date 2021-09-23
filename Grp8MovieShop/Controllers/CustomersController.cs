using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grp8MovieShop.Models;
using Grp8MovieShop.Models.ViewModels;

namespace Grp8MovieShop.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult CheckOrders(string email)
        {
            List<OrderHistoryVM> OrdVm=new List<OrderHistoryVM>();
            if (!String.IsNullOrEmpty(email))
            {
                Customer cust = db.Customers.Where(c => c.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
              if(cust!=null)
                {
                    OrdVm = db.Orders.Where(o => o.CustomerId == cust.Id)
                   .Join(db.OrderRows, o => o.Id, r => r.OrderId, (o, r) => new { o, r })
                   //.GroupBy(ors => ors.o.Id)
                   .Select(ord => new OrderHistoryVM
                   {
                       OrderId = ord.o.Id,
                       CustomerName = ord.o.Customer.FirstName + " " + ord.o.Customer.LastName,
                       MovieTitle = ord.r.Movie.Title,
                       OrderedDate = ord.o.OrderedDate,
                       Price = ord.r.Price,
                       TotalPrice = ord.o.TotalPrice,
                       NoofCopies = ord.r.NoofCopies

                   }).ToList();
                }
                else
                {
                    ViewBag.Message = "Please enter valid email to check orders!";
                }

                //var Tuplelist=Tuple.Create<List<Movie>,List<Order>,List<Customer>> (db.Movies.ToList(),db.Orders.ToList(), db.Customers.ToList())
              
            }
            else if(email!=null)
            {
                ViewBag.Message = "Please enter email to check orders!";
            }
            return View(OrdVm);

        }
        public ActionResult ValidateCustomer(string email)
        {
            Session["Msg"] = "Already a Customer!";
            Session["Valid"] = 1;
            Customer Cust = db.Customers.Where(c => c.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
            if (Cust == null)
            {
                Session["Msg"] = "Not a registered Customer!";
                Session["Valid"] = 0;
            }
            else
            {
                Session["CustomerId"] = Cust.Id;
            }
            return View();
        }
        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,BillingAddress,BillingZip,BillingCity,DeliveryAddress,DeliveryZip,DeliveryCity,EmailAddress,PhoneNo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var Cust = db.Customers.Where(c => c.EmailAddress.ToLower() == customer.EmailAddress.ToLower());
                if (Cust.Count() == 0)
                {
                    Session.Remove("EmailvalidMsg");
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    Session["CustomerId"] = customer.Id;
                    ViewBag.Email = customer.EmailAddress;
                    return View("SuccessMsg");
                }
                else
                {
                    Session["EmailvalidMsg"] = "Customer already exists with email " + customer.EmailAddress;
                    return View(customer);
                }


                //return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,BillingAddress,BillingZip,BillingCity,DeliveryAddress,DeliveryZip,DeliveryCity,EmailAddress,PhoneNo")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
