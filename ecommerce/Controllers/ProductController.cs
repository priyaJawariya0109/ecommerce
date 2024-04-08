using ecommerce.ecoomerceAccessLayer.DataLayer;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace ecommerce.Controllers
{


    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ecommerceRepository _ecommerceRepository;
        public ProductController(ecommerceRepository ecommerceRepository, ILogger<ProductController> logger)
        {
            _ecommerceRepository = ecommerceRepository;
            _logger = logger;

        }

        public ActionResult Details()
        {
            //list of category
            var categoryActiveList=_ecommerceRepository.ActiveCategories();
            ViewBag.categories = new SelectList(categoryActiveList, "CId", "Name");

            return View();
        }


        public ActionResult List()
        {

            return View();
        }



        [HttpPost]
        public IActionResult DetailsRequest(ProductModel product)
        {
            try
            {
            
                if (_ecommerceRepository.AddProductDetails(product))
                {
                    ViewBag.Message = "true";
                    TempData["Message"] = "Category details saved successfully!";
                }



                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                TempData["Message"] = "An error occurred: " + ex.Message;


                return RedirectToAction("details");
            }

        }






    }
}
