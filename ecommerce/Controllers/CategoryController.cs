using ecommerce.ecoomerceAccessLayer.DataLayer;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Buffers;

namespace ecommerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IecommerceRepository _ecommerceRepository;
        public CategoryController(IecommerceRepository ecommerceRepository, ILogger<CategoryController> logger)
        {
            _ecommerceRepository = ecommerceRepository;
            _logger = logger;

        }

        //List page
        public ActionResult List()
        {
            return View();
        }
        //List page
        [HttpPost]
        public JsonResult CategoryList()
        {
            try
            {
                var req = Request.Form;
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Convert.ToInt32(Request.Form["start"]); // Retrieve the 'start' parameter
                var pageSize = Convert.ToInt32(Request.Form["length"]); // 
                int pageIndex = (start / pageSize) + 1;
                var searchValue = Request.Form["search[value]"].FirstOrDefault(); // Retrieve the search keyword

                var categories = _ecommerceRepository.GetPaginatedCategory(pageIndex, pageSize, searchValue); // Convert to list
                var totalRecord = _ecommerceRepository.GetTotalCategoryCount(searchValue);
                //var data = Json(categories);


                var jdata = new
                {
                    draw = Request.Form["draw"],
                    recordsTotal = totalRecord,
                    recordsFiltered = totalRecord, // This might need to be adjusted if you support filtering
                    data = categories
                };
                return new JsonResult(jdata);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while processing CategoryList action.");
                //return StatusCode(500, "An error occurred while processing your request."); // Handle the exception
                return Json(ex);
            }
        }

        //Add or Edit 
        public IActionResult Details(int Id)
        {
            if (Id == 0)
            {
                ViewData["Title"] = "Add Category";
            }
            else
            {
                ViewData["Title"] = "Edit Category";
            }
            var category = _ecommerceRepository.GetCategoryById(Id);
            // Assuming "details" is the name of the view and "category" is the model to be passed to the view
            return View("Details", category);
        }
     
    
        [HttpPost]
      /*  public IActionResult DetailsRequest(CategoryModel category)
        {
            try
            {

                if (_ecommerceRepository.AddCategoryDetails(category) >0 )
                {
                    ViewBag.Message = "true";
                    TempData["Message"] = "Category details saved successfully!";


                }
                else
                {

                    TempData["Message"] = "Failed to save category details.";
                }
                if (category.CId != 0)
                {
                    return RedirectToAction("details", new { id = category.CId });

                }
                else
                {
                    return RedirectToAction("details");
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                TempData["Message"] = "An error occurred: " + ex.Message;


                return RedirectToAction("details");
            }

        }
       */ public IActionResult DetailsRequest(CategoryModel category)
        {
            try
            {
                int result = _ecommerceRepository.AddCategoryDetails(category);

                if (result > 0)
                {
                    ViewBag.Message = "true";
                    TempData["Message"] = "Category details saved successfully!";

                    if (category.CId != 0)
                    {
                        return RedirectToAction("details", new { id = category.CId });
                    }
                    else
                    {
                        return RedirectToAction("details");
                    }
                }
                else if (result == -2) // Check for custom error code indicating duplicate category name
                {
                    TempData["error"] = "Category with this name already exists.";
                    return RedirectToAction("details");
                }
                else
                {
                    TempData["error"] = "Failed to save category details.";
                    return RedirectToAction("details");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                TempData["error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("details");
            }
        }

        //Delete
        public JsonResult Delete(int Id)
        {


            /*            customerrepository customerrepository = new customerrepository();
            */
            try
            {
                if (_ecommerceRepository.DeleteCategory(Id) > 0)
                {
                    return Json(new { success = true, message = "category deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "category not found or could not be deleted" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "error deleting category: " + ex.Message });
            }
        }

       
    }
}
