using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrumtopia.Common;
using Scrumtopia.Persistency;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    class SprintBacklogVM
    {

        public ObservableCollection<Story> TodoCollection { get; set; }
        public ObservableCollection<Story> DoingCollection { get; set; }
        public ObservableCollection<Story> DoneCollection { get; set; }
        public ObservableCollection<Story> DoneDoneCollection { get; set; }
        public Singleton LeSingleton { get; set; }


        public SprintBacklogVM()
        {
         TodoCollection = new ObservableCollection<Story>();
         DoingCollection = new ObservableCollection<Story>();
         DoneCollection = new ObservableCollection<Story>();
         DoneDoneCollection = new ObservableCollection<Story>();

         LeSingleton = Singleton.Instance;

         LoadStories();
        }

        public async void LoadStories()
        {
            List<Story> st = await StoryPer.LoadSprintBacklog(LeSingleton.SelectedSprint.Sprint_Id);
            if (st != null)
            {
                foreach (Story storey in st)
                {
                    switch (storey.Story_State)
                    {
                        case "ToDo": 
                            TodoCollection.Add(storey);
                            break;
                        case "Doing":
                            DoingCollection.Add(storey);
                            break;
                        case "Done":
                            DoneCollection.Add(storey);
                            break;
                        case "DoneDone":
                            DoneDoneCollection.Add(storey);
                            break;
                    }
                }
            }
        }
    }
}
