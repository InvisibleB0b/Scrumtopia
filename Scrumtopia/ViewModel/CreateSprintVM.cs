using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
   public class CreateSprintVM : INotifyPropertyChanged
    {


        #region CreateSprint Properties

        private DateTimeOffset _sprintStartDate;
        private TimeSpan _sprintStartTime;
        private DateTimeOffset _sprintEndDate;
        private TimeSpan _sprintEndTime;
        private string _sprintGoalVm;

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


        #region Button states

        private string _sprintButton;
        private string _sletState;

        public string SprintButton
        {
            get { return _sprintButton; }
            set { _sprintButton = value; OnPropertyChanged(); }
        }

        public string SletState
        {
            get { return _sletState; }
            set { _sletState = value;  OnPropertyChanged();}
        }

        #endregion


        #region Commands

        private ICommand _createCommand;

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; OnPropertyChanged();}
        }

        public ICommand SletCommand { get; set; }

        #endregion


        #region Visnigs properties

        public ObservableCollection<Story> Backlog { get; set; }
        public ObservableCollection<Story> SprintBacklog { get; set; }
        public Singleton LeSingleton { get; set; }
        public ObservableCollection<Sprint> Sprints { get; set; }

        #endregion

        
        #region Objecter til redigering props

        public Story DragStory { get; set; }
        public Sprint SelectedSprint { get; set; }


        #endregion



        public CreateSprintVM()
        {
            Backlog = new ObservableCollection<Story>();
            SprintBacklog = new ObservableCollection<Story>();
            Sprints = new ObservableCollection<Sprint>();
            LeSingleton = Singleton.Instance;
            SletCommand = new RelayCommand(DeleteSprint);
            CreateCommand = new RelayCommand(Create);
            SletState = "Collapsed";
            LoadSprints();
            Load();
        }

        #region Metoder

        #region Load Metoder

            /// <summary>
            ///Sætter load udenfor ¨konstrunktoren for at kunne overskue det bedre
            /// </summary>
            public async void Load()
        {
            SprintButton = "Create";
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

        /// <summary>
        /// Anvender det valgte projekt i singletonnen til at hente alle sprint der ligger tilknyttet til projektet ud og sætte det ind i OC der er binded op til viewet.
        /// </summary>
        public async void LoadSprints()
        {
            List<Sprint> sp = await SprintsPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);
            if (sp != null)
            {
                foreach (Sprint sprint in sp)
                {
                    Sprints.Add(sprint);
                }
            }
        }


        #endregion


        #region Create Metode

        /// <summary>
        /// Generere et nyt Objec af typen sprint for at kunne sende det med til WEB API'en for at kunne generere et nyt object der bliver sendt tilbage fra api'en hvis det lykkes.
        /// </summary>
        public async void Create()
        {
            List<int> storyId = new List<int>();

            foreach (Story storey in SprintBacklog)
            {
                storyId.Add(storey.Story_Id);
            }

            Sprint sp = await SprintsPer.Create(new Sprint() { Story_Ids = storyId, Sprint_Goal = Sprint_GoalVM, Sprint_Start = TimeConverter.ConverterToDateTime(Sprint_StartDate, Sprint_StartTime), Sprint_End = TimeConverter.ConverterToDateTime(Sprint_EndDate, Sprint_EndTime) }, LeSingleton.SelectedProject.Project_Id);

            if (sp != null)
            {
                Sprints.Add(sp);
            }

            SprintReset();
        }

        #endregion


        #region Delete metode

            /// <summary>
            /// Beder persitencn om at slette sprintet ved hjælp af API'en, his det lykkes så fjernes sprintet fra view.
            /// </summary>
        public async void DeleteSprint()
        {
            bool success = await SprintsPer.DeleteSprint(SelectedSprint.Sprint_Id);

            Sprint sprintToRemove = null;

            if (success)
            {
                foreach (Sprint sprint in Sprints)
                {
                    if (SelectedSprint.Sprint_Id == sprint.Sprint_Id)
                    {
                        sprintToRemove = sprint;
                    }
                }

                if (sprintToRemove != null)
                {
                    Sprints.Remove(sprintToRemove);
                }

            }
            /// Derudover fjerner vi også dataen i indput felterne i sprint viewet 
            SprintReset();
        }



        #endregion


        #region Flyt story metode

        /// <summary>
        /// Tilføjer en story til et sprint. Når man tilføjer en story til et sprint vil den blive fjernet fra backloggen.
        /// </summary>
        /// <param name="name">String name er navnet på det listviewet den skal flyttes hen til </param>
        
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
        #endregion 


        #region Edit metoder

        /// <summary>
        /// Starter med at opdaterer knappen til at udføre en anden funktion end normalt.
        /// Derefter udfylder vi inout felterne i Viewet så brugeren kan se hvad de er igang med at redigere.
        /// </summary>
        public async void StartEdit() 
        {
            
            SprintReset();

            SletState = "Visible";
            SprintButton = "Edit";
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

        /// <summary>
        /// Når brugeren har redigeret og trykket på knappen sendes et nyt object med til API'en for at kunne redigere objektet,
        /// hvis API'en lykkes retunere den true og objektet bliver opdateret i viewet.
        /// </summary>
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

        #endregion


        #region Reset sprint metode

        /// <summary>
        /// Bruges til at restte viewet tilbage til normalen så der ikke står noget i input felterne
        /// </summary>
        public void SprintReset()
        {
            SprintBacklog.Clear();
            Backlog.Clear();
            Sprint_GoalVM = "";
            SletState = "Collapsed";
            Load();
        }

        #endregion
        

        #endregion


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
