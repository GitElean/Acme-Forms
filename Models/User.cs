using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeForms.Models
{
    public class User
    {
        public int ID_user { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }


        public string ConfirmPassword { get; set; }

    }
}
