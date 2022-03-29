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
    public class DocumentController : Controller
    {
        // GET: Document
        private static readonly HttpClient client;
        private readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        static DocumentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/api/");
        }

        // GET: Document/List
        public ActionResult List()
        {
            //objective: communicate with our Document data api to retrieve a list of Documents
            //curl https://localhost:44340/api/Documentdata/listdocuments


            string url = "documentdata/listdocuments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DocumentDto> Documents = response.Content.ReadAsAsync<IEnumerable<DocumentDto>>().Result;
            //Debug.WriteLine("Number of Documents received : ");
            //Debug.WriteLine(Documents.Count());


            return View(Documents);
        }

        // GET: Document/Details/5
        public ActionResult Details(int id)
        {
            DetailsDocument ViewModel = new DetailsDocument();

            //objective: communicate with our Document data api to retrieve one Document
            //curl https://localhost:44340/api/Documentdata/finddocument/{id}

            string url = "documentdata/findDocument/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DocumentDto SelectedDocument = response.Content.ReadAsAsync<DocumentDto>().Result;
            Debug.WriteLine("Document received : ");
            Debug.WriteLine(SelectedDocument.DocumentName);

            ViewModel.SelectedDocument = SelectedDocument;

            //show all Countries which need this Document
            url = "countrydata/listcountriesfordocument/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CountryDto> NeedCountries = response.Content.ReadAsAsync<IEnumerable<CountryDto>>().Result;

            ViewModel.NeedCountries = NeedCountries;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Document/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Document/Create
        [HttpPost]
        public ActionResult Create(Document Document)
        {
            Debug.WriteLine("the json payload is :");
            //objective: add a new Document into our system using the API
            //curl -H "Content-Type:application/json" -d @Document.json https://localhost:44340/api/Documentdata/addDocument 
            string url = "documentdata/adddocument";


            string jsonpayload = jss.Serialize(Document);
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

        // GET: Document/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "documentdata/finddocument/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DocumentDto selectedDocument = response.Content.ReadAsAsync<DocumentDto>().Result;
            return View(selectedDocument);
        }

        // POST: Document/Update/5
        [HttpPost]
        public ActionResult Update(int id, Document Document)
        {

            string url = "documentdata/updatedocument/" + id;
            string jsonpayload = jss.Serialize(Document);
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

        // GET: Document/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "documentdata/finddocument/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DocumentDto selectedDocument = response.Content.ReadAsAsync<DocumentDto>().Result;
            return View(selectedDocument);
        }

        // POST: Document/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "documentdata/deletedocument/" + id;
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
