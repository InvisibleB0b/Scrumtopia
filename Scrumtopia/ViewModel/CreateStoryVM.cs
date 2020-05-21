using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Windows.System;
using Windows.UI.Composition;
using Scrumtopia.Annotations;
using Scrumtopia.Common;
using Scrumtopia.Persistency;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    class CreateStoryVM : INotifyPropertyChanged
    {
        #region CreateStory Properties

        private ScrumUser _scrumUser;
        private Category _storyCategory;
        private string _storyName;
        private string _storyDescription;
        private int _storyPoints;
        private int _storyPriority;
        private ScrumUser _storyAsignee;
        
       

        public Category Story_CategoryVM
        {
            get { return _storyCategory; }
            set { _storyCategory = value; OnPropertyChanged(); }
        }

        public ScrumUser AssigneeVM
        {
            get { return _scrumUser; }
            set { _scrumUser = value; OnPropertyChanged(); }
        }

        public string Story_NameVM
        {
            get { return _storyName; }
            set { _storyName = value; OnPropertyChanged(); }
        }

        public string Story_descriptionVM
        {
            get { return _storyDescription; }
            set { _storyDescription = value; OnPropertyChanged(); }
        }

        public int Story_PointsVM
        {
            get { return _storyPoints; }
            set { _storyPoints = value; OnPropertyChanged(); }
        }

      

        public int Story_PriorityVM
        {
            get { return _storyPriority; }
            set { _storyPriority = value; OnPropertyChanged(); }
        }

        public ScrumUser Story_AsigneeVM
        {
            get { return _storyAsignee; }
            set { _storyAsignee = value; OnPropertyChanged(); }
        }


        #endregion

        #region Create Category Properties

        private string _categoryNameVm;
        private string _categoryColorVm;
        private string _storyButton;
        private Story _selectedStory;
        private ICommand _createStoryCommand;
        private ICommand _createCatCommand;
        private string _sletButtonState;
        private string _catButton;

        public string Category_NameVM
        {
            get { return _categoryNameVm; }
            set { _categoryNameVm = value; OnPropertyChanged(); }
        }

        public string Category_ColorVM
        {
            get { return _categoryColorVm; }
            set { _categoryColorVm = value; OnPropertyChanged();}
        }

        #endregion

        public string StoryButton
        {
            get { return _storyButton; }
            set { _storyButton = value; OnPropertyChanged(); }
        }

        public string CatButton
        {
            get { return _catButton; }
            set { _catButton = value; OnPropertyChanged();}
        }

        public string SletButtonState
        {
            get { return _sletButtonState; }
            set { _sletButtonState = value; OnPropertyChanged();}
        }

        public ICommand CreateCatCommand
        {
            get { return _createCatCommand; }
            set { _createCatCommand = value; OnPropertyChanged();}
        }

        public ICommand CreateStoryCommand
        {
            get { return _createStoryCommand; }
            set { _createStoryCommand = value; OnPropertyChanged(); }
        }

        public ICommand SletCatCommand { get; set; }

        public ICommand AnnullerCatCommand { get; set; }

        public ICommand RemoveStoryCommand { get; set; }

        public Singleton LeSingleton { get; set; }

        public ObservableCollection<Story> Stories { get; set; }

        public ObservableCollection<Category> CategoriesForStory { get; set; }

        public ObservableCollection<ScrumUser> UsersInProject { get; set; }

        public Category SelectedCategory { get; set; }

        public Story SelectedStory
        {
            get { return _selectedStory; }
            set { _selectedStory = value; OnPropertyChanged();}
        }
       


        public CreateStoryVM()
        {
            Stories = new ObservableCollection<Story>();
            CreateStoryCommand = new RelayCommand(CreateStory);
            CreateCatCommand = new RelayCommand(CreatCategory);
            CategoriesForStory = new ObservableCollection<Category>();
            LeSingleton = Singleton.Instance;
            AssigneeVM = new ScrumUser(){User_Id = 0};
            UsersInProject = new ObservableCollection<ScrumUser>();
            StoryButton = "Opret";
            CatButton = "Opret";
            SletButtonState = "Collapsed";
            SletCatCommand = new RelayCommand(DeleteCategory);
            AnnullerCatCommand = new RelayCommand(ResetCategory);
            RemoveStoryCommand = new RelayCommand(RemoveStory);
            Load();
        }

        public async void RemoveStory()
        {
            bool success = await StoryPer.Delete(SelectedStory.Story_Id);

            if (success)
            {
                Story s = null;
                foreach (Story story in Stories)
                {
                    if (story.Story_Id == SelectedStory.Story_Id) s = story;
                }

                Stories.Remove(s);
            }
            
            StoryReset();
        }

        public void Load()
        { 
            LoadUsers();
            LoadStories();
            LoadCategories();
           
        }


        public async void DeleteCategory()
        {
            bool success = await CategoryPer.Delete(SelectedCategory.Category_Id);

            if (success)
            {

                Category c = null;
                foreach (Category category in CategoriesForStory)
                {
                    if (category.Category_Id == SelectedCategory.Category_Id) c = category;
                }

                CategoriesForStory.Remove(c);
            }

            ResetCategory();

        }


        public void StartEditCat()
        {
            Category_NameVM = SelectedCategory.Category_Name;
            Category_ColorVM = SelectedCategory.Category_Color;
            SletButtonState = "Visible";
            CreateCatCommand = new RelayCommand(EditCat);
            CatButton = "Ret";
        }

        public async void EditCat()
        {
            Category c = new Category(){Category_Color = Category_ColorVM, Category_Name = Category_NameVM};

            bool success = await CategoryPer.EditCategory(c, SelectedCategory.Category_Id);

            if (success)
            {
                foreach (Category category in CategoriesForStory)
                {
                    if (category.Category_Id == SelectedCategory.Category_Id)
                    {
                        category.Category_Name = Category_NameVM;
                        category.Category_Color = Category_ColorVM;
                        break;
                    }
                }
                foreach (Story storey in Stories)
                {
                    if (storey.Category.Category_Id == SelectedCategory.Category_Id)
                    {
                        storey.Category.Category_Color = Category_ColorVM;
                    }
                }
            }
            ResetCategory();

        }

        public void ResetCategory()
        {
            Category_NameVM = "";
            SelectedCategory = null;
            SletButtonState = "Collapsed";
            Category_ColorVM = "#FFFFFFFF";
            CatButton = "Opret";
            CreateCatCommand = new RelayCommand(CreatCategory);
        }

     

        public async void CreatCategory()
        {
            Category c = new Category() {Category_Name = Category_NameVM, Category_Color = Category_ColorVM};

            Category categoryToAdd = await CategoryPer.Create(c);

            if (categoryToAdd != null)
            {
                CategoriesForStory.Add(categoryToAdd);
            }
        }

        public async void LoadStories()
        {
            List<Story> st = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (st!= null)
            {
                foreach (Story storey in st)
                {
                    Stories.Add(storey);
                }
            }
        }

        public async void LoadUsers()
        {
            List<ScrumUser> scrumUsers = await UsersPer.GetProjectUsers(LeSingleton.SelectedProject.Project_Id);
            if (scrumUsers!=null)
            {
                foreach (ScrumUser scrumUser in scrumUsers)
                {
                    UsersInProject.Add(scrumUser);
                }
            }
        }

        public async void LoadCategories()
        {
            List<Category> categories = await CategoryPer.GetCategories();

            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    CategoriesForStory.Add(category);
                }
            }

        }
        

        public async void CreateStory()
        {
            Story s = new Story(){Project_Id = LeSingleton.SelectedProject.Project_Id, Category = Story_CategoryVM, Story_Asignee = AssigneeVM, Story_Name = Story_NameVM, Story_description = Story_descriptionVM, Story_Points = Story_PointsVM, Story_Priority = Story_PriorityVM, Story_State = "ToDo", Story_Referee = LeSingleton.LoggedUser};

            Story storyToAdd = await StoryPer.Create(s);

            if (storyToAdd != null)
            {
                Stories.Add(storyToAdd);
            }
            StoryReset();
        }

        public void StartStoryEdit()
        {
            StoryButton = "Ret";
            CreateStoryCommand = new RelayCommand(Edit);

            Story_PointsVM = SelectedStory.Story_Points;
            Story_descriptionVM = SelectedStory.Story_description;
            Story_PriorityVM = SelectedStory.Story_Priority;
            Story_NameVM = SelectedStory.Story_Name;

            foreach (Category cat in CategoriesForStory)
            {
                if (cat.Category_Id == SelectedStory.Category.Category_Id)
                {
                    Story_CategoryVM = cat;
                }
            }

            AssigneeVM = new ScrumUser(){User_Id = 0};

            foreach (ScrumUser su in UsersInProject)
            {
                if (su.User_Id == SelectedStory.Story_Asignee.User_Id)
                {
                    AssigneeVM = su;
                }
            }
            
        }

        public async void Edit()
        {
            Story s = new Story(){Category = Story_CategoryVM, Story_Name = Story_NameVM, Story_Asignee = AssigneeVM, Story_description = Story_descriptionVM, Story_Priority = Story_PriorityVM, Story_Points = Story_PointsVM};
            bool success = await StoryPer.Edit(s, SelectedStory.Story_Id);

            if (success)
            {
                SelectedStory.Story_Points = Story_PointsVM;
                SelectedStory.Story_description = Story_descriptionVM;
                SelectedStory.Story_Priority = Story_PriorityVM;
                SelectedStory.Story_Name = Story_NameVM;
                SelectedStory.Category = Story_CategoryVM;
                SelectedStory.Story_Asignee = AssigneeVM;
            }

            StoryReset();

        }

        public void StoryReset()
        {
            StoryButton = "Opret";
            CreateStoryCommand = new RelayCommand(CreateStory);

            Story_PointsVM =0;
            Story_descriptionVM = "";
            Story_PriorityVM = 0;
            Story_NameVM = "";
            Story_CategoryVM = null;

            AssigneeVM = new ScrumUser(){User_Id = 0};

            SelectedStory = null;

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
