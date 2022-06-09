namespace CleanArchitecture.Application.Common.Exceptions;

public class TransactionCompletedException : InvalidOperationException
{
    public TransactionCompletedException() : base("This transaction has completed; it is no longer usable.") { }
}
