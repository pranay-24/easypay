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
    public class PortfoliosController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PortfoliosController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/");
        }

        // GET: Portfolio/List
        public ActionResult List()
        {
            //objective: communicate with our Portfolio data api to retrieve a list of Portfolios
            //curl https://localhost:44324/api/Portfoliodata/listPortfolios


            string url = "Portfoliodata/listPortfolios";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PortfolioDto> Portfolios = response.Content.ReadAsAsync<IEnumerable<PortfolioDto>>().Result;
            //Debug.WriteLine("Number of Portfolios received : ");
            //Debug.WriteLine(Portfolios.Count());


            return View(Portfolios);
        }

        // GET: Portfolio/Details/5
        public ActionResult Details(int id)
        {
            DetailsPortfolio ViewModel = new DetailsPortfolio();

            //objective: communicate with our Portfolio data api to retrieve one Portfolio
            //curl https://localhost:44324/api/Portfoliodata/findPortfolio/{id}

            string url = "Portfoliodata/FindPortfolio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PortfolioDto SelectedPortfolio = response.Content.ReadAsAsync<PortfolioDto>().Result;
            Debug.WriteLine("Portfolio received : ");
            Debug.WriteLine(SelectedPortfolio.PortfolioName);

            ViewModel.SelectedPortfolio = SelectedPortfolio;

            //show all animals under the care of this Portfolio
            url = "StockData/ListStocksforPortfolio/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StockDto> PartStocks = response.Content.ReadAsAsync<IEnumerable<StockDto>>().Result;

            ViewModel.PartStocks = PartStocks;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Portfolio/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Portfolio/Create
        [HttpPost]
        public ActionResult Create(Portfolio Portfolio)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Portfolio.PortfolioName);
            //objective: add a new Portfolio into our system using the API
            //curl -H "Content-Type:application/json" -d @Portfolio.json https://localhost:44324/api/Portfoliodata/addPortfolio 
            string url = "Portfoliodata/addPortfolio";


            string jsonpayload = jss.Serialize(Portfolio);
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

        // GET: Portfolio/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Portfoliodata/findPortfolio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PortfolioDto selectedPortfolio = response.Content.ReadAsAsync<PortfolioDto>().Result;
            return View(selectedPortfolio);
        }

        // POST: Portfolio/Update/5
        [HttpPost]
        public ActionResult Update(int id, Portfolio Portfolio)
        {

            string url = "Portfoliodata/updatePortfolio/" + id;
            string jsonpayload = jss.Serialize(Portfolio);
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

        // GET: Portfolio/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Portfoliodata/findPortfolio/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PortfolioDto selectedPortfolio = response.Content.ReadAsAsync<PortfolioDto>().Result;
            return View(selectedPortfolio);
        }

        // POST: Portfolio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Portfoliodata/deletePortfolio/" + id;
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
    }
}