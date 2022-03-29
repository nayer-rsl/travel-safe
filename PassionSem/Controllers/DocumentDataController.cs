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
    public class DocumentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Documents in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Documents in the database, including their associated Language.
        /// </returns>
        /// <example>
        /// GET: api/DocumentData/ListDocuments
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DocumentDto))]
        public IHttpActionResult ListDocuments()
        {
            List<Document> Documents = db.Documents.ToList();
            List<DocumentDto> DocumentDtos = new List<DocumentDto>();

            Documents.ForEach(d => DocumentDtos.Add(new DocumentDto()
            {
                DocumentID = d.DocumentID,
                DocumentName = d.DocumentName,
                
            }));

            return Ok(DocumentDtos);
        }

        /// <summary>
        /// Returns all Documents in the system associated with a particular Country.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Documents in the database taking care of a particular Country
        /// </returns>
        /// <param name="id">Country Primary Key</param>
        /// <example>
        /// GET: api/DocumentData/ListDocumentsForCountry/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DocumentDto))]
        public IHttpActionResult ListDocumentsForCountry(int id)
        {
            List<Document> Documents = db.Documents.Where(
                d => d.Countries.Any(
                    a => a.CountryID == id)
                ).ToList();
            List<DocumentDto> DocumentDtos = new List<DocumentDto>();

            Documents.ForEach(d => DocumentDtos.Add(new DocumentDto()
            {
                DocumentID = d.DocumentID,
                DocumentName = d.DocumentName,
            }));

            return Ok(DocumentDtos);
        }


        /// <summary>
        /// Returns Documents in the system not caring for a particular Country.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Documents in the database not taking care of a particular Country
        /// </returns>
        /// <param name="id">Country Primary Key</param>
        /// <example>
        /// GET: api/DocumentData/ListDocumentsNotCaringForCountry/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DocumentDto))]
        public IHttpActionResult listDocumentsNotForCountry(int id)
        {
            List<Document> Documents = db.Documents.Where(
                d => !d.Countries.Any(
                    a => a.CountryID == id)
                ).ToList();
            List<DocumentDto> DocumentDtos = new List<DocumentDto>();

            Documents.ForEach(d => DocumentDtos.Add(new DocumentDto()
            {
                DocumentID = d.DocumentID,
                DocumentName = d.DocumentName
            }));

            return Ok(DocumentDtos);
        }

        /// <summary>
        /// Returns all Documents in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Document in the system matching up to the Document ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Document</param>
        /// <example>
        /// GET: api/DocumentData/FindDocument/5
        /// </example>
        [ResponseType(typeof(DocumentDto))]
        [HttpGet]
        public IHttpActionResult FindDocument(int id)
        {
            Document Document = db.Documents.Find(id);
            DocumentDto DocumentDto = new DocumentDto()
            {
                DocumentID = Document.DocumentID,
                DocumentName = Document.DocumentName
            };
            if (Document == null)
            {
                return NotFound();
            }

            return Ok(DocumentDto);
        }

        /// <summary>
        /// Updates a particular Document in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Document ID primary key</param>
        /// <param name="Document">JSON FORM DATA of an Document</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DocumentData/UpdateDocument/5
        /// FORM DATA: Document JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDocument(int id, Document Document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Document.DocumentID)
            {

                return BadRequest();
            }

            db.Entry(Document).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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
        /// Adds an Document to the system
        /// </summary>
        /// <param name="Document">JSON FORM DATA of an Document</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Document ID, Document Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DocumentData/AddDocument
        /// FORM DATA: Document JSON Object
        /// </example>
        [ResponseType(typeof(Document))]
        [HttpPost]
        public IHttpActionResult AddDocument(Document Document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Documents.Add(Document);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Document.DocumentID }, Document);
        }

        /// <summary>
        /// Deletes an Document from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Document</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DocumentData/DeleteDocument/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Document))]
        [HttpPost]
        public IHttpActionResult DeleteDocument(int id)
        {
            Document Document = db.Documents.Find(id);
            if (Document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(Document);
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

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.DocumentID == id) > 0;
        }
    }
}

