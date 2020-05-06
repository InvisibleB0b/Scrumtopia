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
using Scrumtopia_classes;
using Scrumtopia.Converter;
using Scrumtopia.Persistency;

namespace Scrumtopia.ViewModel
{
    class ProjectsVM:INotifyPropertyChanged
    {
       

        #region Project props
        private string _projectName;
        private string _projectDescription;
        private DateTimeOffset  _projectDeadlineDate;
        private TimeSpan _projectDeadlineTime;

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

        public ICommand CreateCommand { get; set; }


        public ProjectsVM()
        {
            Projects = new ObservableCollection<Project>();
            CreateCommand = new RelayCommand(CreateProject);
        }

        public async void CreateProject()
        {
           Project p = new Project(){Project_Name = Project_NameVM, Project_Description = Project_DescriptionVM, Project_Deadline = TimeConverter.ConverterToDateTime(Project_DeadlineDate, Project_DeadlineTime)};


           Project projectToAdd = await ProjectsPer.Create(p);

           if (projectToAdd != null)
           {
               Projects.Add(projectToAdd);
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
