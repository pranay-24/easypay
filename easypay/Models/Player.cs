using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Player
    {
        [Key]
        public int PlayerID { get; set; }

        public string PlayerName { get; set; }

        public int PlayerBalance { get; set; }

        public int PlayerPosition { get; set; }

    }
}