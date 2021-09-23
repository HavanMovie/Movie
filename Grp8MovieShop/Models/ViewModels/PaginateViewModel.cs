using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grp8MovieShop.Models.ViewModels
{
    public class PaginateViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public int MoviesPerPage { get; set; }
        public int MoviesPerLoad { get; set; }
        public int MoviesPerRow { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public static int NoofPages(int Count, int MoviesPerPage)
        {
            if (Count < MoviesPerPage) return 1;
            return Convert.ToInt32(Math.Ceiling(Count / (double)MoviesPerPage));
        }
        public PaginateViewModel()
        {
            Movies = new List<Movie>();
        }

    }
}