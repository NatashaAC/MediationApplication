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

        // GET: api/MeditationSessionData/ListSessions
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

        // GET: api/MeditationSessionData/FindSession/5
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
                MantraName = Session.Mantra.MantraName
            };

            if (Session == null)
            {
                return NotFound();
            }

            return Ok(MeditationSessionDto);
        }

        // POST: api/MeditationSessionData/UpdateSession/5
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

        // POST: api/MeditationSessionData/AddSession
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

        // POST: api/MeditationSessionData/DeleteSession/5
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