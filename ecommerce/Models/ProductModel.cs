using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class ProductModel
    {
        [DataNames("CId")]
        public int CId { get; set; }
        [DataNames("PId")]
        public int PId { get; set; }
        [DataNames("PName")]
        public string PName { get; set; }
        [DataNames("PDescription")]
        public string PDescription { get; set; }
        [DataNames("PCostPrice")]
        public decimal PCostPrice { get; set; } // Assuming PCostPrice is the cost price of the product
        [DataNames("PsellPrice")]

        public decimal PsellPrice { get; set; } // Assuming PsellPrice is the selling price of the product
        [DataNames("PSavings")]
        public decimal PSavings { get; set; }    // Assuming PSavings represents savings, if any
        [DataNames("StockQt")]
        public int StockQt { get; set; } // Assuming StockQt represents the stock quantity
        [DataNames("RemainQt")]
        public int RemainQt { get; set; } // Assuming RemainQt represents the remaining quantity
      
        [DataNames("PImage")]
        [Display(Name = "Upload File")]
       //[Required(ErrorMessage = "Please choose file to upload.")]
        public IFormFile? PImage { get; set; }
   

    }


    public class ProductViewModel
    {
        [DataNames("CId")]
        public int CId { get; set; }
        [DataNames("PId")]
        public int PId { get; set; }
        [DataNames("PName")]
        public string PName { get; set; }
        [DataNames("PDescription")]
        public string PDescription { get; set; }
        [DataNames("PCostPrice")]
        public decimal PCostPrice { get; set; } // Assuming PCostPrice is the cost price of the product
        [DataNames("PsellPrice")]

        public decimal PsellPrice { get; set; } // Assuming PsellPrice is the selling price of the product
        [DataNames("PSavings")]
        public decimal PSavings { get; set; }    // Assuming PSavings represents savings, if any
        [DataNames("StockQt")]
        public int StockQt { get; set; } // Assuming StockQt represents the stock quantity
        [DataNames("RemainQt")]
        public int RemainQt { get; set; } // Assuming RemainQt represents the remaining quantity

        [DataNames("PhotoPath")]
        [Display(Name = "Upload File")]
        //[Required(ErrorMessage = "Please choose file to upload.")]
        public string  PhotoPath { get; set; }
       


    }
    public class ProductModelMapper

    {
        [DataNames("RowNum")]
        public int RowNum { get; set; }
        [DataNames("CId")]
        public int CId { get; set; }
        [DataNames("PId")]
        public int PId { get; set; }
        [DataNames("PName")]
        public string PName { get; set; }
        [DataNames("PDescription")]
        public string PDescription { get; set; }
        [DataNames("PCostPrice")]
        public decimal PCostPrice { get; set; } // Assuming PCostPrice is the cost price of the product
        [DataNames("PsellPrice")]

        public decimal PsellPrice { get; set; } // Assuming PsellPrice is the selling price of the product
        [DataNames("PSavings")]
        public decimal PSavings { get; set; }    // Assuming PSavings represents savings, if any
        [DataNames("StockQt")]
        public int StockQt { get; set; } // Assuming StockQt represents the stock quantity
        [DataNames("RemainQt")]
        public int RemainQt { get; set; } // Assuming RemainQt represents the remaining quantity

        [DataNames("PImage")]
        [Display(Name = "Upload File")]
       // [Required(ErrorMessage = "Please choose file to upload.")]
        public string PImage { get; set; }
        [DataNames("CategoryName")]
        public string? CategoryName { get;set; }


    }

    public class ProductMapperView
    {
        public int CId { get; set; }
        public string CName { get; set; }
      public List<ProductModelMapper> ProductList { get; set; }

        public string SearchTerm { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)RecordCount / PageSize);
            }
        }




    }

}
