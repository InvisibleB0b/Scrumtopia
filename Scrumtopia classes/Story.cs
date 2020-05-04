using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
    class Story
    {
        public int Story_Id { get; set; }
        public int Project_Id { get; set; }
        public int Sprint_Id { get; set; }
        public Category Category { get; set; }
        public string Story_Name { get; set; }
        public string Story_description { get; set; }
        public int Story_Points { get; set; }
        public int Story_Priority { get; set; }
        public string Story_State { get; set; }
        public User Story_Referee { get; set; }
        public User Story_Asignee { get; set; }

    }
}
