using Microsoft.EntityFrameworkCore;
using SharesAPI.ExternalAPI;
using SharesAPI.Models;
using SharesAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;

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

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string username, string password)
        {
            string savedPasswordHash = GetUser(username).Password;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i=0; i < 20; i++)
                if (hashBytes[i+16] != hash[i])
                    return false;
            return true;
        }

        public User Add(CreateUserRequest user)
        {
            User existingUser = GetUser(user.Username);
            if (existingUser != null) return null;

            User newUser = new User();
            newUser.Username = user.Username;
            newUser.Password = HashPassword(user.Password);
            newUser.Wallet = 1000;
            newUser.StockOwned = new List<StockOwnership>();

            Context.Users.Add(newUser);
            Context.SaveChanges();
            return newUser;
        }

        public User Update(User userChanges)
        {
            User User = GetUser(userChanges.Username);
            if (User != null)
            {
                User.Username = userChanges.Username;
                User.Password = HashPassword(userChanges.Password);
                User.Wallet = userChanges.Wallet;
                User.StockOwned = userChanges.StockOwned;
                
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
