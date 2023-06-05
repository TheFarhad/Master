namespace Master.Core.Domain.Common.Enumm;

public abstract class Enumer
{
    public string Value { get; private set; }
    public abstract string Display { get; }

    public Enumer(string value) => Value = value;
}
