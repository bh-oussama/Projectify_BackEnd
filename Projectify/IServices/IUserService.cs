using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.IServices
{
    interface IUserService {
        IEnumerable<ApplicationUser> GetUsers();
       // IEnumerable<ApplicationUser> GetAvailableDevelopers();
    }
}
