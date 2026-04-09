using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Mechanics;
using MaintenanceApi.Dto.Pagination;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class PreventativeMaintenanceService
    {
        private readonly PreventativeMaintenanceRepo _repo;
        private readonly MechanicsRepo _mechanicsRepo;
        private readonly LogService _logService;
        public PreventativeMaintenanceService(PreventativeMaintenanceRepo repo, MechanicsRepo mechrepo, LogService logService)
        {
            _repo = repo;
            _mechanicsRepo = mechrepo;
            _logService = logService;
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

            if (id > 0) 
            {
                await _logService.CreateNewEvent(new Dto.Logs.CreateNewLogRequest
                {
                    event_type = "pm_template",
                    event_action = "Created",
                    entity_id = id.ToString(),
                    description = $"New Pm template created : ${pm.Description}",
                    performed_by = pm.CreatedBy

                });
            }



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

        public async Task<dynamic> GetShortPmTemplatesQuery(int page, int pageSize, string searchTerm, string frequency) 
        {

            if (searchTerm.IsWhiteSpace())
            {
                Console.WriteLine("String is whitespace");
                searchTerm = string.Empty;
            }
            if (frequency.IsWhiteSpace())
            {
                frequency = string.Empty;
            }
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var pmtemplates = await _repo.GetShortPmTempaltesQuery(page, pageSize, searchTerm, frequency);
            var count = await _repo.CountPmTemplates(searchTerm, frequency);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            return new PaginatedList<ShortPmTemplateResponse>
            {
                Items = pmtemplates,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = totalPages
            };
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
            await _logService.CreateNewEvent(new Dto.Logs.CreateNewLogRequest
            {
                event_type = "pm_template",
                event_action = "Modified",
                entity_id = id.ToString(),
                description = $"Pm template was modified : ${pm.Description}",
                performed_by = pm.UpdatedBy

            });
            return res;
        }

        public async Task<dynamic> DeletePmTemplate(int id) 
        {
            var template = await _repo.GetPmTemplateById(id);
            var res = await _repo.DeletePmTemplate(id);
            await _logService.CreateNewEvent(new Dto.Logs.CreateNewLogRequest
            {
                event_type = "pm_template",
                event_action = "Deleted",
                entity_id = id.ToString(),
                description = $"Pm template was Deleted : ${template.Description}",
                performed_by = "maintenance"

            });
            return res;
        }
    }
}
