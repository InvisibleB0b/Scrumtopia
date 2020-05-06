using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        private Category _storyCategory;
        private string _storyName;
        private string _storyDescription;
        private int _storyPoints;
        private int _storyPriority;
        private User _storyAsignee;
        private List<StoryTask> _tasks;

        public Category Story_CategoryVM
        {
            get => _storyCategory;
            set { _storyCategory = value; OnPropertyChanged(); }
        }

        public string Story_NameVM
        {
            get => _storyName;
            set { _storyName = value; OnPropertyChanged(); }
        }

        public string Story_descriptionVM
        {
            get => _storyDescription;
            set { _storyDescription = value; OnPropertyChanged(); }
        }

        public int Story_PointsVM
        {
            get => _storyPoints;
            set { _storyPoints = value; OnPropertyChanged(); }
        }

        public int Story_PriorityVM
        {
            get => _storyPriority;
            set { _storyPriority = value; OnPropertyChanged(); }
        }

        public User Story_AsigneeVM
        {
            get => _storyAsignee;
            set { _storyAsignee = value; OnPropertyChanged(); }
        }

        public List<StoryTask> TasksVM
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        } 
        #endregion

        public ObservableCollection<Story> Stories { get; set; }
        public ICommand CreaCommand { get; set; }

        public CreateStoryVM()
        {
            Stories = new ObservableCollection<Story>();
            CreaCommand = new RelayCommand(CreateStory);
        }

        public async void CreateStory()
        {
            Story s = new Story(){Project_Id = 1, Category = null, Story_Asignee = null, Story_Name = Story_NameVM, Story_description = Story_descriptionVM, Story_Points = Story_PointsVM, Story_Priority = Story_PriorityVM, Story_State = "ToDo"};

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
