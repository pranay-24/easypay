using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public string TransactionType { get; set; }

        public string TransactionRemarks { get; set; }

        //transcation table can keep track of all the transactions

        public ICollection<Property> Properties { get; set; }

    }
}