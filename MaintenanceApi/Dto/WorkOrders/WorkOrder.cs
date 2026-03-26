using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Mechanics;

namespace MaintenanceApi.Dto.WorkOrders
{
    public class WorkOrder
    {
        public int Id { get; set; }
        
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string Requestor { get; set; }
        public string Priority { get; set; }
        public string Asset { get; set; }
        public string AssetDescription { get; set; }
        public int MechanicId { get; set; }
        public string MechanicFirstName { get; set; }
        public string MechanicLastName { get; set; }


    }
}
