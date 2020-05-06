using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
   public class StoryTask
    {
        public int Task_Id { get; set; }
        public string Task_Name { get; set; }
        public string Task_State { get; set; }
        public int Story_Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Task_Name)}: {Task_Name}, {nameof(Task_State)}: {Task_State}, {nameof(Story_Id)}: {Story_Id}";
        }
    }
}
