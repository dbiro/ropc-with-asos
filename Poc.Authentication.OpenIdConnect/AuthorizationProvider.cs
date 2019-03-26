using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Poc.Authentication.OpenIdConnect.Users;

namespace Poc.Authentication.OpenIdConnect
{
    public sealed class AuthorizationProvider : OpenIdConnectServerProvider
    {
        private readonly UserStore userStore;

        public AuthorizationProvider(UserStore userStore)
        {
            this.userStore = userStore;
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
            if (userStore.ValidateUserSecret(context.Request.Username, context.Request.Password))
            {
                var user = userStore.Get(context.Request.Username);

                var identity = new ClaimsIdentity(OpenIdConnectConstants.Schemes.Bearer);
                // Note: the subject claim is always included in both identity and
                // access tokens, even if an explicit destination is not specified
                identity.AddClaim(OpenIdConnectConstants.Claims.Subject, user.Id.ToString());
                // When adding custom claims, you MUST specify one or more destinations.
                identity.AddClaim(ClaimTypes.Name, user.Name, OpenIdConnectConstants.Destinations.AccessToken);                

                var principal = new ClaimsPrincipal(identity);
                                
                var ticket = new AuthenticationTicket(principal, OpenIdConnectConstants.Schemes.Bearer);                
                //ticket.SetScopes(
                //    // access reresh token
                //    OpenIdConnectConstants.Scopes.OfflineAccess,
                //    // access identity token
                //    OpenIdConnectConstants.Scopes.OpenId);

                context.Validate(ticket);
            }
            else
            {
                context.Reject(OpenIdConnectConstants.Errors.InvalidGrant);
            }

            return Task.CompletedTask;
        }
    }
}
