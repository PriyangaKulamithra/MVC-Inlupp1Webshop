using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminNewUserViewModel
    {

        [Required]
        [MinLength(5, ErrorMessage = "För kort namn")]
        [MaxLength(40, ErrorMessage = "För långt namn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email måste fyllas i")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

        [Required]
        public string SelectedRoleId { get; set; }

        [Required(ErrorMessage = "Du måste skriva in ett lösenord.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Var god skriv lösenordet igen.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Du har ej angivit samma lösenord.")]
        public string ConfirmPassword { get; set; }
    }
}