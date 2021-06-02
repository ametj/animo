using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IBaseDbContext _dbContext;

        public UserService(IMapper mapper,
            UserManager<User> userManager,
            IBaseDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<PagedList<UserListDto>> GetUsersAsync(PagedListRequest request)
        {
            var query = _userManager.Users
                .Where(
                    !request.Filter.IsNullOrEmpty(),
                    predicate => predicate.NormalizedUserName.Contains(request.Filter.ToUpper()) ||
                                 predicate.NormalizedEmail.Contains(request.Filter.ToUpper()))
                .OrderBy(request.SortBy, nameof(User.Id));

            var usersCount = await query.CountAsync();
            var users = query.PagedBy(request.PageIndex, request.PageSize).ToList();
            var userListOutput = _mapper.Map<List<UserListDto>>(users);

            return new(userListOutput, usersCount);
        }

        public async Task<GetUserForCreateOrUpdate> GetUserForCreateOrUpdateAsync(int id)
        {
            var allRoles = _mapper.Map<List<RoleDto>>(_dbContext.Roles).OrderBy(r => r.Name).ToList();

            if (id == 0)
            {
                return new GetUserForCreateOrUpdate(allRoles);
            }

            return await GetUserForUpdateAsync(id, allRoles);
        }

        public async Task<IdentityResult> AddUserAsync(CreateOrUpdateUser request)
        {
            var user = new User
            {
                Id = request.User.Id,
                UserName = request.User.UserName,
                Email = request.User.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, request.User.Password);
            if (createUserResult.Succeeded)
            {
                GrantRolesToUser(request.GrantedRoleIds, user);
            }

            return createUserResult;
        }

        public async Task<IdentityResult> EditUserAsync(CreateOrUpdateUser request)
        {
            var user = await _userManager.FindByIdAsync(request.User.Id.ToString());

            if (user == null) return null;

            if (user.UserName == request.User.UserName && user.Id != request.User.Id)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNameAlreadyExist",
                    Description = "This user name already exist!"
                });
            }

            if (!request.User.Password.IsNullOrEmpty())
            {
                var changePasswordResult = await ChangePassword(user, request.User.Password);
                if (!changePasswordResult.Succeeded)
                {
                    return changePasswordResult;
                }
            }

            return await UpdateUser(request, user);
        }

        public async Task<IdentityResult> RemoveUserAsync(int id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null;

            if (DefaultUsers.All().Select(u => u.UserName).Contains(user.UserName))
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "CannotRemoveSystemUser",
                    Description = "You cannot remove system user!"
                });
            }

            var removeUserResult = await _userManager.DeleteAsync(user);
            if (!removeUserResult.Succeeded)
            {
                return removeUserResult;
            }

            user.UserRoles.Clear();

            return removeUserResult;
        }

        private void GrantRolesToUser(IEnumerable<int> grantedRoleIds, User user)
        {
            foreach (var roleId in grantedRoleIds)
            {
                _dbContext.UserRoles.Add(new UserRole
                {
                    RoleId = roleId,
                    UserId = user.Id
                });
            }
        }

        private async Task<IdentityResult> ChangePassword(User user, string password)
        {
            var changePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (changePasswordResult.Succeeded)
            {
                changePasswordResult = await _userManager.AddPasswordAsync(user, password);
            }

            return changePasswordResult;
        }

        private async Task<IdentityResult> UpdateUser(CreateOrUpdateUser request, User user)
        {
            user.UserName = request.User.UserName;
            user.Email = request.User.Email;
            user.UserRoles.Clear();
            user.SecurityStamp = Guid.NewGuid().ToString();

            var updateUserResult = await _userManager.UpdateAsync(user);
            if (updateUserResult.Succeeded)
            {
                GrantRolesToUser(request.GrantedRoleIds, user);
            }

            return updateUserResult;
        }

        private async Task<GetUserForCreateOrUpdate> GetUserForUpdateAsync(int id, List<RoleDto> allRoles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return null;

            var userDto = _mapper.Map<UserDto>(user);
            var grantedRoles = user.UserRoles.Select(ur => ur.Role.Id);

            return new GetUserForCreateOrUpdate(allRoles)
            {
                User = userDto,
                GrantedRoleIds = grantedRoles,
            };
        }
    }
}