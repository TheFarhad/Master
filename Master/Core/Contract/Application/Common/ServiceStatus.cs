namespace Master.Core.Contract.Application.Common;

public enum ServiceStatus
{
    Ok = 1,
    NotFound = 2,
    ValidationError = 3,
    InvalidDomainState = 4,
    Exception = 5,
}
