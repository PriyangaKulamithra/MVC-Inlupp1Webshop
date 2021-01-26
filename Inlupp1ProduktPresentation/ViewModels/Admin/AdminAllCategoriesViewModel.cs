using System.Collections.Generic;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminAllCategoriesViewModel
    {
        public List<AdminCategoryViewModel> Categories { get; set; } = new List<AdminCategoryViewModel>();
        public int NumberOfCategories { get; set; }

        public class AdminCategoryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int NumberOfProducts { get; set; }
        }
    }
}