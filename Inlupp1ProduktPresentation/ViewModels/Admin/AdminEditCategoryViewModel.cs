using System.ComponentModel.DataAnnotations;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminEditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(2, ErrorMessage = "Namnet är för kort.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [MinLength(5, ErrorMessage = "Beskrivningen är för kort.")]
        public string Description { get; set; }

    }
}