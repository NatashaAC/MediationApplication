using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MediationApplication.Models;

namespace MediationApplication.Controllers
{
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///     Returns a list of data in the categories table within the database
        /// </summary>
        /// <returns> List of Categories </returns>
        /// <example>
        ///     GET: api/CategoryData/ListCategories -> Category Object, Category Object, etc...
        /// </example>
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategories()
        {
            List<Category> Categories = db.Categories.ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(Category => CategoryDtos.Add(new CategoryDto()
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName,
                CategoryDescription = Category.CategoryDescription,
                CategoryColour = Category.CategoryColour
            }));

            return CategoryDtos;
        }

        /// <summary>
        ///     Returns a list of Categories for a specific Mantra
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns> List of Categories related to a specific mantra id </returns>
        /// <example>
        ///     GET: api/CategoryData/ListCategoriesForMantra/6 -> Category Object, Category Object, etc...
        /// </example>
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategoriesForMantra(int id)
        {
            List<Category> Categories = db.Categories.Where(
                c => c.Mantras.Any(
                m => m.MantraID == id
                )).ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(Category => CategoryDtos.Add(new CategoryDto()
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName,
                CategoryDescription = Category.CategoryDescription,
                CategoryColour = Category.CategoryColour
            }));

            return CategoryDtos;
        }

        /// <summary>
        ///     Returns a list of Categories
        /// </summary>
        /// <param name="id"> Mantra ID </param>
        /// <returns> List of Categories not assigned to a specific mantra id </returns>
        /// <example>
        ///     GET: api/CategoryData/ListCategoriesNotAssignedToMantra/6 -> Category Object, Category Object, etc...
        /// </example>
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategoriesNotAssignedToMantra(int id)
        {
            List<Category> Categories = db.Categories.Where(
                c => !c.Mantras.Any(
                m => m.MantraID == id
                )).ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(Category => CategoryDtos.Add(new CategoryDto()
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName,
                CategoryDescription = Category.CategoryDescription,
                CategoryColour = Category.CategoryColour
            }));

            return CategoryDtos;
        }

        /// <summary>
        ///     Returns the data of a specific Category based on the Category ID
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     CONTENT: A Category related to Category ID
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     GET: api/CategoryData/FindCategory/3 -> Soul
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName,
                CategoryDescription = Category.CategoryDescription,
                CategoryColour = Category.CategoryColour
            };

            if (Category == null)
            {
                return NotFound();
            }

            return Ok(CategoryDto);
        }

        /// <summary>
        ///     Updates a Category's information in the categories table within the database
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <param name="category"> json Form data of a Category </param>
        /// <returns>
        ///     HEADER: 200 (Success, No content)
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/CategoryData/UpdateCategory/2
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Adds a new category to the categories table within the database
        /// </summary>
        /// <param name="category"> json Form Data of a category </param>
        /// <returns>
        ///     HEADER: 201 (Created)
        ///     CONTENT: Category data
        ///     or
        ///     HEADER: 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        ///     POST: api/CategoryData/AddCategory
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        /// <summary>
        ///     Deletes a category from the categories table within the database
        /// </summary>
        /// <param name="id"> Category ID </param>
        /// <returns>
        ///     HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///     POST: api/CategoryData/DeleteCategory/4
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}