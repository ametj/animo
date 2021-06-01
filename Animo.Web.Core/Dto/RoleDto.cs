using System.Collections.Generic;

namespace Animo.Web.Core.Dto
{
    public record RoleDto() : IdNameDto();

    public record CreateOrUpdateRole(RoleDto Role, ICollection<int> GrantedPermissionIds);

    public record GetRoleForCreateOrUpdate(RoleDto Role, ICollection<PermissionDto> AllPermisions, ICollection<int> GrantedPermissions);
}