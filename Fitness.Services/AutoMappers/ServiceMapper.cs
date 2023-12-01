using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Fitness.Context.Contracts.Enums;
using Fitness.Context.Contracts.Models;
using Fitness.Services.Contracts.Enums;
using Fitness.Services.Contracts.Models;

namespace Fitness.Services.AutoMappers
{
    public class ServiceMapper : Profile
    {
        public ServiceMapper()
        {
            CreateMap<Category, CategoryModel>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();
            CreateMap<DocumentTypes, DocumentTypesModel>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();

            CreateMap<Club, ClubModel>(MemberList.Destination);
            CreateMap<Coach, CoachModel>(MemberList.Destination);
            CreateMap<Gym, GymModel>(MemberList.Destination);
            CreateMap<Document, DocumentModel>(MemberList.Destination)
                .ForMember(x => x.Coach, opt => opt.Ignore());
            CreateMap<Study, StudyModel>(MemberList.Destination);
            CreateMap<TimeTableItem, TimeTableItemModel>(MemberList.Destination)
                .ForMember(x => x.Club, opt => opt.Ignore())
                .ForMember(x => x.Coach, opt => opt.Ignore())
                .ForMember(x => x.Gym, opt => opt.Ignore())
                .ForMember(x => x.Study, opt => opt.Ignore());
        }
    }
}
