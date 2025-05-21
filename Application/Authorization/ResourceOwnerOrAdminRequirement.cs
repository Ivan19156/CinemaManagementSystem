using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    using Microsoft.AspNetCore.Authorization;

    namespace YourProject.Authorization
    {
        public class ResourceOwnerOrAdminRequirement : IAuthorizationRequirement { }
    }

}
