using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Models
{
    public class User
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Username")]
        public string username { get; set; }
        [Display(Name = "E-mail")]
        public string email { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }

        public Customer customer { get; set; }

        public Admin admin { get; set; }


    }
}
