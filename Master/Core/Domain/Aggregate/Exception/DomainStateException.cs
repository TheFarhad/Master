namespace Master.Core.Domain.Aggregate.Exception;

using System;
using Utilities.Extentions;

public abstract class DomainStateException : Exception
{
    public string[] Parameters { get; protected set; }

    public DomainStateException(string message, params string[] parameters) : base(message) =>
        Parameters = parameters;

    public override string ToString() =>
        Message.PlaceHolder(Parameters);
}