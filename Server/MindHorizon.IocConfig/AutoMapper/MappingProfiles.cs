using AutoMapper;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Category;

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
        }
    }
}
