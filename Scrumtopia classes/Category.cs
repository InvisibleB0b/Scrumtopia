using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Scrumtopia_classes.Annotations;

namespace Scrumtopia_classes
{
    public class Category : INotifyPropertyChanged
    {
        private string _categoryColor;
        private string  _categoryName;

        public int Category_Id { get; set; }

        public string Category_Name
        {
            get { return _categoryName; }
            set {  _categoryName = value; OnPropertyChanged();}
        }

        public string Category_Color
        {
            get { return _categoryColor; }
            set { _categoryColor = value; OnPropertyChanged();}
        }


        public override string ToString()
        {
            return $"{nameof(Category_Name)}: {Category_Name}, {nameof(Category_Color)}: {Category_Color}";
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
