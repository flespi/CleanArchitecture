namespace CleanArchitecture.Application.Common.Exceptions;

public class IdempotencyException : Exception
{
    private const string Msg = "Operation already requested.";

    public IdempotencyException()
        : base(Msg)
    {
    }

    public IdempotencyException(Exception innerException)
        : base(Msg, innerException)
    {
    }
}
