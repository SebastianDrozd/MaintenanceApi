using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Logs;
using MaintenanceApi.Dto.Pagination;

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

        public async Task<PaginatedList<LogResponse>> GetLogs(int page, int pageSize, string searchTerm, string type, string action) 
        {
            if (searchTerm.IsWhiteSpace())
            {
                Console.WriteLine("String is whitespace");
                searchTerm = string.Empty;
            }
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var logs = await _logsRepo.GetLogsQuery(page,pageSize,searchTerm,type,action);
            var count = await _logsRepo.CountLogs(searchTerm,type,action);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            return new PaginatedList<LogResponse>
            {
                Items = logs,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = totalPages
            };
          
        }
    }
}
