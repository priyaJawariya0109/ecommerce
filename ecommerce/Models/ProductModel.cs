using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class ProductModel
    {

        public int CId { get; set; }
        public int PId { get; set; }
        public string PName { get; set; }
        public string PDescription { get; set; }
        public decimal PCostPrice { get; set; } // Assuming PCostPrice is the cost price of the product
        public decimal PsellPrice { get; set; } // Assuming PsellPrice is the selling price of the product
        public decimal PSavings { get; set; }    // Assuming PSavings represents savings, if any
        public int StockQt { get; set; } // Assuming StockQt represents the stock quantity
        public int RemainQt { get; set; } // Assuming RemainQt represents the remaining quantity

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string PImage { get; set; }

    }
}
