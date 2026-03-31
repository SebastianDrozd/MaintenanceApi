using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Mechanics;

namespace MaintenanceApi.Dto.Pm
{
    public class FullPmTemplateResponse
    {
        public PmTemplate PmTemplate { get; set; }
        public Mechanic Mechanic { get; set; }
        public Asset Asset { get; set; }

        public List<PmTask> Tasks { get; set; }
       
    }
}
