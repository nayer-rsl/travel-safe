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
using PassionSem.Models;
using System.Diagnostics;



namespace PassionSem.Controllers
{
    public class LanguageDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Languages in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Languages in the database, including their associated Language.
        /// </returns>
        /// <example>
        /// GET: api/LanguageData/ListLanguage
        /// </example>
        [HttpGet]
        [ResponseType(typeof(LanguageDto))]
        public IHttpActionResult ListLanguages()
        {
            List<Language> Languages = db.Languages.ToList();
            List<LanguageDto> LanguageDtos = new List<LanguageDto>();

            Languages.ForEach(l => LanguageDtos.Add(new LanguageDto()
            {
                LanguageID = l.LanguageID,
                LanguageName = l.LanguageName
            }));

            return Ok(LanguageDtos);
        }

        /// <summary>
        /// Returns all Languages in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Language in the system matching up to the Language ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Language</param>
        /// <example>
        /// GET: api/LanguageData/FindLanguage/5
        /// </example>
        [ResponseType(typeof(LanguageDto))]
        [HttpGet]
        public IHttpActionResult FindLanguage(int id)
        {
            Language Language = db.Languages.Find(id);
            LanguageDto LanguageDto = new LanguageDto()
            {
                LanguageID = Language.LanguageID,
                LanguageName = Language.LanguageName
            };
            if (Language == null)
            {
                return NotFound();
            }

            return Ok(LanguageDto);
        }

        /// <summary>
        /// Updates a particular Language in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Language ID primary key</param>
        /// <param name="Language">JSON FORM DATA of an Language</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/LanguageData/UpdateLanguage/5
        /// FORM DATA: Language JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateLanguage(int id, Language Language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Language.LanguageID)
            {

                return BadRequest();
            }

            db.Entry(Language).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
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
        /// Adds an Language to the system
        /// </summary>
        /// <param name="Language">JSON FORM DATA of an Language</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Language ID, Language Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/LanguageData/AddLanguage
        /// FORM DATA: Language JSON Object
        /// </example>
        [ResponseType(typeof(Language))]
        [HttpPost]
        public IHttpActionResult AddLanguage(Language Language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Languages.Add(Language);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Language.LanguageID }, Language);
        }

        /// <summary>
        /// Deletes an Language from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Language</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/LanguageData/DeleteLanguage/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Language))]
        [HttpPost]
        public IHttpActionResult DeleteLanguage(int id)
        {
            Language Language = db.Languages.Find(id);
            if (Language == null)
            {
                return NotFound();
            }

            db.Languages.Remove(Language);
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

        private bool LanguageExists(int id)
        {
            return db.Languages.Count(e => e.LanguageID == id) > 0;
        }
    }
}