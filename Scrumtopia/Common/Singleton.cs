﻿using System;
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
  public class Singleton:INotifyPropertyChanged
    {
        private static Singleton _instance = null;
        private Project _selectedProject;
        private Sprint _selectedSprint;

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
        public Sprint SelectedSprint
        {
            get { return _selectedSprint; }
            set { _selectedSprint = value; OnPropertyChanged();}
        }

        public ScrumUser LoggedUser { get; set; }


        private Singleton()
        {
            
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
