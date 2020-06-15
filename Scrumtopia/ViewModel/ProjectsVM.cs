using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.WebUI;
using Scrumtopia.Annotations;
using Scrumtopia.Common;
using Scrumtopia.Converter;
using Scrumtopia.Persistency;
using Scrumtopia.View;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    public class ProjectsVM : INotifyPropertyChanged
    {
        #region Project props
        private string _projectNameVm;
        private string _projectDescriptionVm;
        private DateTimeOffset  _projectDeadlineDateVm;
        private TimeSpan _projectDeadlineTimeVm;
        private List<ScrumUser> _selectedUsers;
        private string _projectButton;
        private ICommand _createCommand;
        private string _selectedProState;

        public string Project_NameVM
        {
            get { return _projectNameVm; }
            set { _projectNameVm = value; OnPropertyChanged();}
        }

        public string Project_DescriptionVM
        {
            get { return _projectDescriptionVm; }
            set { _projectDescriptionVm = value; OnPropertyChanged(); }
        }

        public DateTimeOffset Project_DeadlineDateVM
        {
            get { return _projectDeadlineDateVm; }
            set {  _projectDeadlineDateVm = value; OnPropertyChanged(); }
        }

        public TimeSpan Project_DeadlineTimeVM
        {
            get { return _projectDeadlineTimeVm; }
            set { _projectDeadlineTimeVm = value; OnPropertyChanged(); }
        }

       

        #endregion

        public string ProjectButton
        {
            get { return _projectButton; }
            set { _projectButton = value; OnPropertyChanged();}
        }

        public string SelectedProState
        {
            get { return _selectedProState; }
            set { _selectedProState = value; OnPropertyChanged();}
        }

      

        public ObservableCollection<Project> Projects { get; set; }

        public ObservableCollection<ScrumUser> Users { get; set; }

        public ObservableCollection<Sprint> SprintsInProj { get; set; }

        public ObservableCollection<Story> StoryInProj { get; set; }

        public List<ScrumUser> selectedUsers
        {
            get { return _selectedUsers; }
            set { _selectedUsers = value; OnPropertyChanged();}
        }

        public Singleton LeSingleton { get; set; }

        public List<ScrumUser> ProjectUsers { get; set; }

        #region Commands

        public ICommand CreateCommand
        {
            get { return _createCommand; }
            set { _createCommand = value; OnPropertyChanged(); }
        }

        public ICommand DeleteProCommand { get; set; }

        public ICommand StartDeleteCommand { get; set; } 
        #endregion

        public ProjectsVM()
        {
            Projects = new ObservableCollection<Project>();
            Users = new ObservableCollection<ScrumUser>();
            CreateCommand = new RelayCommand(CreateProject);
            StartDeleteCommand = new RelayCommand(StartDelete);
            DeleteProCommand = new RelayCommand(DeletePro);
            LeSingleton = Singleton.Instance;
            Project_DeadlineDateVM = TimeConverter.ConvertToDate(DateTime.Now.AddDays(14));
            Project_DeadlineTimeVM = TimeConverter.ConvertToTime(DateTime.Now);
            selectedUsers = new List<ScrumUser>();
            ProjectButton = "Create";
            SelectedProState = "Collapsed";
            SprintsInProj = new ObservableCollection<Sprint>();
            StoryInProj = new ObservableCollection<Story>();
            LoadProjects();
            LoadUsers();
        }

        #region Load methods
        public async void LoadUsers()
        {
            List<ScrumUser> u = await UsersPer.GetAllUsers();

            if (u != null)
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

        #endregion

        #region Delete methods

        public async void StartDelete()
        {
            SprintsInProj.Clear();
            StoryInProj.Clear();

            List<Sprint> s = await SprintsPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);
            if (s != null)
            {
                foreach (Sprint spr in s)
                {
                    SprintsInProj.Add(spr);
                }
            }

            List<Story> stor = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);
            if (stor != null)
            {
                foreach (Story storey in stor)
                {
                    StoryInProj.Add(storey);
                }
            }
        }


        public async void DeletePro()
        {
            bool success = await ProjectsPer.Delete(LeSingleton.SelectedProject.Project_Id);

            if (success)
            {
                Project p = null;

                foreach (Project project in Projects)
                {
                    if (project.Project_Id == LeSingleton.SelectedProject.Project_Id) p = project;
                }

                Projects.Remove(p);
            }

            Reset();

        }


        #endregion

        #region Create method

        public async void CreateProject()
        {
            Project p = new Project() { Project_Name = Project_NameVM, Project_Description = Project_DescriptionVM, Project_Deadline = TimeConverter.ConverterToDateTime(Project_DeadlineDateVM, Project_DeadlineTimeVM), UserIds = new List<int>() };

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
            Reset();

        }


        #endregion

        #region Handle check

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
        #endregion

        #region Edit methods

        public void StartProjectEdit()
        {
            ProjectButton = "Edit";
            SelectedProState = "Visible";

            CreateCommand = new RelayCommand(Edit);

            Project_NameVM = LeSingleton.SelectedProject.Project_Name;
            Project_DescriptionVM = LeSingleton.SelectedProject.Project_Description;
            Project_DeadlineDateVM = TimeConverter.ConvertToDate(LeSingleton.SelectedProject.Project_Deadline);
            Project_DeadlineTimeVM = TimeConverter.ConvertToTime(LeSingleton.SelectedProject.Project_Deadline);




        }

        public async void Edit()
        {
            Project p = new Project() { Project_Name = Project_NameVM, Project_Description = Project_DescriptionVM, Project_Deadline = TimeConverter.ConverterToDateTime(Project_DeadlineDateVM, Project_DeadlineTimeVM), UserIds = new List<int>() };

            p.UserIds.Add(LeSingleton.LoggedUser.User_Id);

            foreach (ScrumUser selectedUser in selectedUsers)
            {
                p.UserIds.Add(selectedUser.User_Id);
            }

            bool success = await ProjectsPer.Edit(p, LeSingleton.SelectedProject.Project_Id);

            if (success)
            {
                foreach (Project project in Projects)
                {
                    if (project.Project_Id == LeSingleton.SelectedProject.Project_Id)
                    {
                        project.Project_Name = Project_NameVM;
                        project.Project_Description = Project_DescriptionVM;
                        project.Project_Deadline =
                            TimeConverter.ConverterToDateTime(Project_DeadlineDateVM, Project_DeadlineTimeVM);
                        break;

                    }
                }
            }

            Reset();
        }

        #endregion

        public void Reset()
        {
            Project_NameVM = "";
            Project_DescriptionVM = "";
            LeSingleton = Singleton.Instance;
            Project_DeadlineDateVM = TimeConverter.ConvertToDate(DateTime.Now.AddDays(14));
            Project_DeadlineTimeVM = TimeConverter.ConvertToTime(DateTime.Now);
            selectedUsers = new List<ScrumUser>();
            ProjectButton = "Create";
            SelectedProState = "Collapsed";
            CreateCommand = new RelayCommand(CreateProject);
            

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
