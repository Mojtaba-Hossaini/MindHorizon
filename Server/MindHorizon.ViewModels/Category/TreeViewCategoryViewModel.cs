using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.ViewModels.Category
{
    public class TreeViewCategory
    {
        public TreeViewCategory()
        {
            subs = new List<TreeViewCategory>();
        }
        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public List<TreeViewCategory> subs { get; set; }
    }
}
