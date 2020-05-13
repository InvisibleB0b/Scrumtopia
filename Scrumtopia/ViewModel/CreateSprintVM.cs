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
        public Story DragStory { get; set; }
        public Sprint SelectedSprint { get; set; }

        public string SprintButton
        {
            get { return _sprintButton; }
            set { _sprintButton = value; OnPropertyChanged(); }
        }

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; OnPropertyChanged();}
        }

        #region Create props
        private DateTimeOffset _sprintStartDate;
        private TimeSpan _sprintStartTime;
        private DateTimeOffset _sprintEndDate;
        private TimeSpan _sprintEndTime;
        private string _sprintGoalVm;
        private ICommand _createCommand;
        private string _sprintButton;

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
            LoadSprints();
            Load();
        }

        public async void LoadSprints()
        {
            List<Sprint> sp = await SprintsPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);
            if (sp !=null)
            {
                foreach (Sprint sprint in sp)
                {
                    Sprints.Add(sprint);
                } 
            }
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

            SprintReset();
        }

        public  async void Load()
        {
            SprintButton = "Opret";
            Sprint_StartDate = DateTimeOffset.Now;
            Sprint_EndDate = DateTimeOffset.Now.AddDays(14);

            List<Story> st = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (st != null)
            {
                foreach (Story storey in st)
                {
                    Backlog.Add(storey);
                }
            }
        }


        public void MoveStory(string name)
        {

            switch (name)
            {

                case "Backlog":
                    Backlog.Add(DragStory);
                    SprintBacklog.Remove(DragStory);
                    
                    break;

                case "SprintBacklog":
                    SprintBacklog.Add(DragStory);
                    Backlog.Remove(DragStory);
                    break;
            }


        }

        public async void StartEdit()
        {
            SprintReset();

            SprintButton = "Ret";
            CreateCommand = new RelayCommand(EditSprint);
            Sprint_StartDate = TimeConverter.ConvertToDate(SelectedSprint.Sprint_Start);
            Sprint_StartTime = TimeConverter.ConvertToTime(SelectedSprint.Sprint_Start);
            Sprint_EndDate = TimeConverter.ConvertToDate(SelectedSprint.Sprint_End);
            Sprint_EndTime = TimeConverter.ConvertToTime(SelectedSprint.Sprint_End);
            Sprint_GoalVM = SelectedSprint.Sprint_Goal;
            SelectedSprint.Story_Ids = new List<int>();

            List<Story> storiesInSprintBacklog = await StoryPer.LoadSprintBacklog(SelectedSprint.Sprint_Id);

            if (storiesInSprintBacklog != null)
            {
                foreach (Story story in storiesInSprintBacklog)
                {
                    foreach (Story storey in Backlog)
                    {
                        if (storey.Story_Id == story.Story_Id)
                        {
                            DragStory = storey;
                        }
                    }

                    if (DragStory !=null)
                    {
                        MoveStory("SprintBacklog");
                    }

                    DragStory = null;

                }
            }

        }

        public async void EditSprint()
        {
            
            Sprint sprint = new Sprint(){Sprint_End = TimeConverter.ConverterToDateTime(Sprint_EndDate,Sprint_EndTime), Sprint_Start = TimeConverter.ConverterToDateTime(Sprint_StartDate, Sprint_StartTime), Story_Ids = new List<int>(), Sprint_Goal = Sprint_GoalVM};
            foreach (Story story in SprintBacklog)
            {
                sprint.Story_Ids.Add(story.Story_Id);
            }

            bool success = await SprintsPer.EditSprint(sprint, SelectedSprint.Sprint_Id);

            if (success)
            {
                SelectedSprint.Sprint_Start = TimeConverter.ConverterToDateTime(Sprint_StartDate, Sprint_StartTime);
                SelectedSprint.Sprint_End = TimeConverter.ConverterToDateTime(Sprint_EndDate, Sprint_EndTime);
                SelectedSprint.Sprint_Goal = Sprint_GoalVM;
                
            }

            SprintReset();
        }



        public void SprintReset()
        {
            SprintBacklog.Clear();
            Backlog.Clear();
            Sprint_GoalVM = "";
            Load();
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
