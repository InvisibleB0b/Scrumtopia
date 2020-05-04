using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
    class Category
    {
        public int Category_Id { get; set; }
        public string Category_Color { get; set; }


        public override string ToString()
        {
            return $"{nameof(Category_Id)}: {Category_Id}, {nameof(Category_Color)}: {Category_Color}";
        }
    }
}
