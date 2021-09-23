using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grp8MovieShop.Models.ViewModels
{
    public class DisplayCartVM
    {
        public List<Movie> DispMovieList { get; set; }
        public List<int> MovieCopiesList { get; set; }
        public List<decimal> PriceList { get; set; }
        public decimal TotalPrice { get; set; }
        public DisplayCartVM()
        {
            DispMovieList = new List<Movie>();
            MovieCopiesList = new List<int>();
            PriceList = new List<decimal>();
        }

    }

}