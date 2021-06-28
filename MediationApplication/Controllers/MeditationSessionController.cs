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

        // GET: MeditationSession/List
        /// <summary>
        ///     Routes to a dynamically generated "Meditation List" Page. 
        ///     Gathers information about all the sessions in the database.
        /// </summary>
        /// <returns> A dynamic webpage which displays a List of Sessions </returns>
        /// <example>
        ///     GET: MeditationSession/List
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a list of Sessions
            // curl https://localhost:44316/api/MeditationSessionData/ListSessions
            string url = "MeditationSessionData/ListSessions";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<MeditationSessionDto> MeditationSessions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;
            // Debug.WriteLine("Number of sessions -> " + MeditationSessions.Count());

            if (response.IsSuccessStatusCode)
            {
                return View(MeditationSessions);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: MeditationSession/Details/5
        /// <summary>
        ///     Routes to a dynamically generated "Meditation Session Details" Page.
        ///     Gathers information about a specific session from the database.
        /// </summary>
        /// <param name="id"> Session ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     A dynamic webpage which provides the current information of a Session and Entries
        ///     related to the session.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     GET: MeditationSession/Details/5
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult Details(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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

            if(response.IsSuccessStatusCode)
            {
                return View(ViewModel);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: MeditationSession/Error
        /// <summary>
        ///     Routes to a dynamically generated "Error" Page.
        /// </summary>
        /// <returns> A dynamic webpage which provides an Error Message. </returns>
        /// <example>
        ///     GET: MeditationSession/Error
        /// </example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // GET: MeditationSession/New
        /// <summary>
        ///     Routes to a dynamically generated "Meditation Session New" Page. 
        ///     Gathers information about a new Session from a form 
        ///     that will be added to the database.
        /// </summary>
        /// <returns> A dynamic webpage which asks the user for new information regarding a Session as part of a form. </returns>
        /// <example>
        ///     GET: MeditationSession/New
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            // Get Token Credentials
            GetApplicationCookie(); 

            // Objective: Communicate with Mantra Data Api to RETRIEVE a list of Mantras
            // curl https://localhost:44316/api/MantraData/ListMantras
            // GET: api/MantraData/ListMantras
            string url = "MantraData/ListMantras";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MantraDto> MantraOptions = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;

            if(response.IsSuccessStatusCode)
            {
                return View(MantraOptions);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: MeditationSession/Create
        /// <summary>
        ///    Receives a POST request containing information about a new Meditation Session, 
        ///    Conveys this information to the AddSession Method, inorder
        ///    to add the specific Session to the database.
        ///    Redirects to the "Session List" page.
        /// </summary>
        /// <param name="meditationSession"> Meditation Session Object </param>
        /// <returns> 
        ///     A dynamic webpage which provides a new Category's information.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     MeditationSession/Create
        ///     {
        ///         "SessionDate": "2021-06-01",
        ///         "SessionStartTime": "1:30:00",
        ///         "SessionEndTime": "2:30:00",
        ///         "SessionDuration": 1,
        ///         "MantraID": 6
        ///     }
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Create(MeditationSession meditationSession)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Debug.WriteLine("The date of new session -> " + meditationSession.SessionStartTime);
            // Debug.WriteLine("The start time -> " + meditationSession.SessionEndTime);

            // Objective: Communicate with Meditation Session Data Api to ADD a new Session
            // curl -H "Content-Type:application/json" -d @session.json https://localhost:44316/api/MeditationSessionData/AddSession
            string url = "MeditationSessionData/AddSession";

            string jsonpayload = jss.Serialize(meditationSession);
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

        // GET: MeditationSession/Edit/5
        /// <summary>
        ///     Routes to a dynamically generated "Meditation Edit" Page. 
        ///     That asks the user for new information as part of a form.
        ///     Gathers information from the MeditationApplication database.
        /// </summary>
        /// <param name="id"> Session ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Session. </returns>
        /// <example>
        ///     MeditationSession/Edit/5
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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

            if(response.IsSuccessStatusCode)
            {
                return View(ViewModel);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: MeditationSession/Update/5
        /// <summary>
        ///     Receives a POST request containing information about an existing Meditation Session in the database, 
        ///     with new values. Conveys this information to the UpdateSession Method, 
        ///     and redirects to the "Meditation Session List" page.
        /// </summary>
        /// <param name="id"> Session ID </param>
        /// <param name="meditationSession"> Meditation Session Object </param>
        /// <returns> A dynamic webpage which provides the current information of a Session </returns>
        /// <example>
        ///     MeditationSession/Update/5
        ///     {
        ///         "SessionID": 5,
        ///         "SessionDate": "2021-06-01",
        ///         "SessionStartTime": "1:30:00",
        ///         "SessionEndTime": "2:30:00",
        ///         "SessionDuration": 1,
        ///         "MantraID": 6
        ///     }
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, MeditationSession meditationSession)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with the Meditation Session Data Api to UPDATE a Session
            // curl -H "Content-Type:application/json" -d @animal.json https://localhost:44316/api/MeditationSession/UpdateSession/{id}
            string url = "MeditationSessionData/UpdateSession/" + id;

            string jsonpayload = jss.Serialize(meditationSession);
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

        // GET: MeditationSession/DeleteConfirmation/4
        /// <summary>
        ///     Routes to a dynamically generated "Meditation Session DeleteConfirmation" Page. 
        ///     Gathers information about a specific Session that will be deleted from the database
        /// </summary>
        /// <param name="id"> Session ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Session </returns>
        /// <example>
        ///     GET: MeditationSession/DeleteConfirmation/4
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirmation(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a Session
            // curl https://localhost:44316/api/MeditationSessionData/FindSession/{id}
            string url = "MeditationSessionData/FindSession/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MeditationSessionDto SelectedSession = response.Content.ReadAsAsync<MeditationSessionDto>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(SelectedSession);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: MeditationSession/Delete/4
        /// <summary>
        ///    Receives a POST request containing information about an existing Meditation Session in the database, 
        ///    Conveys this information to the DeleteSession Method, inorder
        ///    to remove the specific Session from the database.
        ///    Redirects to the "Meditation Session List" page.
        /// </summary>
        /// <param name="id"> Session ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Session </returns>
        /// <example>
        ///     POST: MeditationSession/Delete/4
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Objective: Communicate with Meditation Session Data Api to DELETE an Entry
            // curl -d "" https://localhost:44316/api/MantraData/DeleteSession/{id}
            string url = "MeditationSessionData/DeleteSession/" + id;

            HttpContent content = new StringContent("");
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
    }
}
