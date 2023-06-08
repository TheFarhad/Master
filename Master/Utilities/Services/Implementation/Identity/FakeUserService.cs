namespace Master.Utilities.Services.Implementation.Identity;

using Abstraction.Identity;

public class FakeUserService : IUserService
{
    private const string _defaultUserId = "1";

    public string FirstName() => "FirstName";

    public string LastName() => "LastName";
    public string Agent() => _defaultUserId;
    public string Id() => _defaultUserId;
    public string IdOrDefault() => _defaultUserId;
    public string IdOrDefault(string defaultValue) => defaultValue;
    public string Ip() => "0.0.0.0";
    public string Username() => "Username";
    public bool HasAccess(string access) => true;
    public string Claim(string claimType) => claimType;
    public bool IsCurrentUser(string userId) => true;
}
