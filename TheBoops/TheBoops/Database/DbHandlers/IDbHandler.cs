using TheBoops.Database.Tables;
using TheBoops.Global;

namespace TheBoops.Database.DbHandlers
{
    public interface IDbHandler
    {
        Task<object> GetTableData(string tableName, List<SearchQuery> searchQueries = null);
        Task<bool> SaveTableData(string tableName, object tableData);
        Task<bool> AddRecordToDb(string RecordType, string Name, string Value = "", string ID = "");
        Task<bool> AddRecordToDb(string RecordType, object tableData);
    }
}
