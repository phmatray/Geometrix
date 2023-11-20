using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Geometrix.WebApi.Modules.Common;

/// <summary>
/// Test authentication handler.
/// </summary>
public sealed class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options">The monitor.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="encoder">The encoder.</param>
    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <summary>
    /// Handle authenticate async.
    /// </summary>
    /// <returns>The authenticate result.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Claim[] claims =
        {
            new(ClaimTypes.NameIdentifier, "test"),
            new(ClaimTypes.Name, "test"),
            new("id", "92b93e37-0995-4849-a7ed-149e8706d8ef")
        };

        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket))
            .ConfigureAwait(false);
    }
}