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
        /// <summary>
        ///     Routes to a dynamically generated "Journal Entry List" Page. 
        ///     Gathers information about all the entries in the database.
        /// </summary>
        /// <returns> A dynamic webpage which displays a List of Entries </returns>
        /// <example>
        ///     GET: JournalEntry/List
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///     Routes to a dynamically generated "Journal Entry Details" Page.
        ///     Gathers information about a specific entry from the database
        /// </summary>
        /// <param name="id"> Entry ID </param>
        /// <returns> A dynamic webpage which provides the current information of an entry </returns>
        /// <example>
        ///     GET: JournalEntry/Details/5
        /// </example>
        [HttpGet]
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

        // GET: JournalEntry/Error
        /// <summary>
        ///     Routes to a dynamically generated "Error" Page.
        /// </summary>
        /// <returns> A dynamic webpage which provides an Error Message. </returns>
        /// <example>
        ///     GET: JournalEntry/Error
        /// </example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // GET: JournalEntry/New
        /// <summary>
        ///     Routes to a dynamically generated "Journal Entry New" Page. 
        ///     Gathers information about a new Entry from a form 
        ///     that will be added to the database.
        /// </summary>
        /// <returns> A dynamic webpage which asks the user for new information regarding an Entry as part of a form. </returns>
        /// <example>
        ///     GET: JournalEntry/New
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///    Receives a POST request containing information about a new Entry, 
        ///    Conveys this information to the AddEntry Method, inorder
        ///    to add the specific entry to the database.
        ///    Redirects to the "Journal Entry List" page.
        /// </summary>
        /// <param name="journalEntry"> Journal Entry Object </param>
        /// <returns> 
        ///     A dynamic webpage which provides a new Entries's information.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     POST: JournalEntry/Create
        ///     {
        ///        "Location": "Outdoors",
        ///         "MoodBefore": "Happy, Restless",
        ///         "MoodAfter": "Happy, Relaxed",
        ///         "Thoughts": "Living life",
        ///         "SessionID": 9
        ///     }
        /// </example>
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

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: JournalEntry/Edit/5
        /// <summary>
        ///     Routes to a dynamically generated "Journal Entry Edit" Page. 
        ///     That asks the user for new information as part of a form.
        ///     Gathers information from the MeditationApplication database.
        /// </summary>
        /// <param name="id"> Entry ID </param>
        /// <returns> A dynamic webpage which provides the current information of an Entry. </returns>
        /// <example>
        ///     JournalEntry/Edit/5
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///     Receives a POST request containing information about an existing Entry in the database, 
        ///     with new values. Conveys this information to the UpdateEntry Method, 
        ///     and redirects to the "Journal Entry List" page.
        /// </summary>
        /// <param name="id"> Entry ID </param>
        /// <param name="journalEntry"> Journal Entry Object </param>
        /// <returns> A dynamic webpage which provides the current information of a Entry </returns>
        /// <example>
        ///     JournalEntry/Update/6
        ///     {
        ///         "JournalEntryID": 6,
        ///         "Location": "Outdoors",
        ///         "MoodBefore": "Happy, Restless",
        ///         "MoodAfter": "Happy, Relaxed",
        ///         "Thoughts": "Living life",
        ///         "SessionID": 9
        ///     }
        /// </example>
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
        /// <summary>
        ///     Routes to a dynamically generated "Journal Entry DeleteConfirmation" Page. 
        ///     Gathers information about a specific Entry that will be deleted from the database
        /// </summary>
        /// <param name="id"> Entry ID </param>
        /// <returns> A dynamic webpage which provides the current information of an Entry </returns>
        /// <example>
        ///     GET: JournalEntry/DeleteConfirmation/5
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///    Receives a POST request containing information about an existing Journal Entry in the database, 
        ///    Conveys this information to the DeleteEntry Method, inorder
        ///    to remove the specific Entry from the database.
        ///    Redirects to the "Journal Entry List" page.
        /// </summary>
        /// <param name="id"> Entry ID </param>
        /// <returns> A dynamic webpage which provides the current information of an Entry </returns>
        /// <example>
        ///     POST: JournalEntry/Delete/5
        /// </example>
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
