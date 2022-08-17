using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.Models
{
    public class UserApp : IdentityUser
    {
        public string City { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }


    }
}
