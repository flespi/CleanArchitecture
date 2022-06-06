namespace CleanArchitecture.Application.Common.Exceptions;

public class ConcurrencyException : Exception
{
    private const string Msg = "Data may have been modified or deleted since entities were loaded.";

    public ConcurrencyException()
        : base(Msg)
    {
    }

    public ConcurrencyException(Exception innerException)
        : base(Msg, innerException)
    {
    }
}
