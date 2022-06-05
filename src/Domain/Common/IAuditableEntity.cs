namespace CleanArchitecture.Domain.Common;

public interface IAuditableEntity
{
    Auditability Audit { get; set; }
}
