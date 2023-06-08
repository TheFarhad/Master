namespace Master.Utilities.Services.Abstraction.Identity;
public interface IUserService
{
    string Id();
    string Ip();
    string FirstName();
    string LastName();
    string Username();
    string Agent();
    string Claim(string claimType);
    bool IsCurrentUser(string userId);
    string IdOrDefault();
}
