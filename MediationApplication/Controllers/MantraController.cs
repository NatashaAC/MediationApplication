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
        /// <summary>
        ///     Routes to a dynamically generated "Mantra List" Page.
        ///     Gathers information about all the mantras in the database.
        /// </summary>
        /// <param name="searchkey"> A string that represents a mantra's name </param>
        /// <returns> A dynamic webpage which displays a List of Mantras </returns>
        /// <example>
        ///     GET: Mantra/List
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///     Routes to a dynamically generated "Mantra Details" Page.
        ///     Gathers information about a specific Mantra from the database
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     A dynamic webpage which provides the current information of a Mantra.
        ///     The Categories and Sessions related to the Mantra.
        ///     The Categories not assigned to the Mantra.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     GET: Mantra/Details/5
        /// </example>
        [HttpGet]
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


            // Objective: Communicate with Meditation Session Data Api to RETRIEVE a List Sessions related to Mantra Id
            // curl https://localhost:44316/api/MeditationSessionData/ListSessionsForMantras/{id}
            // GET: api/MeditationSessionData/ListSessionsForMantras/2
            url = "MeditationSessionData/ListSessionsForMantras/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<MeditationSessionDto> RelatedSessions = response.Content.ReadAsAsync<IEnumerable<MeditationSessionDto>>().Result;
            ViewModel.RelatedSessions = RelatedSessions;

            // Objective: Communicate with Category Data Api to RETRIEVE a list of Categories related to specific Mantra
            // curl https://localhost:44316/api/CategoryData/ListCategoriesForMantra/{id}
            url = "CategoryData/ListCategoriesForMantra/" + id;

            response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> RelatedCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.RelatedCategories = RelatedCategories;

            // Objective: Communicate with Category Data Api to RETRIEVE a list of Categories not assigned to specific Mantra
            // curl https://localhost:44316/api/CategoryData/ListCategoriesNotAssignedToMantra/{id}
            url = "CategoryData/ListCategoriesNotAssignedToMantra/" + id;

            response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> UnassignedCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.UnassignedCategories = UnassignedCategories;

            if (response.IsSuccessStatusCode)
            {
                return View(ViewModel);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Mantra/Assign/{mantraid}/{categoryid}
        /// <summary>
        ///    Receives a POST request containing information about an existing Mantra in the database, 
        ///    Conveys this information to the AssignMantraToCategory Method, inorder
        ///    to assign a mantra to a category.
        ///    Redirects to the "Mantra Details" page.
        /// <param name="id"> Mantra ID </param>
        /// <param name="CategoryID"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     A dynamic webpage which provides the current information of a Mantra.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     POST: Mantra/Assign/4/1
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Assign(int id, int CategoryID)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Debug.WriteLine("Assigning mantra to -> " + id + "to category -> " + CategoryID);

            // Objective: Communicate with Mantra Data Api to ASSIGN Mantra to Category
            // curl -d "" -v https://localhost:44316/api/MantraData/AssignMantraToCategory/13/3
            string url = "MantraData/AssignMantraToCategory/" + id + "/" + CategoryID;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Mantra/UnAssign/{id}?CategoryID=={categoryid}
        /// <summary>
        ///    Receives a POST request containing information about an existing Mantra in the database, 
        ///    Conveys this information to the UnAssignMantraToCategory Method, inorder
        ///    to unassign a mantra to a category.
        ///    Redirects to the "Mantra Details" page.
        /// <param name="id"> Mantra ID </param>
        /// <param name="CategoryID"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     A dynamic webpage which provides the current information of a Mantra.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     POST: Mantra/UnAssign/4/1
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult UnAssign(int id, int CategoryID)
        {
            // Get Token Credentials
            GetApplicationCookie();

            // Debug.WriteLine("UnAssigning mantra to -> " + id + "to category -> " + CategoryID);

            // Objective: Communicate with Mantra Data Api to UNASSIGN Mantra to Category
            // curl -d "" -v https://localhost:44316/api/MantraData/UnAssignMantraToCategory/13/3
            string url = "MantraData/UnAssignMantraToCategory/" + id + "/" + CategoryID;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Mantra/Error
        /// <summary>
        ///     Routes to a dynamically generated "Error" Page.
        /// </summary>
        /// <returns> A dynamic webpage which provides an Error Message. </returns>
        /// <example>
        ///     GET: Mantra/Error
        /// </example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // GET: Mantra/New
        /// <summary>
        ///     Routes to a dynamically generated "Mantra New" Page. 
        ///     Gathers information about a new Mantra from a form 
        ///     that will be added to the database.
        /// </summary>
        /// <returns> A dynamic webpage which asks the user for new information regarding a Mantra as part of a form. </returns>
        /// <example>
        ///     GET: Mantra/New
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            // Get Token Credentials
            GetApplicationCookie();

            return View();
        }

        // POST: Mantra/Create
        /// <summary>
        ///    Receives a POST request containing information about a new Mantra, 
        ///    Conveys this information to the AddMantra Method, inorder
        ///    to add the specific Mantra to the database.
        ///    Redirects to the "Mantra List" page.
        /// </summary>
        /// <param name="mantra"> Mantra Object </param>
        /// <returns> 
        ///     A dynamic webpage which provides a new Category's information.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     Mantra/Create
        ///     {
        ///         "MantraName": "Stay calm like a pond"
        ///     }
        /// </example>
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
        /// <summary>
        ///     Routes to a dynamically generated "Mantra Edit" Page. 
        ///     That asks the user for new information as part of a form.
        ///     Gathers information from the MeditationApplication database.
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Mantra. </returns>
        /// <example>
        ///     Mantra/Edit/5
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///     Receives a POST request containing information about an existing Mantra in the database, 
        ///     with new values. Conveys this information to the UpdateMantra Method, 
        ///     and redirects to the "Mantra List" page.
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <param name="mantra"> Mantra Object </param>
        /// <returns> A dynamic webpage which provides the current information of a Mantra </returns>
        /// <example>
        ///     Mantra/Update/11
        ///     {
        ///         "MantraID": 11,
        ///         "MantraName": "Stay calm like a pond"
        ///     }
        /// </example>
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
        /// <summary>
        ///     Routes to a dynamically generated "Mantra DeleteConfirmation" Page. 
        ///     Gathers information about a specific Mantra that will be deleted from the database
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Mantra </returns>
        /// <example>
        ///     GET: Mantra/DeleteConfirmation/5
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///    Receives a POST request containing information about an existing Mantra in the database, 
        ///    Conveys this information to the DeleteMantra Method, inorder
        ///    to remove the specific Mantra from the database.
        ///    Redirects to the "Mantra List" page.
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Mantra </returns>
        /// <example>
        ///     POST: Mantra/Delete/5
        /// </example>
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
