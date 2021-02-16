using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inlupp1ProduktPresentation.Models.ViewModels
{
    public class AdminAllUsersViewModel
    {
        public List<RegisteredUser> AllUsers { get; set; }

        public class RegisteredUser
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}