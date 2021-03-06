﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Scrumtopia.ViewModel;
using Scrumtopia_classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Scrumtopia.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Projects : Page
    {
        public Projects()
        {
            this.InitializeComponent();
        }

        private void Open_Project(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Backlog));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            ProjectsVM vm = (ProjectsVM)this.DataContext;
            CheckBox box = sender as CheckBox;
            string name = box.Content.ToString();
            vm.HandleCheck(name);
        }

        private void HandleUncheck(object sender, RoutedEventArgs e)
        {
            ProjectsVM vm = (ProjectsVM)this.DataContext;
            CheckBox box = sender as CheckBox;
            string name = box.Content.ToString();
            vm.HandleUncheck(name);
        }

        private void startdel(object sender, RoutedEventArgs e)
        {
            ProjectsVM vm = (ProjectsVM) this.DataContext;
            vm.StartDelete();


            Grid mview = this.FindName("mainView") as Grid;
            Grid pview = this.FindName("popview") as Grid;

            mview.Visibility = Visibility.Collapsed;
            pview.Visibility = Visibility.Visible;

        }

        private void Reset(object sender, RoutedEventArgs e)
        {

            Grid mview = this.FindName("mainView") as Grid;
            Grid pview = this.FindName("popview") as Grid;

            mview.Visibility = Visibility.Visible;
            pview.Visibility = Visibility.Collapsed;
        }

        private void StartEdit(object sender, ItemClickEventArgs e)
        {
            ProjectsVM vm = (ProjectsVM)this.DataContext;
            vm.LeSingleton.SelectedProject = (Project) e.ClickedItem;

            vm.StartProjectEdit();
        }
    }
}
