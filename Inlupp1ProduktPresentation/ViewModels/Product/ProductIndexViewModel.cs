﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class ProductIndexViewModel
    {
        public string Title { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}