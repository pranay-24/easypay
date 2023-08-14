using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioID { get; set; }
        public string PortfolioName { get; set; }

        public string Risk { get; set; }

        public ICollection<Stock> Stocks { get; set; }
        //public ICollection<ETF> ETFs { get; set; }


    }

    public class PortfolioDto
    {
        public int PortfolioID { get; set; }
        public string PortfolioName { get; set; }

        public string Risk { get; set; }


    }

}