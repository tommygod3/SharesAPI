using SharesAPI.ExternalAPI;
using SharesAPI.Models;
using SharesAPI.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SharesAPI.DatabaseAccess
{
    public class SqlUserRepository : IUserRepository
    {
        private AppDbContext Context { get; }

        public SqlUserRepository(AppDbContext context)
        {
            Context = context;
        }

        public User GetUser(string username)
        {
            return Context.Users.FirstOrDefault(s => s.Username == username);
        }
    
        public User AuthenticateUser(string username, string password)
        {
            User claimedUser = Context.Users.FirstOrDefault(s => s.Username == username);
            if (claimedUser.Password != password) return null;
            return claimedUser;
        }

        public User Add(CreateUserRequest User)
        {
            User newUser = new User();
            newUser.Username = User.Username;
            newUser.Password = User.Password;
            newUser.Wallet = 1000;

            Context.Users.Add(newUser);
            Context.SaveChanges();
            return newUser;
        }

        public User Update(User UserChanges)
        {
            User User = GetUser(UserChanges.Username);
            if (User != null)
            {
                User.Username = UserChanges.Username;
                User.Password = UserChanges.Password;
                User.Wallet = UserChanges.Wallet;
                User.StockOwned = UserChanges.StockOwned;
                
                Context.SaveChanges();
            }
            return User;
        }

        public User Delete(string username)
        {
            User User = Context.Users.FirstOrDefault(s => s.Username == username);
            if (User != null)
            {
                Context.Users.Remove(User);
                Context.SaveChanges();   
            }
            return User;
        }
    }
}
