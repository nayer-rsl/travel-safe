using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PassionSem.Models;
using PassionSem.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace PassionSem.Controllers
{
    public class CountryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CountryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/api/");
        }

        // GET: Country/List
        public ActionResult List()
        {
            //objective: communicate with our Country data api to retrieve a list of Countrys
            //curl https://localhost:44340/api/countrydata/listcountries


            string url = "countrydata/listcountries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CountryDto> countries = response.Content.ReadAsAsync<IEnumerable<CountryDto>>().Result;
            


            return View(countries);
        }

        // GET: Country/Details/5
        public ActionResult Details(int id)
        {
            DetailsCountry ViewModel = new DetailsCountry();

            //objective: communicate with our Country data api to retrieve one Country
            //curl https://localhost:44340/api/countrydata/findcountry/{id}

            string url = "countrydata/findcountry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CountryDto SelectedCountry = response.Content.ReadAsAsync<CountryDto>().Result;
            Debug.WriteLine("country received : ");
            Debug.WriteLine(SelectedCountry.CountryName);

            ViewModel.SelectedCountry = SelectedCountry;

            //show associated Documents with this Country
            url = "documentdata/listdocumentsforcountry/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DocumentDto> NecessaryDocuments = response.Content.ReadAsAsync<IEnumerable<DocumentDto>>().Result;

            ViewModel.NecessaryDocuments = NecessaryDocuments;

            url = "documentdata/listDocumentsNotForCountry/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DocumentDto> AvailableDocuments = response.Content.ReadAsAsync<IEnumerable<DocumentDto>>().Result;


            ViewModel.AvailableDocuments = AvailableDocuments;


            return View(ViewModel);
        }


        //POST: Country/Associate/{countryid}
        [HttpPost]
        public ActionResult Associate(int id, int DocumentID)
        {
            Debug.WriteLine("Attempting to associate country :" + id + " with document " + DocumentID);

            //call our api to associate Country with Document
            string url = "countrydata/associatecountrywithdocument/" + id + "/" + DocumentID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Country/UnAssociate/{id}?DocumentID={documentID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int DocumentID)
        {
            Debug.WriteLine("Attempting to unassociate country :" + id + " with document: " + DocumentID);

            //call our api to associate Country with Document
            string url = "countrydata/unassociatecountrywithdocument/" + id + "/" + DocumentID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {

            return View();
        }

        // GET: Country/New
        public ActionResult New()
        {
            //information about all Languages in the system.
            //GET api/Languagedata/listLanguages

            string url = "languagedata/listlanguages";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<LanguageDto> LanguageOptions = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;

            return View(LanguageOptions);
        }

        // POST: Country/Create
        [HttpPost]
        public ActionResult Create(Country country)
        {
            Debug.WriteLine("the json payload is :");
            //objective: add a new Country into our system using the API
            //curl -H "Content-Type:application/json" -d @country.json https://localhost:44340/api/countrydata/addcountry 
            string url = "countrydata/addcountry";


            string jsonpayload = jss.Serialize(country);
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

        // GET: Country/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCountry ViewModel = new UpdateCountry();

            //the existing Country information
            string url = "countrydata/findcountry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CountryDto SelectedCountry = response.Content.ReadAsAsync<CountryDto>().Result;
            ViewModel.SelectedCountry = SelectedCountry;

            // all Languages to choose from when updating this Country
            //the existing Country information
            url = "languagedata/listlanguages/";
            response = client.GetAsync(url).Result;
            IEnumerable<LanguageDto> LanguageOptions = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;

            ViewModel.LanguageOptions = LanguageOptions;

            return View(ViewModel);
        }

        // POST: Country/Update/5
        [HttpPost]
        public ActionResult Update(int id, Country country)
        {

            string url = "countrydata/updatecountry/" + id;
            string jsonpayload = jss.Serialize(country);
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

        // GET: Country/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "countrydata/findcountry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CountryDto selectedcountry = response.Content.ReadAsAsync<CountryDto>().Result;
            return View(selectedcountry);
        }

        // POST: Country/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "countrydata/deletecountry/" + id;
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
