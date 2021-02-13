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
    }
}
