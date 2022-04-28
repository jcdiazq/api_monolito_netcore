namespace MonolitoApi.Settings
{
    public class FileImageStorageData
    {
        public string ConnectionString { get; set; }  = null!;
        public string DatabaseName { get; set; } = null!;
        public string ImageCollectionName { get; set; } = null!;
    }
}