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
    public class MantraController : Controller
    {
        private static readonly HttpClient client;

        static MantraController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/MantraData/");
        }

        // GET: Mantra/List
        public ActionResult List()
        {
            // Objective: Communicate with mantra data api to retrieve a list of mantras
            // curl https://localhost:44316/api/MantraData/ListMantras

            string url = "ListMantras";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<MantraDto> Mantras = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;
            Debug.WriteLine("Number of mantras -> " + Mantras.Count());

            return View(Mantras);
        }

        // GET: Mantra/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with mantra data api to retrieve a list of mantras
            // curl https://localhost:44316/api/MantraData/FindMantra/{id}

            string url = "FindMantra/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The status code is " + response.StatusCode);

            MantraDto SelectedMantra = response.Content.ReadAsAsync<MantraDto>().Result;
            Debug.WriteLine("Name of selected mantra -> " + SelectedMantra.MantraName);

            // See a list of mantras that are in the same category

            return View(SelectedMantra);
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

            string url = "AddMantra";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(mantra);
            Debug.WriteLine("The json payload is -> " + jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Mantra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mantra/Edit/5
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

        // GET: Mantra/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mantra/Delete/5
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
