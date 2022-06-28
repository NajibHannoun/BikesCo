using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Models
{
    public class Admin
    {
        public int id { get; set; }

        [Display(Name = "Logged In")]
        public bool isCurrentlyLogged { get; set; }

        [Display(Name = "Suspended")]
        public bool isSuspended { get; set; }

        internal bool IsUsernameExist(string username)
        {
            throw new NotImplementedException();
        }

        public int? user_id { get; set; }
        public User user { get; set; }

        public List<Transaction> transactions { get; set; }
    }
}
