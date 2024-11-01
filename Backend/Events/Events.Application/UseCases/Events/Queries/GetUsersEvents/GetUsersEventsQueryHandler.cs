using AutoMapper;
using Events.Application.UseCases.Events.DTOs;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace Events.Application.UseCases.Events.Queries.GetUsersEvents;

public class GetUsersEventsQueryHandler : EventsQueryHandlerBase, IRequestHandler<GetUsersEventsQuery, EventsResponseDTO>
{

    private readonly UserManager<ApplicationUser> _userManager;
    public GetUsersEventsQueryHandler(IMapper mapper, IEventRepository eventRepository,
        UserManager<ApplicationUser> userManager) : base(mapper, eventRepository)
    {
        _userManager = userManager;
    }

    public async Task<EventsResponseDTO> Handle(GetUsersEventsQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await _userManager.FindByIdAsync(query.UserId);
        if (userEntity == null)
            throw new NotFoundException(nameof(userEntity), query.UserId);

        var events = await _eventRepository.GetEventsByUserIdAsync(query.UserId, query.PageNumber, query.PageSize, cancellationToken);
        var totalCount = await _eventRepository.GetUserEventsCountAsync(query.UserId, cancellationToken);
        return new EventsResponseDTO
        {
            Events = _mapper.Map<IEnumerable<EventResponseDTO>>(events),
            TotalCount = totalCount
        };
    }
}
