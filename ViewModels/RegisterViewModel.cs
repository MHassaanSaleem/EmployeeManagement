using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "isEmailInUse", controller: "Account")]
        [ValidEmailDomain(allowedDomain: "ncr.com", ErrorMessage = "Email Domain must be ncr.com ")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirmation password do not match")]
        public string ConfirmPassword { get; set; }


        public string City { get; set; }

    }
}
