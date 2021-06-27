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
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: Category/List
        /// <summary>
        ///     Communicate with the Category Data Api to RETRIEVE a list of Categories
        /// </summary>
        /// <returns> Dynamically rendered view </returns>
        /// <example>
        ///     Category/List
        /// </example>
        public ActionResult List()
        {
            // Objective: Communicate with the Category Data Api to RETRIEVE a list of Categories
            // curl https://localhost:44316/api/CategoryData/ListCategories
            string url = "CategoryData/ListCategories";

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is " + response.StatusCode);

            IEnumerable<CategoryDto> Categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            // Debug.WriteLine("Number of Categories -> " + Categories.Count());

            return View(Categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with Category Data Api to RETRIEVE a Category
            // curl https://localhost:44316/api/CategoryData/FindCategory/{id}
            string url = "CategoryData/FindCategory/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is -> " + response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            // Debug.WriteLine("Data of Category -> " + SelectedCategory);

            return View(SelectedCategory);
        }

        // GET: Category/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Category/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            // Objective: Communicate with Category Data Api to add a new Category
            // curl -H "Content-Type:application/json" -d @category.json https://localhost:44316/api/CategoryData/AddCategory
            string url = "CategoryData/AddCategory";

            string jsonpayload = jss.Serialize(category);
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

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            // Objective: Communicate with Category Data Api to RETRIEVE a Category
            // curl https://localhost:44316/api/CategoryData/FindCategory/{id}
            string url = "CategoryData/FindCategory/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is -> " + response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            // Debug.WriteLine("Data of Category -> " + SelectedCategory);

            if(response.IsSuccessStatusCode)
            {
                return View(SelectedCategory);

            } else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category category)
        {
            // Objective: Communicate with Category Data Api to UPDATE a Category
            // curl -H "Content-Type:application/json" -d @category.json https://localhost:44316/api/CategoryData/UpdateCategory/{id}
            string url = "CategoryData/UpdateCategory/" + id;

            string jsonpayload = jss.Serialize(category);
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

        // GET: Category/DeleteConfirmation/5
        public ActionResult DeleteConfirmation(int id)
        {
            // Objective: Communicate with Category Data Api to RETRIEVE a Category
            // curl https://localhost:44316/api/CategoryData/FindCategory/{id}
            string url = "CategoryData/FindCategory/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is -> " + response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            // Debug.WriteLine("Data of Category -> " + SelectedCategory);

            if (response.IsSuccessStatusCode)
            {
                return View(SelectedCategory);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Objective: Communicate with Category Data Api to DELETE a Category
            // curl -d "" https://localhost:44316/api/CategoryData/DeleteCategory/{id}
            string url = "CategoryData/DeleteCategory/" + id;

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
