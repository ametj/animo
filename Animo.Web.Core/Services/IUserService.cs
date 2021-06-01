using Animo.Web.Core.Dto;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public interface IUserService
    {
        Task<PagedList<UserListDto>> GetUsersAsync(PagedListRequest request);

        Task<GetUserForCreateOrUpdate> GetUserForCreateOrUpdateAsync(int id);

        Task<IdentityResult> AddUserAsync(CreateOrUpdateUser dto);

        Task<IdentityResult> EditUserAsync(CreateOrUpdateUser dto);

        Task<IdentityResult> RemoveUserAsync(int id);
    }
}