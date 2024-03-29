﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inlupp1ProduktPresentation.Data
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public float Price { get; set; }
        public ProductCategory Category { get; set; }
        public bool PublishedOnWebsite { get; set; }
    }
}
