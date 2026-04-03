namespace MaintenanceApi.Dto.Assets
{
    public class FullAsset
    {
        public string compid { get; set; }
        public string comp_desc { get; set; }
        public string? full_desc { get; set; }
        public string? line_no { get; set; }
        public string? department { get; set; }
        public string? manufacturer { get; set; }
        public string? model_no { get; set; }
        public string? serial_no { get; set; }
        public string? status { get; set; }
        public string created { get; set; }

        public DateTime? purchase_date { get; set; }
        public DateTime? install_date { get; set; }
        public DateTime? last_service_date { get; set; }
        public DateTime? next_service_date { get; set; }

    }
}
