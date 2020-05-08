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
    class BacklogVM
    {
        public ObservableCollection<Sprint> Sprints { get; set; }

        public ObservableCollection<Story> Stories { get; set; }

        public Singleton LeSingleton { get; set; }


        public BacklogVM()
        {
            Sprints = new ObservableCollection<Sprint>();
            Stories = new ObservableCollection<Story>();
            LeSingleton = Singleton.Instance;
            Load();
        }

        public async void Load()
        {
            List<Sprint> spr = await SprintsPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (spr != null)
            {
                foreach (Sprint sprint in spr)
                {
                        Sprints.Add(sprint);
                }
            }

            List<Story> st = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (st!= null)
            {
                foreach (Story storey in st)
                {
                    Stories.Add(storey);
                }
            }
        }
    }
}
