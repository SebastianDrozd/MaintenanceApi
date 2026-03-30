using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class PreventativeMaintenanceService
    {
        private readonly PreventativeMaintenanceRepo _repo;

        public PreventativeMaintenanceService(PreventativeMaintenanceRepo repo)
        {
            _repo = repo;
        }

        public async Task<dynamic> GetPmTemplates() 
        {
            var pms = await _repo.GetPmTemplates();
            return pms;
        }

        public async Task<dynamic> UpdatePmDates(UpdateDatesPmTemplate pm, int id ) 
        {
            var res = await _repo.UpdateTemplateDates(pm, id);
            return res;
        }

        public async Task<int> CreateNewPmTemplate(CreatePmTemplateRequest pm) 
        {
            var id = await _repo.CreatePmTemplate(pm);

            if (id != 0 && pm.Tasks.Count > 0) 
            {
                foreach (string s in pm.Tasks)  
                {
                    await _repo.CreatePmTemplateTasks(s, id);
                }
            }

            return id;
        }

        public async Task<List<ShortPmTemplateResponse>> GetShortPmTemplates() 
        {
            var pmTemplates = await _repo.GetShortPmTempaltes();
            return pmTemplates;
        }
    }
}
