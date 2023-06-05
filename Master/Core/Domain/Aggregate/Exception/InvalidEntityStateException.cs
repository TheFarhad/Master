namespace Master.Core.Domain.Aggregate.Exception;

public class InvalidEntityStateException : DomainStateException
{
    public InvalidEntityStateException(string message, params string[] parameters) : base(message) { }
}

