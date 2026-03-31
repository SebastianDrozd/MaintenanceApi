using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Mechanics;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class PreventativeMaintenanceService
    {
        private readonly PreventativeMaintenanceRepo _repo;
        private readonly MechanicsRepo _mechanicsRepo;

        public PreventativeMaintenanceService(PreventativeMaintenanceRepo repo, MechanicsRepo mechrepo)
        {
            _repo = repo;
            _mechanicsRepo = mechrepo;
        }

        public async Task<dynamic> GetPmTemplates()
        {
            var pms = await _repo.GetPmTemplates();
            return pms;
        }

        public async Task<dynamic> UpdatePmDates(UpdateDatesPmTemplate pm, int id)
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

        public async Task<FullPmTemplateResponse> GetPmTemplateById(int id)
        {
            var pmWrapper = await _repo.GetPmTemplateById(id);
            var tasks = await _repo.GetPmTasksByTemplateId(id);

            PmTemplate template = new PmTemplate
            {
                Id = pmWrapper.Id,
                Description = pmWrapper.Description,
                Priority = pmWrapper.Priority,
                NextRunDate = pmWrapper.NextRunDate,
                CreatedBy = pmWrapper.CreatedBy,
                LastRun = pmWrapper.LastRun,
                Frequency = pmWrapper.Frequency,
                CreatedDate = pmWrapper.CreatedDate,
                Status = pmWrapper.Status,

            };
            Mechanic mechanic = new Mechanic
            {
                Id = pmWrapper.MechId,
                FirstName = pmWrapper.MechFirstname,
                Lastname = pmWrapper.MechLastname
            };

            Asset asset = new Asset
            {
                Id = pmWrapper.AssetId,
                Description = pmWrapper.AssetDesc
            };

            return new FullPmTemplateResponse
            {
                PmTemplate = template,
                Mechanic = mechanic,
                Asset = asset,
                Tasks = tasks

            };
        }

        public async Task<dynamic> UpdatePmTemplateById(UpdatePmTemplateRequest pm, int id) 
        {
            var res = await _repo.UpdatePmTemplateById(pm, id);
            return res;
        }

        public async Task<dynamic> DeletePmTemplate(int id) 
        {
            var res = await _repo.DeletePmTemplate(id);
            return res;
        }
    }
}
