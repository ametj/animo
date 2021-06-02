using Animo.Web.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetGrantedPermissionsAsync(string userNameOrEmail);

        Task<bool> IsUserGrantedToPermissionAsync(string userNameOrEmail, string permissionName);
    }
}