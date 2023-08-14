using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Description;
using easypay.Models;
//using pm3.Models;

namespace pm3.Controllers
{
    public class StocksDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StocksData



        [HttpGet]
        [Route("api/StockData/ListStocks")]
        [ResponseType(typeof(StockDto))]
        public IHttpActionResult ListStocks()
        {
            List<Stock> Stocks = db.Stocks.ToList();
            List<StockDto> StocksDtos = new List<StockDto>();

            Stocks.ForEach(a => StocksDtos.Add(new StockDto()
            {
                StocksID = a.StocksID,
                StockName = a.StockName,
                StockBuyPrice = a.StockBuyPrice,
                StockSellPrice = a.StockSellPrice,
                StockQty = a.StockQty,
                /*
                SpeciesID = a.Species.SpeciesID,
                SpeciesName = a.Species.SpeciesName*/
            }));

            return Ok(StocksDtos);
        }

        /*
        // GET: api/StocksData/5
        [ResponseType(typeof(Stock))]
        public IHttpActionResult FindStock(int id)
        {
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }
        */

        /// <summary>
        /// Returns all Stock in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Stock in the system matching up to the Stock ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Stock</param>
        /// <example>
        /// GET: api/StockData/FindStock/5
        /// </example>
        [ResponseType(typeof(StockDto))]
        [Route("api/StockData/FindStock/{stockid}")]
        [HttpGet]
        public IHttpActionResult FindStock(int stockid)
        {
            Stock Stock = db.Stocks.Find(stockid);
            StockDto StockDto = new StockDto()
            {
                StocksID = Stock.StocksID,
                StockName = Stock.StockName,
                StockBuyPrice = Stock.StockBuyPrice,
                StockSellPrice = Stock.StockSellPrice,
                StockQty = Stock.StockQty,
            };
            if (Stock == null)
            {
                return NotFound();
            }

            return Ok(StockDto);


        }

        /// <summary>
        /// Gathers information about Stock related to a particular Portfolio
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Stock in the database, including their associated species that match to a particular Portfolio id
        /// </returns>
        /// <param name="id">Portfolio ID.</param>
        /// <example>
        /// GET: api/StockData/ListStockForPortfolio/1
        /// 
        /// 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StockDto))]
        public IHttpActionResult ListStocksforPortfolio(int id)
        {
            //all Stock that have Portfolios which match with our ID
            List<Stock> Stocks = db.Stocks.Where(
                a => a.Portfolios.Any(
                    k => k.PortfolioID == id
                )).ToList();
            List<StockDto> StockDtos = new List<StockDto>();

            Stocks.ForEach(a => StockDtos.Add(new StockDto()
            {
                StocksID = a.StocksID,
                StockName = a.StockName,
                StockBuyPrice = a.StockBuyPrice,
                StockSellPrice = a.StockSellPrice,
                StockQty = a.StockQty,
            }));

            return Ok(StockDtos);
        }


        /// <summary>
        /// Associates a particular Portfolio with a particular Stock
        /// </summary>
        /// <param name="Stockid">The Stock ID primary key</param>
        /// <param name="Portfolioid">The Portfolio ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StockData/AssociateStockWithPortfolio/9/1
        /// </example>
        [HttpPost]
        [Route("api/StockData/AssociateStockWithPortfolio/{stockid}/{portfolioid}")]
        public IHttpActionResult AssociateStockWithPortfolio(int stockid, int portfolioid)
        {

            Stock SelectedStock = db.Stocks.Include(a => a.Portfolios).Where(a => a.StocksID == stockid).FirstOrDefault();
            Portfolio SelectedPortfolio = db.Portfolios.Find(portfolioid);

            if (SelectedStock == null || SelectedPortfolio == null)
            {
                return NotFound();
            }
            /*
            Debug.WriteLine("input Stock id is: " + Stockid);
            Debug.WriteLine("selected Stock name is: " + SelectedStock.StockName);
            Debug.WriteLine("input Portfolio id is: " + Portfolioid);
            Debug.WriteLine("selected Portfolio name is: " + SelectedPortfolio.PortfolioFirstName);

            */
            SelectedStock.Portfolios.Add(SelectedPortfolio);
            db.SaveChanges();

            return Ok();


        }

        /// <summary>
        /// Removes an association between a particular Portfolio and a particular Stock
        /// </summary>
        /// <param name="Stockid">The Stock ID primary key</param>
        /// <param name="Portfolioid">The Portfolio ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StockData/AssociateStockWithPortfolio/9/1
        /// </example>
        /// 
        [HttpPost]
        [Route("api/StockData/UnAssociateStockWithPortfolio/{stockid}/{portfolioid}")]
        public IHttpActionResult UnAssociateStockWithPortfolio(int stockid, int portfolioid)
        {

            Stock SelectedStock = db.Stocks.Include(a => a.Portfolios).Where(a => a.StocksID == stockid).FirstOrDefault();
            Portfolio SelectedPortfolio = db.Portfolios.Find(portfolioid);

            if (SelectedStock == null || SelectedPortfolio == null)
            {
                return NotFound();
            }
            /*
            Debug.WriteLine("input Stock id is: " + Stockid);
            Debug.WriteLine("selected Stock name is: " + SelectedStock.StockName);
            Debug.WriteLine("input Portfolio id is: " + Portfolioid);
            Debug.WriteLine("selected Portfolio name is: " + SelectedPortfolio.PortfolioFirstName);
            */

            SelectedStock.Portfolios.Remove(SelectedPortfolio);
            db.SaveChanges();

            return Ok();
        }


        // PUT: api/StocksData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStock(int id, Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stock.StocksID)
            {
                return BadRequest();
            }

            db.Entry(stock).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StocksData
        [ResponseType(typeof(Stock))]
        public IHttpActionResult AddStock(Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stocks.Add(stock);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stock.StocksID }, stock);
        }

        // DELETE: api/StocksData/5
        [ResponseType(typeof(Stock))]
        public IHttpActionResult DeleteStock(int id)
        {
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }

            db.Stocks.Remove(stock);
            db.SaveChanges();

            return Ok(stock);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StockExists(int id)
        {
            return db.Stocks.Count(e => e.StocksID == id) > 0;
        }
    }
}