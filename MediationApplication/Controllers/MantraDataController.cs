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
using MediationApplication.Models;

namespace MediationApplication.Controllers
{
    public class MantraDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MantraData/ListMantras
        [HttpGet]
        public IEnumerable<MantraDto> ListMantras()
        {
            List<Mantra> Mantras = db.Mantras.ToList();
            List<MantraDto> MantraDtos = new List<MantraDto>();

            Mantras.ForEach(Mantra => MantraDtos.Add(new MantraDto() 
            { 
                MantraID = Mantra.MantraID,
                MantraName = Mantra.MantraName
            }));

            return MantraDtos;
        }

        // GET: api/MantraData/FindMantra/5
        [ResponseType(typeof(Mantra))]
        [HttpGet]
        public IHttpActionResult FindMantra(int id)
        {
            Mantra Mantra = db.Mantras.Find(id);
            MantraDto MantraDto = new MantraDto()
            {
                MantraID = Mantra.MantraID,
                MantraName = Mantra.MantraName
            };

            if (Mantra == null)
            {
                return NotFound();
            }

            return Ok(MantraDto);
        }

        // POST: api/MantraData/UpdateMantra/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMantra(int id, Mantra mantra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mantra.MantraID)
            {
                return BadRequest();
            }

            db.Entry(mantra).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MantraExists(id))
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

        // POST: api/MantraData/AddMantra
        [ResponseType(typeof(Mantra))]
        [HttpPost]
        public IHttpActionResult AddMantra(Mantra mantra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Mantras.Add(mantra);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mantra.MantraID }, mantra);
        }

        // POST: api/MantraData/DeleteMantra/5
        [ResponseType(typeof(Mantra))]
        [HttpPost]
        public IHttpActionResult DeleteMantra(int id)
        {
            Mantra mantra = db.Mantras.Find(id);
            if (mantra == null)
            {
                return NotFound();
            }

            db.Mantras.Remove(mantra);
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

        private bool MantraExists(int id)
        {
            return db.Mantras.Count(e => e.MantraID == id) > 0;
        }
    }
}