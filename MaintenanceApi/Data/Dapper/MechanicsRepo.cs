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

        public async Task<MechanicResponse> GetMechanicById(int id) 
        {
            var sql = @"Select * from mechanics where Id = @id";
            await using var connection = new MySqlConnection(_MysqlConnectionString);
            return (await connection.QueryFirstAsync<MechanicResponse>(sql, new 
            {
                id
            }));
        }

        public async Task<int> CreateNewMechanic(CreateMechanicRequest mechanic) 
        {
            var sql = @"insert into mechanics (FirstName,Lastname,Shift,Department,Notes)
                        values(@FirstName,@LastName,@Shift,@Department,@Notes)";
            await using var connection = new MySqlConnection(_MysqlConnectionString);
            return await connection.ExecuteAsync(sql, new 
            {
                mechanic.FirstName,
                mechanic.LastName,
          
                mechanic.Shift,
                mechanic.Department,
                mechanic.Notes
            });
        }

        public async Task<List<FullMechanicResponse>> GetAllMechanicsFull() 
        {
            string sql = @"select * from mechanics";

            await using var connection = new MySqlConnection(_MysqlConnectionString);
            return (await connection.QueryAsync<FullMechanicResponse>(sql)).AsList();
        }
    }
}
