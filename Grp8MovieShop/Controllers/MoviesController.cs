using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Grp8MovieShop.Models;
using Grp8MovieShop.Models.ViewModels;
namespace Grp8MovieShop.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public ActionResult Index()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");
            return View(db.Movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id, int isdefault)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");
            ViewBag.Isdefault = isdefault;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult PaginateMovies(int page = 1)
        {
            if (Session["Search"] != null) Session["PrevSearch"] = Session["Search"];
            Session.Remove("Search");
            Session["CurrentPage"] = page;
            PaginateViewModel MoviesView;
            MoviesView = new PaginateViewModel();

            MoviesView.MoviesPerLoad = 24;
            MoviesView.MoviesPerRow = 4;
            MoviesView.MoviesPerPage = 8;
            MoviesView.CurrentPage = page;
            MoviesView.Movies = CurrentPageMovies(MoviesView.CurrentPage, MoviesView.MoviesPerPage);
            MoviesView.PageCount = PaginateViewModel.NoofPages(db.Movies.Count(), MoviesView.MoviesPerPage);
            return View(MoviesView);

        }
        public IEnumerable<Movie> CurrentPageMovies(int CurrentPage, int MoviesPerPage)
        {

            int start = (CurrentPage - 1) * MoviesPerPage;
            var movies = db.Movies.OrderBy(m => m.Id);
            if (movies.Skip(start).Count() < MoviesPerPage) return movies.Skip(start);
            return movies.Skip(start).Take(MoviesPerPage);
        }

        public ActionResult MovieList()
        {
            if (Session["Search"] != null) Session["PrevSearch"] = Session["Search"];
            Session.Remove("Search");
            Session.Remove("CurrentPage");
            return View(db.Movies.ToList());

        }
        public ActionResult SearchMovie()
        {
           Session.Remove("CurrentPage");
            if (Session["PrevSearch"] != null) Session["Search"] = Session["PrevSearch"];
            return View();
        }

        public ActionResult SearchMovieList(string movie)
        {
            Session["Search"] = movie;
            var movielist = db.Movies.Where(m => m.Title.Contains(movie));
            if (String.IsNullOrEmpty(movie))
            {
                ViewBag.Flag = 1;
                ViewBag.Count = 0;
            }
            else
            {
                ViewBag.Flag = 0;
                ViewBag.Count = movielist.Count();
            }

            return View("MovieList", movielist);
        }


        public ActionResult AddtoCart(int MovieId, int IncorDec, int IsCart)
        {
            List<int> MovieIdLst = new List<int>();
            if (Session["MovieList"] != null) MovieIdLst = (List<int>)Session["MovieList"];
            if (IncorDec == 1) MovieIdLst.Add(MovieId);
            else //MovieIdLst.Remove(MovieId);
            { //to follow the sequence of selection
                MovieIdLst.Reverse();
                MovieIdLst.Remove(MovieId);
                MovieIdLst.Reverse();
            }
            //to display in particular sequence irrespective of selected sequence
            //MovieIdLst.Sort();
            Session["MovieList"] = MovieIdLst;
            Session["Count"] = MovieIdLst.Count;
            if (IsCart == 1) return RedirectToAction("DisplayCart", new { summary = 0 });
            else if (IsCart == 2) return RedirectToAction("SearchMovie");
            else if (IsCart == 3) return RedirectToAction("PaginateMovies",new { page=Session["CurrentPage"]});
            return RedirectToAction("MovieList");
        }
        public ActionResult DisplayCart(int summary)
        {
            Session["summary"] = summary;
            if (Session["MovieList"] == null)
            {
                return View("NoItems");
            }
            List<int> MovieIdLst = new List<int>();
            MovieIdLst = (List<int>)Session["MovieList"];
            List<int> DistinctMovieIdLst = new List<int>();
            DisplayCartVM VMobj = new DisplayCartVM();
            //To make the Distinct ID list from the selected movie Id's
            foreach (int id in MovieIdLst)
            {
                if (VMobj.MovieCopiesList.Count == 0)
                {
                    DistinctMovieIdLst.Add(id);
                    VMobj.MovieCopiesList.Add(MovieIdLst.Count(m => m == id));
                }
                else if (!DistinctMovieIdLst.Contains(id))
                {
                    DistinctMovieIdLst.Add(id);
                    VMobj.MovieCopiesList.Add(MovieIdLst.Count(m => m == id));
                }

            }
            //To read movie details for the selected list of movie Id's
            foreach (int id in DistinctMovieIdLst)
            {
                VMobj.DispMovieList.Add(db.Movies.Find(id));
            }
            Movie movie;
            int cnt = 0;
            //To calculate price for each selected movie based on no.of copies
            foreach (int copies in VMobj.MovieCopiesList)
            {
                movie = new Movie();
                movie = VMobj.DispMovieList.ElementAt(cnt);
                VMobj.PriceList.Add(copies * movie.Price);
                cnt += 1;
            }
            VMobj.TotalPrice = VMobj.PriceList.Sum();
            //Adding values to session to use inside SubmitOrder action
            Session["DistinctMovieIdLst"] = DistinctMovieIdLst;
            Session["PriceList"] = VMobj.PriceList;
            Session["CopiesList"] = VMobj.MovieCopiesList;
            Session["TotalPrice"] = VMobj.TotalPrice;

            return View(VMobj);
        }
        public ActionResult SubmitOrder()
        {
            Order ord = new Order();
            ord.CustomerId = (int)Session["CustomerId"];
            ord.OrderedDate = DateTime.Now;
            ord.TotalPrice = (decimal)Session["TotalPrice"];
            db.Orders.Add(ord);
            db.SaveChanges();
            var orderId = ord.Id;
            //orderId = db.Orders.Where(o => o.CustomerId == ord.CustomerId).Select(o => o.Id).FirstOrDefault();
            var ordId = db.Orders.OrderByDescending(o => o.Id).Select(o => o.Id).Take(1);
            List<int> MovieIdList = new List<int>();
            MovieIdList = (List<int>)Session["DistinctMovieIdLst"];
            List<int> CopiesList = new List<int>();
            CopiesList = (List<int>)Session["CopiesList"];
            List<decimal> PriceList = new List<decimal>();
            PriceList = (List<decimal>)Session["PriceList"];
            int idx = 0;
            foreach (int id in MovieIdList)
            {
                OrderRow ordw = new OrderRow();
                ordw.OrderId = orderId;
                ordw.MovieId = id;
                ordw.NoofCopies = CopiesList.ElementAt(idx);
                ordw.Price = PriceList.ElementAt(idx);
                //ordw.Order.TotalPrice += PriceList.ElementAt(idx);
                db.OrderRows.Add(ordw);
                db.SaveChanges();
                idx += 1; //idx=idx+1;
            }
            Session.Clear();
            Session["OrderId"] = orderId;
            return View();
        }
        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Director,ReleaseYear,Price,ImageUrl")] Movie movie)
        {

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Director,ReleaseYear,Price,ImageUrl")] Movie movie)
        {

            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
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
