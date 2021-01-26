using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminEditProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Produkten måste ha ett namn.")]
        [MaxLength(100)]
        [MinLength(2, ErrorMessage = "Namnet är för kort.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Produkten måste ha en beskrivning.")]
        [MaxLength(300)]
        [MinLength(5, ErrorMessage = "Beskrivningen är för kort.")]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public bool PublishOnWebsite { get; set; }

        [Range(1,1000,ErrorMessage = " - Du måste välja kategori.")]
        public int SelectedCategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}