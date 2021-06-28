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
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
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

        // GET: Category/List
        /// <summary>
        ///     Routes to a dynamically generated "Category List" Page. 
        ///     Gathers information about all the categories in the database.
        /// </summary>
        /// <returns> A dynamic webpage which displays a List of Categories </returns>
        /// <example>
        ///     GET: Category/List
        /// </example>
        [HttpGet]
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
        /// <summary>
        ///     Routes to a dynamically generated "Category Details" Page.
        ///     Gathers information about a specific Category from the database
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns> 
        ///     A dynamic webpage which provides the current information of a Category and
        ///     a list of Mantras related to the category
        /// </returns>
        /// <example>
        ///     GET: Category/Details/5
        /// </example>
        [HttpGet]
        public ActionResult Details(int id)
        {
            DetailsCategory ViewModel = new DetailsCategory();

            // Objective: Communicate with Category Data Api to RETRIEVE a Category
            // curl https://localhost:44316/api/CategoryData/FindCategory/{id}
            string url = "CategoryData/FindCategory/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debug.WriteLine("The status code is -> " + response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            // Debug.WriteLine("Data of Category -> " + SelectedCategory);
            ViewModel.SelectedCategory = SelectedCategory;

            // Objective: Communicate with Mantra Data Api to RETRIEVE a list of Mantras for Specific Category
            // curl https://localhost:44316/api/MantraData/ListMantrasForCategory/{id}
            url = "MantraData/ListMantrasForCategory/" + id;

            response = client.GetAsync(url).Result;

            IEnumerable<MantraDto> RelatedMantras = response.Content.ReadAsAsync<IEnumerable<MantraDto>>().Result;
            ViewModel.RelatedMantras = RelatedMantras;

            return View(ViewModel);
        }

        // GET: Category/Error
        /// <summary>
        ///     Routes to a dynamically generated "Error" Page.
        /// </summary>
        /// <returns> A dynamic webpage which provides an Error Message. </returns>
        /// <example>
        ///     GET: Category/Error
        /// </example>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // GET: Category/New
        /// <summary>
        ///     Routes to a dynamically generated "Category New" Page. 
        ///     Gathers information about a new Category from a form 
        ///     that will be added to the database.
        /// </summary>
        /// <returns> A dynamic webpage which asks the user for new information regarding a Category as part of a form. </returns>
        /// <example>
        ///     GET: Category/New
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            // Get Token Credentials
            GetApplicationCookie();

            return View();
        }

        // POST: Category/Create
        /// <summary>
        ///    Receives a POST request containing information about a new Category, 
        ///    Conveys this information to the AddCategory Method, inorder
        ///    to add the specific Category to the database.
        ///    Redirects to the "Category List" page.
        /// </summary>
        /// <param name="category"> Category Object </param>
        /// <returns> 
        ///     A dynamic webpage which provides a new Category's information.
        ///     or
        ///     A dynamic webpage which provides an Error Message.
        /// </returns>
        /// <example>
        ///     "CategoryName": "General",
        ///     "CategoryDescription": "Just general good to wind down",
        ///     "CategoryColour": "#8B80F9"
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Create(Category category)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
        /// <summary>
        ///     Routes to a dynamically generated "Category Edit" Page. 
        ///     That asks the user for new information as part of a form.
        ///     Gathers information from the MeditationApplication database.
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Category. </returns>
        /// <example>
        ///     Category/Edit/1
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
        /// <summary>
        ///     Receives a POST request containing information about an existing Category in the database, 
        ///     with new values. Conveys this information to the UpdateCategory Method, 
        ///     and redirects to the "Category List" page.
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <param name="category"> Category Object </param>
        /// <returns> A dynamic webpage which provides the current information of a Category </returns>
        /// <example>
        ///     Category/Update/5
        ///     {
        ///         "CategoryID": 5,
        ///         "CategoryName": "General Wellness",
        ///         "CategoryDescription": "Just general good to wind down",
        ///         "CategoryColour": "#8B80F9"
        ///     }
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Category category)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
        /// <summary>
        ///     Routes to a dynamically generated "Category DeleteConfirmation" Page. 
        ///     Gathers information about a specific Category that will be deleted from the database
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Category. </returns>
        /// <example>
        ///     Category/DeleteConfirmation/6
        /// </example>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirmation(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
        /// <summary>
        ///    Receives a POST request containing information about an existing Category in the database, 
        ///    Conveys this information to the DeleteCategory Method, inorder
        ///    to remove the specific Category from the database.
        ///    Redirects to the "Category List" page.
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns> A dynamic webpage which provides the current information of a Category. </returns>
        /// <example>
        ///     Category/Delete/6
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Get Token Credentials
            GetApplicationCookie();

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
