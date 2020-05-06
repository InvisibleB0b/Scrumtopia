using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Scrumtopia.Annotations;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    class CreateStoryVM : INotifyPropertyChanged
    {
        public int Story_Id { get; set; }
        public int Project_Id { get; set; }
        public Category Story_Category { get; set; }
        public string Story_Name { get; set; }
        public string Story_description { get; set; }
        public int Story_Points { get; set; }
        public int Story_Priority { get; set; }
        public User Story_Asignee { get; set; }
        public List<StoryTask> Tasks { get; set; }

        public CreateStoryVM()
        {
            
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
