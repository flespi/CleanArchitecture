namespace CleanArchitecture.Domain.Common;

public interface IDeletableEntity
{
    bool IsDeleted { get; set; }
}
