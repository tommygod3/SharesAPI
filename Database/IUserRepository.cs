using SharesAPI.Models;
using SharesAPI.Contracts;
using System.Collections.Generic;

namespace SharesAPI.DatabaseAccess
{
    public interface IUserRepository
    {
        User GetUser(string username);
        User Add(CreateUserRequest user);
        User Update(User userChanges);
        User Delete(string username);
        User PurchaseStock(string username, Stock stock, int quantity);
        User SellStock(string username, Stock stock, int quantity);
        string HashPassword(string password);
        bool VerifyPassword(string username, string password);
    }
}
