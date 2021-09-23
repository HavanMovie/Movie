using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grp8MovieShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required,Display(Name ="Ordered Date"),DataType(DataType.Date)]
        public DateTime OrderedDate { get; set; }
        //Foreign Key
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        //------
        [Required,DataType(DataType.Currency),Display(Name ="Total Price")]
        public decimal TotalPrice { get; set; }
    }
}