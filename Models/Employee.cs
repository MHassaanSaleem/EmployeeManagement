﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage ="Name Cannot Excceed 20 chars ")]
        public string Name { get; set; } // getter setter 

        [Required]
       // [RegularExpression(@"^[a-zA-Z0-9_.+-] +@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$" , ErrorMessage ="Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }

        [Required]
        public Dept? Department { get; set; }

        public string PhotoPath { get; set; }

    }
}