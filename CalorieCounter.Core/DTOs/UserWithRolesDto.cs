using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.DTOs
{
    public class UserWithRolesDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int OrgFK { get; set; }
        public List<string> Roles { get; set; }
    }
}
