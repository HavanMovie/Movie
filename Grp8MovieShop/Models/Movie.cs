using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grp8MovieShop.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required,StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(100)]
        public string Director { get; set; }
        [Required,Display(Name ="Release Year")]
        public int ReleaseYear { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required,DataType(DataType.ImageUrl), Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
    }
}