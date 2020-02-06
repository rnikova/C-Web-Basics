using System;

namespace SIS.SandBox
{
    public class User
    {
        [StringLengthSis(3, 20, "Error Message")]
        public string Username { get; set; }
    }
}
