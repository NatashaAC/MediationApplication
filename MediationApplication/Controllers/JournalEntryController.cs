using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using MediationApplication.Models;
using System.Web.Script.Serialization;

namespace MediationApplication.Controllers
{
    public class JournalEntryController : Controller
    {
        private static readonly HttpClient client;

        static JournalEntryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/JournalEntryData/");
        }

        // GET: JournalEntry/List
        public ActionResult List()
        {
            // Objective: Communicate with journal entry data api to retrieve a list of entries
            // curl https://localhost:44316/api/JournalEntryData/ListEntries

            string url = "ListEntries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<JournalEntryDto> JournalEntries = response.Content.ReadAsAsync<IEnumerable<JournalEntryDto>>().Result;
            Debug.WriteLine("Number of Journal Entries -> " + JournalEntries.Count());

            return View(JournalEntries);
        }

        // GET: JournalEntry/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with journal entry data api to retrieve a list of entries
            // curl https://localhost:44316/api/JournalEntryData/FindEntry/{id}

            string url = "FindEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The status code is " + response.StatusCode);

            JournalEntryDto SelectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;
            Debug.WriteLine("Date of Journal Entry -> " + SelectedEntry);

            return View(SelectedEntry);
        }

        // GET: JournalEntry/New
        public ActionResult New()
        {
            return View();
        }

        // POST: JournalEntry/Create
        [HttpPost]
        public ActionResult Create(JournalEntry journalEntry)
        {
            Debug.WriteLine("The date of new session -> " + journalEntry.JournalEntryID);

            // Objective: Communicate with Journal entry data api to add a new entry
            // curl -H "Content-Type:application/json" -d @entry.json https://localhost:44316/api/JournalEntryData/AddEntry

            string url = "AddEntry";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(journalEntry);
            Debug.WriteLine("The json payload is -> " + jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: JournalEntry/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JournalEntry/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: JournalEntry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JournalEntry/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
