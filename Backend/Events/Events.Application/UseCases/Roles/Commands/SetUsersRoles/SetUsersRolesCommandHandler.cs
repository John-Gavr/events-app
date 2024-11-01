using AutoMapper;
using Events.Application.UseCases.Roles.Queries;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.Roles.Commands.SetUsersRoles
{
    public class SetUsersRolesCommandHandler : RolesQueryHandlerBase, IRequestHandler<SetUsersRolesCommand, Unit>
    {
        public SetUsersRolesCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper) : base(userManager, roleManager, mapper)
        { }

        public async Task<Unit> Handle(SetUsersRolesCommand command, CancellationToken cancellationToken)
        {
            var userEntity = await _userManager.FindByIdAsync(command.UserId);
            if (userEntity == null)
                throw new NotFoundException(nameof(userEntity), command.UserId);

            var role = await _roleManager.FindByNameAsync(command.RoleName);
            if (role == null)
                throw new NotFoundException(nameof(role), command.RoleName);

            cancellationToken.ThrowIfCancellationRequested();

            await _userManager.AddToRoleAsync(userEntity, command.RoleName);

            return Unit.Value;
        }
    }
}
