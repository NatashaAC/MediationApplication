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

        /// <summary>
        ///     Returns a list of the data in the mantras table within the database.
        /// </summary>
        /// <returns> List of Mantras </returns>
        /// <example>
        ///     GET: api/MantraData/ListMantras -> Mantra Object, Mantra Object, etc...
        /// </example>
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

        /// <summary>
        ///     Returns a list of Mantras for a specific Category
        /// </summary>
        /// <returns> List of Mantras related to a specific category id </returns>
        /// <param name="id"> Category ID </param>
        /// <example>
        ///     GET: api/MantraData/ListMantrasForCategory/3 -> Mantra Object, Mantra Object, etc...
        /// </example>
        [HttpGet]
        public IEnumerable<MantraDto> ListMantrasForCategory(int id)
        {
            // All Mantras that have Category IDs that match with id
            List<Mantra> Mantras = db.Mantras.Where(
                m => m.Categories.Any(
                c => c.CategoryID == id
                )).ToList();
            List<MantraDto> MantraDtos = new List<MantraDto>();

            Mantras.ForEach(Mantra => MantraDtos.Add(new MantraDto()
            {
                MantraID = Mantra.MantraID,
                MantraName = Mantra.MantraName
            }));

            return MantraDtos;
        }

        /// <summary>
        ///     Assigns a specific category with a specific mantra
        /// </summary>
        /// <param name="mantraid"> Mantra ID </param>
        /// <param name="categoryid"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MantraData/AssignMantraToCategory/13/3
        /// </example>
        [HttpPost] 
        [Route("api/MantraData/AssignMantraToCategory/{mantraid}/{categoryid}")]
        public IHttpActionResult AssignMantraToCategory(int mantraid, int categoryid)
        {
            Mantra SelectedMantra = db.Mantras.Include(m => m.Categories).Where(m => m.MantraID == mantraid).FirstOrDefault();
            Category SelectedCategory = db.Categories.Find(categoryid);

            if(SelectedMantra == null || SelectedCategory == null)
            {
                return NotFound();
            }

            SelectedMantra.Categories.Add(SelectedCategory);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        ///     UnAssigns a specific category from a specific mantra
        /// </summary>
        /// <param name="mantraid"> Mantra ID </param>
        /// <param name="categoryid"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MantraData/UnAssignMantraToCategory/10/3
        /// </example>
        [HttpPost]
        [Route("api/MantraData/UnAssignMantraToCategory/{mantraid}/{categoryid}")]
        public IHttpActionResult UnAssignMantraToCategory(int mantraid, int categoryid)
        {
            Mantra SelectedMantra = db.Mantras.Include(m => m.Categories).Where(m => m.MantraID == mantraid).FirstOrDefault();
            Category SelectedCategory = db.Categories.Find(categoryid);

            if (SelectedMantra == null || SelectedCategory == null)
            {
                return NotFound();
            }

            SelectedMantra.Categories.Remove(SelectedCategory);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        ///     Returns the data of a specific Mantra based on the mantra id
        /// </summary>
        /// <param name="id"> Mantra Id </param>
        /// <returns> 
        ///     HEADER: 200 (OK) 
        ///     CONTENT: A Mantra related to Mantra Id
        ///     or 
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     GET: api/MantraData/FindMantra/7 -> I create my own path and walk it with joy
        /// </example>
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

        /// <summary>
        ///     Updates a Mantra's information in the mantras table within the database.
        /// </summary>
        /// <param name="id"> Mantra Id </param>
        /// <param name="mantra">json Form data of a Mantra</param>
        /// <returns>
        ///     HEADER: 200 (Success, No Content)
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MantraData/UpdateMantra/8
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        ///     Adds a new mantra to the mantras table within the database.
        /// </summary>
        /// <param name="mantra"> json Form Data of a mantra </param>
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Mantra data
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        ///     POST: api/MantraData/AddMantra
        /// </example>
        [ResponseType(typeof(Mantra))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        ///     Deletes a mantra from the mantra table within the database
        /// </summary>
        /// <param name="id"> Mantra Id </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MantraData/DeleteMantra/5
        /// </example>
        [ResponseType(typeof(Mantra))]
        [HttpPost]
        [Authorize]
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