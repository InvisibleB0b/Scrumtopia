using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
    public class Story
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
        public List<StoryTask> Tasks { get; set; }


        public override string ToString()
        {
            return $"{nameof(Project_Id)}: {Project_Id}, {nameof(Sprint_Id)}: {Sprint_Id}, {nameof(Category)}: {Category}, {nameof(Story_Name)}: {Story_Name}, {nameof(Story_description)}: {Story_description}, {nameof(Story_Points)}: {Story_Points}, {nameof(Story_Priority)}: {Story_Priority}, {nameof(Story_State)}: {Story_State}, {nameof(Story_Referee)}: {Story_Referee}, {nameof(Story_Asignee)}: {Story_Asignee}, {nameof(Tasks)}: {Tasks}";
        }
    }
}
