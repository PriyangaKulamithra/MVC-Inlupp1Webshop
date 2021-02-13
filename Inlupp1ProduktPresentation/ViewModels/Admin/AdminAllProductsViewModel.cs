using System.Collections.Generic;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminAllProductsViewModel
    {
        public int NumberOfProducts { get; set; }
        public List<AdminProductViewModel> Products { get; set; }

        public class AdminProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public float Price { get; set; }
            public bool IsPublished { get; set; }
        }
    }
}