using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Scrumtopia.Annotations;
using Scrumtopia_classes;
using Scrumtopia.Common;
using Scrumtopia.Persistency;

namespace Scrumtopia.ViewModel
{
    class CreateCategoryVM : INotifyPropertyChanged
    {
        #region Category Proerties

        private int _categoryId;
        private string _categoryName;
        private string _categoryColor;
        

        public int Category_IdVM
        {
            get => _categoryId;
            set { _categoryId = value; OnPropertyChanged();}
        }

        public string Category_NameVM
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged();}
        }

        public string Category_ColorVM
        {
            get => _categoryColor;
            set { _categoryColor = value; OnPropertyChanged();}
        }
        
        #endregion

        public ObservableCollection<Category> Categories{ get; set; }
        public Singleton LeSingleton { get; set; }
        public ICommand CreateCommand { get; set; }

        public CreateCategoryVM()
        {
            Categories = new ObservableCollection<Category>();
            LeSingleton = Singleton.Instance;
            CreateCommand = new RelayCommand(CreateCategory);
            Load();
        }

        public async void Load()
        {
            List<Category> categories = await CategoryPer.GetCategories();

            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    Categories.Add(category);
                }
            }
        }

        public  async void CreateCategory()
        {
           Category c = new Category(){Category_Color = Category_ColorVM, Category_Id = Category_IdVM, Category_Name = Category_NameVM};
           Category categoryToAdd = await CategoryPer.Create(c);

           if (categoryToAdd != null)
           {
               Categories.Add(categoryToAdd);
           }
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
