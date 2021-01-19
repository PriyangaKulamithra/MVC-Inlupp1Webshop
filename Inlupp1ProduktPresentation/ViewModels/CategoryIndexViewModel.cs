using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class CategoryIndexViewModel
    {
        public string CategoryName { get; set; }
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
