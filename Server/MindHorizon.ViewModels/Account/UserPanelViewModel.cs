using MindHorizon.Entities.Identity;
using MindHorizon.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.ViewModels.Account
{
    public class UserPanelViewModel
    {
        public UserPanelViewModel(User user, List<PostViewModel> bookmarks)
        {
            User = user;
            Bookmarks = bookmarks;
        }
        public User User { get; set; }
        public List<PostViewModel> Bookmarks { get; set; }
    }
}
