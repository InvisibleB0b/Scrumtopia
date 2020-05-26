using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Scrumtopia_classes.Annotations;

namespace Scrumtopia_classes
{
    public class Project : INotifyPropertyChanged
    {
        private DateTime _projectDeadline;
        private string _projectDescription;
        private string _projectName;
        public int Project_Id { get; set; }

        public string Project_Name
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged();}
        }

        public string Project_Description
        {
            get { return _projectDescription; }
            set { _projectDescription = value; OnPropertyChanged(); }
        }

        public DateTime Project_Deadline
        {
            get { return _projectDeadline; }
            set { _projectDeadline = value; OnPropertyChanged(); }
        }

        public List<int> UserIds { get; set; }


        public override string ToString()
        {
            return $"{nameof(Project_Name)}: {Project_Name}, {nameof(Project_Description)}: {Project_Description}, {nameof(Project_Deadline)}: {Project_Deadline}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
