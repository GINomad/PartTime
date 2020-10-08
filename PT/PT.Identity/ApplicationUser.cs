using Microsoft.AspNetCore.Identity;
using PT.Core.Client.Domain;
using System;

namespace PT.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int? ClientId { get; set; }

        public virtual Client Client { get;  set; }
    }
}
