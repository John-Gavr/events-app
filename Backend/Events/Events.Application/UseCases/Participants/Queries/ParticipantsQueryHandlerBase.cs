using AutoMapper;
using Events.Core.Entities;
using Events.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Participants.Queries;

public class ParticipantsQueryHandlerBase
{
    protected readonly IEventParticipantRepository _eventParticipantRepository;
    protected readonly IMapper _mapper;
    protected readonly UserManager<ApplicationUser> _userManager;

    public ParticipantsQueryHandlerBase(IMapper mapper, IEventParticipantRepository eventParticipantRepository, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _eventParticipantRepository = eventParticipantRepository;
        _userManager = userManager;
    }
}
