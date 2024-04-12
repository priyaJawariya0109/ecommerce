using ecommerce.ecoomerceAccessLayer.DataLayer;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using System.Reflection;
namespace ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IecommerceRepository _ecommerceRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IecommerceRepository ecommerceRepository, ILogger<ProductController> logger, IWebHostEnvironment environment )
        {
            _ecommerceRepository = ecommerceRepository;
            _logger = logger;
            this._environment = environment;
        }


        public ActionResult List()
        {
            return View();
        }
        //List page
        [HttpPost]
        public JsonResult ProductList()
        {
            try
            {
                var req = Request.Form;
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Convert.ToInt32(Request.Form["start"]); // Retrieve the 'start' parameter
                var pageSize = Convert.ToInt32(Request.Form["length"]); // 
                int pageIndex = (start / pageSize) + 1;
                var searchValue = Request.Form["search[value]"].FirstOrDefault(); // Retrieve the search keyword

                var products = _ecommerceRepository.GetPaginatedProduct(pageIndex, pageSize, searchValue); // Convert to list
                var totalRecord = _ecommerceRepository.GetTotalProductCount(searchValue);
                //var data = Json(products);


                var jdata = new
                {
                    draw = Request.Form["draw"],
                    recordsTotal = totalRecord,
                    recordsFiltered = totalRecord, // This might need to be adjusted if you support filtering
                    data = products
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


        public ActionResult Details(int Id)
        {
            if (Id == 0)
            {
                ViewData["Title"] = "Add Product";
            }
            else
            {
                ViewData["Title"] = "Edit Product";
            }
            //list of category
            var categoryActiveList = _ecommerceRepository.ActiveCategories();

            ViewBag.categories = new SelectList(categoryActiveList, "CId", "Name");
            var product = _ecommerceRepository.GetProductById(Id);

            return View("Details", product);

        }

        // Generate a short random string
        string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }




        string CreateUniqueShortFilename(string originalFilename)
        {
            string currentDate = DateTime.Now.ToString("yyMMddHHmmss");
            string randomString = GetRandomString(4); // Adjust the length of the random string as needed
            string fileExtension = Path.GetExtension(originalFilename);
            return $"{currentDate}_{randomString}{fileExtension}";
        }

        [HttpPost]
        public IActionResult DetailsRequest(ProductModel product)
        {
            try
            {

                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (product.PImage != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "Content","Image");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                   // uniqueFileName = Guid.NewGuid().ToString() + "_" + product.PImage.FileName;
                    uniqueFileName = CreateUniqueShortFilename(product.PImage.FileName); 
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    product.PImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }


            /*    string fileName = null;
                if (product.PImage == null || product.PImage.Length == 0)
                {
                    ModelState.AddModelError("PImage", "Please select a file.");
                    return View(product);
                }

                 fileName = Path.GetFileName(product.PImage.FileName);
                var filePath = Path.Combine("Content", "Image", fileName);
                var fullPath = Path.Combine(_environment.WebRootPath, filePath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    product.PImage.CopyTo(stream);
                }*/

                ProductViewModel newProduct = new ProductViewModel
                {
                    CId=product.CId,
                    PId=product.PId,
                    PName = product.PName,
                    PDescription = product.PDescription,
                    PCostPrice = product.PCostPrice,
                    PsellPrice = product.PsellPrice,
                    PSavings = product.PSavings,
                    StockQt = product.StockQt,
                    RemainQt = product.RemainQt,
                  


                    PhotoPath = uniqueFileName
                };
          

                if (_ecommerceRepository.AddProductDetails(newProduct) > 0)
                {
                    ViewBag.Message = "true";
                    TempData["Message"] = "Category details saved successfully!";

                    if (product.PId > 0)
                    {
                        //edit
                        return RedirectToAction("Details", new { id = product.PId });

                    }
                    else
                    {
                        //add
                        return RedirectToAction("Details");

                    }
                }

                return RedirectToAction("Details");

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                TempData["Message"] = "An error occurred: " + ex.Message;


                return RedirectToAction("Details");
            }

        }

        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }



        //Delete
        public JsonResult Delete(int Id)
        {


            /*            customerrepository customerrepository = new customerrepository();
            */
            try
            {
                if (_ecommerceRepository.DeleteProduct(Id) > 0)
                {
                    return Json(new { success = true, message = "Product deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Product not found or could not be deleted" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "error deleting Product: " + ex.Message });
            }
        }




        //list of products for catalog

        public ActionResult Catalog(int page = 1, int pageSize = 6, string searchValue = null)
        {
            try
            {
                var categoryActiveList = _ecommerceRepository.ActiveCategories();
                var products = _ecommerceRepository.GetPaginatedProduct(page, pageSize, searchValue);
                var totalRecord = _ecommerceRepository.GetTotalProductCount(searchValue);


                ViewBag.Categories = new SelectList(categoryActiveList, "CId", "Name");
                var ProductMapperView = new ProductMapperView()
                {
                    ProductList=products,
                        PageIndex = page,
                    PageSize = pageSize,
                    RecordCount = totalRecord
                };

                return View(ProductMapperView);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing Catalog action.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult CatalogAjaxCall(int page = 1, int pageSize = 6, string searchValue = null, int CId = 0)
        {
            try
            {
                var categoryActiveList = _ecommerceRepository.ActiveCategories();
                var products = _ecommerceRepository.GetPaginatedProductCId(page, pageSize, searchValue, CId);
                var totalRecord = _ecommerceRepository.GetTotalProductCount(searchValue);


                ViewBag.Categories = new SelectList(categoryActiveList, "CId", "Name");
                var ProductMapperView = new ProductMapperView()
                {
                    ProductList = products,
                        PageIndex = page,
                    PageSize = pageSize,
                    RecordCount = totalRecord
                };

                return Json(ProductMapperView);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing Catalog action.");
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
