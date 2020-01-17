using SharesAPI.Models;
using SharesAPI.Contracts;
using System.Collections.Generic;

namespace SharesAPI.DatabaseAccess
{
    public interface IUserRepository
    {
        User GetUser(string username);
        User AuthenticateUser(string username, string password);
        User Add(CreateUserRequest User);
        User Update(User UserChanges);
        User Delete(string username);
        User PurchaseStock(string username, Stock stock, int quantity);
        User SellStock(string username, Stock stock, int quantity);
    }
}
