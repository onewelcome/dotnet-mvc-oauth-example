using System;

namespace DotnetAspCoreResourceGatewayExample.Model
{
    //Person, User, Profile or whatever...
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}