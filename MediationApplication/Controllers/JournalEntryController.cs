using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using MediationApplication.Models;
using MediationApplication.Models.ViewModels;
using System.Web.Script.Serialization;

namespace MediationApplication.Controllers
{
    public class JournalEntryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JournalEntryController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";

            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: JournalEntry/List
        [Authorize]
        public ActionResult List()
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Journal Entry Data Api to RETRIEVE a List of Entries
            // curl https://localhost:44316/api/JournalEntryData/ListEntries

            string url = "JournalEntryData/ListEntries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<JournalEntryDto> JournalEntries = response.Content.ReadAsAsync<IEnumerable<JournalEntryDto>>().Result;
            // Debug.WriteLine("Number of Journal Entries -> " + JournalEntries.Count());

            return View(JournalEntries);
        }

        // GET: JournalEntry/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Journal Entry Data Api to RETRIEVE an Entry
            // curl https://localhost:44316/api/JournalEntryData/FindEntry/{id}

            string url = "JournalEntryData/FindEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            JournalEntryDto SelectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;
            // Debug.WriteLine("Date of Journal Entry -> " + SelectedEntry);

            return View(SelectedEntry);
        }

        // GET: MeditationSession/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: JournalEntry/New
        [Authorize]
        public ActionResult New()
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Meditation Session Data api to RETRIEVE a list of Sessions
            // curl https://localhost:44316/api/MeditationSessionData/ListSessions
            // Get api/MeditationSessionData/ListSessions 
            string url = "MeditationSessionData/ListSessions";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MeditationSessionDto> SessionOptions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;

            return View(SessionOptions);
        }

        // POST: JournalEntry/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(JournalEntry journalEntry)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Debug.WriteLine("The date of new session -> " + journalEntry.JournalEntryID);

            // Objective: Communicate with Journal entry data api to add a new entry
            // curl -H "Content-Type:application/json" -d @entry.json https://localhost:44316/api/JournalEntryData/AddEntry

            string url = "JournalEntryData/AddEntry";

            string jsonpayload = jss.Serialize(journalEntry);
            // Debug.WriteLine("The json payload is -> " + jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: JournalEntry/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            UpdateEntry ViewModel = new UpdateEntry();
            
            // Objective: Communicate with Journal Entry Data Api to RETRIEVE an Entry
            // curl https://localhost:44316/api/JournalEntryData/FindEntry/{id}
            string url = "JournalEntryData/FindEntry/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            JournalEntryDto SelectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;
            ViewModel.SelectedEntry = SelectedEntry;

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a list of Sessions
            // curl https://localhost:44316/api/MeditationSessionData/ListSessions
            url = "MeditationSessionData/ListSessions/";
            response = client.GetAsync(url).Result;

            IEnumerable<MeditationSessionDto> SessionOptions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;
            ViewModel.SessionOptions = SessionOptions;

            return View(ViewModel);
        }

        // POST: JournalEntry/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, JournalEntry journalEntry)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Journal Entry Data api to UPDATE a Entry
            // curl -H "Content-Type:application/json" -d "" https://localhost:44316/api/JournalEntryData/UpdateEntry/{id}
            string url = "JournalEntryData/UpdateEntry/" + id;

            string jsonpayload = jss.Serialize(journalEntry);
            // Debug.WriteLine("The json payload is -> " + jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: JournalEntry/DeleteConfirmation/5
        [Authorize]
        public ActionResult DeleteConfirmation(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Journal Entry Data Api to RETRIEVE an Entry
            // curl https://localhost:44316/api/JournalEntryData/FindEntry/{id}
            string url = "JournalEntryData/FindEntry/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            JournalEntryDto SelectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;

            return View(SelectedEntry);
        }

        // POST: JournalEntry/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Journal Entry Data Api to DELETE an Entry
            // curl -d "" https://localhost:44316/api/JournalEntryData/DeleteEntry/{id}
            string url = "JournalEntryData/DeleteEntry/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            } else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
