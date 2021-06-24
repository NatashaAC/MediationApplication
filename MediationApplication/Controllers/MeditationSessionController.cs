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
    public class MeditationSessionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MeditationSessionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: MeditationSession/List
        public ActionResult List()
        {
            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a list of Sessions
            // curl https://localhost:44316/api/MeditationSessionData/ListSessions

            string url = "MeditationSessionData/ListSessions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<MeditationSessionDto> MeditationSessions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;
            // Debug.WriteLine("Number of sessions -> " + MeditationSessions.Count());

            return View(MeditationSessions);
        }

        // GET: MeditationSession/Details/5
        public ActionResult Details(int id)
        {
            DetailsSession ViewModel = new DetailsSession();

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a Session
            // curl https://localhost:44316/api/MeditationSessionData/FindSession/{id}
            string url = "MeditationSessionData/FindSession/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is " + response.StatusCode);

            MeditationSessionDto SelectedSession = response.Content.ReadAsAsync<MeditationSessionDto>().Result;
            // Debug.WriteLine("Date of Session -> " + SelectedSession.SessionDate);
            ViewModel.SelectedSession = SelectedSession;

            // Objective: Communicate with Journal Entry Data Api to RETRIEVE a Journal Entry related to a Session Id
            // curl https://localhost:44316/api/JournalEntryData/FindEntryForSession/{id}
            // GET: api/MeditationSessionData/ListSessionsForMantras/2
            url = "JournalEntryData/FindEntryForSession/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<JournalEntryDto> RelatedEntry = response.Content.ReadAsAsync<IEnumerable<JournalEntryDto>>().Result;

            ViewModel.RelatedEntry = RelatedEntry;

            return View(ViewModel);
        }

        // GET: MeditationSession/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: MeditationSession/New
        public ActionResult New()
        {
            // Objective: Communicate with Mantra Data Api to RETRIEVE a list of Mantras
            // curl https://localhost:44316/api/MantraData/ListMantras
            // GET: api/MantraData/ListMantras
            string url = "MantraData/ListMantras";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MantraDto> MantraOptions = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;

            return View(MantraOptions);
        }

        // POST: MeditationSession/Create
        [HttpPost]
        public ActionResult Create(MeditationSession meditationSession)
        {
            Debug.WriteLine("The date of new session -> " + meditationSession.SessionStartTime);
            Debug.WriteLine("The start time -> " + meditationSession.SessionEndTime);

            // Objective: Communicate with Meditation Session Data Api to ADD a new Session
            // curl -H "Content-Type:application/json" -d @session.json https://localhost:44316/api/MeditationSessionData/AddSession

            string url = "MeditationSessionData/AddSession";

            string jsonpayload = jss.Serialize(meditationSession);
            Debug.WriteLine("The json payload is -> " + jsonpayload);

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

        // GET: MeditationSession/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateSession ViewModel = new UpdateSession();

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a Session
            // curl https://localhost:44316/api/MeditationSessionData/FindSession/{id}
            string url = "MeditationSessionData/FindSession/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MeditationSessionDto SelectedSession = response.Content.ReadAsAsync<MeditationSessionDto>().Result;
            ViewModel.SelectedSession = SelectedSession;

            // Objective: Communicate with the Mantra Data Api to RETRIEVE a list of Mantras
            // curl https://localhost:44316/api/MantraData/ListMantras
            // GET: api/MantraData/ListMantras
            url = "MantraData/ListMantras";

            response = client.GetAsync(url).Result;

            IEnumerable<MantraDto> MantraOptions = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;
            ViewModel.MantraOptions = MantraOptions;

            return View(ViewModel);
        }

        // POST: MeditationSession/Update/5
        [HttpPost]
        public ActionResult Update(int id, MeditationSession meditationSession)
        {
            // Objective: Communicate with the Meditation Session Data Api to UPDATE a Session
            // curl -H "Content-Type:application/json" -d @animal.json https://localhost:44316/api/MeditationSession/UpdateSession/{id}
            string url = "MeditationSessionData/UpdateSession/" + id;

            string jsonpayload = jss.Serialize(meditationSession);
            Debug.WriteLine("The json payload is -> " + jsonpayload);

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

        // GET: MeditationSession/DeleteConfirmation/10
        public ActionResult DeleteConfirmation(int id)
        {
            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a Session
            // curl https://localhost:44316/api/MeditationSessionData/FindSession/{id}
            string url = "MeditationSessionData/FindSession/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MeditationSessionDto SelectedSession = response.Content.ReadAsAsync<MeditationSessionDto>().Result;

            return View(SelectedSession);
        }

        // POST: MeditationSession/Delete/10
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Objective: Communicate with Meditation Session Data Api to DELETE an Entry
            // curl -d "" https://localhost:44316/api/MantraData/DeleteEntry/{id}
            string url = "MeditationSessionData/DeleteSession/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
