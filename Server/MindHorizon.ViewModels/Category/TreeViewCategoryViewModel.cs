﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<TreeViewCategory> subs { get; set; }
    }
}