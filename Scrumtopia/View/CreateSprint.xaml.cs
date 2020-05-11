using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class CreateSprint : Page
    {
        public CreateSprint()
        {
            this.InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Backlog));
        }

        private void StartDrag(object sender, DragItemsStartingEventArgs e)
        {
            CreateSprintVM vm = (CreateSprintVM) this.DataContext;

            vm.DragStory = (Story) e.Items[0];
        }

        private void DragOverEvent(object sender, DragEventArgs e)
        {
            e.DragUIOverride.Caption = "Add/Remove";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private void OnDropList(object sender, DragEventArgs e)
        {
            GridView gw = sender as GridView;

            string name = gw.Name;

            CreateSprintVM vm = (CreateSprintVM) this.DataContext;

            vm.MoveStory(name);
        }
    }
}
