using AutoMapper;
using Events.Application.UseCases.Events.Commands.CreateEvent;
using Events.Application.UseCases.Events.Commands.UpdateEvent;
using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Participants.Commands.RegisterParticipant;
using Events.Application.UseCases.Participants.DTOs;
using Events.Application.UseCases.Roles.DTOs;
using Events.Application.UseCases.Users.DTOs;
using Events.Core.Entities;

namespace Events.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventResponseDTO>();
            CreateMap<EventResponseDTO, Event>();
            CreateMap<CreateEventCommand, Event>()
                .ForMember(dest => dest.Image, opt => opt.ConvertUsing(new Base64ToByteArrayConverter(), src => src.Image));          ;
            CreateMap<UpdateEventCommand, Event>();
            CreateMap<RegisterParticipantCommand, EventParticipant>();
            CreateMap<EventParticipantResponseDTO, EventParticipant>();
            CreateMap<EventParticipant, EventParticipantResponseDTO>();
            CreateMap<ApplicationRole, RoleNameResponseDTO>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<ApplicationUser, UserDataResponseDTO>();
        }
    }
}
