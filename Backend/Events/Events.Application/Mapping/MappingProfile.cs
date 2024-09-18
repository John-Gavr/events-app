using AutoMapper;
using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Responces;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Responses;
using Events.Application.DTOs.Roles.Responses;
using Events.Application.DTOs.Users.Responses;
using Events.Core.Entities;

namespace Events.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventResponse>();
            CreateMap<EventResponse, Event>();
            CreateMap<CreateEventRequest, Event>()
                .ForMember(dest => dest.Image, opt => opt.ConvertUsing(new Base64ToByteArrayConverter(), src => src.Image));          ;
            CreateMap<UpdateEventRequest, Event>();
            CreateMap<RegisterParticipantRequest, EventParticipant>();
            CreateMap<EventParticipantResponse, EventParticipant>();
            CreateMap<EventParticipant, EventParticipantResponse>();
            CreateMap<ApplicationRole, RoleResponse>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<ApplicationUser, UserDataResponse>();
        }
    }
}
