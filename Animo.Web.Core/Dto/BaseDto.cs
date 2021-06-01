namespace Animo.Web.Core.Dto
{
    public abstract record BaseDto() : BaseDto<int>();

    public abstract record BaseDto<T>()
    {
        public T Id { get; init; }
    }

    public record IdNameDto() : IdNameDto<int>();

    public record IdNameDto<T>() : BaseDto<T>()
    {
        public string Name { get; init; }
    }

    public record IdValueDto() : IdValueDto<int>();

    public record IdValueDto<T>() : BaseDto<T>()
    {
        public string Value { get; init; }

        public IdValueDto(T id, string name) : this() => (Id, Value) = (id, name);
    }
}