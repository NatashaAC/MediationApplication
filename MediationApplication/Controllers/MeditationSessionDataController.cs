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
    public class MeditationSessionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///     Returns a list of the data in the meditationsessions table within the database.
        /// </summary>
        /// <returns> List of Meditation Sessions </returns>
        /// <example>
        ///     GET: api/MeditationSessionData/ListSessions
        /// </example>
        [HttpGet]
        public IEnumerable<MeditationSessionDto> ListSessions()
        {
            List<MeditationSession> MeditationSessions = db.MeditationSessions.ToList();
            List<MeditationSessionDto> MeditationSessionDtos = new List<MeditationSessionDto>();

            MeditationSessions.ForEach(Session => MeditationSessionDtos.Add(new MeditationSessionDto()
            {
                SessionID = Session.SessionID,
                SessionDate = Session.SessionDate,
                SessionStartTime = Session.SessionStartTime,
                SessionEndTime = Session.SessionEndTime,
                SessionDuration = Session.SessionDuration,
                MantraName = Session.Mantra.MantraName
            }));

            return MeditationSessionDtos;
        }

        /// <summary>
        ///     Gathers a list of Meditation Sessions related to a Mantra ID
        /// </summary>
        /// <returns> List of Meditation Sessions </returns>
        /// <param name="id"> Mantra ID </param>
        /// <example>
        ///     GET: api/MeditationSessionData/ListSessionsForMantras/2
        /// </example>
        [HttpGet]
        public IEnumerable<MeditationSessionDto> ListSessionsForMantras(int id)
        {
            List<MeditationSession> MeditationSessions = db.MeditationSessions.Where(ms => ms.MantraID == id).ToList();
            List<MeditationSessionDto> MeditationSessionDtos = new List<MeditationSessionDto>();

            MeditationSessions.ForEach(Session => MeditationSessionDtos.Add(new MeditationSessionDto()
            {
                SessionID = Session.SessionID,
                SessionDate = Session.SessionDate,
                SessionStartTime = Session.SessionStartTime,
                SessionEndTime = Session.SessionEndTime,
                SessionDuration = Session.SessionDuration,
                MantraID = Session.MantraID,
                MantraName = Session.Mantra.MantraName
            }));

            return MeditationSessionDtos;
        }

        /// <summary>
        ///     Gathers the data of a specific Meditation Session based on the Session id
        /// </summary>
        /// <param name="id"> Meditation Session Id </param>
        /// <returns>
        ///     HEADER: 200 (OK) 
        ///     CONTENT: A Session related to Meditation Session Id
        ///     or 
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     GET: api/MeditationSessionData/FindSession/5
        /// </example>
        [ResponseType(typeof(MeditationSessionDto))]
        [HttpGet]
        public IHttpActionResult FindSession(int id)
        {
            MeditationSession Session = db.MeditationSessions.Find(id);
            MeditationSessionDto MeditationSessionDto = new MeditationSessionDto()
            {
                SessionID = Session.SessionID,
                SessionDate = Session.SessionDate,
                SessionDuration = Session.SessionDuration,
                SessionStartTime = Session.SessionStartTime,
                SessionEndTime = Session.SessionEndTime,
                MantraID = Session.Mantra.MantraID,
                MantraName = Session.Mantra.MantraName
            };

            if (Session == null)
            {
                return NotFound();
            }

            return Ok(MeditationSessionDto);
        }

        /// <summary>
        ///     Updates a Session's information in the meditationsessions table within the database.
        /// </summary>
        /// <param name="id"> Session Id </param>
        /// <param name="meditationSession"> Meditation Session Object </param>
        /// <returns>
        ///     HEADER: 200 (Success, No Content)
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MeditationSessionData/UpdateSession/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSession(int id, MeditationSession meditationSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meditationSession.SessionID)
            {
                return BadRequest();
            }

            db.Entry(meditationSession).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeditationSessionExists(id))
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
        ///     Adds a new session to the meditationsessions table within the database.
        /// </summary>
        /// <param name="meditationSession"> json Form Data of a Session </param>
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Meditation Session data
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        ///     POST: api/MeditationSessionData/AddSession
        /// </example>
        [ResponseType(typeof(MeditationSession))]
        [HttpPost]
        public IHttpActionResult AddSession(MeditationSession meditationSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MeditationSessions.Add(meditationSession);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = meditationSession.SessionID }, meditationSession);
        }

        /// <summary>
        ///     Deletes a session from the meditationsessions table within the database
        /// </summary>
        /// <param name="id"> Session Id </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/MeditationSessionData/DeleteSession/5
        /// </example>
        [ResponseType(typeof(MeditationSession))]
        [HttpPost]
        public IHttpActionResult DeleteSession(int id)
        {
            MeditationSession meditationSession = db.MeditationSessions.Find(id);
            if (meditationSession == null)
            {
                return NotFound();
            }

            db.MeditationSessions.Remove(meditationSession);
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

        private bool MeditationSessionExists(int id)
        {
            return db.MeditationSessions.Count(e => e.SessionID == id) > 0;
        }
    }
}