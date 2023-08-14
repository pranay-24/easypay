using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using easypay.Models;
//using pm3.Models;

namespace pm3.Controllers
{
    public class PortfoliosDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Portfolios in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Portfolios in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/PortfolioData/ListPortfolios
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PortfolioDto))]
        public IHttpActionResult ListPortfolios()
        {
            List<Portfolio> Portfolios = db.Portfolios.ToList();
            List<PortfolioDto> PortfolioDtos = new List<PortfolioDto>();

            Portfolios.ForEach(k => PortfolioDtos.Add(new PortfolioDto()
            {
                PortfolioID = k.PortfolioID,
                PortfolioName = k.PortfolioName,

            }));

            return Ok(PortfolioDtos);
        }

        /// <summary>
        /// Returns all Portfolios in the system associated with a particular Stock.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Portfolios in the database taking care of a particular Stock
        /// </returns>
        /// <param name="id">Stock Primary Key</param>
        /// <example>
        /// GET: api/PortfolioData/ListPortfoliosForStock/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PortfolioDto))]
        public IHttpActionResult ListPortfoliosFortheStock(int stockid)
        {
            List<Portfolio> Portfolios = db.Portfolios.Where(
                k => k.Stocks.Any(
                    a => a.StocksID == stockid)
                ).ToList();
            List<PortfolioDto> PortfolioDtos = new List<PortfolioDto>();

            Portfolios.ForEach(k => PortfolioDtos.Add(new PortfolioDto()
            {
                PortfolioID = k.PortfolioID,
                PortfolioName = k.PortfolioName,

            }));

            return Ok(PortfolioDtos);
        }


        /// <summary>
        /// Returns Portfolios in the system not caring for a particular Stock.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Portfolios in the database not taking care of a particular Stock
        /// </returns>
        /// <param name="id">Stock Primary Key</param>
        /// <example>
        /// GET: api/PortfolioData/ListPortfoliosNotCaringForStock/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PortfolioDto))]
        public IHttpActionResult ListPortfoliosNotContainingStock(int stockid)
        {
            List<Portfolio> Portfolios = db.Portfolios.Where(
                k => !k.Stocks.Any(
                    a => a.StocksID == stockid)
                ).ToList();
            List<PortfolioDto> PortfolioDtos = new List<PortfolioDto>();

            Portfolios.ForEach(k => PortfolioDtos.Add(new PortfolioDto()
            {
                PortfolioID = k.PortfolioID,
                PortfolioName = k.PortfolioName,

            }));

            return Ok(PortfolioDtos);
        }

        /// <summary>
        /// Returns all Portfolios in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Portfolio in the system matching up to the Portfolio ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Portfolio</param>
        /// <example>
        /// GET: api/PortfolioData/FindPortfolio/5
        /// </example>
        [ResponseType(typeof(PortfolioDto))]
        [HttpGet]
        public IHttpActionResult FindPortfolio(int portfolioid)
        {
            Portfolio Portfolio = db.Portfolios.Find(portfolioid);
            PortfolioDto PortfolioDto = new PortfolioDto()
            {
                PortfolioID = Portfolio.PortfolioID,
                PortfolioName = Portfolio.PortfolioName,

            };
            if (Portfolio == null)
            {
                return NotFound();
            }

            return Ok(PortfolioDto);
        }

        /// <summary>
        /// Updates a particular Portfolio in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Portfolio ID primary key</param>
        /// <param name="Portfolio">JSON FORM DATA of an Portfolio</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PortfolioData/UpdatePortfolio/5
        /// FORM DATA: Portfolio JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePortfolio(int id, Portfolio Portfolio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Portfolio.PortfolioID)
            {

                return BadRequest();
            }

            db.Entry(Portfolio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortfolioExists(id))
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

        /// <summary>
        /// Adds an Portfolio to the system
        /// </summary>
        /// <param name="Portfolio">JSON FORM DATA of an Portfolio</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Portfolio ID, Portfolio Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PortfolioData/AddPortfolio
        /// FORM DATA: Portfolio JSON Object
        /// </example>
        [ResponseType(typeof(Portfolio))]
        [HttpPost]
        public IHttpActionResult AddPortfolio(Portfolio Portfolio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Portfolios.Add(Portfolio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Portfolio.PortfolioID }, Portfolio);
        }

        /// <summary>
        /// Deletes an Portfolio from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Portfolio</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PortfolioData/DeletePortfolio/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Portfolio))]
        [HttpPost]
        public IHttpActionResult DeletePortfolio(int id)
        {
            Portfolio Portfolio = db.Portfolios.Find(id);
            if (Portfolio == null)
            {
                return NotFound();
            }

            db.Portfolios.Remove(Portfolio);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PortfolioExists(int id)
        {
            return db.Portfolios.Count(e => e.PortfolioID == id) > 0;
        }




    }
}