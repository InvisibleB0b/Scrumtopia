using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumtopia_classes
{
  public class Sprint
    {
        public int Sprint_Id { get; set; }
        public DateTime Sprint_Start { get; set; }
        public DateTime Sprint_End { get; set; }
        public string Sprint_Goal { get; set; }
        public List<int> Story_Ids { get; set; }

        public override string ToString()
        {
            return $"{nameof(Sprint_Start)}: {Sprint_Start}, {nameof(Sprint_End)}: {Sprint_End}, {nameof(Sprint_Goal)}: {Sprint_Goal}";
        }
    }
}
