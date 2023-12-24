using TheBoops.Database.Tables;
using TheBoops.Global;

namespace TheBoops.Database.DbHandlers
{
    public interface IDbHandler
    {
        /// <summary>
        /// Gets object data from table
        /// </summary>
        /// <param name="tableName">Name of table getting the data from</param>
        /// <param name="isAdmin">Is this admin access, Default Admin</param>
        /// <param name="searchvalue">Value being searched for</param>
        /// <returns></returns>
        Task<object> GetDataFromTable(string tableName, string isAdmin = Constants.isAdmin, string searchvalue = "");
        /// <summary>
        /// Sets object data in table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        Task<bool> SetDataInTable(string tableName, string isAdmin, object tableData);
    }
}
