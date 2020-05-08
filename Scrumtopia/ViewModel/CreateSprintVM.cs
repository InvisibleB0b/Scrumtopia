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
using Scrumtopia.Common;
using Scrumtopia.Converter;
using Scrumtopia.Persistency;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
   public class CreateSprintVM:INotifyPropertyChanged
    {

        public ObservableCollection<Story> Backlog { get; set; }
        public ObservableCollection<Story> SprintBacklog { get; set; }
        public Singleton LeSingleton { get; set; }
        public ObservableCollection<Sprint> Sprints { get; set; }

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; OnPropertyChanged();}
        }

        #region Create props
        private int _sprintIdVm;
        private DateTimeOffset _sprintStartDate;
        private TimeSpan _sprintStartTime;
        private DateTimeOffset _sprintEndDate;
        private TimeSpan _sprintEndTime;
        private string _sprintGoalVm;
        private ICommand _createCommand;

        public int Sprint_IdVM
        {
            get { return _sprintIdVm; }
            set { _sprintIdVm = value; OnPropertyChanged();}
        }

        public DateTimeOffset Sprint_StartDate
        {
            get { return _sprintStartDate; }
            set { _sprintStartDate = value; OnPropertyChanged(); }
        }

        public TimeSpan Sprint_StartTime
        {
            get { return _sprintStartTime; }
            set { _sprintStartTime = value; OnPropertyChanged(); }
        }

        public DateTimeOffset Sprint_EndDate
        {
            get { return _sprintEndDate; }
            set { _sprintEndDate = value; OnPropertyChanged(); }
        }

        public TimeSpan Sprint_EndTime
        {
            get { return _sprintEndTime; }
            set { _sprintEndTime = value; OnPropertyChanged(); }
        }

        public string Sprint_GoalVM
        {
            get { return _sprintGoalVm; }
            set { _sprintGoalVm = value; OnPropertyChanged(); }
        }

        #endregion

        public CreateSprintVM()
        {
            Backlog = new ObservableCollection<Story>();
            SprintBacklog = new ObservableCollection<Story>();
            LeSingleton = Singleton.Instance;
            CreateCommand = new RelayCommand(Create);
            Sprints = new ObservableCollection<Sprint>();
            Load();
        }

        public async void Create()
        {
            List<int> storyId = new List<int>();

            foreach (Story storey in SprintBacklog)
            {
                storyId.Add(storey.Story_Id);
            }

            Sprint sp = await SprintsPer.Create(new Sprint(){Story_Ids = storyId, Sprint_Goal = Sprint_GoalVM, Sprint_Start = TimeConverter.ConverterToDateTime(Sprint_StartDate, Sprint_StartTime), Sprint_End = TimeConverter.ConverterToDateTime(Sprint_EndDate, Sprint_EndTime)}, LeSingleton.SelectedProject.Project_Id);

            if (sp != null)
            {
                Sprints.Add(sp);
            }
        }

        public  async void Load()
        {

            List<Story> st = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (st != null)
            {
                foreach (Story storey in st)
                {
                    Backlog.Add(storey);
                }
            }
        }

        #region PropChange
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
           

        }

        #endregion

    }
}
