using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Scrumtopia_classes.Annotations;

namespace Scrumtopia_classes
{
  public class Sprint:INotifyPropertyChanged
    {
        private DateTime _sprintStart;
        private DateTime _sprintEnd;
        private string _sprintGoal;

        public int Sprint_Id { get; set; }

        public DateTime Sprint_Start
        {
            get { return _sprintStart; }
            set { _sprintStart = value; OnPropertyChanged(); }
        }

        public DateTime Sprint_End
        {
            get { return _sprintEnd; }
            set { _sprintEnd = value; OnPropertyChanged(); }
        }

        public string Sprint_Goal
        {
            get { return _sprintGoal; }
            set { _sprintGoal = value; OnPropertyChanged(); }
        }

        public List<int> Story_Ids { get; set; }

        public override string ToString()
        {
            return $"{nameof(Sprint_Start)}: {Sprint_Start}, {nameof(Sprint_End)}: {Sprint_End}, {nameof(Sprint_Goal)}: {Sprint_Goal}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
