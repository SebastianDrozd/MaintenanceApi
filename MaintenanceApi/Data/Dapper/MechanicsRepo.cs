using Dapper;
using MaintenanceApi.Dto.Mechanics;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class MechanicsRepo
    {
        private readonly string _MysqlConnectionString;

        public MechanicsRepo(IConfiguration config) 
        {
            _MysqlConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<List<MechanicResponse>> GetAllMechanics() 
        {
            var sql = @"Select Id, FirstName,LastName from mechanics";

            await using var connection = new MySqlConnection(_MysqlConnectionString);

            return (await connection.QueryAsync<MechanicResponse>(sql)).AsList();
        }
    }
}
