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
    class ProjectsVM
    {
        #region Project props
        private string _projectName;
        private string _projectDescription;
        private DateTimeOffset  _projectDeadlineDate;
        private TimeSpan _projectDeadlineTime;
        private List<ScrumUser> _selectedUsers;

        public string Project_NameVM
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged();}
        }

        public string Project_DescriptionVM
        {
            get { return _projectDescription; }
            set { _projectDescription = value; OnPropertyChanged(); }
        }

        public DateTimeOffset Project_DeadlineDate
        {
            get { return _projectDeadlineDate; }
            set {  _projectDeadlineDate = value; OnPropertyChanged(); }
        }

        public TimeSpan Project_DeadlineTime
        {
            get { return _projectDeadlineTime; }
            set { _projectDeadlineTime = value; OnPropertyChanged(); }
        }

       

        #endregion

        public ObservableCollection<Project> Projects { get; set; }

        public ObservableCollection<ScrumUser> Users { get; set; }

        public List<ScrumUser> selectedUsers
        {
            get { return _selectedUsers; }
            set { _selectedUsers = value; OnPropertyChanged();}
        }

        public ICommand CreateCommand { get; set; }

        public Singleton LeSingleton { get; set; }


        public ProjectsVM()
        {
            Projects = new ObservableCollection<Project>();
            Users = new ObservableCollection<ScrumUser>();
            CreateCommand = new RelayCommand(CreateProject);
            LeSingleton = Singleton.Instance;
            Project_DeadlineDate = TimeConverter.ConvertToDate(DateTime.Now.AddDays(14));
            Project_DeadlineTime = TimeConverter.ConvertToTime(DateTime.Now);
            selectedUsers = new List<ScrumUser>();
            LoadProjects();
            LoadUsers();
        }

        public async void LoadUsers()
        {
            List<ScrumUser> u = await UsersPer.GetAllUsers();

            if (u!= null)
            {
                foreach (ScrumUser scrumUser in u)
                {
                    if (scrumUser.User_Id != LeSingleton.LoggedUser.User_Id) { Users.Add(scrumUser); }
                    
                } 
            }
        }

        public async void LoadProjects()
        {
            List<Project> leProjects = await ProjectsPer.GetProjects(LeSingleton.LoggedUser.User_Id);

            if (leProjects != null)
            {
                foreach (Project leProject in leProjects)
                {
                    Projects.Add(leProject);
                }
            }
        }

        public async void CreateProject()
        {
           Project p = new Project(){Project_Name = Project_NameVM, Project_Description = Project_DescriptionVM, Project_Deadline = TimeConverter.ConverterToDateTime(Project_DeadlineDate, Project_DeadlineTime), UserIds = new List<int>()};

            p.UserIds.Add(LeSingleton.LoggedUser.User_Id);

           foreach (ScrumUser selectedUser in selectedUsers)
           {
               p.UserIds.Add(selectedUser.User_Id);
           }

           Project projectToAdd = await ProjectsPer.Create(p);

           if (projectToAdd != null)
           {
               Projects.Add(projectToAdd);
           }
            

        }

        public void HandleCheck(string name)
        {
            foreach (ScrumUser scrumUser in Users)
            {
                if (scrumUser.User_Name == name)
                {
                    selectedUsers.Add(scrumUser);
                }
            }
        }

        public void HandleUncheck(string name)
        {
            ScrumUser u = null;

            foreach (ScrumUser selectedUser in selectedUsers)
            {
                if (name == selectedUser.User_Name)
                {
                    u = selectedUser;
                }
            }

            if (u != null)
            {
                selectedUsers.Remove(u);
            }
        }



        #region Prop change

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
