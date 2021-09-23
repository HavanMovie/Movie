using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Grp8MovieShop.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required,StringLength(50),Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(150), Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Required, StringLength(10), DataType(DataType.PostalCode), Display(Name = "Billing Zip")]
        public string BillingZip { get; set; }
        [Required, StringLength(165), Display(Name = "Billing City")]
        public string BillingCity { get; set; }
        [Required, StringLength(150), Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Required, StringLength(10),DataType(DataType.PostalCode), Display(Name = "Delivery Zip")]
        public string DeliveryZip { get; set; }
        [Required, StringLength(165), Display(Name = "Delivery City")]
        public string DeliveryCity { get; set; }
        [Required, StringLength(256),DataType(DataType.EmailAddress), Display(Name = "Email")]
        [Index(IsUnique = true)]
        public string EmailAddress { get; set; }
        [Required, StringLength(15), DataType(DataType.PhoneNumber), Display(Name = "Phone")]
        public string PhoneNo { get; set; }
    }
}