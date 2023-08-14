using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easypay.Models
{
    public class Stock
    {
        [Key]
        public int StocksID { get; set; }

        public string StockName { get; set; }

        public int StockBuyPrice { get; set; }

        public int StockSellPrice { get; set; }

        public int StockQty { get; set; }

        //A stock can be part of many portfolios

        public ICollection<Portfolio> Portfolios { get; set; }


    }

    public class StockDto
    {
        public int StocksID { get; set; }

        public string StockName { get; set; }

        public int StockBuyPrice { get; set; }

        public int StockSellPrice { get; set; }

        public int StockQty { get; set; }



    }
}