//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using System.DirectoryServices.AccountManagement;

//namespace Poc.Authentication.OpenIdConnect
//{
//    public class ActiveDirectoryAuthenticator
//    {
//        public ClaimsIdentity Authenticate(string username, string password)
//        {            
//            // TODO: move domain parameter to config
//            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, configuration.AuthorizedDomain, queryUser, queryPwd))
//            {
//                if (principalContext.ValidateCredentials(userName, password))
//                {
//                    using (var userPrincipal = UserPrincipal.FindByIdentity(principalContext, userName))
//                    {
//                        if (userPrincipal == null)
//                        {
//                            throw new UserPrincipalNotFoundException(userName);
//                        }
//                        else
//                        {
//                            if (!ValidateUserGroups(userPrincipal) || !ValidateUserPrincipal(userPrincipal))
//                            {
//                                return new UserAuthenticationResult();
//                            }
//                            else
//                            {
//                                ClaimsIdentity identity = await FindOrCreateUser(userPrincipal);                                
//                                return new UserAuthenticationResult(identity);
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    return null;
//                }
//            }
//        }
//    }
//}
