using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;

namespace Poc.Authentication.OpenIdConnect
{
    public sealed class AuthorizationProvider : OpenIdConnectServerProvider
    {
        public AuthorizationProvider()
        {
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType())
            {
                context.Reject(OpenIdConnectConstants.Errors.UnsupportedGrantType, "Only supports resource owner password credentials grant!");
            }
            else
            {
                // Since there's only one application and since it's a public client
                // (i.e a client that cannot keep its credentials private), call Skip()
                // to inform the server the request should be accepted without 
                // enforcing client authentication.
                context.Skip();                
            }

            return Task.CompletedTask;
        }

        public override Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (context.Request.Username == "dani" && context.Request.Password == "iszap")
            {
                var identity = new ClaimsIdentity(context.Scheme.Name);
                // Note: the subject claim is always included in both identity and
                // access tokens, even if an explicit destination is not specified
                identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Subject, context.Request.Username));
                // TODO: When adding custom claims, you MUST specify one or more destinations.
                // identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Name, context.Request.Username, OpenIdConnectConstants.Destinations.AccessToken)); 

                var principal = new ClaimsPrincipal(identity);

                context.Validate(principal);
            }
            else
            {
                context.Reject(OpenIdConnectConstants.Errors.InvalidGrant);
            }

            return Task.CompletedTask;
        }
    }
}
