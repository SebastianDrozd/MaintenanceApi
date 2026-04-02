namespace MaintenanceApi.Dto.Assets
{
    public class SingleAssetResponse
    {
        public FullAsset asset { get; set; }
        public List<AssetImage> images { get; set; }
    }
}
