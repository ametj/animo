using Animo.Web.Core.Dto;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using AutoMapper;

namespace Animo.Web.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(u => u.Password, opt => opt.Ignore());

            CreateMap<User, UserListDto>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<Role, RoleDto>();
        }
    }
}