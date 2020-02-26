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
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.IocConfig.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap()
                .ForMember(p => p.Parent, opt => opt.Ignore())
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ForMember(p => p.PostCategories, opt => opt.Ignore());

            CreateMap<Role, RolesViewModel>().ReverseMap()
                    .ForMember(p => p.Users, opt => opt.Ignore())
                    .ForMember(p => p.Claims, opt => opt.Ignore());

            CreateMap<Tag, TagViewModel>().ReverseMap()
                   .ForMember(p => p.PostTags, opt => opt.Ignore());

            CreateMap<Video, VideoViewModel>().ReverseMap();

            CreateMap<User, UsersViewModel>().ReverseMap()
                  .ForMember(p => p.Post, opt => opt.Ignore())
                  .ForMember(p => p.Bookmarks, opt => opt.Ignore())
                  .ForMember(p => p.Claims, opt => opt.Ignore());

            CreateMap<User, ProfileViewModel>().ReverseMap()
                   .ForMember(p => p.Post, opt => opt.Ignore())
                   .ForMember(p => p.Bookmarks, opt => opt.Ignore())
                   .ForMember(p => p.Claims, opt => opt.Ignore());

            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();

        }
    }
}
