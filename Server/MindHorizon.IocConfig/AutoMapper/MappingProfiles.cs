using AutoMapper;
using MindHorizon.Entities;
using MindHorizon.Entities.Identity;
using MindHorizon.ViewModels.Category;
using MindHorizon.ViewModels.Comments;
using MindHorizon.ViewModels.Manage;
using MindHorizon.ViewModels.Post;
using MindHorizon.ViewModels.RoleManager;
using MindHorizon.ViewModels.Tag;
using MindHorizon.ViewModels.UserManager;
using MindHorizon.ViewModels.Video;

namespace MindHorizon.IocConfig.AutoMapper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap()
                .ForMember(p => p.Parent, c => c.Ignore())
                .ForMember(p => p.Categories, c => c.Ignore())
                .ForMember(p => p.PostCategories, c => c.Ignore());

            CreateMap<Role, RolesViewModel>().ReverseMap()
                    .ForMember(p => p.Users, c => c.Ignore())
                    .ForMember(p => p.Claims, c => c.Ignore());

            CreateMap<Tag, TagViewModel>().ReverseMap()
                   .ForMember(p => p.PostTags, c => c.Ignore());

            CreateMap<Video, VideoViewModel>().ReverseMap();

            CreateMap<User, UsersViewModel>().ReverseMap()
                  .ForMember(p => p.Post, c => c.Ignore())
                  .ForMember(p => p.Bookmarks, c => c.Ignore())
                  .ForMember(p => p.Claims, c => c.Ignore());

            CreateMap<User, ProfileViewModel>().ReverseMap()
                   .ForMember(p => p.Post, c => c.Ignore())
                   .ForMember(p => p.Bookmarks, c => c.Ignore())
                   .ForMember(p => p.Claims, c => c.Ignore());

            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
        }
    }
}
