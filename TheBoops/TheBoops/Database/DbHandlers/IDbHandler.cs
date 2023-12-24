using TheBoops.Database.Tables;

namespace TheBoops.Database.DbHandlers
{
    public interface IDbHandler
    {
        Task<bool> UserExists(string userName);
        Task<UsersDb> Login(string userName, string password);
    }
}
