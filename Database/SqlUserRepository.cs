using Microsoft.EntityFrameworkCore;
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
            return Context.Users.Include(own => own.StockOwned).FirstOrDefault(s => s.Username == username);
        }
    
        public User AuthenticateUser(string username, string password)
        {
            User claimedUser = GetUser(username);
            if (claimedUser.Password != password) return null;
            return claimedUser;
        }

        public User Add(CreateUserRequest User)
        {
            User newUser = new User();
            newUser.Username = User.Username;
            newUser.Password = User.Password;
            newUser.Wallet = 1000;
            newUser.StockOwned = new List<StockOwnership>();

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
            User User = GetUser(username);
            if (User != null)
            {
                Context.Users.Remove(User);
                Context.SaveChanges();
            }
            return User;
        }

        public User PurchaseStock(string username, Stock stock, int quantity)
        {
            User user = GetUser(username);

            if (user.Wallet - (stock.Price * quantity) < 0) return null;

            user.Wallet -= stock.Price * quantity;


            StockOwnership ownership = user.StockOwned.FirstOrDefault(o => o.Symbol == stock.Symbol);
            if (ownership == null)
            {
                StockOwnership newOwnership = new StockOwnership();
                newOwnership.Symbol = stock.Symbol;
                newOwnership.AmountOwned = quantity;
                Context.StockOwnerships.Add(newOwnership);
                user.StockOwned.Add(newOwnership);
            }
            else
            {
                StockOwnership existingOwnership = Context.StockOwnerships.FirstOrDefault(o => o.Id == ownership.Id);
                existingOwnership.AmountOwned += quantity;
            }

            Context.SaveChanges();
            User updatedUser = GetUser(username);
            return updatedUser;
        }

        public User SellStock(string username, Stock stock, int quantity)
        {
            User user = GetUser(username);

            user.Wallet += stock.Price * quantity;

            StockOwnership ownership = user.StockOwned.FirstOrDefault(o => o.Symbol == stock.Symbol);
            StockOwnership existingOwnership = Context.StockOwnerships.FirstOrDefault(o => o.Id == ownership.Id);
            existingOwnership.AmountOwned -= quantity;
            if (existingOwnership.AmountOwned == 0) user.StockOwned.Remove(existingOwnership);

            Context.SaveChanges();
            User updatedUser = GetUser(username);
            return updatedUser;
        }
    }
}
