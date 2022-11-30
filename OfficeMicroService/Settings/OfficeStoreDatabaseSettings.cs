namespace OfficeMicroService.Settings
{
    public class OfficeStoreDatabaseSettings : IOfficeStoreDatabaseSettings
    {
        public string OfficesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
