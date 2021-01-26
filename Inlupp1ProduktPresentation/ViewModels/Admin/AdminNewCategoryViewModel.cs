using System.ComponentModel.DataAnnotations;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminNewCategoryViewModel
    {
        [Required(ErrorMessage = "Kategorin måste ha ett namn.")]
        [MaxLength(50)]
        [MinLength(2, ErrorMessage = "Namnet är för kort.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Var god ange en beskrivning.")]
        [MaxLength(500)]
        [MinLength(5, ErrorMessage = "Beskrivningen är för kort.")]
        public string Description { get; set; }
    }
}