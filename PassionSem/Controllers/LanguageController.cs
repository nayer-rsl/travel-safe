using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionSem.Models;
using PassionSem.Models.ViewModels;
using System.Web.Script.Serialization;

namespace PassionSem.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LanguageController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/api/");
        }

        // GET: Language/List
        public ActionResult List()
        {
            //objective: communicate with our Language data api to retrieve a list of Languages
            //curl https://localhost:44340/api/Languagedata/listLanguages


            string url = "languagedata/listlanguages";
            HttpResponseMessage response = client.GetAsync(url).Result;


            IEnumerable<LanguageDto> Languages = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;


            return View(Languages);
        }

        // GET: Language/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Language data api to retrieve one Language
            //curl https://localhost:44340/api/Languagedata/findLanguage/{id}

            DetailsLanguage ViewModel = new DetailsLanguage();

            string url = "Languagedata/findLanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            LanguageDto SelectedLanguage = response.Content.ReadAsAsync<LanguageDto>().Result;
            Debug.WriteLine("Language received : ");
            Debug.WriteLine(SelectedLanguage.LanguageName);

            ViewModel.SelectedLanguage = SelectedLanguage;

            //show information about Countries related to this Language
            //send a request to gather information about Countries related to a particular Language ID
            url = "countrydata/listcountriesforlanguage/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CountryDto> SpokenCountries = response.Content.ReadAsAsync<IEnumerable<CountryDto>>().Result;

            ViewModel.SpokenCountries = SpokenCountries;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Language/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Language/Create
        [HttpPost]
        public ActionResult Create(Language Language)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Language.LanguageName);
            //objective: add a new Language into our system using the API
            //curl -H "Content-Type:application/json" -d @Language.json https://localhost:44340/api/Languagedata/addLanguage 
            string url = "languagedata/addlanguage";


            string jsonpayload = jss.Serialize(Language);
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

        // GET: Language/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "languagedata/findlanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LanguageDto SelectedLanguage = response.Content.ReadAsAsync<LanguageDto>().Result;
            return View(SelectedLanguage);
        }

        // POST: Language/Update/5
        [HttpPost]
        public ActionResult Update(int id, Language Language)
        {

            string url = "languagedata/updatelanguage/" + id;
            string jsonpayload = jss.Serialize(Language);
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

        // GET: Language/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "languagedata/findlanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LanguageDto SelectedLanguage = response.Content.ReadAsAsync<LanguageDto>().Result;
            return View(SelectedLanguage);
        }

        // POST: Language/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "languagedata/deletelanguage/" + id;
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
