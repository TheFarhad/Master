namespace Master.Utilities.Services.Implementation.Identity;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Extentions;
using Abstraction.Identity;

public class UserService : IUserService
{
    private readonly HttpContext _context;
    private readonly UserServiceConfig _configs;

    public UserService(IHttpContextAccessor contextAccessor, IOptions<UserServiceConfig> options)
    {
        if (contextAccessor.IsNull() || contextAccessor.HttpContext.IsNull())
            throw new ArgumentNullException(nameof(contextAccessor));

        _context = contextAccessor.HttpContext;
        _configs = options.Value;
    }

    public string Id() => _context.GetClaim(ClaimTypes.NameIdentifier);
    public string Ip() => _context?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    public string FirstName() => Claim(ClaimTypes.GivenName);
    public string LastName() => Claim(ClaimTypes.Surname);
    public string Username() => Claim(ClaimTypes.Name);
    public string Agent() => _context?.Request.Headers["User-Agent"] ?? "Unknown";
    public string Claim(string claimType) => _context.GetClaim(claimType);
    public string IdOrDefault()
    {
        var id = Id();
        return id.IsNull() ? _configs.DefaultId : id;
    }
    public bool IsCurrentUser(string userId) => String.Equals(Id(), userId, StringComparison.OrdinalIgnoreCase);
}

public class UserServiceConfig
{
    public string DefaultId { get; set; } = String.Empty;
}

#region api setting

// program.cs
// builder.Services.AddHttpContextAccessor();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Description = "Bearer Authentication with JWT Token",
//        Type = SecuritySchemeType.Http,
//    });

//options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Id = "Bearer",
//                    Type = ReferenceType.SecurityScheme,
//                }
//            },
//            new List<string>()
//        }
//    });
//});
//builder.Services.UserServiceWireupExtensions(c =>
//{
//    c.DefaultUserId = "1";
//});

// appsetting.json
// "Identity": {
//    "Authority": "https://demo.duendesoftware.com/",
//    "ClientId": "interactive.confidential",
//    "ClientSecret": "secret",
//    "Scope": [
//      "openid",
//      "profile",
//      "email",
//      "api",
//      "offline_access"
//    ],

#endregion

#region mvc setting

// program.cs
// builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "cookie";
//    options.DefaultChallengeScheme = "oidc";
//})
//    .AddCookie("cookie")
//    .AddOpenIdConnect("oidc", options =>
//    {
//        builder.Configuration.GetSection("Identity").Bind(options);
//    });

//builder.Services.AddHttpContextAccessor();
//builder.Services.UserServiceWireupExtensions(c =>
//{
//    c.DefaultUserId = "1";
//});

// appsetting.json
//"Identity": {
//    "Authority": "https://demo.duendesoftware.com/",
//    "ClientId": "interactive.confidential",
//    "ClientSecret": "secret",
//    "Scope": [
//      "openid",
//      "profile",
//      "email",
//      "api",
//      "offline_access"
//    ],

#endregion
