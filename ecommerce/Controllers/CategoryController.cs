using ecommerce.ecoomerceAccessLayer.DataLayer;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace ecommerce.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IecommerceRepository _ecommerceRepository;
        public CategoryController(IecommerceRepository ecommerceRepository,ILogger<CategoryController> logger)
        {
            _ecommerceRepository = ecommerceRepository;
            _logger = logger;

        }

        public ActionResult List()
        {
            /*  var categories = _ecommerceRepository.GetPaginatedCategory(1, 10); // Convert to list
           return   Json(categories);*/
            return View();
        }
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
               var categories = _ecommerceRepository.GetPaginatedCategory(pageIndex, pageSize); // Convert to list
                // var totalRecords = _ecommerceRepository.GetTotalCategoryCount();

           

                var cjson = Json(categories);

                var totalRecord = categories.Count();
                var JsonData = new
                {
                    //data = categories,
                    draw = draw,
                    recordsTotal = totalRecord,
                   data= cjson,
                   recordsFiltered= totalRecord
                
                };

             //   _logger.LogInformation("CategoryList method called with page={page} and pageSize={pageSize}.", page, pageSize);
                return new JsonResult(JsonData); // Return model as JSON
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while processing CategoryList action.");
                //return StatusCode(500, "An error occurred while processing your request."); // Handle the exception
                return Json(ex);
            }
        }
        //public IActionResult details(int Id)
        //{
        //    var category = _ecommerceRepository.GetCategoryById(Id);

        //    return View(category);
        //}
        //public IActionResult CategoryDetails()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult detailsRequest(CategoryModel category)
        //{
        //    try
        //    {

        //        if (_ecommerceRepository.AddCategoryDetails(category))
        //        {
        //            ViewBag.Message = "true";
        //            TempData["Message"] = "Category details saved successfully!";

                 

        //        }
        //        else
        //        {

        //        TempData["Message"] = "Failed to save category details.";
        //        }
        //        if (category.CId != 0)
        //        {
        //            return RedirectToAction("details", new { id = category.CId });

        //        }
        //        else
        //        {
        //          return RedirectToAction("details");
        //        }
          
              
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        TempData["Message"] = "An error occurred: " + ex.Message;


        //        return RedirectToAction("details");
        //    }

        //}


        //public JsonResult Delete(int Id)
        //{


        //    /*            CustomerRepository customerrepository = new CustomerRepository();
        //    */
        //    try
        //    {
        //        if (_ecommerceRepository.DeleteCategory(Id))
        //        {
        //            return Json(new { success = true, message = "Category deleted successfully" });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Category not found or could not be deleted" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "Error deleting Category: " + ex.Message });
        //    }
        //}


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
