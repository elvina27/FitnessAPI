﻿using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Fitnes.API.Models.CreateRequest;
using Fitnes.API.Models.Response;
using Fitness.Api.Enums;
using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
using Fitness.API.Models.Response;
using Fitness.Context.Contracts.Enums;
using Fitness.Services.Contracts.Enums;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;

namespace Fitness.Api.AutoMappers
{
    /// <summary>
    /// Маппер
    /// </summary>
    public class APIMappers : Profile
    {
        public APIMappers()
        {
            CreateMap<CategoryModel, CategoryResponse>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();
            CreateMap<DocumentTypesModel, DocumentTypesResponse>().ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();

            CreateMap<CreateClubRequest, ClubModel>(MemberList.Destination)
            .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateCoachRequest, CoachModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateGymRequest, GymModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateStudyRequest, StudyModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateDocumentRequest, DocumentRequestModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore());
          
            CreateMap<ClubRequest, ClubModel>(MemberList.Destination);
            CreateMap<CoachRequest, CoachModel>(MemberList.Destination);
            CreateMap<DocumentRequest, DocumentModel>(MemberList.Destination)
                .ForMember(x => x.Coach, opt => opt.Ignore());
            CreateMap<GymRequest, GymModel>(MemberList.Destination);
            CreateMap<StudyRequest, StudyModel>(MemberList.Destination);
            CreateMap<TimeTableItemRequest, TimeTableItemModel>(MemberList.Destination)
                .ForMember(x => x.Club, opt => opt.Ignore())
                .ForMember(x => x.Coach, opt => opt.Ignore())
                .ForMember(x => x.Gym, opt => opt.Ignore())
                .ForMember(x => x.Study, opt => opt.Ignore());

            CreateMap<TimeTableItemRequest, TimeTableItemRequestModel>(MemberList.Destination);
            CreateMap<CreateTimeTableItemRequest, TimeTableItemRequestModel>(MemberList.Destination)
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ClubModel, ClubResponse>(MemberList.Destination);
            CreateMap<CoachModel, CoachResponse>(MemberList.Destination)
                .ForMember(x => x.Name, opt => opt.MapFrom(src => $"{src.Surname} {src.Name} {src.Patronymic}"));
            CreateMap<DocumentModel, DocumentResponse>(MemberList.Destination);
            CreateMap<GymModel, GymResponse>(MemberList.Destination);
            CreateMap<StudyModel, StudyResponse>(MemberList.Destination);
            CreateMap<TimeTableItemModel, TimeTableItemResponse>(MemberList.Destination);

            CreateMap<DocumentRequest, DocumentRequestModel>(MemberList.Destination);
        }
    }
}
