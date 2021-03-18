namespace Configuration.Options
{
    public class SqlDbOptions : IDbOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string SchemaName { get; set; }
    }
}
