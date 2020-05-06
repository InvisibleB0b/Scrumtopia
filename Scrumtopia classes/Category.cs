using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Category_Color { get; set; }


        public override string ToString()
        {
            return $"{nameof(Category_Name)}: {Category_Name}, {nameof(Category_Color)}: {Category_Color}";
        }
    }
}
