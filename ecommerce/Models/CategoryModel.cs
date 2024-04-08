using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ecommerce.Models
{
    public class CategoryModel
    {


        public int CId { get; set; }
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public bool Active { get; set; }
        public int count1 { get; set; }

    }
/*
    public class CategoryPageModel
    {
        public List<CategoryModel> Category { get; set; }

       // public string SearchTerm { get; set; }

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
    }*/
    public class CategoryPageModel
    {
        public int Page { get; set; }

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
