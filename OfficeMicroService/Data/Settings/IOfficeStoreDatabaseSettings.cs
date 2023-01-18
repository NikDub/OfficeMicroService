namespace OfficeMicroService.Data.Settings;

public interface IOfficeStoreDatabaseSettings
{
    string OfficesCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}