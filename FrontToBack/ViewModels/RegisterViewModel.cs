using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
