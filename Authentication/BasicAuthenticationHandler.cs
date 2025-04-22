
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Gallery.Models;
using Gallery.Interfaces;
using MyGalleryApi.Models;

namespace Gallery.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{

    private readonly IUserRepository _userRepo;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, IUserRepository userRepo)
        : base(options, logger, encoder, clock)
    {
        _userRepo = userRepo;
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Gallery API""");

        var authHeader = base.Request.Headers["Authorization"].ToString();

        LoginModel? loginModel = null;

        if (!string.IsNullOrEmpty(authHeader))
        {
            var encodedHeader = authHeader.Split(' ')[1];

            var bytes = Convert.FromBase64String(encodedHeader);

            var credentials = Encoding.UTF8.GetString(bytes);

            loginModel = new LoginModel(credentials.Split(":")[0], credentials.Split(":")[1]);

            var user = _userRepo.GetByEmailAsync(loginModel.Email).Result;

            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash);

            if (isValidPassword)
            {
                var claims = new[]
                {
                    new Claim("name", $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, "Basic");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                Response.StatusCode = 401;

                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
            }

        }
        else
        {
            Response.StatusCode = 401;

            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }


    } 
}