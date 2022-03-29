using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionSem.Models;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace PassionSem.Controllers
{
    public class CountryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all countrys in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all countrys in the database, including their associated Language.
        /// </returns>
        /// <example>
        /// GET: api/CountryData/Listcountrys
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CountryDto))]
        public IHttpActionResult ListCountries()
        {
            List<Country> Countries = db.Countries.ToList();
            List<CountryDto> CountryDtos = new List<CountryDto>();

            Countries.ForEach(a => CountryDtos.Add(new CountryDto()
            {
                CountryID = a.CountryID,
                CountryName = a.CountryName,
                CountryContinent = a.CountryContinent,
                LanguageID = a.Language.LanguageID,
                LanguageName = a.Language.LanguageName
            }));

            return Ok(CountryDtos);
        }

        /// <summary>
        /// Gathers information about all countriess associated to a Language ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all countrys in the database, including their associated Language matched with a particular Language ID
        /// </returns>
        /// <param name="id">Language ID.</param>
        /// <example>
        /// GET: api/countryData/ListcountrysForLanguage/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CountryDto))]
        public IHttpActionResult ListCountriesForLanguage(int id)
        {
            List<Country> Countries = db.Countries.Where(a => a.LanguageID == id).ToList();
            List<CountryDto> CountryDtos = new List<CountryDto>();

            Countries.ForEach(a => CountryDtos.Add(new CountryDto()
            {
                CountryID = a.CountryID,
                CountryName = a.CountryName,
                CountryContinent = a.CountryContinent,
                LanguageID = a.Language.LanguageID,
                LanguageName = a.Language.LanguageName
            }));

            return Ok(CountryDtos);
        }

        /// <summary>
        /// Gathers information about countrys related to a particular Document
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all countrys in the database, including their associated Language that match to a particular Document id
        /// </returns>
        /// <param name="id">Document ID.</param>
        /// <example>
        /// GET: api/countryData/ListCountriesForDocument/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CountryDto))]
        public IHttpActionResult ListCountriesForDocument(int id)
        {
            //all countrys that have Documents which match with our ID
            List<Country> Countries = db.Countries.Where(
                a => a.Documents.Any(
                    d => d.DocumentID == id
                )).ToList();
            List<CountryDto> CountryDtos = new List<CountryDto>();

            Countries.ForEach(a => CountryDtos.Add(new CountryDto()
            {
                CountryID = a.CountryID,
                CountryName = a.CountryName,
                CountryContinent = a.CountryContinent,
                LanguageID = a.Language.LanguageID,
                LanguageName = a.Language.LanguageName
            }));

            return Ok(CountryDtos);
        }


        /// <summary>
        /// Associates a particular Document with a particular country
        /// </summary>
        /// <param name="countryid">The country ID primary key</param>
        /// <param name="documentid">The Document ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/countryData/AssociatecountryWithDocument/9/1
        /// </example>
        [HttpPost]
        [Route("api/CountryData/AssociateCountryWithDocument/{countryid}/{documentid}")]
        public IHttpActionResult AssociateCountryWithDocument(int countryid, int documentid)
        {

            Country SelectedCountry = db.Countries.Include(a => a.Documents).Where(a => a.CountryID == countryid).FirstOrDefault();
            Document SelectedDocument = db.Documents.Find(documentid);

            if (SelectedCountry == null || SelectedDocument == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input country id is: " + countryid);
            Debug.WriteLine("selected country name is: " + SelectedCountry.CountryName);
            Debug.WriteLine("input document id is: " + documentid);
            Debug.WriteLine("selected document name is: " + SelectedDocument.DocumentName);


            SelectedCountry.Documents.Add(SelectedDocument);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular Document and a particular country
        /// </summary>
        /// <param name="countryid">The country ID primary key</param>
        /// <param name="documentid">The Document ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/countryData/AssociatecountryWithDocument/9/1
        /// </example>
        [HttpPost]
        [Route("api/CountryData/UnAssociateCountryWithDocument/{countryid}/{documentid}")]
        public IHttpActionResult UnAssociateCountryWithDocument(int countryid, int documentid)
        {

            Country SelectedCountry = db.Countries.Include(a => a.Documents).Where(a => a.CountryID == countryid).FirstOrDefault();
            Document SelectedDocument = db.Documents.Find(documentid);

            if (SelectedCountry == null || SelectedDocument == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input country id is: " + countryid);
            Debug.WriteLine("selected country name is: " + SelectedCountry.CountryName);
            Debug.WriteLine("input document id is: " + documentid);
            Debug.WriteLine("selected document name is: " + SelectedDocument.DocumentName);


            SelectedCountry.Documents.Remove(SelectedDocument);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all countrys in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An country in the system matching up to the country ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the country</param>
        /// <example>
        /// GET: api/CountryData/FindCountry/5
        /// </example>
        [ResponseType(typeof(CountryDto))]
        [HttpGet]
        public IHttpActionResult FindCountry(int id)
        {
            Country Country = db.Countries.Find(id);
            CountryDto CountryDto = new CountryDto()
            {
                CountryID = Country.CountryID,
                CountryName = Country.CountryName,
                CountryContinent = Country.CountryContinent,
                LanguageID = Country.Language.LanguageID,
                LanguageName = Country.Language.LanguageName
            };
            if (Country == null)
            {
                return NotFound();
            }

            return Ok(CountryDto);
        }

        /// <summary>
        /// Updates a particular country in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the country ID primary key</param>
        /// <param name="country">JSON FORM DATA of an country</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CountryData/UpdateCountry/5
        /// FORM DATA: country JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != country.CountryID)
            {

                return BadRequest();
            }

            db.Entry(country).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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
        /// Adds an country to the system
        /// </summary>
        /// <param name="country">JSON FORM DATA of an country</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: country ID, country Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CountryData/AddCountry
        /// FORM DATA: country JSON Object
        /// </example>
        [ResponseType(typeof(Country))]
        [HttpPost]
        public IHttpActionResult AddCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Countries.Add(country);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = country.CountryID }, country);
        }

        /// <summary>
        /// Deletes an country from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the country</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/countryData/Deletecountry/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Country))]
        [HttpPost]
        public IHttpActionResult DeleteCountry(int id)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }

            db.Countries.Remove(country);
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

        private bool CountryExists(int id)
        {
            return db.Countries.Count(e => e.CountryID == id) > 0;
        }
    }
}

