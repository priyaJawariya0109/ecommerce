using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ecommerce.Models
{
    public class CategoryModel
    {

        [DataNames("CId")]
        public int CId { get; set; }
        [DataNames("Name")]
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DataNames("Description")]
        public string Description { get; set; }
        [DataNames("Active")]
        public bool Active { get; set; }
       

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
