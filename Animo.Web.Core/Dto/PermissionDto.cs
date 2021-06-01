namespace Animo.Web.Core.Dto
{
    public record PermissionDto() : IdNameDto()
    {
        public string DisplayName { get; init; }
    }
}