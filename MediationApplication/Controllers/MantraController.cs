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

        // GET: Mantra/List
        public ActionResult List(string searchkey)
        {
            // Objective: Communicate with mantra data api to RETRIEVE a list of mantras
            // curl https://localhost:44316/api/MantraData/ListMantras
            string url = "MantraData/ListMantras";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<MantraDto> Mantras = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;
            // Debug.WriteLine("Number of mantras -> " + Mantras.Count());

            if(response.IsSuccessStatusCode)
            {
                return View(searchkey == null ? Mantras : Mantras.Where(mName => mName.MantraName.Contains(searchkey)).ToList());

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Mantra/Details/5
        public ActionResult Details(int id)
        {
            DetailsMantra ViewModel = new DetailsMantra();

            // Objective: Communicate with mantra data api to RETRIEVE a list of mantras
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
            
            if(response.IsSuccessStatusCode)
            {
                return View(ViewModel);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: MeditationSession/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Mantra/New
        [Authorize]
        public ActionResult New()
        {
            // Get Token Credentials
            GetApplicationCookie();

            return View();
        }

        // POST: Mantra/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Mantra mantra)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Debug.WriteLine("The name of the mantra -> " + mantra.MantraName);

            // Objective: Communicate with Mantra Data Api to add a new Mantra
            // curl -H "Content-Type:application/json" -d @mantra.json https://localhost:44316/api/MantraData/AddMantra
            string url = "MantraData/AddMantra";

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

        // GET: Mantra/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Mantra Data Api to RETRIEVE a Mantra
            // curl https://localhost:44316/api/MantraData/FindMantra/{id}
            string url = "MantraData/FindMantra/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            // Debug.WriteLine("Mantra -> " + SelectedMantra.MantraName);

            if(response.IsSuccessStatusCode)
            {
                return View(SelectedMantra);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Mantra/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Mantra mantra)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
        [Authorize]
        public ActionResult DeleteConfirmation(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Mantra Data Api to DELETE a Mantra
            // curl https://localhost:44316/api/MantraData/FindMantra/{id}
            string url = "MantraData/FindMantra/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            // Debug.WriteLine("Mantra name -> " + SelectedMantra.MantraName);

            if(response.IsSuccessStatusCode)
            {
                return View(SelectedMantra);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Mantra/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
