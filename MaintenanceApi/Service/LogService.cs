using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Logs;

namespace MaintenanceApi.Service
{
    public class LogService
    {
        private readonly LogsRepo _logsRepo;

        public LogService(LogsRepo logsRepo) 
        {
            _logsRepo = logsRepo;
        }


        public async Task<int> CreateNewEvent(CreateNewLogRequest log) 
        {
            var res = await _logsRepo.CreateNewEvent(log);
            return res;
        }

        public async Task<List<LogResponse>> GetLogs() 
        {
            var res = await _logsRepo.GetLogs();
            return res;
        }
    }
}
