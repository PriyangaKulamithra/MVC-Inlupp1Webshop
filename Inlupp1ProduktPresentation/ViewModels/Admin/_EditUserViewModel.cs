using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class _EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "För kort namn")]
        [MaxLength(40, ErrorMessage = "För långt namn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email måste fyllas i")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        public string SelectedRoleId { get; set; }
        public string PreviousSelectedRoleId { get; set; }
    }
}