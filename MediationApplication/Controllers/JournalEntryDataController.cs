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
    public class JournalEntryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/JournalEntryData/ListEntries
        [HttpGet]
        public IEnumerable<JournalEntryDto> ListEntries()
        {
            List<JournalEntry> JournalEntries = db.JournalEntries.ToList();
            List<JournalEntryDto> JournalEntryDtos = new List<JournalEntryDto>();

            JournalEntries.ForEach(JournalEntry => JournalEntryDtos.Add(new JournalEntryDto()
            {
                JournalEntryID = JournalEntry.JournalEntryID,
                Location = JournalEntry.Location,
                MoodBefore = JournalEntry.MoodBefore,
                MoodAfter = JournalEntry.MoodAfter,
                Thoughts = JournalEntry.Thoughts,
                SessionDate = JournalEntry.MeditationSession.SessionDate,
                SessionDuration = JournalEntry.MeditationSession.SessionDuration
            }));

            return JournalEntryDtos;
        }

        // GET: api/JournalEntryData/FindEntry/4
        [ResponseType(typeof(JournalEntryDto))]
        [HttpGet]
        public IHttpActionResult FindEntry(int id)
        {
            JournalEntry JournalEntry = db.JournalEntries.Find(id);
            JournalEntryDto JournalEntryDto = new JournalEntryDto()
            {
                JournalEntryID = JournalEntry.JournalEntryID,
                Location = JournalEntry.Location,
                MoodBefore = JournalEntry.MoodBefore,
                MoodAfter = JournalEntry.MoodAfter,
                Thoughts = JournalEntry.Thoughts,
                SessionDate = JournalEntry.MeditationSession.SessionDate,
                SessionDuration = JournalEntry.MeditationSession.SessionDuration
            };

            if (JournalEntry == null)
            {
                return NotFound();
            }

            return Ok(JournalEntryDto);
        }

        // POST: api/JournalEntryData/UpdateEntry/4
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateEntry(int id, JournalEntry JournalEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != JournalEntry.JournalEntryID)
            {
                return BadRequest();
            }

            db.Entry(JournalEntry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalEntryExists(id))
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

        // POST: api/JournalEntryData/AddEntry
        [ResponseType(typeof(JournalEntry))]
        [HttpPost]
        public IHttpActionResult AddEntry(JournalEntry JournalEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JournalEntries.Add(JournalEntry);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = JournalEntry.JournalEntryID }, JournalEntry);
        }

        // POST: api/JournalEntryData/DeleteEntry/5
        [ResponseType(typeof(JournalEntry))]
        [HttpPost]
        public IHttpActionResult DeleteEntry(int id)
        {
            JournalEntry JournalEntry = db.JournalEntries.Find(id);
            if (JournalEntry == null)
            {
                return NotFound();
            }

            db.JournalEntries.Remove(JournalEntry);
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

        private bool JournalEntryExists(int id)
        {
            return db.JournalEntries.Count(e => e.JournalEntryID == id) > 0;
        }
    }
}