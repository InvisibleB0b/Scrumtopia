using System;

namespace Scrumtopia_classes
{
    public class Project
    {
        public int Project_Id { get; set; }
        public string Project_Name { get; set; }
        public string Project_Description { get; set; }
        public DateTime Project_Deadline { get; set; }


        public override string ToString()
        {
            return $"{nameof(Project_Name)}: {Project_Name}, {nameof(Project_Description)}: {Project_Description}, {nameof(Project_Deadline)}: {Project_Deadline}";
        }
    }
}
