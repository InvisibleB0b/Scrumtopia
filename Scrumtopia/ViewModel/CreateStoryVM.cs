using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Windows.System;
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
        private List<StoryTask> _tasks;
       

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

        public List<StoryTask> TasksVM
        {
            get { return _tasks; }
            set { _tasks = value; OnPropertyChanged(); }
        }

        #endregion

        #region Create Category Properties

        private string _categoryNameVm;
        private string _categoryColorVm;

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

        public ICommand CreateCatCommand { get; set; }

        public ObservableCollection<Story> Stories { get; set; }

        public ICommand CreateCommand { get; set; }

        public ObservableCollection<Category> CategoriesForStory { get; set; }

        public Singleton LeSingleton { get; set; }

        public List<ScrumUser> UsersInProject { get; set; }





        public CreateStoryVM()
        {
            Stories = new ObservableCollection<Story>();
            CreateCommand = new RelayCommand(CreateStory);
            CreateCatCommand = new RelayCommand(CreatCategory);
            CategoriesForStory = new ObservableCollection<Category>();
            LeSingleton = Singleton.Instance;
            AssigneeVM = new ScrumUser(){User_Id = 0};
            UsersInProject = new List<ScrumUser>();
            LoadStories();
            LoadCategories();
            LoadUsers();
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
