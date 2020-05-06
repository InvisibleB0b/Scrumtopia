using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Scrumtopia.Annotations;
using Scrumtopia_classes;

namespace Scrumtopia.Common
{
    class Singleton:INotifyPropertyChanged
    {
        private static Singleton _instance = null;
        private Project _selectedProject;


        public static Singleton Instance
        {
            get
            {

                return _instance ?? (_instance = new Singleton());

            }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; OnPropertyChanged();}
        }

        public ScrumUser LoggedUser { get; set; }


        private Singleton()
        {
         
            LoggedUser = new ScrumUser(){User_Id = 1};
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
