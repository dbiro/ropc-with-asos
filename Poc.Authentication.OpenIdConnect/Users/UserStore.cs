using System;
using System.Collections.Generic;

namespace Poc.Authentication.OpenIdConnect.Users
{
    public class UserStore
    {
        private readonly UserSecretStore userSecretStore;
        private readonly Dictionary<string, User> users;

        public User Dani => new User(Guid.Parse("F2845A10-5C9A-41EB-A91F-863BC0D2E716"), "dani");

        public UserStore()
        {
            users = new Dictionary<string, User>();
            userSecretStore = new UserSecretStore();            
        }

        public void Add(User user, string secret)
        {
            users[user.Name] = user;
            userSecretStore.Add(user.Name, secret);
        }

        public User Get(string name)
        {
            return users[name];
        }

        public bool ValidateUserSecret(string name, string secret)
        {
            return userSecretStore.Validate(name, secret);
        }
    }
}
