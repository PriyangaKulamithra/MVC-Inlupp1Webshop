using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
