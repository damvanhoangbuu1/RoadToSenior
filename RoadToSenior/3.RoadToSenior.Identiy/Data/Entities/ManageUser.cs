using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace _3.RoadToSenior.Identiy.Data.Entities
{
    public class ManageUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}