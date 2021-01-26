using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminNewProductViewModel
    {
        [Required]
        [MaxLength(100)]
        [MinLength(2, ErrorMessage = "Namnet är för kort.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        [MinLength(5, ErrorMessage = "Beskrivningen är för kort.")]
        public string Description { get; set; }

        [Range(0,10000)]
        public float Price { get; set; }

        public bool PublishOnWebsite { get; set; }

        [Range(1, 20, ErrorMessage = " - Kategori måste fyllas i.")]
        public int SelectedCategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}