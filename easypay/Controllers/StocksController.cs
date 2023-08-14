using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net.Http;
using System.Diagnostics;
//using pm3.Models;
//using pm3.Models.ViewModels;
using System.Web.Script.Serialization;
using easypay.Models.ViewModels;
using easypay.Models;

namespace pm3.Controllers
{
    public class StocksController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StocksController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/");


        }
        // GET: Stocks
        public ActionResult Index()
        {
            return View();
        }

        /*
        // GET: Stocks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        */

        // GET: Stocks/List
        public ActionResult List()
        {
            //objective: communicate with our Stock data api to retrieve a list of Stock
            //curl https://localhost:44324/api/Stockdata/listStock


            string url = "StockData/ListStocks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<StockDto> stocks = response.Content.ReadAsAsync<IEnumerable<StockDto>>().Result;
            //Debug.WriteLine("Number of Stock received : ");
            //Debug.WriteLine(Stock.Count());


            return View(stocks);
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int id)
        {
            DetailsStock ViewModel = new DetailsStock();

            //objective: communicate with our Stock data api to retrieve one Stock
            //curl https://localhost:44324/api/Stockdata/findStock/{id}

            string url = "Stockdata/FindStock/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            StockDto SelectedStock = response.Content.ReadAsAsync<StockDto>().Result;
            Debug.WriteLine("Stock received : ");
            Debug.WriteLine(SelectedStock.StockName);

            ViewModel.SelectedStock = SelectedStock;

            //show associated keepers with this Stock
            url = "Portfoliodata/ListPortfoliosFortheStock/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PortfolioDto> Portfolios = response.Content.ReadAsAsync<IEnumerable<PortfolioDto>>().Result;

            ViewModel.Portfolios = Portfolios;

            url = "Portfoliodata/ListPortfoliosNotContainingStock/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PortfolioDto> AvailablePortfolios = response.Content.ReadAsAsync<IEnumerable<PortfolioDto>>().Result;

            ViewModel.Portfolios = AvailablePortfolios;


            return View(ViewModel);
        }


        //POST: Stock/Associate/{stocksid}
        [HttpPost]
        public ActionResult Associate(int id, int PortfolioID)
        {
            //  Debug.WriteLine("Attempting to associate Stock :" + id + " with keeper " + KeeperID);

            //call our api to associate Stock with keeper
            string url = "Stockdata/AssociateStockWithPortfolio/" + id + "/" + PortfolioID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Stock/UnAssociate/{id}?KeeperID={keeperID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int PortfolioID)
        {
            // Debug.WriteLine("Attempting to unassociate Stock :" + id + " with keeper: " + KeeperID);

            //call our api to associate Stock with keeper
            string url = "Stockdata/UnAssociateStockWithPortfolio/" + id + "/" + PortfolioID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(Stock stock)
        {
            // Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Stock.StockName);
            //objective: add a new Stock into our system using the API
            //curl -H "Content-Type:application/json" -d @Stock.json https://localhost:44324/api/Stockdata/addStock 
            string url = "Stockdata/AddStock";


            string jsonpayload = jss.Serialize(stock);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        // GET: Stock/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateStock ViewModel = new UpdateStock();

            //the existing Stock information
            string url = "Stockdata/FindStock/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StockDto SelectedStock = response.Content.ReadAsAsync<StockDto>().Result;
            ViewModel.SelectedStock = SelectedStock;

            /*
            // all species to choose from when updating this Stock
            //the existing Stock information

            url = "speciesdata/listspecies/";
            response = client.GetAsync(url).Result;
            IEnumerable<SpeciesDto> SpeciesOptions = response.Content.ReadAsAsync<IEnumerable<SpeciesDto>>().Result;

            ViewModel.SpeciesOptions = SpeciesOptions;

            */
            return View(ViewModel);
        }


        // POST: Stock/Update/5
        [HttpPost]
        public ActionResult Update(int id, Stock stock)
        {

            string url = "Stockdata/UpdateStock/" + id;
            string jsonpayload = jss.Serialize(stock);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Stock/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Stockdata/FindStock/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StockDto selectedstock = response.Content.ReadAsAsync<StockDto>().Result;
            return View(selectedstock);
        }


        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Stockdata/DeleteStock/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /*
         
      // GET: Stocks/Create
      public ActionResult Create()
      {
          return View();
      }


      // POST: Stocks/Create
      [HttpPost]
      public ActionResult Create(FormCollection collection)
      {
          try
          {
              // TODO: Add insert logic here

              return RedirectToAction("Index");
          }
          catch
          {
              return View();
          }
      }

      // GET: Stocks/Edit/5
      public ActionResult Edit(int id)
      {
          return View();
      }

      // POST: Stocks/Edit/5
      [HttpPost]
      public ActionResult Edit(int id, FormCollection collection)
      {
          try
          {
              // TODO: Add update logic here

              return RedirectToAction("Index");
          }
          catch
          {
              return View();
          }
      }

      // GET: Stocks/Delete/5
      public ActionResult Delete(int id)
      {
          return View();
      }

      // POST: Stocks/Delete/5
      [HttpPost]
      public ActionResult Delete(int id, FormCollection collection)
      {
          try
          {
              // TODO: Add delete logic here

              return RedirectToAction("Index");
          }
          catch
          {
              return View();
          }
      }
      */
    }
}