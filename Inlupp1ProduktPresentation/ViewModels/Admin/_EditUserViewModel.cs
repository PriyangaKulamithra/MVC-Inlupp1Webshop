﻿using System;
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
        [EmailAddress]
        public string Email { get; set; }

        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        [Required]
        public string SelectedRoleId { get; set; }
    }
}