using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
   public class User
    {
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string User_Password { get; set; }

        public override string ToString()
        {
            return $"{nameof(User_Name)}: {User_Name}";
        }
    }
}
