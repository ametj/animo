using System.Collections.Generic;

namespace Animo.Web.Core.Dto
{
    public record UserDto() : UserListDto()
    {
        public string Password { get; init; }
    }

    public record UserListDto() : BaseDto()
    {
        public string UserName { get; init; }
        public string Email { get; init; }
    }

    public record CreateOrUpdateUser(UserDto User, IEnumerable<int> GrantedRoleIds);

    public record GetUserForCreateOrUpdate(IEnumerable<RoleDto> AllRoles)
    {
        public UserDto User { get; init; }
        public IEnumerable<int> GrantedRoleIds { get; init; }
    }
}