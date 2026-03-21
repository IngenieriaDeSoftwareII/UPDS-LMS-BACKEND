using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class User : IdentityUser
{
    public int PersonId { get; set; }

    public Person Person { get; set; } = null! ;
}
