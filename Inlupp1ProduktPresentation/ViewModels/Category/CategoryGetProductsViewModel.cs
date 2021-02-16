using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class CategoryGetProductsViewModel
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public float Price { get; set; }
            public string ImageName { get; set; }
        }

    }
}
