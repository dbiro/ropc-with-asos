using System;
using System.Collections.Generic;

namespace Poc.Authentication.OpenIdConnect.Users
{
    public class UserSecretStore
    {
        private readonly Dictionary<string, string> secrets;

        public UserSecretStore()
        {
            secrets = new Dictionary<string, string>();
        }

        public void Add(string name, string secret)
        {
            secrets[name] = secret;
        }

        public bool Validate(string name, string secret)
        {
            if (secrets.TryGetValue(name, out string _secret) && secret.Equals(_secret))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
