using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }

        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageName { get; set; }

        }
    }
}
