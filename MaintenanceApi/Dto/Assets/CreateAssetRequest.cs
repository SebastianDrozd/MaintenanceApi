namespace MaintenanceApi.Dto.Assets
{
    public class CreateAssetRequest
    {
        public string comp_desc { get; set; }
        public string line_no { get; set;}
        public string department { get; set; }
        public string manufacturer { get; set; }
        public string model_no { get; set; }
        public string serial_no { get; set; }
        public string status { get; set; }
        public string? service_date { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public string CreatedBy { get; set; }
    }
}
