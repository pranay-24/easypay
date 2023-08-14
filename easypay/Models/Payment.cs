using easypay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // USER iD
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        //Transaction ID

        [ForeignKey("Transaction")]
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
    }

    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}