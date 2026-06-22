using Microsoft.Data.SqlClient;
using System.Data;

namespace ArenaStats.API.Data;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}

public class SqlConnectionFactory(IConfiguration config) : IDbConnectionFactory
{
    private readonly string _cs =
        config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not found.");

    public IDbConnection Create() => new SqlConnection(_cs);
}