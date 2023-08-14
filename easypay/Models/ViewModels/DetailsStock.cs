using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easypay.Models.ViewModels
{
    public class DetailsStock
    {
        public StockDto SelectedStock { get; set; }
        public IEnumerable<PortfolioDto> Portfolios { get; set; }

        public IEnumerable<PortfolioDto> AvailablePortfolios { get; set; }
    }
}