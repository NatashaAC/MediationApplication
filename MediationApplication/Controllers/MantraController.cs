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
    public class MantraController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MantraController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Mantra/List
        public ActionResult List()
        {
            // Objective: Communicate with mantra data api to retrieve a list of mantras
            // curl https://localhost:44316/api/MantraData/ListMantras

            string url = "MantraData/ListMantras";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<MantraDto> Mantras = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;
            // Debug.WriteLine("Number of mantras -> " + Mantras.Count());

            return View(Mantras);
        }

        // GET: Mantra/Details/5
        public ActionResult Details(int id)
        {
            DetailsMantra ViewModel = new DetailsMantra();

            // Objective: Communicate with mantra data api to retrieve a list of mantras
            // curl https://localhost:44316/api/MantraData/FindMantra/{id}

            string url = "MantraData/FindMantra/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            // Debug.WriteLine("Name of selected mantra -> " + SelectedMantra.MantraName);
            ViewModel.SelectedMantra = SelectedMantra;

            // See a list of mantras that are in the same category

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a List Sessions related to Mantra Id
            // curl https://localhost:44316/api/MeditationSessionData/ListSessionsForMantras/{id}
            // GET: api/MeditationSessionData/ListSessionsForMantras/2
            url = "MeditationSessionData/ListSessionsForMantras/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<MeditationSessionDto> RelatedSessions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;

            ViewModel.RelatedSessions = RelatedSessions;
            

            return View(ViewModel);
        }

        // GET: MeditationSession/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Mantra/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Mantra/Create
        [HttpPost]
        public ActionResult Create(Mantra mantra)
        {
            Debug.WriteLine("The name of the mantra -> " + mantra.MantraName);

            // Objective: Communicate with mantra data api to add a new mantra
            // curl -H "Content-Type:application/json" -d @mantra.json https://localhost:44316/api/MantraData/AddMantra

            string url = "MantraData/AddMantra";

            string jsonpayload = jss.Serialize(mantra);
            // Debug.WriteLine("The json payload is -> " + jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Mantra/Edit/5
        public ActionResult Edit(int id)
        {

            // Objective: Communicate with Mantra Data Api to RETRIEVE a Mantra
            // curl https://localhost:44316/api/MantraData/FindMantra/{id}
            string url = "MantraData/FindMantra/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            // Debug.WriteLine("Mantra -> " + SelectedMantra.MantraName);

            return View(SelectedMantra);
        }

        // POST: Mantra/Update/5
        [HttpPost]
        public ActionResult Update(int id, Mantra mantra)
        {
            // Objective: Communicate with Mantra Data Api to UPDATE a Mantra
            // curl -H "Content-Type:application/json" -d @mantra.json https://localhost:44316/api/MantraData/UpdateMantra/{id}
            string url = "MantraData/UpdateMantra/" + id;

            string jsonpayload = jss.Serialize(mantra);
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

        // GET: Mantra/DeleteConfirmation/5
        public ActionResult DeleteConfirmation(int id)
        {
            // Objective: Communicate with Mantra Data Api to DELETE a Mantra
            // curl -d "" https://localhost:44316/api/MantraData/FindMantra/{id}
            string url = "MantraData/FindMantra/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            // Debug.WriteLine("Mantra name -> " + SelectedMantra.MantraName);

            return View(SelectedMantra);
        }

        // POST: Mantra/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Objective: Communicate with Mantra Data Api to DELETE a Mantra
            // curl -d "" https://localhost:44316/api/MantraData/DeleteMantra/{id}
            string url = "MantraData/DeleteMantra/" + id;

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
