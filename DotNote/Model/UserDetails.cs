using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Model
{
    public class UserDetails : IHasId
    {
        public string Id { get; set; } // this UserDetail record's Id
        public string UserId { get; set; } // the Firebase Authentication User's Id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureBlobName { get; set; }
    }
}
