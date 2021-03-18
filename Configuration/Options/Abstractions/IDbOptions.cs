namespace Configuration.Options
{
    public interface IDbOptions
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
        
        string SchemaName { get; set; }
    }
}
