using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Scrumtopia_classes.Annotations;

namespace Scrumtopia_classes
{
    public class Story : INotifyPropertyChanged
    {
        private ScrumUser _storyAsignee;
        private int _storyPriority;
        private string _storyName;
        private string _storyDescription;
        private int _storyPoints;
        private Category _category;
        public int Story_Id { get; set; }
        public int Project_Id { get; set; }
        public int Sprint_Id { get; set; }
        public string Story_State { get; set; }
        public ScrumUser Story_Referee { get; set; }

        public Category Category
        {
            get { return _category; }
            set {_category = value; OnPropertyChanged();}
        }

        public string Story_Name
        {
            get { return _storyName; }
            set { _storyName = value; OnPropertyChanged();}
        }

        public string Story_description
        {
            get { return _storyDescription; }
            set { _storyDescription = value; OnPropertyChanged();}
        }

        public int Story_Points
        {
            get { return _storyPoints; }
            set { _storyPoints = value; OnPropertyChanged();}
        }

        public int Story_Priority
        {
            get { return _storyPriority; }
            set { _storyPriority = value; OnPropertyChanged();}
        }

       

        public ScrumUser Story_Asignee
        {
            get { return _storyAsignee; }
            set { _storyAsignee = value; OnPropertyChanged(); }
        }


        public override string ToString()
        {
            return $"{nameof(Project_Id)}: {Project_Id}, {nameof(Sprint_Id)}: {Sprint_Id}, {nameof(Category)}: {Category}, {nameof(Story_Name)}: {Story_Name}, {nameof(Story_description)}: {Story_description}, {nameof(Story_Points)}: {Story_Points}, {nameof(Story_Priority)}: {Story_Priority}, {nameof(Story_State)}: {Story_State}, {nameof(Story_Referee)}: {Story_Referee}, {nameof(Story_Asignee)}: {Story_Asignee}";
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
