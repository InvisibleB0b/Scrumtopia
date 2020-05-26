using System;
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
    public sealed partial class Backlog : Page
    {
        public Backlog()
        {
            this.InitializeComponent();
        }

        private void GotoCreateSprint(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateSprint));
        }

        private void GotoCreateStory(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateStory));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Projects));
        }

        private void GoToSprintBacklog(object sender, ItemClickEventArgs e)
        {
            BacklogVM vm = (BacklogVM) this.DataContext;
            vm.LeSingleton.SelectedSprint = (Sprint) e.ClickedItem;
            this.Frame.Navigate(typeof(SprintBacklog));
        }
    }
}
