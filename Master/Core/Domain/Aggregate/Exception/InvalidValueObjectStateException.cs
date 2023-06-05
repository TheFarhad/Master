namespace Master.Core.Domain.Aggregate.Exception;

public class InvalidValueObjectStateException : DomainStateException
{
    public InvalidValueObjectStateException(string message, params string[] parameters) : base(message) { }
}

