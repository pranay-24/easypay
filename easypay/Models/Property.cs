using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Property
    {
        [Key]
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }

        //Rent is in dollars
        public int PropertyRent { get; set; }

        //Price is in dollars
        public int PropertyPrice { get; set; }

        //A property can have only one owner
        //A Player can have multiple properties
        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        //properties brought can be tracked in transaction table

        public ICollection<Transaction> Transactions { get; set; }
    }

    public class PropertyDto
    {
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }

        //Rent is in dollars
        public int PropertyRent { get; set; }

        //Price is in dollars
        public int PropertyPrice { get; set; }

        public string PlayerName { get; set; }
    }
}