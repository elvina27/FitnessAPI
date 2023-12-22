using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Fitness.Context.Contracts.Enums;
using Fitness.Context.Contracts.Models;
using Fitness.Services.Contracts.Enums;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using System.Net.Sockets;

namespace Fitness.Services.AutoMappers
{
    public class ServiceMapper : Profile
    {
        public ServiceMapper()
        {
            CreateMap<Category, CategoryModel>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();
            CreateMap<DocumentTypes, DocumentTypesModel>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();

            CreateMap<Club, ClubModel>(MemberList.Destination).ReverseMap();
            CreateMap<Coach, CoachModel>(MemberList.Destination).ReverseMap();
            CreateMap<Gym, GymModel>(MemberList.Destination).ReverseMap();
            CreateMap<Document, DocumentModel>(MemberList.Destination)
                .ForMember(x => x.Coach, opt => opt.Ignore()).ReverseMap();
            CreateMap<Study, StudyModel>(MemberList.Destination).ReverseMap();
            CreateMap<TimeTableItem, TimeTableItemModel>(MemberList.Destination)
                .ForMember(x => x.Club, opt => opt.Ignore())
                .ForMember(x => x.Coach, opt => opt.Ignore())
                .ForMember(x => x.Gym, opt => opt.Ignore())
                .ForMember(x => x.Study, opt => opt.Ignore()).ReverseMap();


            CreateMap<TimeTableItemRequestModel, TimeTableItem>(MemberList.Destination)
                .ForMember(x => x.Club, opt => opt.Ignore())
                .ForMember(x => x.Coach, opt => opt.Ignore())
                .ForMember(x => x.Gym, opt => opt.Ignore())
                .ForMember(x => x.Study, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore());


            CreateMap<DocumentRequestModel, Document>(MemberList.Destination)
                .ForMember(x => x.Coach, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.DeletedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore());
        }
    }
}
